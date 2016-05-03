# KeyboardShortcutDetector

C# keyboard listener detecting shortcuts.

Build status: 
[![Build status](https://ci.appveyor.com/api/projects/status/yy9xm6q5uw8crd64?svg=true)](https://ci.appveyor.com/project/damian-krychowski/keyboardshortcutdetector)

## Shortcuts

This library allows to subscribe to keyboard shortcut events. There are two kinds of events: shortcut can be pressed or released.
Each shortcut is represented by a class. There are two base classes provided: `Shortcut`, `SeriesShortcut` and `RangeShortcut`. Each can be used to create
derived shortcut classes. 

```csharp
public class CtrlAltDelShortcut : Shortcut
{
    public CtrlAltDelShortcut() : base(Key.LeftCtrl, Key.LeftAlt, Key.Delete)
    {        
    }
}
```

`Shortcut` class allows to create simple shortcuts with provided keys. Order of pressing the keys does matter: 
`Key.LeftCtrl -> Key.LeftAlt -> Key.Delete` and `Key.LeftAlt -> Key.LeftCtrl -> Key.Delete` are different shortcuts.

For more complicated shortcuts there is `RangeShortcut` base class prepared. It allows to create shortcuts where last key can be 
picked from defined range like for example from digits between 0-9 or from letters between A-Z. Custom ranges can be defined as well.
There is `LastKeyInRange` property defined - it allows to check which key was the shortcut triggered with.

```csharp
public class LeftCtrlDigitShortcut : RangeShortcut
{
    public LeftCtrlDigitShortcut() : base(
        new []{Key.LeftCtrl }, 
        KeyRange.Digits())
    {
    }
}

public class RightCtrlSomeLettersShortcut : RangeShortcut
{
    public RightCtrlSomeLettersShortcut() : base(
        new[] {Key.RightCtrl},
        new KeyRange(fromKey: Key.A, toKey: Key.G))
    {
    }
}
```

There is also `SeriesShortcut` available. Last key can be picked from specified list of possible keys.

```csharp
public class LeftCtrlEvenDigitShortcut : SeriesShortcut
{
    public LeftCtrlEvenDigitShortcut() : base(
        new []{Key.LeftCtrl }, 
        new []{Key.D0, Key.D2, Key.D4, Key.D6, Key.D8})
    {
    }
}
```

There is also a possibility to implement own shortcut type with `IKeyboardShortcut` interface.

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
    public class LeftCtrlDigitShortcut : RangeShortcut
    {
        public LeftCtrlDigitShortcut() : base(
            new []{Key.LeftCtrl }, 
            KeyRange.Digits())
        {
        }
    }

    public class RightCtrlLetterShortcut : RangeShortcut
    {
        public RightCtrlLetterShortcut() : base(
            new []{Key.RightCtrl},
            KeyRange.Letters())
        {
        }
    }

    public partial class MainWindow : Window, 
        IShortcutObserver<LeftCtrlDigitShortcut>,
        IShortcutObserver<RightCtrlLetterShortcut>
    {
        private IKeyboardShortuctDetector _detector;

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
            Result.Text = "LeftCtr + " + shortcut.LastKeyInRange.ToString();
        }

        public void ShortcutReleased(LeftCtrlDigitShortcut shortcut)
        {
            Result.Text = "LeftCtrl combination released";
        }

        public void ShortcutPressed(RightCtrlLetterShortcut shortcut)
        {
            Result.Text = "RightCtrl + " + shortcut.LastKeyInRange.ToString();
        }

        public void ShortcutReleased(RightCtrlLetterShortcut shortcut)
        {
            Result.Text = "RightCtrl combination released";
        }
    }
}
```

## Remarks

- Each shortcut should be registered before any subscriber can observe it - otherwise exception will be thrown.
- `KeyboardShortcutDetector` should be a singleton, and it should not be garbage collected - it will cause and exception as it uses unmanaged code to listen for keyboard events.

