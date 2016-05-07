using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KeyboardShortcutDetector.Keys;
using KeyboardShortcutDetector.Shortcuts;

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
