using System.Collections.Generic;
using System.Linq;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Models;
using LinkFounder.Logic;

namespace LinkFounder.ConsoleView
{
    public class LinkViewer
    {
        private readonly ResultWritter _consoleWritter;
        private readonly FullSiteCrawler _siteCrawler;

        public LinkViewer(ResultWritter consoleWritter, FullSiteCrawler siteCrawler)
        {
            _consoleWritter = consoleWritter;
            _siteCrawler = siteCrawler;
        }

        public virtual void StartWork()
        {
            var inputUrl = ReadUrl();

            _consoleWritter.WriteLine("Program is working, please don't close it.");

            var htmlLinks = _siteCrawler.GetHtmlLinks(inputUrl);
            var sitemapLinks = _siteCrawler.GetSitemapLinks(inputUrl);
            var allLinks = _siteCrawler.GetLinks(inputUrl);

            if (allLinks.Any() == false)
            {
                _consoleWritter.WriteLine("Sorry, but you write a bad url");
                return;
            }

            PrintAllInformation(htmlLinks, sitemapLinks, allLinks);
        }

        public virtual string ReadUrl()
        {
            _consoleWritter.WriteLine("Please enter a url");
            return _consoleWritter.ReadLine();
        }

        public virtual void PrintAllInformation(IEnumerable<Link> htmlLinks, IEnumerable<Link> sitemapLinks, IEnumerable<Link> allLinks)
        {
            PrintCaption("\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            PrintList(sitemapLinks.Except(htmlLinks, (x, y) => x.Url == y.Url));

            PrintCaption("\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            PrintList(htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url));

            PrintWithTime(allLinks.OrderBy(p => p.TimeResponse));
            PrintCounts(htmlLinks.Count(), sitemapLinks.Count(), allLinks.Count());
        }

        public virtual void PrintCounts(int htmlCount, int sitemapCount, int allCount)
        {
            _consoleWritter.WriteLine($"All links found {allCount} \n");
            _consoleWritter.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            _consoleWritter.WriteLine($"Urls(html documents) found after crawling a website: {htmlCount} \n");
        }

        public virtual void PrintCaption(string caption)
        {
            _consoleWritter.WriteLine("\n\n\n");
            _consoleWritter.WriteLine(caption);
            WriteRaw('_');
        }

        public virtual void PrintList(IEnumerable<Link> links)
        {
            int index = 1;
            WriteRaw('_');
            foreach (var link in links)
            {
                _consoleWritter.WriteLine("\n");
                _consoleWritter.WriteLine($" {index}) " + link.Url);
                WriteRaw('_');
                index++;
            }
        }

        private void WriteRaw(char symbol)
        {
            for (int i = 0; i < _consoleWritter.GetOutputWidth(); i++)
                _consoleWritter.Write(symbol.ToString());
        }

        public virtual void PrintWithTime(IEnumerable<Link> links)
        {
            int index = 1;
            WriteRaw('_');
            _consoleWritter.Write("|  Url");

            _consoleWritter.ChangeCursorPositonX(_consoleWritter.GetOutputWidth() - 16);

            _consoleWritter.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > _consoleWritter.GetOutputWidth() - 25)
                {
                    link.Url = SliceWithWidth(link.Url, (_consoleWritter.GetOutputWidth() - 25));
                }
                _consoleWritter.WriteLine("\n|  ");
                _consoleWritter.Write($"{index}) " + link.Url);

                _consoleWritter.ChangeCursorPositonX(_consoleWritter.GetOutputWidth() - 15);

                _consoleWritter.WriteLine(" | " + link.TimeResponse + "ms  |");
                index++;
                WriteRaw('_');
            }
        }

        public virtual string SliceWithWidth(string input, int maxWidth)
        {
            if (maxWidth < 0)
            {
                return input;
            }

            int insertSymbols = input.Length / maxWidth;
            for (int i = 1; i <= insertSymbols; i++)
            {
                input = input.Insert(maxWidth * i, "\n   ");
            }
            return input;
        }
    }
}
