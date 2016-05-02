namespace KeyboardShortcutDetector
{
    public enum ShortcutStateChange
    {
        Pressed, Released, None
    }

    public interface IKeyboardShortcut
    {
        ShortcutStateChange KeyboardStateChanged(KeyboardState state);
    }
}