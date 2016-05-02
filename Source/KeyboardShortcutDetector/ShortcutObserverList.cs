using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardShortcutDetector
{
    internal class ShortcutObserverList
    {
        private readonly Dictionary<Type, List<Action<IKeyboardShortcut>>> _observers = new Dictionary<Type, List<Action<IKeyboardShortcut>>>(); 

        public void RegisterShortcut(IKeyboardShortcut shortcut)
        {
            if (_observers.ContainsKey(shortcut.GetType()))
            {
                throw new InvalidOperationException($"Shortcut {shortcut.GetType().Name} was already registered");
            }

            _observers.Add(shortcut.GetType(), new List<Action<IKeyboardShortcut>>());
        }

        public void RegisterObserver<TShortcut>(Action<IKeyboardShortcut> observerNotifyAction)
            where TShortcut: class, IKeyboardShortcut
        {
            if (!_observers.ContainsKey(typeof (TShortcut)))
            {
                throw new InvalidOperationException($"Shortcut of type {typeof(TShortcut).Name} was not registered. Register it before subscribe new observer.");
            }

            _observers[typeof(TShortcut)].Add(observerNotifyAction);
        }

        public List<Action<IKeyboardShortcut>> GetObserverNotifyActions(IKeyboardShortcut shortcut)
        {
            return _observers[shortcut.GetType()].ToList();
        }
    }
}