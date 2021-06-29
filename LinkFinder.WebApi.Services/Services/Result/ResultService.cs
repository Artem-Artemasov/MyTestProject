using LinkFinder.DbWorker;
using LinkFinder.WebApi.Services.Filters;
using LinkFinder.WebApi.Services.Request;
using LinkFinder.WebApi.Services.Response;
using System.Linq;
using System.Threading.Tasks;

namespace LinkFinder.WebApi.Services
{
    public class ResultService
    {
        private readonly DatabaseWorker _dbWorker;
        private readonly ResultFilter _resultFilter;

        public ResultService(DatabaseWorker dbWorker, ResultFilter resultsFilter)
        {
            _dbWorker = dbWorker;
            _resultFilter = resultsFilter;
        }

        /// <summary>
        /// Return count of results with test id.
        /// </summary>
        /// <param name="testId"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual async Task<ResponseMessage> GetResultCountAsync(int testId, GetTestDetailParam param)
        {
            var results = (await _dbWorker.GetResultsAsync(testId));

            results = _resultFilter.Filter(results, param);

            return new ResponseMessage()
            {
                Content = new ResultCountDto() { CountResults = results.Count() },
            };
        }

    }
}
