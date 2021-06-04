using LinkFinder.ConsoleOutput;
using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Services;
using LinkFinder.Logic.Validators;
using LinkFinder.Logic;
using System;
using System.Linq;

namespace LinkFinder.DbWorker
{
    public class LinkConsoleApp
    {
        private readonly DatabaseWorker _dbWorker;

        public LinkConsoleApp(DatabaseWorker worker)
        {
            _dbWorker = worker;
        }
        public void Start()
        {
            var requestService = new RequestService();
            var linkParser = new LinkParser();
            var linkConverter = new LinkConverter();
            var linkValidator = new LinkValidator();

            var htmlCrawler = new HtmlCrawler(requestService, linkConverter, linkParser, linkValidator);
            var sitemapCrawler = new SitemapCrawler(requestService, linkConverter, linkParser, linkValidator);

            var consoleWritter = new ConsoleWritter();
            var linkPrinter = new LinkPrinter(consoleWritter);

            var inputUrl = linkPrinter.AskUrl();

            linkPrinter.PrintMessageAboutCrawling();

            var htmlLinks = htmlCrawler.GetLinks(inputUrl);
            var sitemapLinks = sitemapCrawler.GetLinks(inputUrl);

            linkPrinter.PrintAllInformation(htmlLinks, sitemapLinks);

            _dbWorker.SaveAsync(inputUrl,htmlLinks,sitemapLinks).Wait();

            Environment.Exit(0);
        }
    }
}
