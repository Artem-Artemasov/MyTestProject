using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UKAD.Interfaces;
using UKAD.Models;

namespace UKAD
{
    class LinkView : ILinkView
    {
        /// <summary>
        /// Display main operation and it code
        /// </summary>

        public virtual bool Processing()
        {
            Console.WriteLine("Program is working, please don't close it.");
            return true;
        }
        /// <summary>
        /// Caclulate links and print it to console
        /// </summary>
        public bool PrintCounts(ILinkRepository linkRepository)
        {
            int allCount = linkRepository.GetAllLinksAsync().Result.Count();
            int sitemapCount = allCount - linkRepository.GetViewLinksAsync().Result.Count();
            int viewCount = allCount - linkRepository.GetSiteMapLinksAsync().Result.Count();
            Console.WriteLine($"All founded urls - {allCount} \n");
            Console.WriteLine($"Urls found in sitemap: {sitemapCount} \n");
            Console.WriteLine($"Urls(html documents) found after crawling a website: {viewCount} \n");
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
                Console.WriteLine("\n");
                Console.WriteLine($" {i}) " + link.Url);
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
            Console.Write("|  Url");
            Console.CursorLeft = Console.BufferWidth - 16;
            Console.WriteLine(" | Timing (ms)");
            WriteRaw('_');
            foreach (var link in links)
            {
                if (link.Url.Length > Console.BufferWidth - 25)
                {
                    link.Url = InsertNewLine(link.Url);
                }

                Console.WriteLine("\n");
                Console.Write("|  ");
                Console.Write($"{i}) " + link.Url);
                Console.CursorLeft = Console.BufferWidth-15;
                Console.WriteLine(" | " + link.TimeDuration + "ms  |" );
                i++;
                WriteRaw('_');
            }

            return true;
        }
        public string InsertNewLine(string input)
        {
            int insertSymbols = input.Length / (Console.BufferWidth - 25);
            
            for (int i = 1; i <= insertSymbols; i++)
            {
                input = input.Insert((Console.BufferWidth - 25) * i,"\n   ");
            }

            return input;
        }
        public bool WriteRaw(char symbol)
        {
            for(int i = 0; i < Console.BufferWidth; i++)
            {
                Console.Write(symbol);
            }
            return true;
        }
    }
}
