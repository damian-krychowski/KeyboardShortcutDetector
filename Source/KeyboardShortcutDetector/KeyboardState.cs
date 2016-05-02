using System.Collections.Immutable;
using System.Windows.Input;

namespace KeyboardShortcutDetector
{
    public class KeyboardState
    {
        public ImmutableArray<Key> BeforePreviousCombination { get; }
        public ImmutableArray<Key> PreviousCombination { get; }
        public ImmutableArray<Key> CurrentCombination { get; }

        public KeyboardState(
            ImmutableArray<Key> beforePreviousCombination, 
            ImmutableArray<Key> previousCombination, 
            ImmutableArray<Key> currentCombination)
        {
            BeforePreviousCombination = beforePreviousCombination;
            PreviousCombination = previousCombination;
            CurrentCombination = currentCombination;
        }

        public bool IsPressed(Key key)
        {
            return CurrentCombination.Contains(key);
        }

        public bool IsReleased(Key key)
        {
            return !IsPressed(key);
        }

        public static KeyboardState Empty()
        {
            return new KeyboardState(
                ImmutableArray<Key>.Empty,
                ImmutableArray<Key>.Empty,
                ImmutableArray<Key>.Empty);
        }
    }               

    public static class KeyboardStateExtensions
    {
        public static KeyboardState KeyWasPressed(this KeyboardState currentState, Key pressedKey)
        {
            var newCombination = currentState.CurrentCombination.Add(pressedKey);

            return new KeyboardState(
                currentState.PreviousCombination, 
                currentState.CurrentCombination, 
                newCombination);
        }

        public static KeyboardState KeyWasReleased(this KeyboardState currentState, Key releasedKey)
        {
            var newCombination = currentState.CurrentCombination.Remove(releasedKey);

            return new KeyboardState(
                currentState.PreviousCombination,
                currentState.CurrentCombination, 
                newCombination);
        }
    }
}