using LinkFinder.WebApi.Services;
using LinkFinder.WebApi.Services.Request;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestService _testsService;
        private readonly ResultService _resultsService;

        public TestController(TestService testsService, ResultService resultsService)
        {
            _testsService = testsService;
            _resultsService = resultsService;
        }

        /// <summary>
        /// Return all existing tests
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _testsService.GetAllTestsAsync();

            return Ok(response);
        }

        /// <summary>
        /// Return detail information about test
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestResults(int id, [FromQuery] GetTestDetailParam param)
        {
            var response = await _testsService.GetTestAsync(id, param);

            return Ok(response);
        }

        /// <summary>
        /// Add new test for input site
        /// </summary>
        /// <param name="param">Param with it url will be crawled</param>
        /// <returns></returns>
        /// <response code="200">Website with input URL has been crawled</response>     
        /// <response code="400">When input URL not valid, return errorMessage </response>     
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTestParam param)
        {
            var response = await _testsService.AddTestAsync(param.Url);

            if (response.IsSuccessful == false)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        /// <summary>
        /// Return a count of results with test id
        /// </summary>
        /// <param name="id">Id of test which count results you want to get</param>
        /// <param name="param">Results will be sought with that params</param>
        /// <returns></returns>
        [HttpGet("{id}/count")]
        public async Task<IActionResult> GetResultCount(int id, [FromQuery] GetTestDetailParam param)
        {
            var response = await _resultsService.GetResultCountAsync(id, param);

            return Ok(response);
        }
    }
}
