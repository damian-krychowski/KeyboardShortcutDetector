using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using KeyboardShortcutDetector.Keys;

namespace KeyboardShortcutDetector.Shortcuts
{
    public class CtrlAltDelShortcut : CombinationShortcut
    {
        public CtrlAltDelShortcut() : base(
            new Ctrl(),
            new Alt(),
            new SingleKey(Key.Delete))
        {
            
        }
    }
}
