using FuzzyExpert.Application.Common.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Application.UnitTests.Common.Implementations
{
    [TestFixture]
    public class UniqueNameProviderTests
    {
        private UniqueNameProvider _uniqueNameProvider;

        [SetUp]
        public void SetUp()
        {
            _uniqueNameProvider = new UniqueNameProvider();
        }

        [Test]
        public void GetName_FirstRun()
        {
            // Arrange
            string expectedName = "A1";

            // Act
            string actualName = _uniqueNameProvider.GetName();

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void GetName_ChangingNumber()
        {
            // Arrange
            string expectedName = "A2";

            // Act
            _uniqueNameProvider.GetName();
            string actualName = _uniqueNameProvider.GetName();

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void GetName_ChangingLetter()
        {
            // Arrange
            string expectedName = "B1";

            // Act
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            string actualName = _uniqueNameProvider.GetName();

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [Test]
        public void Reset_ChangingNumberAndLetterToDefaultValues()
        {
            // Arrange
            string expectedName = "A1";
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();
            _uniqueNameProvider.GetName();

            // Act
            _uniqueNameProvider.Reset();
            string actualName = _uniqueNameProvider.GetName();

            // Assert
            Assert.AreEqual(expectedName, actualName);
        }
    }
}