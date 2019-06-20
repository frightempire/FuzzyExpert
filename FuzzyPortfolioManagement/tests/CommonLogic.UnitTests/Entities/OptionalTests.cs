using Base.UnitTests.TestEntitites;
using CommonLogic.Entities;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Entities
{
    [TestFixture]
    public class OptionalTests
    {
        [Test]
        public void Empty_CreateOptionalWithoutValue()
        {
            // Arrange
            Optional<TestClass> testClass = Optional<TestClass>.Empty();

            // Assert
            Assert.IsFalse(testClass.IsPresent);
        }

        [Test]
        public void For_CreateOptionalWithValue()
        {
            // Arrange
            TestClass expectedTest = new TestClass {IntField = 1, StringField = "wow"};
            Optional<TestClass> testClass = Optional<TestClass>.For(expectedTest);

            // Assert
            Assert.IsTrue(testClass.IsPresent);
            Assert.AreEqual(expectedTest, testClass.Value);
        }
    }
}