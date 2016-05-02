using System;
using System.Windows.Input;
using FluentAssertions;
using KeyboardShortcutDetector.Tests.Fixture;
using NUnit.Framework;

namespace KeyboardShortcutDetector.Tests
{
    internal class ShortcutDetextorFixture : ITestFixture
    {
        public KeyboardMock Keyboard { get; private set; }
        public IKeyboardShortuctDetector ShortuctDetector { get; private set; }

        public void SetUp()
        {
            Keyboard = new KeyboardMock();
            ShortuctDetector = new ShortcutDetector(Keyboard);
        }

        public void TearDown()
        {
        }

        public void Press(Key key)
        {
            Keyboard.PressKey(key);
        }

        public void Release(Key key)
        {
            Keyboard.ReleaseKey(key);
        }
    }


    [TestFixture]
    internal class KeyboardShortcutDetectorTests : TestBase<ShortcutDetextorFixture>
    {
        [Test]
        public void Shortcut_pressed_event_should_be_triggered()
        {
            //Arrange
            var shortcutObserver = new ShortcutObserverMock();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());
            Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.LeftAlt);
            Fixture.Press(Key.Delete);

            //Assert
            shortcutObserver.PressedCounter.Should().Be(1);
            shortcutObserver.ReleasedCounter.Should().Be(0);
        }
        
        [Test]
        public void Shortcut_pressed_and_released_event_should_be_triggered_many_times()
        {
            //Arrange
            var shortcutObserver = new ShortcutObserverMock();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());
            Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.LeftAlt);
            Fixture.Press(Key.Delete);

            Fixture.Release(Key.Delete);
            Fixture.Press(Key.Delete);

            Fixture.Release(Key.Delete);
            Fixture.Press(Key.Delete);

            //Assert
            shortcutObserver.PressedCounter.Should().Be(3);
            shortcutObserver.ReleasedCounter.Should().Be(2);
        }

        [Test]
        public void Range_shortcut_pressed_and_released_events_should_be_triggered()
        {
            //Arrange
            var shortcutObserver = new RangeShortcutObserver();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDigit());
            Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.LeftAlt);
            Fixture.Press(Key.D0);
            
            Fixture.Release(Key.D0);
            Fixture.Press(Key.D1);
            
            Fixture.Release(Key.D1);
            Fixture.Press(Key.D2);
            
            //Assert
            shortcutObserver.LastPressedKey[0].Should().Be(Key.D0);
            shortcutObserver.LastReleasedKey[0].Should().Be(Key.D0);
            shortcutObserver.LastPressedKey[1].Should().Be(Key.D1);
            shortcutObserver.LastReleasedKey[1].Should().Be(Key.D1);
            shortcutObserver.LastPressedKey[2].Should().Be(Key.D2);

            shortcutObserver.PressedCounter.Should().Be(3);
            shortcutObserver.ReleasedCounter.Should().Be(2);
        }

        [Test]
        public void Shortcut_pressed_event_should_not_be_triggered_due_to_wrong_pressed_keys_order()
        {
            //Arrange
            var shortcutObserver = new ShortcutObserverMock();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());
            Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.Delete);
            Fixture.Press(Key.LeftAlt);

            //Assert
            shortcutObserver.PressedCounter.Should().Be(0);
            shortcutObserver.ReleasedCounter.Should().Be(0);
        }

        [Test]
        public void Many_observers_should_be_notify_about_pressed_and_released_events()
        {
            //Arrange
            var firstShortcutObserver = new ShortcutObserverMock();
            var secondShortcutObserver = new ShortcutObserverMock();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());
            Fixture.ShortuctDetector.Subscribe(firstShortcutObserver);
            Fixture.ShortuctDetector.Subscribe(secondShortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.LeftAlt);
            Fixture.Press(Key.Delete);

            Fixture.Release(Key.Delete);
            
            //Assert
            firstShortcutObserver.PressedCounter.Should().Be(1);
            firstShortcutObserver.ReleasedCounter.Should().Be(1);

            secondShortcutObserver.PressedCounter.Should().Be(1);
            secondShortcutObserver.ReleasedCounter.Should().Be(1);
        }

        [Test]
        public void Multiple_shortcuts_observer_should_be_notified_about_different_events()
        {
            //Arrange
            var shortcutObserver = new MultipleShortcutsObserver();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());
             Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDigit());
            Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            //Act
            Fixture.Press(Key.LeftCtrl);
            Fixture.Press(Key.LeftAlt);
            Fixture.Press(Key.Delete);

            Fixture.Release(Key.Delete);
            Fixture.Press(Key.D7);

            Fixture.Release(Key.D7);

            //Assert
            shortcutObserver.CtrlAltDelPressedCounter.Should().Be(1);
            shortcutObserver.CtrlAltDelReleasedCounter.Should().Be(1);

            shortcutObserver.CtrlAltDigitPressedCounter.Should().Be(1);
            shortcutObserver.CtrlAltDigitReleasedCounter.Should().Be(1);
            shortcutObserver.LastPressedDigitKey[0].Should().Be(Key.D7);
            shortcutObserver.LastReleasedDigitKey[0].Should().Be(Key.D7);
        }

        [Test]
        public void Should_throw_on_subscribing_observer_without_proper_interfaces()
        {
            //Arrange
            var notAObserver = new NotAShortcutObserver();

            // Act & Assert
            Action subscription = () => Fixture.ShortuctDetector.Subscribe(notAObserver);

            subscription.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Should_throw_on_subscribing_observer_when_shortcut_is_not_registered()
        {
            //Arrange
            var shortcutObserver = new MultipleShortcutsObserver();

            Fixture.ShortuctDetector.RegisterShortcut(new CtrlAltDel());

            //Act & Assert
            Action subscription = () => Fixture.ShortuctDetector.Subscribe(shortcutObserver);

            subscription.ShouldThrow<Exception>();
        }
    }
}
