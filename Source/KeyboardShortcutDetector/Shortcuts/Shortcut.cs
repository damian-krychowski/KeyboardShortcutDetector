using System.Collections.Immutable;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Shortcuts
{
    public abstract class Shortcut : IKeyboardShortcut
    {
        public virtual ShortcutStateChange KeyboardStateChanged(KeyboardState state)
        {
            if (WasPressed(state))
            {
                LastTriggeredBy = state.CurrentCombination;
                return ShortcutStateChange.Pressed;
            }

            if (WasReleased(state))
            {
                LastTriggeredBy = state.PreviousCombination;
                return ShortcutStateChange.Released;
            }

            return ShortcutStateChange.None;
        }

        protected abstract bool WasPressed(KeyboardState state);
        protected abstract bool WasReleased(KeyboardState state);

        public ImmutableArray<Key> LastTriggeredBy { get; private set; }
    }
}