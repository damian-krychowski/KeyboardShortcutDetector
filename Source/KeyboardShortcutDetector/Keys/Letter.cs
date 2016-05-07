using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class Letter : RangeKey
    {
        public Letter() : base(Key.A, Key.Z)
        {
        }
    }
}