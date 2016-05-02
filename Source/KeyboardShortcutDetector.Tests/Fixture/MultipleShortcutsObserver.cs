using System.Collections.Generic;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal class MultipleShortcutsObserver :
        IShortcutObserver<CtrlAltDigit>,
        IShortcutObserver<CtrlAltDel>
    {
        public int CtrlAltDelPressedCounter { get; private set; }
        public int CtrlAltDelReleasedCounter { get; private set; }

        public int CtrlAltDigitPressedCounter { get; private set; }
        public int CtrlAltDigitReleasedCounter { get; private set; }
        public List<Key> LastPressedDigitKey { get; } = new List<Key>();
        public List<Key> LastReleasedDigitKey { get; } = new List<Key>();

        public void ShortcutPressed(CtrlAltDigit shortcut)
        {
            CtrlAltDigitPressedCounter ++;
            LastPressedDigitKey.Add(shortcut.LastKeyInRange.Value);
        }

        public void ShortcutReleased(CtrlAltDigit shortcut)
        {
            CtrlAltDigitReleasedCounter ++;
            LastReleasedDigitKey.Add(shortcut.LastKeyInRange.Value);
        }

        public void ShortcutPressed(CtrlAltDel shortcut)
        {
            CtrlAltDelPressedCounter ++;
        }

        public void ShortcutReleased(CtrlAltDel shortcut)
        {
            CtrlAltDelReleasedCounter ++;
        }
    }
}