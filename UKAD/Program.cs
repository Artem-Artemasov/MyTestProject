using System;
using System.Threading.Tasks;
using UKAD.Controllers;
using UKAD.Filters;
using UKAD.Repository;
using UKAD.Services;
using UKAD.Views;

namespace UKAD
{
   
    class Program
    {
        public static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            LinkRepository linkRepository = new LinkRepository();
            LinkService linkService = new LinkService(linkRepository);
            ConsoleWritter resultWritter = new ConsoleWritter();
            LinkView linkView = new LinkView(resultWritter);
            LinkController linkController = new LinkController(linkService,linkRepository,linkView);

            linkController.StartWork();
        }
        
    }
}
