using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace KeyboardShortcutDetector
{
    internal static class ImmutableArrayExtensions
    {
        public static IEnumerable<T> SkipLast<T>(this ImmutableArray<T> array)
        {
            return array.Take(array.Length - 1);
        }
    }
}