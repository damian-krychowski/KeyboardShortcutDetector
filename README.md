# KeyboardShortcutDetector

C# keyboard listener detecting shortcuts.

Build status: 
[![Build status](https://ci.appveyor.com/api/projects/status/yy9xm6q5uw8crd64?svg=true)](https://ci.appveyor.com/project/damian-krychowski/keyboardshortcutdetector)

## Shortcuts

This library allows to subscribe to keyboard shortcut events. There are two kinds of events: shortcut can be pressed or released.
Each shortcut is represented by a class. There are two base classes provided: `CombinationShortcut` and `PermutationShortcut`. Each can be used to create
derived shortcut classes. 

To create a shortcut user should specify the keys he want to use. `CombinationShortcut` doesn't recognize order of pressing the keys, while `PermutationShortcut` does. Each of the shortcut type
has two constructor's overloads: 
 - `(params Key[] keys)`
 - `(params IKey[] keys)`

Where `Key` represents single keyboard key, and `IKey` represents object able to interpret more than one `Key`, for example:
 - `Ctrl` represents `Key.LeftCtrl` or `Key.RightCtrl`
 - `SeriesKey` represents specified collection of `Keys` (like {`Key.D0`, `Key.D2`, `Key.D4`, `Key.D6`, `Key.D8`} - even numbers)
 - `RangeKey` represents specified range of `Keys` (like `Key.A` - `Key.Z` - all letters)
 - etc.

```csharp
    public class CtrlAltDelShortcut : CombinationShortcut
    {
        public CtrlAltDelShortcut() : base(
            new Ctrl(),
            new Alt(),
            new SingleKey(Key.Delete))
        {
            
        }
    }
```
In the example above different keyboard keys combinations will trigger the shortcut, for example:
 - LeftAlt -> LeftCtrl -> Delete
 - RightCtrl -> Delete -> LeftAlt

If the shortcut would be defined like this:

```csharp
    public class CtrlAltDelShortcut : PermutationShortcut
    {
        public CtrlAltDelShortcut() : base(
            Key.LeftCtr,
            Key.LeftAlt,
            Key.Delete)
        {
            
        }
    }
```

Then only one combination would be valid: LeftCtrl -> LeftAlt -> Delete.

Provided `CombinationShortcut` and `PermutationShortcut` alongside with ability to define own `IKey` implementation
will allow user to create any shortcut he can imagine. However, there is also a possibility to implement own shortcut type with `IKeyboardShortcut` interface.

## Observers

To subscribe to events, a class have to implement one of the following interfaces: 

```csharp
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
```

Each subscriber can observe as many shortcuts as user wishes to. For each shortcut type, desired interface should be implemented.

## Example

Example how to use the KeyboardShortcutDetector (from WPF demo project):

```csharp
namespace KeyboardShortcutDetector.Demo
{
    public class LeftCtrlDigitShortcut : PermutationShortcut
    {
        public LeftCtrlDigitShortcut() : base(
            new SingleKey(Key.LeftCtrl), 
            new Digit())
        {
        }

        public Key PressedDigit => LastTriggeredBy[1];
    }

    public class RightCtrlLetterShortcut : PermutationShortcut
    {
        public RightCtrlLetterShortcut() : base(
            new SingleKey(Key.RightCtrl),
            new Letter())
        {
        }

        public Key PressedLetter => LastTriggeredBy[1];
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, 
        IShortcutObserver<LeftCtrlDigitShortcut>,
        IShortcutObserver<RightCtrlLetterShortcut>
    {
        private IKeyboardShortcutDetector _detector;

        public MainWindow()
        {
            InitializeComponent();
            InitializeShortcutDetector();
        }

        private void InitializeShortcutDetector()
        {
            _detector = new KeyboardShortcutDetectorFactory().Create();
            _detector.RegisterShortcut(new LeftCtrlDigitShortcut());
            _detector.RegisterShortcut(new RightCtrlLetterShortcut());

            _detector.Subscribe(this);
        }

        public void ShortcutPressed(LeftCtrlDigitShortcut shortcut)
        {
            Result.Text = "LeftCtr + " + shortcut.PressedDigit.ToString();
        }

        public void ShortcutReleased(LeftCtrlDigitShortcut shortcut)
        {
            Result.Text = "LeftCtrl combination released";
        }

        public void ShortcutPressed(RightCtrlLetterShortcut shortcut)
        {
            Result.Text = "RightCtrl + " + shortcut.PressedLetter.ToString();
        }

        public void ShortcutReleased(RightCtrlLetterShortcut shortcut)
        {
            Result.Text = "RightCtrl combination released";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _detector.Dispose();
        }
    }
}
```

## Remarks

- Each shortcut should be registered before any subscriber can observe it - otherwise exception will be thrown.
- `KeyboardShortcutDetector` should be a singleton, and it should not be garbage collected - it will cause and exception as it uses unmanaged code to listen for keyboard events.

