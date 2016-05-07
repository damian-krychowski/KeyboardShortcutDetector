using System;
using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector
{
    public interface IKeyboardShortcutDetector : IDisposable
    {
        void Restart();
        void RegisterShortcut(IKeyboardShortcut shortcut);
        void Subscribe(object observer);
    }
}