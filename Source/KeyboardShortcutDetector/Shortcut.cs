using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;

namespace KeyboardShortcutDetector
{
    public class Shortcut : IKeyboardShortcut
    {
        private readonly ImmutableArray<Key> _keys;

        public Shortcut(params Key[] keys)
        {
            _keys = keys.ToImmutableArray();
        }

        public ShortcutStateChange KeyboardStateChanged(KeyboardState state)
        {
            if(WasPressed(state)) return ShortcutStateChange.Pressed;
            if(WasReleased(state)) return ShortcutStateChange.Released;
            return ShortcutStateChange.None;
        }

        private bool WasPressed(KeyboardState state)
        {
            return Enumerable.SequenceEqual(state.PreviousCombination, _keys.SkipLast()) &&
                   Enumerable.SequenceEqual(state.CurrentCombination, _keys);
        }

        private bool WasReleased(KeyboardState state)
        {
            return Enumerable.SequenceEqual(state.BeforePreviousCombination, _keys.SkipLast()) &&
                   Enumerable.SequenceEqual(state.PreviousCombination, _keys) &&
                   Enumerable.SequenceEqual(state.CurrentCombination, _keys.SkipLast());
        }
    }
}