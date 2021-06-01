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
        private readonly LinkParser _linkParser;
        private readonly LinkValidator _linkValidator;
        private readonly RequestService _requestService;
        private readonly LinkConverter _linkConverter;

        public SitemapCrawler(RequestService requestService, LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
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
            {
                return storage;
            }

            if (baseUrl.EndsWith('/') == false)
            {
                baseUrl += '/';
            }

            var sitemapLink = new Link(GetSitemapUrl(baseUrl));

            var responseMessage = _requestService.DownloadPage(sitemapLink);

            var parsedUrls = _linkParser.Parse(responseMessage);

            storage = _linkConverter.RelativeToAbsolute(parsedUrls, baseUrl)
                                  .Select(p => new Link(p))
                                  .Where(p => _linkValidator.IsInCurrentSite(p.Url, baseUrl))
                                  .ToList();

            storage = SetupTimeResponse(storage).ToList();

            return storage.OrderBy(p=>p.TimeResponse);
        }

        private IEnumerable<Link> SetupTimeResponse(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                _requestService.SendRequest(link.Url, out int timeResponse);
                link.TimeResponse = timeResponse;
            }
            return links;
        }

        public virtual string GetSitemapUrl(string baseUrl)
        {
            return baseUrl + "sitemap.xml";
        }
    }
}
