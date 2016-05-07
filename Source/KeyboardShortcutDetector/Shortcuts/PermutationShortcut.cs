using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Input;
using KeyboardShortcutDetector.Keys;

namespace KeyboardShortcutDetector.Shortcuts
{
    public class PermutationShortcut : Shortcut
    {
        private readonly ImmutableArray<IKey> _keys;

        public PermutationShortcut(params Key[] keys)
        {
            _keys = keys
                .Select(key=>new SingleKey(key))
                .ToImmutableArray<IKey>();
        }

        public PermutationShortcut(params IKey[] keys)
        {
            _keys = keys.ToImmutableArray();
        }

        protected override bool WasPressed(KeyboardState state)
        {
            return _keys.SkipLast().KeysSequenceEqual(state.PreviousCombination) &&
                   _keys.KeysSequenceEqual(state.CurrentCombination);
        }

        protected override bool WasReleased(KeyboardState state)
        {
            return _keys.SkipLast().KeysSequenceEqual(state.BeforePreviousCombination) &&
                   _keys.KeysSequenceEqual(state.PreviousCombination) &&
                   _keys.SkipLast().KeysSequenceEqual(state.CurrentCombination);
        }
    }
}