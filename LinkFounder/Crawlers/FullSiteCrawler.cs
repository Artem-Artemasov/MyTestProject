using LinkFounder.Logic.Interfaces;
using LinkFounder.Logic.Models;
using System.Collections.Generic;
using System.Linq;


namespace LinkFounder.Logic.Crawlers
{
    public class FullSiteCrawler
    {
        private readonly ICrawler _htmlCrawler;
        private readonly ICrawler _sitemapCrawler;

        public FullSiteCrawler(ICrawler htmlCrawler, ICrawler sitemapCrawler)
        {
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var htmlLinks = _htmlCrawler.GetLinks(baseUrl);
            var sitemapLinks = _sitemapCrawler.GetLinks(baseUrl);

            return htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                               .Concat(sitemapLinks)
                                               .ToList();
        }
    }
}
