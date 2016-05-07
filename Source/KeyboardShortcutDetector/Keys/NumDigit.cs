using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class NumDigit : RangeKey
    {
        public NumDigit() : base(Key.NumPad0, Key.NumPad9)
        {
        }
    }
}