using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using KeyboardShortcutDetector.Keys;
using System.Windows.Input;

namespace KeyboardShortcutDetector
{
    internal static class ImmutableArrayExtensions
    {
        public static ImmutableArray<T> SkipLast<T>(this ImmutableArray<T> array)
        {
            return array.Take(array.Length - 1).ToImmutableArray();
        }

        public static bool KeysSequenceEqual(this ImmutableArray<IKey> keys, ImmutableArray<Key> keyboardState)
        {
            if (keys.Length == keyboardState.Length)
            {
                return !keys.Where((key, i) => !key.IsIncluded(keyboardState[i])).Any();
            }

            return false;
        }
    }

    internal static class ListExtensions
    {
        public static T PickFirstOrDefault<T>(this List<T> list, Func<T, bool> predicate)
        {
            var item = list.FirstOrDefault(predicate);

            if (item != null)
            {
                list.Remove(item);
            }

            return item;
        }
    }

    internal static class ImmutableHashSetExtensions
    {
        public static bool IsProperSupersetOfKeys(
            this ImmutableHashSet<IKey> validKeys,
            ImmutableArray<Key> keyboardState)
        {
            var validKeysList = validKeys.ToList();

            return validKeysList.Count > keyboardState.Length &&
                   Enumerable.All(keyboardState, key => validKeysList.PickFirstOrDefault(x => x.IsIncluded(key)) != null);
        }

        public static bool KeysSetEquals(
            this ImmutableHashSet<IKey> validKeys,
            ImmutableArray<Key> keyboardState)
        {
            var validKeysList = validKeys.ToList();

            return validKeysList.Count == keyboardState.Length &&
                   Enumerable.All(keyboardState, key => validKeysList.PickFirstOrDefault(x => x.IsIncluded(key)) != null);
        }
    }
}