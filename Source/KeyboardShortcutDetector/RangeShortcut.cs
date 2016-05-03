using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;

namespace KeyboardShortcutDetector
{
    public class KeyRange
    {
        private readonly Key _fromKey;
        private readonly Key _toKey;

        public KeyRange(Key fromKey, Key toKey)
        {
            if (fromKey > toKey) throw new ArgumentException("fromKey cannot be greater than toKey");

            _fromKey = fromKey;
            _toKey = toKey;
        }

        public IEnumerable<Key> ToSeries()
        {
            for (var key = _fromKey; key <= _toKey; key++)
            {
                yield return key;
            }
        }

        public static KeyRange Digits()
        {
            return new KeyRange(Key.D0, Key.D9);
        }

        public static KeyRange NumPadDigits()
        {
            return new KeyRange(Key.NumPad0, Key.NumPad9);
        }

        public static KeyRange Letters()
        {
            return new KeyRange(Key.A, Key.Z);
        }
    }

    public class RangeShortcut : SeriesShortcut
    {
        private readonly ImmutableArray<Key> _keys;
        private readonly KeyRange _lastKeyRange;

        public RangeShortcut(IEnumerable<Key> keys, KeyRange lastKeyRange) : base(
            keys, lastKeyRange.ToSeries())
        {
        }
    }
}
