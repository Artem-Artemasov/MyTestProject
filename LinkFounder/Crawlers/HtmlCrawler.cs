using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;
using LinkFounder.Logic.Services;
using LinkFounder.Logic.Interfaces;

namespace LinkFounder.Logic.Crawlers
{
    public class HtmlCrawler : ICrawler
    {
        private readonly LinkParser _LinkParser;
        private readonly LinkValidator _LinkValidator;
        private readonly RequestService _RequestService;
        private readonly LinkConverter _LinkConverter;

        public HtmlCrawler(RequestService requestService, LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
        {
            _LinkParser = linkParser;
            _LinkValidator = linkValidator;
            _RequestService = requestService;
            _LinkConverter = linkConverter;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var storage = new List<Link>();

            if (_LinkValidator.IsCorrectLink(baseUrl) == false)
                return storage;

            if (baseUrl.EndsWith('/') == false)
            {
                baseUrl += '/';
            }

            if (_RequestService.SendRequest(baseUrl,out int time).IsSuccessStatusCode == false)
            {
                return storage;
            }

            storage.Add(new Link(baseUrl));
            storage = AnalyzeLink(storage.First(), storage).ToList();

            return storage.OrderBy(p=>p.TimeResponse);
        }

        // TODO: Splite a
        private IEnumerable<Link> AnalyzeLink(Link currentPage, List<Link> existingLinks)
        {
            var page = _RequestService.DownloadPage(currentPage);

            var parsedUrls = _LinkParser.Parse(page);

            var foundedLinks = _LinkConverter.RelativeToAbsolute(parsedUrls, currentPage.Url)
                               .Select(p => new Link(p))
                               .ToList();

            foundedLinks = NormalizeLink(foundedLinks, existingLinks.FirstOrDefault().Url)
                           .Except(existingLinks, (x, y) => x.Url == y.Url)
                           .ToList();

            existingLinks.AddRange(foundedLinks);

            foreach (var link in foundedLinks)
            {
                AnalyzeLink(link, existingLinks);
            }

            return existingLinks;
        }

        private IEnumerable<Link> NormalizeLink(List<Link> links, string domain)
        {
            var validLinks = new List<Link>();
            foreach (var link in links)
            {
                if (validLinks.FirstOrDefault(p => p.Url == link.Url) != null)
                    continue;

                if (_LinkValidator.IsFileLink(link.Url))
                    continue;

                if (_LinkValidator.IsInCurrentSite(link.Url, domain) == false)
                    continue;

                validLinks.Add(link);
            }
            return validLinks;
        }
    }
}
