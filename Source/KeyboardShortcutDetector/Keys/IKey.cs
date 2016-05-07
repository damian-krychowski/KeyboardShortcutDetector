using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public interface IKey
    {
        bool IsIncluded(Key key);
    }
}