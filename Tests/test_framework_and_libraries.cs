using NUnit.Framework;
using Should;

namespace Tests
{
    [TestFixture]
    class test_framework_and_libraries
    {
        [Test]
        public void should_always_work()
        {
            const string someValue = "stuff";
            Assert.AreEqual("stuff", someValue, "NUnit is working.");
        }

        [Test]
        public void should_have_should_working()
        {
            const bool trueYeah = true;
            trueYeah.ShouldBeTrue("Should is working.");
        }
    }
}
