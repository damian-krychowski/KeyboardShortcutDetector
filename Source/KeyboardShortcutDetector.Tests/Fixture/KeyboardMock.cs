using System;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal class KeyboardMock : IKeyboard
    {
        public KeyboardState State { get; private set; } = KeyboardState.Empty();

        public void Dispose()
        {
        }

        public void Restart()
        {
            State = KeyboardState.Empty();
        }

        public event Action<KeyboardState> StateChanged;

        public void PressKey(Key key)
        {
            State = State.KeyWasPressed(key);
            StateChanged?.Invoke(State);
        }

        public void ReleaseKey(Key key)
        {
            State = State.KeyWasReleased(key);
            StateChanged?.Invoke(State);
        }
    }
}