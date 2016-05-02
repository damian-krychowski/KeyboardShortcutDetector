using System.Collections.Immutable;
using System.Windows.Input;
using FluentAssertions;
using NUnit.Framework;

namespace KeyboardShortcutDetector.Tests
{
    [TestFixture]
    internal class RangeShortcutTests
    {
        [TestCase(
            new[] {Key.LeftCtrl},
            new[] {Key.LeftCtrl, Key.LeftAlt},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.A})]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1, Key.A},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1})]

        public void Shortcut_should_not_be_pressed(Key[] beforePreviousState, Key[] previousState, Key[] currentState)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                previousState.ToImmutableArray(),
                currentState.ToImmutableArray());

            var shortcut = new RangeShortcut(new[] {Key.LeftCtrl, Key.LeftAlt,}, KeyRange.Digits());

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.None);
            shortcut.LastKeyInRange.Should().BeNull();
        }

        [TestCase(
            new[] {Key.LeftCtrl},
            new[] {Key.LeftCtrl, Key.LeftAlt},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D0},
            Key.D0)]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D0},
            new[] {Key.LeftCtrl, Key.LeftAlt},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D9},
            Key.D9)]

        public void Shortcut_should_be_pressed(Key[] beforePreviousState, Key[] previousState, Key[] currentState,
            Key expectedLastKey)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                previousState.ToImmutableArray(),
                currentState.ToImmutableArray());

            var shortcut = new RangeShortcut(new[] {Key.LeftCtrl, Key.LeftAlt,}, KeyRange.Digits());

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.Pressed);
            shortcut.LastKeyInRange.Should().Be(expectedLastKey);
        }

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1, Key.A})]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1, Key.A},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.D1},
            new[] {Key.LeftCtrl, Key.LeftAlt})]

        public void Shortcut_should_not_be_released(Key[] beforePreviousState, Key[] previousState, Key[] currentState)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                previousState.ToImmutableArray(),
                currentState.ToImmutableArray());

            var shortcut = new RangeShortcut(new[] {Key.LeftCtrl, Key.LeftAlt,}, KeyRange.Digits());

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.None);
            shortcut.LastKeyInRange.Should().Be(null);
        }

        [Test]
        public void Shortcut_should_be_released()
        {
            var keyboardState = new KeyboardState(
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt, Key.D0),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt));

            var shortcut = new RangeShortcut(new[] { Key.LeftCtrl, Key.LeftAlt, }, KeyRange.Digits());

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.Released);
            shortcut.LastKeyInRange.Should().Be(Key.D0);
        }
    }
}
