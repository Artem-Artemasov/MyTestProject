using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Validators;
using LinkFounder.Logic.Services;

namespace LinkFounder.ConsoleView
{
    class Program
    {
        static void Main(string[] args)
        {
            var RequestService = new RequestService();
            var LinkParser = new LinkParser();
            var LinkConverter = new LinkConverter();
            var LinkValidator = new LinkValidator();

            var htmlCrawler = new HtmlCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);
            var sitemapCrawler = new SitemapCrawler(RequestService, LinkConverter, LinkParser, LinkValidator);
            var crawler = new FullSiteCrawler(htmlCrawler, sitemapCrawler);

            var ConsoleView = new ResultWritter();
            var LinkView = new LinkViewer(ConsoleView, crawler);

            LinkView.StartWork();
        }
    }
}
