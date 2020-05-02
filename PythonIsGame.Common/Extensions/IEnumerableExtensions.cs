using System;
using System.Collections.Generic;

namespace PythonIsGame.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> GetBigrams<T>(this IEnumerable<T> items)
        {
            var isFirst = true;
            T previous = default;
            foreach (var item in items)
            {
                if (isFirst)
                    isFirst = false;
                else
                    yield return Tuple.Create(previous, item);
                previous = item;
            }
        }
    }
}
