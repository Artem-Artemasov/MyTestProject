using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;
using LinkFounder.Logic.Services;

namespace LinkFounder.Logic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly LinkParser LinkParser;
        private readonly LinkValidator LinkValidator;
        private readonly RequestService RequestService;
        private readonly LinkConverter LinkConverter;

        public HtmlCrawler(RequestService requestService,LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
        {
            LinkParser = linkParser;
            LinkValidator = linkValidator;
            RequestService = requestService;
            LinkConverter = linkConverter;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var storage = new List<Link>();

            if (LinkValidator.IsCorrectLink(baseUrl) == false)
                return storage;

            storage.Add(new Link(baseUrl));
            storage = ProcessLink(baseUrl, storage.First(), storage).ToList();

            return storage;
        }

        // TODO: Splite a
        private IEnumerable<Link> ProcessLink(string domain, Link currentPage, List<Link> existingLinks)
        {
            var page = RequestService.DownloadPage(currentPage);

            var parsedUrls = LinkParser.Parse(page);

            var foundedLinks = LinkConverter.RelativeToAbsolute(domain, parsedUrls, currentPage.Url)
                               .Select(p => new Link(p))
                               .ToList();

            foundedLinks =  NormalizeLink(foundedLinks, existingLinks.FirstOrDefault().Url)
                           .Except(existingLinks, (x, y) => x.Url == y.Url)
                           .ToList();

            existingLinks.AddRange(foundedLinks);

            foreach (var link in foundedLinks)
            {
                ProcessLink(domain, link, existingLinks);
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

                if (LinkValidator.IsFileLink(link.Url))
                    continue;

                if (LinkValidator.IsInDomain(link.Url, domain) == false)
                    continue;

                validLinks.Add(link);
            }
            return validLinks;
        }
    }
}
