using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkFinder.WebApp.Controllers
{
    public class TestsController : Controller
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly CrawlerApp _webCrawlerApp;
        private readonly LinkValidator _linkValidator;

        public TestsController(DatabaseWorker dbWorker, CrawlerApp webCrawlerApp, LinkValidator linkValidator)
        {
            _dbWorker = dbWorker;
            _webCrawlerApp = webCrawlerApp;
            _linkValidator = linkValidator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tests = await _dbWorker.GetTestsAsync();
            ViewBag.Error = TempData["Error"];

            return View(tests);
        }

        [HttpPost]
        public async Task<IActionResult> PostUrl(string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                TempData["Error"] = errorMessage;
                return RedirectToAction("Index");
            }

            await _webCrawlerApp.StartWork(url);

            return RedirectToAction("Index");
        }
    }
}
