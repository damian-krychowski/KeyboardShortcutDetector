using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class RangeKey : IKey
    {
        private readonly Key _from;
        private readonly Key _to;

        public RangeKey(Key from, Key to)
        {
            _from = @from;
            _to = to;
        }

        public bool IsIncluded(Key key)
        {
            return key >= _from && key <= _to;
        }
    }
}