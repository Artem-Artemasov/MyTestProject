using System.Linq;

namespace LinkFinder.WebApi.Logic
{
    public static class PaginationExtension
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> source, int currentPage, int countOfPage)
        {
            currentPage = currentPage - 1;

            if (currentPage < 0 || countOfPage <= 0)
            {
                return source;
            }

            source = source.Skip(currentPage * countOfPage)
                           .Take(countOfPage);

            return source;
        }
    }
}
