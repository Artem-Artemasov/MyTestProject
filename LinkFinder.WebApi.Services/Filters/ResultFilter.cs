using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Logic.Request;
using System.Linq;

namespace LinkFinder.WebApi.Logic.Filters
{
    public class ResultFilter
    {
        public virtual IQueryable<Result> Filter(IQueryable<Result> results, GetTestDetailParam param)
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
