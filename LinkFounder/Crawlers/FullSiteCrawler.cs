using LinkFounder.Logic.Interfaces;
using LinkFounder.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkFounder.Logic.Crawlers
{
    public class FullSiteCrawler
    {
        private readonly ICrawler _HtmlCrawler;
        private readonly ICrawler _SitemapCrawler;

        public FullSiteCrawler(ICrawler htmlCrawler, ICrawler sitemapCrawler)
        {
            _HtmlCrawler = htmlCrawler;
            _SitemapCrawler = sitemapCrawler;
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            var htmlLinks = _HtmlCrawler.GetLinks(baseUrl);
            var sitemapLinks = _SitemapCrawler.GetLinks(baseUrl);

            return htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                               .Concat(sitemapLinks)
                                               .ToList();
        }
    }
}
