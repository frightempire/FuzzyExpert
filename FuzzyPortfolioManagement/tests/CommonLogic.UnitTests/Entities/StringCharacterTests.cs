using CommonLogic.Entities;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Entities
{
    [TestFixture]
    public class StringCharacterTests
    {
        private StringCharacter _stringCharacter;

        [SetUp]
        public void SetUp()
        {
            _stringCharacter = new StringCharacter('S', 3);
        }

        [Test]
        public void Symbol_GetterReturnsValue()
        {
            // Arrange
            char expectedSymbol = 'S';

            // Act
            char actualSymbol = _stringCharacter.Symbol;

            // Assert
            Assert.AreEqual(expectedSymbol, actualSymbol);
        }

        [Test]
        public void Position_GetterReturnsValue()
        {
            // Arrange
            int expectedPosition = 3;

            // Act
            int actualPosition = _stringCharacter.Position;

            // Assert
            Assert.AreEqual(expectedPosition, actualPosition);
        }
    }
}