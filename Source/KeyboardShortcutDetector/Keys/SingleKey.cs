using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class SingleKey : IKey
    {
        private readonly Key _key;

        public SingleKey(Key key)
        {
            _key = key;
        }

        public bool IsIncluded(Key key)
        {
            return _key == key;
        }
    }
}