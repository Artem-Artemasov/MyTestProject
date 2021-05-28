using System.Threading.Tasks;
using LinkFounder.Logic.Crawlers;
using LinkFounder.Logic.Services;

namespace UKAD
{
   
    class Program
    {
        public static void Main(string[] args)
        {
            var s = new ViewCrawler(new RequestService(), new LinkProcessing(), new Logic.Filters.LinkValidator());

            var sz = s.GetViewLinks("https://litedb.org");

        }
    }
}
