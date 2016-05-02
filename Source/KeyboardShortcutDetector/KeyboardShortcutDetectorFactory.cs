using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardShortcutDetector
{
    public class KeyboardShortcutDetectorFactory
    {
        public IKeyboardShortuctDetector Create()
        {
            return new ShortcutDetector(
                new Keyboard());
        }
    }
}
