using System;
using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector
{
    public interface IShortcutReleasedObserver<in TShortcut>
        where TShortcut :class, IKeyboardShortcut
    {
        void ShortcutReleased(TShortcut shortcut);
    }

    public class ActionShortcutReleasedObserver<TShortcut> : 
        IShortcutReleasedObserver<TShortcut>
        where TShortcut : class, IKeyboardShortcut
    {
        private readonly Action<TShortcut> _toExecute;

        public ActionShortcutReleasedObserver(Action<TShortcut> toExecute)
        {
            _toExecute = toExecute;
        }

        public void ShortcutReleased(TShortcut shortcut)
        {
            _toExecute(shortcut);
        }
    }
}