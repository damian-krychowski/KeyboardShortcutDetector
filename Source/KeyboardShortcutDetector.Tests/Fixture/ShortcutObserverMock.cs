using System.Windows.Input;
using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal class CtrlAltDel : PermutationShortcut
    {
        public CtrlAltDel() : base(Key.LeftCtrl, Key.LeftAlt, Key.Delete)
        { }
    }

    internal class ShortcutObserverMock : IShortcutObserver<CtrlAltDel>
    {
        public int PressedCounter { get; private set; }
        public int ReleasedCounter { get; private set; }

        public void ShortcutPressed(CtrlAltDel shortcut)
        {
            PressedCounter++;
        }

        public void ShortcutReleased(CtrlAltDel shortcut)
        {
            ReleasedCounter++;
        }
    }
}
