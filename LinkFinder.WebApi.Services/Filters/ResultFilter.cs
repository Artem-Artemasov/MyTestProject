using LinkFinder.DbWorker.Models;
using LinkFinder.WebApi.Logic.Request;
using System.Linq;

namespace LinkFinder.WebApi.Logic.Filters
{
    public class ResultFilter
    {
        public virtual IQueryable<Result> Filter(IQueryable<Result> results, GetTestDetailParam param)
        {
            if (null != param.InHtml && null != param.InSitemap)
            {
                results = results.Where(p => p.InSitemap == param.InSitemap.Value)
                                .Where(p => p.InHtml == param.InHtml.Value);
            }

            return results;
        }
    }
}
