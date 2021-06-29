using System.Linq;

namespace LinkFinder.WebApi.Services
{
    public static class PaginationExtension
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> source, int currentPage, int countOfPage)
        {
            source = source.Skip(currentPage * countOfPage)
                           .Take(countOfPage);

            return source;
        }
    }
}
