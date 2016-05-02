using System;

namespace KeyboardShortcutDetector
{
    public interface IKeyboardShortuctDetector : IDisposable
    {
        void RegisterShortcut(IKeyboardShortcut shortcut);
        void Subscribe(object observer);
    }
}