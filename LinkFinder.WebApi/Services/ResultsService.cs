using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Filters;
using LinkFinder.WebApi.Models;
using LinkFinder.WebApi.RoutingParams;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Services
{
    public class ResultsService
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly ResultsFilter _resultsFilter;

        public ResultsService(DatabaseWorker dbWorker, ResultsFilter resultsFilter)
        {
            _dbWorker = dbWorker;
            _resultsFilter = resultsFilter;
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

        public virtual async Task<ApiResultCount> GetResultCountAsync(int testId, TestDetailParam param)
        {
            var results = (await _dbWorker.GetResultsAsync(testId));

            results = _resultsFilter.Filter(results, param);

            return new ApiResultCount()
            {
                CountResults = results.Count(),
            };
        }

    }
}
