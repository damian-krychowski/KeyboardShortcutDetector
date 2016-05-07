using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class KeysOr : IKey
    {
        private readonly ImmutableHashSet<IKey> _keys;

        public KeysOr(params IKey[] keys)
        {
            _keys = keys.ToImmutableHashSet();
        }

        public bool IsIncluded(Key key)
        {
            return _keys.Any(x => x.IsIncluded(key));
        }
    }
}