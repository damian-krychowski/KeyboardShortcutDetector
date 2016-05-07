using System.Collections.Generic;
using System.Windows.Input;
using KeyboardShortcutDetector.Keys;
using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal class CtrlAltDigitPermutation : PermutationShortcut
    {
        public CtrlAltDigitPermutation() : base(
            new SingleKey(Key.LeftCtrl),
            new SingleKey(Key.LeftAlt),
            new Digit())
        {
        }

        public Key? SelectedDigit
        {
            get
            {
                if (LastTriggeredBy != null && LastTriggeredBy.Length == 3)
                {
                    return LastTriggeredBy[2];
                }

                return null;
            }
        }
    }

    internal class RangeShortcutObserver : IShortcutObserver<CtrlAltDigitPermutation>
    {
        public int PressedCounter { get; private set; }
        public int ReleasedCounter { get; private set; }
        public List<Key> LastPressedKey { get; } = new List<Key>();
        public List<Key> LastReleasedKey { get; } = new List<Key>();

        public void ShortcutPressed(CtrlAltDigitPermutation shortcut)
        {
            PressedCounter++;
            LastPressedKey.Add(shortcut.SelectedDigit.Value);
        }

        public void ShortcutReleased(CtrlAltDigitPermutation shortcut)
        {
            ReleasedCounter++;
            LastReleasedKey.Add(shortcut.SelectedDigit.Value);
        }
    }
}
