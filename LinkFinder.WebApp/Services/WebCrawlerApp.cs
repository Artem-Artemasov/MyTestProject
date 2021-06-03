using LinkFinder.DbWorker;
using LinkFinder.Logic;
using LinkFinder.Logic.Crawlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApp.Services
{
    public class WebCrawlerApp
    {
        private readonly HtmlCrawler _htmlCrawler;
        private readonly SitemapCrawler _sitemapCrawler;
        private readonly DatabaseWorker _dbWorker;

        public WebCrawlerApp(HtmlCrawler htmlCrawler,SitemapCrawler sitemapCrawler,DatabaseWorker dbWorker)
        {
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            _dbWorker = dbWorker;
        }

        public async Task StartWork(string url)
        {
            var htmlLinks = _htmlCrawler.GetLinks(url);
            var sitemapLinks = _sitemapCrawler.GetLinks(url);
            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                           .Concat(sitemapLinks)
                                           .ToList();

            await _dbWorker.SaveAsync(url, htmlLinks,sitemapLinks);
        }
    }
}
