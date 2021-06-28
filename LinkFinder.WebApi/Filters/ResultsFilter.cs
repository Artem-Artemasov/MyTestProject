using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.RoutingParams;
using System.Linq;

namespace LinkFinder.WebApi.Filters
{
    public class ResultsFilter
    {
        public virtual IQueryable<Result> Filter(IQueryable<Result> results, TestDetailParam param)
        {
            if (param.InHtml || param.InSitemap)
            {
                results = results.Where(p => p.InSitemap == param.InSitemap)
                                 .Where(p => p.InHtml == param.InHtml);
            }

            return results;
        }
    }
}
