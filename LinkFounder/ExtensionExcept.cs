using System.Linq;

namespace System.Collections.Generic
{                           /* Replace Where(p => existingLinks.FirstOrDefault(s => s.Url == p.Url) == null)*/
    public static class ExtensionExcept
    {
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Where(x => second.Count(y => comparer(x, y)) == 0);
        }
    }
}
