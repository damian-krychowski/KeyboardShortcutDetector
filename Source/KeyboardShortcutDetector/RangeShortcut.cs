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

        public bool IsInRange(Key key)
        {
            return key >= _fromKey && key <= _toKey;
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

    public class RangeShortcut : IKeyboardShortcut
    {
        private readonly ImmutableArray<Key> _keys;
        private readonly KeyRange _lastKeyRange;

        public RangeShortcut(IEnumerable<Key> keys, KeyRange lastKeyRange)
        {
            _keys = keys.ToImmutableArray();
            _lastKeyRange = lastKeyRange;
        }

        public Key? LastKeyInRange { get; private set; }

        public ShortcutStateChange KeyboardStateChanged(KeyboardState state)
        {
            if (WasPressed(state))
            {
                LastKeyInRange = state.CurrentCombination.Last();
                return ShortcutStateChange.Pressed;
            }

            if (WasReleased(state))
            {
                LastKeyInRange = state.PreviousCombination.Last();
                return ShortcutStateChange.Released;
            }

            LastKeyInRange = null;
            return ShortcutStateChange.None;
        }

        private bool WasPressed(KeyboardState state)
        {
            return Enumerable.SequenceEqual(state.PreviousCombination, _keys) &&
                   Enumerable.SequenceEqual(state.CurrentCombination.SkipLast(), _keys) &&
                   _lastKeyRange.IsInRange(state.CurrentCombination.Last());
        }

        private bool WasReleased(KeyboardState state)
        {
            return Enumerable.SequenceEqual(state.BeforePreviousCombination, _keys) &&
                   Enumerable.SequenceEqual(state.PreviousCombination.SkipLast(), _keys) &&
                   _lastKeyRange.IsInRange(state.PreviousCombination.Last()) &&
                   Enumerable.SequenceEqual(state.CurrentCombination, _keys);
        }
    }
}
