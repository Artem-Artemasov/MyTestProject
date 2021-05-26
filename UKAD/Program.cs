using System;
using System.Threading.Tasks;
using UKAD.Controllers;
using UKAD.Filters;
using UKAD.Repository;
using UKAD.Services;

namespace UKAD
{
   
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            LinkRepository linkRepository = new LinkRepository();

            LinkFilter linkFilter = new LinkFilter();

            LinkService linkService = new LinkService(linkRepository);

            LinkView linkView = new LinkView();

            LinkController linkController = new LinkController(linkService,linkView);

            linkController.StartWork();

        }
        
    }
}
