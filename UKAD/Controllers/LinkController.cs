using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UKAD.Filters;
using UKAD.Interfaces;
using UKAD.Models;
using UKAD.Repository;

namespace UKAD.Controllers
{
    public class LinkController
    {
        private ILinkService LinkService { get; set; }
        private ILinkView LinkView { get; set; }
        private LinkRepository LinkRepository { get; set; }
        private LinkFilter LinkFilter { get; set; }
        public LinkController(ILinkService linkService,LinkRepository linkRepository, ILinkView linkView)
        {
            LinkService = linkService;
            LinkRepository = linkRepository;
            LinkView = linkView;
            LinkFilter = new LinkFilter();
        }
        /// <summary>
        /// Is calling url and validate it
        /// If url valid, started parsing site
        /// </summary>

        public async Task<bool> AddAllLinksAsync()
        {
            Console.WriteLine("Please type a basic url");
            string input = Console.ReadLine();

            if (LinkFilter.IsCorrectLink(input))
            {
                if (input.EndsWith("/")) input = input.Substring(0, input.Length - 1);

                input = LinkFilter.AddWWW(input);

                LinkService.SetUpBaseUrl(input);

                LinkView.Processing();

                await LinkService.AnalyzeSiteForUrlAsync();
                
                Console.Clear();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// It starting parse url and start Menu method
        /// </summary>
        public bool StartWork()
        {
            if (this.AddAllLinksAsync().Result == false)
            {
                Console.WriteLine("You entered a bad url");
                return false;
            };

            LinkRepository.Sort(p => p.TimeDuration);
            Console.ForegroundColor = ConsoleColor.White;

            Menu();

            return false;
        }

        private bool Menu()
        {
            Console.WriteLine("\n\n\n");
            Console.WriteLine("\t Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
            LinkView.PrintList(LinkRepository.GetSiteMapLinksAsync().Result);
            Console.WriteLine("\n\n\n");

            Console.WriteLine("\tUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
            LinkView.PrintList(LinkRepository.GetViewLinksAsync().Result);
            Console.WriteLine("\n\n\n");


            Console.WriteLine("\t Timing");
            LinkView.PrintWithTime(LinkRepository.GetAllLinksAsync().Result);
            Console.WriteLine("\n\n\n");

            LinkView.PrintCounts(LinkRepository);
            return true;
        }
    }
}
