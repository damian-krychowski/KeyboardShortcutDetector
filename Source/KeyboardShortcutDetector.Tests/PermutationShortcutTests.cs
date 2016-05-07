using System.Collections.Immutable;
using System.Windows.Input;
using FluentAssertions;
using KeyboardShortcutDetector.Shortcuts;
using NUnit.Framework;

namespace KeyboardShortcutDetector.Tests
{
    [TestFixture]
    internal class PermutationShortcutTests
    {
        [TestCase(
            new Key[] {},
            new[] {Key.LeftCtrl},
            new[] {Key.LeftCtrl, Key.LeftAlt})]

        [TestCase(
            new[] {Key.RightCtrl},
            new[] {Key.RightCtrl, Key.LeftAlt},
            new[] {Key.RightCtrl, Key.LeftAlt, Key.C})]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C,},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C, Key.K},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C,})]

        public void Shortcut_should_not_be_pressed(Key[] beforePreviousState, Key[] previousState, Key[] currentState)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                previousState.ToImmutableArray(),
                currentState.ToImmutableArray());

            var shortcut = new PermutationShortcut(Key.LeftCtrl, Key.LeftAlt, Key.C);

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.None);
        }

        [TestCase(new[] {Key.LeftCtrl})]
        [TestCase(new[] {Key.LeftCtrl, Key.LeftAlt, Key.C})]
        public void Shortcut_should_be_pressed(Key[] beforePreviousState)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt, Key.C));

            var shortcut = new PermutationShortcut(Key.LeftCtrl, Key.LeftAlt, Key.C);

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.Pressed);
        }

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C}, 
            new[] {Key.LeftCtrl, Key.LeftAlt}, 
            new[] {Key.LeftCtrl})]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt}, 
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C},
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C, Key.K})]

        [TestCase(
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C, Key.K}, 
            new[] {Key.LeftCtrl, Key.LeftAlt, Key.C},
            new[] {Key.LeftCtrl, Key.LeftAlt})]

        [TestCase(
            new[] { Key.LeftCtrl, Key.LeftAlt,},
            new[] { Key.LeftCtrl, Key.LeftAlt, Key.C },
            new[] { Key.LeftCtrl, Key.C })]

        [TestCase(
            new[] { Key.LeftCtrl, Key.LeftAlt,},
            new[] { Key.LeftCtrl, Key.LeftAlt, Key.C },
            new[] { Key.LeftAlt, Key.C })]

        public void Shortcut_should_not_be_released(Key[] beforePreviousState, Key[] previousState, Key[] currentState)
        {
            var keyboardState = new KeyboardState(
                beforePreviousState.ToImmutableArray(),
                previousState.ToImmutableArray(),
                currentState.ToImmutableArray());

            var shortcut = new PermutationShortcut(Key.LeftCtrl, Key.LeftAlt, Key.C);

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.None);
        }

        [Test]
        public void Shortcut_should_be_released()
        {
            var keyboardState = new KeyboardState(
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt, Key.C),
                ImmutableArray.Create(Key.LeftCtrl, Key.LeftAlt));

            var shortcut = new PermutationShortcut(Key.LeftCtrl, Key.LeftAlt, Key.C);

            shortcut.KeyboardStateChanged(keyboardState).Should().Be(ShortcutStateChange.Released);
        }
    }
}
