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

        private async Task<bool> AddAllLinksAsync()
        {
            var input = LinkView.ReadUrl();

            if (LinkFilter.IsCorrectLink(input))
            {
                if (input.EndsWith("/")) 
                        input = input.Substring(0, input.Length - 1);

                input = LinkFilter.ToSingleStyle(input);
                LinkService.SetUpBaseUrl(input);
                LinkView.PrintProcessingMessage();

                await LinkService.AnalyzeSiteForUrlAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// It starting parse url and start Menu method
        /// Needed time for work
        /// </summary>
        public bool StartWork()
        {
            if (AddAllLinksAsync().Result == false)
            {
                LinkView.PrintErrorMessage("You entered a bad url");
                return false;
            };

            LinkRepository.Sort(p => p.TimeDuration);

            LinkView.PrintAllInformation(LinkRepository);

            return false;
        }
    }
}
