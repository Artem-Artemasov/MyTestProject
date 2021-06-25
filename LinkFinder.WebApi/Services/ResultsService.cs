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

        public static ApiResult MapApiResult(Result result)
        {
            return new ApiResult()
            {
                Id = result.Id,
                TimeResponse = result.TimeResponse,
                Url = result.Url
            };
        }

        public virtual async Task<ApiResultCount> GetResultCountAsync(int testId)
        {
            var countResults = (await _dbWorker.GetResultsAsync(testId)).Count();

            return new ApiResultCount()
            {
                CountResults = countResults,
            };
        }

    }
}
