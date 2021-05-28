using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Models;
using LinkFounder.Logic.Services;

namespace LinkFounder.Logic.Crawlers
{
    public class SitemapCrawler
    {
        private readonly RequestService RequestService;
        private readonly LinkParser LinkParser;
        private readonly LinkValidator LinkFilter;

        public SitemapCrawler(RequestService requestService, LinkParser linkProcessing, LinkValidator linkFilter)
        {
            RequestService = requestService;
            LinkProcessing = linkProcessing;
            LinkFilter = linkFilter;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var storage = new List<Link>();

            if (LinkFilter.IsCorrectLink(baseUrl) == false)
                return storage;

            var sitemapLink = new Link(GetSitemapUrl(baseUrl));
            var responseMessage = RequestService.SendRequest(sitemapLink.Url, out int timeOfResponse);
            sitemapLink.TimeResponse = timeOfResponse;

            var parsedUrls = LinkParser.ResponseMsgToUrlList(responseMessage);
            storage = LinkParser.RelativeToAbsolute(baseUrl, parsedUrls, baseUrl)
                                  .Select(p => new Link(p))
                                  .ToList();

            foreach (var link in storage)
            {
                RequestService.SendRequest(link.Url, out int timeResponse);
                link.TimeResponse = timeResponse;
            }

            return storage;
        }

        public virtual string GetSitemapUrl(string baseUrl)
        {
            return baseUrl + "/sitemap.xml";
        }
    }
}
