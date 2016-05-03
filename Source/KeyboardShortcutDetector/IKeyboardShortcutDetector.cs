using System;

namespace KeyboardShortcutDetector
{
    public interface IKeyboardShortcutDetector : IDisposable
    {
        void RegisterShortcut(IKeyboardShortcut shortcut);
        void Subscribe(object observer);
    }
}