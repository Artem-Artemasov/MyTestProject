using LinkFounder.ConsoleView;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Services;
using LinkFounder.Logic.Validators;
using System;
using System.Linq;

namespace LinkFounder.DbSaver
{
    public class LinkViewerApp
    {
        private readonly DataSaver _saver;

        public LinkViewerApp(DataSaver saver)
        {
            _saver = saver;
        }
        public void Start()
        {
            var RequestService = new RequestService();
            var LinkParser = new LinkParser();
            var LinkConverter = new LinkConverter();
            var LinkValidator = new LinkValidator();

            var htmlCrawler = new HtmlCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);
            var sitemapCrawler = new SitemapCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);
            var crawler = new FullSiteCrawler(htmlCrawler, sitemapCrawler);

            var ConsoleView = new ResultWritter();
            var LinkView = new LinkViewer(ConsoleView,crawler);

            LinkView.StartWork();
            var links = crawler.GetLinks(crawler.lastUrl).OrderBy(p => p.TimeResponse);

            _saver.Save(crawler.lastUrl,links).Wait();

            Environment.Exit(0);
        }
    }
}
