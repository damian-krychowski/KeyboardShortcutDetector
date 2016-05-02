using NUnit.Framework;

namespace KeyboardShortcutDetector.Tests.Fixture
{
    internal abstract class TestBase<TFixture>
        where TFixture: ITestFixture, new()
    {
        public TFixture Fixture { get; private set; } = new TFixture();

        [SetUp]
        public void SetUp()
        {
            Fixture.SetUp();
        }

        [TearDown]
        public void TearDown()
        {
            Fixture.TearDown();
        }
    }
}