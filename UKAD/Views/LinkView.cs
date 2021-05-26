using System;
using System.Collections.Generic;
using System.Linq;
using UKAD.Interfaces;
using UKAD.Interfaces.View;
using UKAD.Models;
using UKAD.Repository;

namespace UKAD.Views
{
    public class LinkView : ILinkView
    {
        private IResultWritter resultWritter { get; set; }

        public LinkView(IResultWritter writter)
        {
            resultWritter = writter;
        }
        public bool Processing()
        {
            resultWritter.WriteLine("Program is working, please don't close it.");
            return true;
        }

        /// <summary>
        /// Caclulate links and print it to console
        /// </summary>
        public bool PrintCounts(LinkRepository linkRepository)
        {
            int allCount = linkRepository.GetAllLinksAsync().Result.Count();
            int sitemapCount = allCount - linkRepository.GetViewLinksAsync().Result.Count();
            int viewCount = allCount - linkRepository.GetSiteMapLinksAsync().Result.Count();

            resultWritter.WriteLine($"All founded urls - {allCount} \n");
            resultWritter.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            resultWritter.WriteLine($"Urls(html documents) found after crawling a website: {viewCount} \n");

            return true;
        }

        /// <summary>
        /// Print input list to console
        /// </summary>
        public bool PrintList(IEnumerable<Link> links)
        {
            WriteRaw('_');
            int i = 1;
            foreach (var link in links)
            {
                resultWritter.WriteLine("\n");
                resultWritter.WriteLine($" {i}) " + link.Url);
                WriteRaw('_');
                i++;
            }

            return true;
        }

        /// <summary>
        /// Print to console all object from list with url and time
        /// </summary>
        public bool PrintWithTime(IEnumerable<Link> links)
        {
            int i = 1;
            WriteRaw('_');
            resultWritter.Write("|  Url");

            resultWritter.ChangeCursorPositonX(resultWritter.GetOutputWidth() - 16);

            resultWritter.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > resultWritter.GetOutputWidth() - 25)
                {
                    link.Url = InsertNewLine(link.Url, (resultWritter.GetOutputWidth() - 25));
                }

                resultWritter.WriteLine("\n|  ");
                resultWritter.Write($"{i}) " + link.Url);

                resultWritter.ChangeCursorPositonX(resultWritter.GetOutputWidth() - 15);

                resultWritter.WriteLine(" | " + link.TimeDuration + "ms  |" );
                i++;
                WriteRaw('_');
            }

            return true;
        }

        /// <summary>
        /// Split input string on the many, insert a \n beetwen and return as one string 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string InsertNewLine(string input,int maxWidth)
        {
            int insertSymbols = input.Length / maxWidth;
            
            for (int i = 1; i <= insertSymbols; i++)
            {
                input = input.Insert(maxWidth * i,"\n   ");
            }

            return input;
        }

        /// <summary>
        /// Print line with input symbol. Width = console.BufferWidth at the call moment
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public bool WriteRaw(char symbol)
        {
            for(int i = 0; i < resultWritter.GetOutputWidth(); i++)
            {
                resultWritter.Write(symbol.ToString());
            }
            return true;
        }
    }
}
