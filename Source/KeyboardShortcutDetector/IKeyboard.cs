using System;

namespace KeyboardShortcutDetector
{
    public interface IKeyboard : IDisposable
    {
        event Action<KeyboardState> StateChanged;
    }
}