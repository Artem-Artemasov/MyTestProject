using LinkFinder.DbWorker;
using LinkFinder.Logic.Validators;
using LinkFinder.WebApi.RoutingParams;
using LinkFinder.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Controllers
{
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly TestsService _testsService;

        public TestsController(TestsService testsService)
        {
            _testsService = testsService;
        }

        /// <summary>
        /// Return all existing tests
        /// </summary>
        /// <returns></returns>
        [Route("api/tests")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var apiTests = await _testsService.GetAllTests();

            return Ok(apiTests);
        }

        /// <summary>
        /// Return detail information about test
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [Route("api/test/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetTestResults(int id, [FromQuery] TestDetailParam param)
        {
            var results = await _testsService.GetTest(id, param);

            return Ok(results);
        }

        /// <summary>
        /// Add new test for input site
        /// </summary>
        /// <param name="url">URL that will be crawled</param>
        /// <returns></returns>
        /// <response code="200">Website with input URL has been crawled</response>     
        /// <response code="400">When input URL not valid, return errorMessage </response>     
        [Route("api/test")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string url)
        {
            var errorMessage = await _testsService.AddTest(url);

            if (String.IsNullOrEmpty(errorMessage) == false)
            {
                ModelState.AddModelError("errorMessage", errorMessage);

                return BadRequest(ModelState);
            }
         
            return Ok();
        }
    }
}
