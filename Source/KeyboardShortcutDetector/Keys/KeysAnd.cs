using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class KeysAnd : IKey
    {
        private readonly ImmutableHashSet<IKey> _keys;

        public KeysAnd(params IKey[] keys)
        {
            _keys = keys.ToImmutableHashSet();
        }

        public bool IsIncluded(Key key)
        {
            return _keys.All(x => x.IsIncluded(key));
        }
    }
}