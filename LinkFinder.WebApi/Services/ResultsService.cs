using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Models;
using LinkFinder.WebApi.RoutingParams;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Services
{
    public class ResultsService
    {
        private readonly DatabaseWorker _dbWorker;

        public ResultsService(DatabaseWorker dbWorker)
        {
            _dbWorker = dbWorker;
        }

        public async Task<IEnumerable<ApiResult>> GetResults(int testId, TestDetailParam param)
        {
            var results = await _dbWorker.GetResultsAsync(testId);

            if (!param.InSitemap && !param.InHtml)
            {
                results.Where(p => (p.InHtml == param.InHtml) && (p.InSitemap == param.InSitemap)); // select results with input param
            }

            return results.OrderBy(p => p.TimeResponse)
                                    .Select(p => MapApiResult(p));
        }

        public static ApiResult MapApiResult(Result result)
        {
            return new ApiResult()
            {
                Id = result.Id,
                TimeResponse = result.TimeResponse,
                Url = result.Url
            };
        }
    }
}
