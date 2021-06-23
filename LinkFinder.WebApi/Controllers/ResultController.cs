using LinkFinder.WebApi.RoutingParams;
using LinkFinder.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly ResultsService _resultsService;

        public ResultController(ResultsService resultsService)
        {
            _resultsService = resultsService;
        }

        /// <summary>
        /// Return results with Test id. When params InHtml and InSitemap == false. Return all objects
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <response code="400">Not set the id</response>     
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] ResultRouteParams param)
        {
            if (null == param.Id)
            {
                return BadRequest();
            }

            var results = await _resultsService.GetResults(param);

            return Ok(results);
        }
    }
}
