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
        private readonly LinkParser _LinkParser;
        private readonly LinkValidator _LinkValidator;
        private readonly RequestService _RequestService;
        private readonly LinkConverter _LinkConverter;

        public SitemapCrawler(RequestService requestService, LinkConverter linkConverter, LinkParser linkParser, LinkValidator linkValidator)
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
            {
                return storage;
            }

            if (baseUrl.EndsWith('/') == false)
            {
                baseUrl += '/';
            }

            var sitemapLink = new Link(GetSitemapUrl(baseUrl));

            var responseMessage = _RequestService.DownloadPage(sitemapLink);

            var parsedUrls = _LinkParser.Parse(responseMessage);

            storage = _LinkConverter.RelativeToAbsolute(parsedUrls, baseUrl)
                                  .Select(p => new Link(p))
                                  .Where(p => _LinkValidator.IsInCurrentSite(p.Url, baseUrl))
                                  .ToList();

            storage = SetupTimeResponse(storage).ToList();

            return storage.OrderBy(p=>p.TimeResponse);
        }

        private IEnumerable<Link> SetupTimeResponse(IEnumerable<Link> links)
        {
            foreach (var link in links)
            {
                _RequestService.SendRequest(link.Url, out int timeResponse);
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
