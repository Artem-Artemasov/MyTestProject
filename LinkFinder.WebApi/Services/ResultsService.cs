using LinkFinder.DbWorker;
using LinkFinder.DbWorker.Models;
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

        public async Task<IEnumerable<Result>> GetResults(ResultRouteParams param)
        {
            var results = await _dbWorker.GetResultsAsync(param.Id.Value);
            
            if (!param.InSitemap && !param.InHtml)
            {
                results.Where(p => (p.InHtml == param.InHtml) && (p.InSitemap == param.InSitemap)); // select results with input param
            }

            return results.OrderBy(p => p.TimeResponse);
        }
    }
}
