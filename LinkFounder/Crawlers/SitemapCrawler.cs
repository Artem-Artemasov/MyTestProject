using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;
using LinkFounder.Logic.Services;
using LinkFounder.Logic.Interfaces;

namespace LinkFounder.Logic.Crawlers
{
    public class SitemapCrawler : ICrawler
    {
        private readonly LinkParser LinkParser;
        private readonly LinkValidator LinkValidator;
        private readonly RequestService RequestService;
        private readonly LinkConverter LinkConverter;

        public SitemapCrawler(RequestService requestService, LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
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

            if (baseUrl.EndsWith('/') == false)
            {
                baseUrl += '/';
            }

            var sitemapLink = new Link(GetSitemapUrl(baseUrl));

            var responseMessage = RequestService.DownloadPage(sitemapLink);

            var parsedUrls = LinkParser.Parse(responseMessage);

            storage = LinkConverter.RelativeToAbsolute(parsedUrls, baseUrl)
                                  .Select(p => new Link(p))
                                  .Where(p => LinkValidator.IsInCurrentSite(p.Url, baseUrl))
                                  .ToList();

            foreach (var link in storage)
            {
                RequestService.SendRequest(link.Url, out int timeResponse);
                link.TimeResponse = timeResponse;
            }

            return storage.OrderBy(p=>p.TimeResponse);
        }

        public virtual string GetSitemapUrl(string baseUrl)
        {
            return baseUrl + "sitemap.xml";
        }
    }
}
