using System.Collections.Immutable;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class SeriesKey : IKey
    {
        private readonly ImmutableHashSet<Key> _keys; 

        public SeriesKey(params Key[] keys)
        {
            _keys = keys.ToImmutableHashSet();
        }

        public bool IsIncluded(Key key)
        {
            return _keys.Contains(key);
        }
    }
}