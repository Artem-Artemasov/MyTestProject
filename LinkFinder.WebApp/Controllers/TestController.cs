using LinkFinder.DbWorker;
using LinkFinder.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkFinder.WebApp.Controllers
{
    public class TestController : Controller
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly WebCrawlerApp _webCrawlerApp;

        public TestController(DatabaseWorker dbWorker,WebCrawlerApp webCrawlerApp)
        {
            _dbWorker = dbWorker;
            _webCrawlerApp = webCrawlerApp;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tests = await _dbWorker.GetTestsAsync();

            return View(tests);
        }

        [HttpPost]
        public async Task<IActionResult> PostUrl(string url) 
        {
            await _webCrawlerApp.StartWork(url);
            return RedirectToAction("Index");
        }
    }
}
