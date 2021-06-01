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
        private List<Link> htmlLinks;
        private List<Link> sitemapLinks;
        public string lastUrl { get; private set; }

        public FullSiteCrawler(ICrawler htmlCrawler, ICrawler sitemapCrawler)
        {
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            htmlLinks = new List<Link>();
            sitemapLinks = new List<Link>();
            lastUrl = "";
        }

        public virtual IEnumerable<Link> GetLinks(string baseUrl)
        {
            if (lastUrl != baseUrl)
            {
                CrawSite(baseUrl);
            }

            return ToOne(htmlLinks, sitemapLinks);
        }

        public virtual IEnumerable<Link> GetSitemapLinks(string baseUrl)
        {
            if (lastUrl != baseUrl)
            {
                CrawSite(baseUrl);
            }

            return sitemapLinks;
        }

        public virtual IEnumerable<Link> GetHtmlLinks(string baseUrl)
        {
            if (lastUrl != baseUrl)
            {
                CrawSite(baseUrl);
            }

            return htmlLinks;
        }
        
        private void CrawSite(string baseUrl)
        {
            lastUrl = baseUrl;
            htmlLinks = _htmlCrawler.GetLinks(baseUrl).ToList();
            sitemapLinks = _sitemapCrawler.GetLinks(baseUrl).ToList();
        }

        private IEnumerable<Link> ToOne(IEnumerable<Link> htmlLinks,IEnumerable<Link> links)
        {
            return htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                            .Concat(sitemapLinks)
                                            .ToList();
        }
    }
}
