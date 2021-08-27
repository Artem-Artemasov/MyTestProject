using System.Collections.Generic;
using System.Linq;
using LinkFinder.Logic.Models;
using LinkFinder.Logic;

namespace LinkFinder.ConsoleOutput
{
    public class LinkPrinter
    {
        private readonly ConsoleWritter _consoleWritter;

        public LinkPrinter(ConsoleWritter consoleWritter)
        {
            _consoleWritter = consoleWritter;
        }

        public virtual void PrintMessageAboutCrawling()
        {
          _consoleWritter.WriteLine("Program is working, please don't close it.");
        }

        public virtual string AskUrl()
        {
            _consoleWritter.WriteLine("Please enter a url");
            return _consoleWritter.ReadLine();
        }

        public virtual void PrintAllInformation(IEnumerable<Link> htmlLinks, IEnumerable<Link> sitemapLinks)
        {
            var allLinks = htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url)
                                .Concat(sitemapLinks)
                                .ToList();

            PrintCaption("\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            PrintList(sitemapLinks.Except(htmlLinks, (x, y) => x.Url == y.Url));

            PrintCaption("\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            PrintList(htmlLinks.Except(sitemapLinks, (x, y) => x.Url == y.Url));

            PrintWithTime(allLinks.OrderBy(p => p.TimeResponse));

            _consoleWritter.WriteLine($"All links found {allLinks.Count()} \n");
            _consoleWritter.WriteLine($"Urls found in sitemap: {sitemapLinks.Count()} \n");
            _consoleWritter.WriteLine($"Urls(html documents) found after crawling a website: {htmlLinks.Count()} \n");
        }

        private void PrintCaption(string caption)
        {
            _consoleWritter.WriteLine("\n\n\n");
            _consoleWritter.WriteLine(caption);
            WriteRaw('_');
        }

        private void PrintList(IEnumerable<Link> links)
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

        private void PrintWithTime(IEnumerable<Link> links)
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
                    var maxOutputStringLenght = _consoleWritter.GetOutputWidth() - 25;
                    link.Url = СutToLength(link.Url,maxOutputStringLenght);
                }
                _consoleWritter.WriteLine("\n|  ");
                _consoleWritter.Write($"{index}) " + link.Url);

                _consoleWritter.ChangeCursorPositonX(_consoleWritter.GetOutputWidth() - 15);

                _consoleWritter.WriteLine(" | " + link.TimeResponse + "ms  |");
                index++;
                WriteRaw('_');
            }
        }

        private string СutToLength(string input, int maxLenght)
        {
            if (maxLenght < 0)
            {
                return input;
            }

            int insertSymbols = input.Length / maxLenght;
            for (int i = 1; i <= insertSymbols; i++)
            {
                input = input.Insert(maxLenght * i, "\n   ");
            }
            return input;
        }
    }
}
