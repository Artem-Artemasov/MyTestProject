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
            var LinkValidator = new LinkValidator();

            var htmlCrawler = new HtmlCrawler(requestService, linkConverter, linkParser, LinkValidator);
            var sitemapCrawler = new SitemapCrawler(requestService, linkConverter, linkParser, LinkValidator);

            var consoleWritter = new ConsoleWritter();
            var linkPrinter = new LinkPrinter(consoleWritter);

            var inputUrl = linkPrinter.AskUrl();

            linkPrinter.PrintMessageAboutCrawling();

            var htmlLinks = htmlCrawler.GetLinks(inputUrl);
            var sitemapLinks = sitemapCrawler.GetLinks(inputUrl);

            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                           .Concat(sitemapLinks)
                                           .ToList();

            linkPrinter.PrintAllInformation(htmlLinks, sitemapLinks, allLinks);

            _dbWorker.Save(inputUrl,allLinks).Wait();

            Environment.Exit(0);
        }
    }
}
