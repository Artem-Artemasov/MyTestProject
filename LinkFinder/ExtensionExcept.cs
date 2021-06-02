using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkFinder.Logic
{                         
    public static class ExtensionExcept
    {
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            return first.Where(x => second.Count(y => comparer(x, y)) == 0);
        }
    }
}
