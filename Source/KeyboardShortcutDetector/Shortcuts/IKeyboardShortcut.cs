using System.Collections.Immutable;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Shortcuts
{
    public enum ShortcutStateChange
    {
        Pressed, Released, None
    }

    public interface IKeyboardShortcut
    {
        ShortcutStateChange KeyboardStateChanged(KeyboardState state);
        ImmutableArray<Key> LastTriggeredBy { get; }
    }
}