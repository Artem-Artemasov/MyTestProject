using System.Threading.Tasks;
using UKAD.Logic.Services;

namespace UKAD
{
   
    class Program
    {
        public static async Task Main(string[] args)
        {
            var f = new ViewCrawler();
            var service = new LinkService("https://www.litedb.org/");

            var s = f.GetViewLinks(service,new Logic.Filters.LinkFilter());
        }
        
    }
}
