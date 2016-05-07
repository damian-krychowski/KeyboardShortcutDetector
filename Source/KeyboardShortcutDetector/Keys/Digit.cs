using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class Digit : RangeKey
    {
        public Digit() : base(Key.D0, Key.D9)
        {
        }
    }
}