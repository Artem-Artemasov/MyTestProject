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
        private readonly LinkParser _linkParser;
        private readonly LinkValidator _linkValidator;
        private readonly RequestService _requestService;
        private readonly LinkConverter _linkConverter;

        public HtmlCrawler(RequestService requestService, LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
        {
            _linkParser = linkParser;
            _linkValidator = linkValidator;
            _requestService = requestService;
            _linkConverter = linkConverter;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var storage = new List<Link>();

            if (_linkValidator.IsCorrectLink(baseUrl) == false)
                return storage;

            if (baseUrl.EndsWith('/') == false)
            {
                baseUrl += '/';
            }

            if (_requestService.SendRequest(baseUrl,out int time).IsSuccessStatusCode == false)
            {
                return storage;
            }

            storage.Add(new Link(baseUrl));
            storage = AnalyzeLink(storage.First(), storage).ToList();

            return storage.OrderBy(p=>p.TimeResponse);
        }

        private IEnumerable<Link> AnalyzeLink(Link currentPage, List<Link> existingLinks)
        {
            var page = _requestService.DownloadPage(currentPage);

            var parsedUrls = _linkParser.Parse(page);

            var foundedLinks = _linkConverter.RelativeToAbsolute(parsedUrls, currentPage.Url)
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

                if (_linkValidator.IsFileLink(link.Url))
                    continue;

                if (_linkValidator.IsInCurrentSite(link.Url, domain) == false)
                    continue;

                validLinks.Add(link);
            }
            return validLinks;
        }
    }
}
