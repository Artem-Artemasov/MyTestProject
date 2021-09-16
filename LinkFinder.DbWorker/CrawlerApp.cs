using LinkFinder.DbWorker.Interfaces;
using LinkFinder.DbWorker.Models;
using LinkFinder.Logic;
using LinkFinder.Logic.Crawlers;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.DbWorker
{
    public class CrawlerApp : ICrawlerApp
    {
        private readonly HtmlCrawler _htmlCrawler;
        private readonly SitemapCrawler _sitemapCrawler;
        private readonly DatabaseWorker _dbWorker;

        public CrawlerApp(HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler, DatabaseWorker dbWorker)
        {
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            _dbWorker = dbWorker;
        }

        public async Task<Test> StartWork(string url)
        {
            var htmlLinks = _htmlCrawler.GetLinks(url);
            var sitemapLinks = _sitemapCrawler.GetLinks(url);
            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                           .Concat(sitemapLinks)
                                           .ToList();

            return await _dbWorker.SaveTestAsync(url, htmlLinks, sitemapLinks);
        }
    }
}
