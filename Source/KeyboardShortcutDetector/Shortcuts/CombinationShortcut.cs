using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;
using KeyboardShortcutDetector.Keys;

namespace KeyboardShortcutDetector.Shortcuts
{
    public class CombinationShortcut : Shortcut
    {
        private readonly ImmutableHashSet<IKey> _keys;

        public CombinationShortcut(params Key[] keys)
        {
            _keys = keys
                .Select(key=> new SingleKey(key))
                .ToImmutableHashSet<IKey>();
        }

        public CombinationShortcut(params IKey[] keys)
        {
            _keys = keys.ToImmutableHashSet();
        }

        protected override bool WasPressed(KeyboardState state)
        {
            return _keys.IsProperSupersetOfKeys(state.PreviousCombination) &&
                   _keys.KeysSetEquals(state.CurrentCombination);
        }

        protected override bool WasReleased(KeyboardState state)
        {
            return _keys.IsProperSupersetOfKeys(state.BeforePreviousCombination) &&
                   _keys.KeysSetEquals(state.PreviousCombination) &&
                   Enumerable.SequenceEqual(state.CurrentCombination, state.PreviousCombination.SkipLast());
        }
    }
}