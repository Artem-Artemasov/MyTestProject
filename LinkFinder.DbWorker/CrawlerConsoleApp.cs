using LinkFinder.ConsoleOutput;
using LinkFinder.Logic.Crawlers;
using System;

namespace LinkFinder.DbWorker
{
    public class CrawlerConsoleApp
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly HtmlCrawler _htmlCrawler;
        private readonly SitemapCrawler _sitemapCrawler;
        private readonly LinkPrinter _linkPrinter;

        public CrawlerConsoleApp(HtmlCrawler htmlCrawler, SitemapCrawler sitemapCrawler, LinkPrinter linkPrinter, DatabaseWorker worker)
        {
            _dbWorker = worker;
            _htmlCrawler = htmlCrawler;
            _sitemapCrawler = sitemapCrawler;
            _linkPrinter = linkPrinter;
        }

        public void Start()
        {
            var inputUrl = _linkPrinter.AskUrl();

            _linkPrinter.PrintMessageAboutCrawling();

            var htmlLinks = _htmlCrawler.GetLinks(inputUrl);
            var sitemapLinks = _sitemapCrawler.GetLinks(inputUrl);

            _linkPrinter.PrintAllInformation(htmlLinks, sitemapLinks);

            _dbWorker.SaveTestAsync(inputUrl, htmlLinks, sitemapLinks).Wait();

            Environment.Exit(0);
        }
    }
}
