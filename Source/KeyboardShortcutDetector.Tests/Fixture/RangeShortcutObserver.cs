using System.Collections.Generic;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal class CtrlAltDigit : RangeShortcut
    {
        public CtrlAltDigit() : base(new[] { Key.LeftCtrl, Key.LeftAlt }, KeyRange.Digits())
        {
        }
    }

    internal class RangeShortcutObserver : IShortcutObserver<CtrlAltDigit>
    {
        public int PressedCounter { get; private set; }
        public int ReleasedCounter { get; private set; }
        public List<Key> LastPressedKey { get; } = new List<Key>();
        public List<Key> LastReleasedKey { get; } = new List<Key>();

        public void ShortcutPressed(CtrlAltDigit shortcut)
        {
            PressedCounter++;
            LastPressedKey.Add(shortcut.LastKey.Value);
        }

        public void ShortcutReleased(CtrlAltDigit shortcut)
        {
            ReleasedCounter++;
            LastReleasedKey.Add(shortcut.LastKey.Value);
        }
    }
}
