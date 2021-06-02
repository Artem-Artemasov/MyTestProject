using LinkFinder.Logic.Crawlers;
using LinkFinder.Logic.Validators;
using LinkFinder.Logic.Services;
using LinkFinder.Logic;
using System.Linq;

namespace LinkFinder.ConsoleOutput
{
    class Program
    {
        static void Main(string[] args)
        {
            var requestService = new RequestService();
            var linkParser = new LinkParser();
            var linkConverter = new LinkConverter();
            var LinkValidator = new LinkValidator();

            var htmlCrawler = new HtmlCrawler(requestService, linkConverter, linkParser, LinkValidator);
            var sitemapCrawler = new SitemapCrawler(requestService, linkConverter, linkParser, LinkValidator);

            var consoleView = new ConsoleWritter();
            var linkPrinter = new LinkPrinter(consoleView);

            var inputUrl = linkPrinter.AskUrl();

            linkPrinter.PrintMessageAboutProcessing();

            var htmlLinks = htmlCrawler.GetLinks(inputUrl);
            var sitemapLinks = sitemapCrawler.GetLinks(inputUrl);

            var allLinks =  htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                           .Concat(sitemapLinks)
                                           .ToList();

            linkPrinter.PrintAllInformation(htmlLinks, sitemapLinks, allLinks);
        }
    }
}
