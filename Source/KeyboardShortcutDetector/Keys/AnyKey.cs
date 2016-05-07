using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyboardShortcutDetector.Keys
{
    public class AnyKey : IKey
    {
        public bool IsIncluded(Key key)
        {
            return true;
        }
    }
}
