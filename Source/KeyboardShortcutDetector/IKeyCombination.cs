using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector
{
    public interface IShortcutPressedObserver<in TShortcut> 
        where TShortcut :class, IKeyboardShortcut
    {
        void ShortcutPressed(TShortcut shortcut);
    }

    public interface IShortcutReleasedObserver<in TShortcut>
       where TShortcut :class, IKeyboardShortcut
    {
        void ShortcutReleased(TShortcut shortcut);
    }

    public interface IShortcutObserver<in TShortcut> :
        IShortcutPressedObserver<TShortcut>,
        IShortcutReleasedObserver<TShortcut>
        where TShortcut :class, IKeyboardShortcut
    {}
}
