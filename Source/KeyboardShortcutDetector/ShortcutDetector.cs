using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeyboardShortcutDetector.Shortcuts;

namespace KeyboardShortcutDetector
{
    internal class ShortcutDetector : IKeyboardShortcutDetector
    {
        private readonly IKeyboard _keyboard;
        private readonly List<IKeyboardShortcut> _shortcuts = new List<IKeyboardShortcut>(); 
        private readonly ShortcutObserverList _pressedObservers = new ShortcutObserverList();
        private readonly ShortcutObserverList _releasedObservers = new ShortcutObserverList();

        public ShortcutDetector(IKeyboard keyboard)
        {
            _keyboard = keyboard;
            _keyboard.StateChanged += KeyboardOnStateChanged;
        }

        private void KeyboardOnStateChanged(KeyboardState keyboardState)
        {
            var shortcutGroups = _shortcuts
                .GroupBy(sc => sc.KeyboardStateChanged(keyboardState))
                .ToList();
            
            NotifyShortcutPressedObservers(shortcutGroups);
            NotifyShortcutReleasedObservers(shortcutGroups);
        }

        private void NotifyShortcutPressedObservers(IEnumerable<IGrouping<ShortcutStateChange, IKeyboardShortcut>> shortcutGroups)
        {
            var pressedShortcutsGroup = shortcutGroups.FirstOrDefault(g => g.Key == ShortcutStateChange.Pressed);

            if (pressedShortcutsGroup != null)
            {
                ExecuteObserverNotifyActions(pressedShortcutsGroup, _pressedObservers);
            }
        }
        
        private void NotifyShortcutReleasedObservers(IEnumerable<IGrouping<ShortcutStateChange, IKeyboardShortcut>> shortcutGroups)
        {
            var releasedShortcutsGroup = shortcutGroups.FirstOrDefault(g => g.Key == ShortcutStateChange.Released);

            if (releasedShortcutsGroup != null)
            {
                ExecuteObserverNotifyActions(releasedShortcutsGroup, _releasedObservers);
            }
        }

        private void ExecuteObserverNotifyActions(
            IEnumerable<IKeyboardShortcut> shortcuts,
            ShortcutObserverList observerList)
        {
            foreach (var shortcut in shortcuts)
            {
                foreach (var observerNotifyAction in observerList.GetObserverNotifyActions(shortcut))
                {
                    observerNotifyAction(shortcut);
                }
            }
        }

        public void RegisterShortcut(IKeyboardShortcut shortcut)
        {
            if (shortcut == null) throw new ArgumentNullException(nameof(shortcut));
            _releasedObservers.RegisterShortcut(shortcut);
            _pressedObservers.RegisterShortcut(shortcut);
            _shortcuts.Add(shortcut);
        }

        public void Subscribe(object observer)
        {
            var pressedObserverInterfaces = GetShortcutPressedObserverInterfaces(observer);
            var releasedObserverInterfaces = GetShortcutReleasedObserverInterfaces(observer);

            ThrowIfObserverNotValid(pressedObserverInterfaces, releasedObserverInterfaces);
            
            SubscribeForAllPressed(observer, pressedObserverInterfaces);
            SubscribeForAllReleased(observer, releasedObserverInterfaces);
        }

        private Type[] GetShortcutPressedObserverInterfaces(object observer)
        {
            return GetInterfaces(observer, typeof(IShortcutPressedObserver<>));
        }

        private Type[] GetShortcutReleasedObserverInterfaces(object observer)
        {
            return GetInterfaces(observer, typeof(IShortcutReleasedObserver<>));
        }

        private Type[] GetInterfaces(object observer, Type genericObserverType)
        {
            return observer.GetType()
                .GetInterfaces()
                .Where(x => x.IsGenericType &&
                            x.GetGenericTypeDefinition() == genericObserverType)
                .ToArray();
        }

        private static void ThrowIfObserverNotValid(
            Type[] pressedObserverInterfaces,
            Type[] releasedObserverInterfaces)
        {
            if (pressedObserverInterfaces.Length == 0 &&
                releasedObserverInterfaces.Length == 0)
            {
                throw new ArgumentException("Given observer doesn't implement any IShortcutObserver interface");
            }
        }

        private void SubscribeForAllPressed(object observer, Type[] pressedObserverInterfaces)
        {
            SubscribeForAll(observer, pressedObserverInterfaces, nameof(SubscribeForPressed));
        }

        private void SubscribeForAllReleased(object observer, Type[] releasedObserverInterfaces)
        {
            SubscribeForAll(observer, releasedObserverInterfaces, nameof(SubscribeForReleased));
        }

        private void SubscribeForAll(object observer, Type[] observerInterfaces, string methodName)
        {
            foreach (var observerInterface in observerInterfaces)
            {
                InvokeGenericSubscribeMethod(observer, observerInterface.GetGenericArguments()[0], methodName);
            }
        }
        
        private void InvokeGenericSubscribeMethod(object observer, Type shortcutType, string methodName)
        {
            typeof(ShortcutDetector)
                    .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
                    .MakeGenericMethod(shortcutType)
                    .Invoke(this, new[] { observer });
        }

        private void SubscribeForReleased<TShortcut>(IShortcutReleasedObserver<TShortcut> observer) where TShortcut : class, IKeyboardShortcut
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            Action<IKeyboardShortcut> updateObserverAction = shortcut => observer.ShortcutReleased((TShortcut) shortcut);
            _releasedObservers.RegisterObserver<TShortcut>(updateObserverAction);
        }

        private void SubscribeForPressed<TShortcut>(IShortcutPressedObserver<TShortcut> observer) where TShortcut : class, IKeyboardShortcut
        {
            if (observer == null) throw new ArgumentNullException(nameof(observer));

            Action<IKeyboardShortcut> updateObserverAction = shortcut => observer.ShortcutPressed((TShortcut)shortcut);
            _pressedObservers.RegisterObserver<TShortcut>(updateObserverAction);
        }
        
        public void Dispose()
        {
            _keyboard.Dispose();
        }
    }
}