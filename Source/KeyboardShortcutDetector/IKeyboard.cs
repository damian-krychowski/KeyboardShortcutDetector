using System;

namespace KeyboardShortcutDetector
{
    public interface IKeyboard : IDisposable
    {
        void Restart();
        event Action<KeyboardState> StateChanged;
    }
}