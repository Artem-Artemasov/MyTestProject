using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly LinkValidator _linkValidator;
        private readonly CrawlerApp _crawlerApp;

        public TestsController(DatabaseWorker dbWorker, CrawlerApp crawlerApp, LinkValidator linkValidator)
        {
            _dbWorker = dbWorker;
            _crawlerApp = crawlerApp;
            _linkValidator = linkValidator;
        }

        /// <summary>
        /// Return all existing tests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tests = await _dbWorker.GetTestsAsync();

            return Ok(tests);
        }

        /// <summary>
        /// Check the input and start crawling the website
        /// </summary>
        /// <param name="url">URL that will be crawled</param>
        /// <returns></returns>
        /// <response code="200">Website with input URL has been crawled</response>     
        /// <response code="400">When input URL not valid, return errorMessage </response>     
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string url)
        {
            if (_linkValidator.IsCorrectLink(url, out string errorMessage) == false)
            {
                ModelState.AddModelError("errorMessage", errorMessage);
                return BadRequest(ModelState);
            }

            await _crawlerApp.StartWork(url);

            return RedirectToAction("Get");
        }
    }
}
