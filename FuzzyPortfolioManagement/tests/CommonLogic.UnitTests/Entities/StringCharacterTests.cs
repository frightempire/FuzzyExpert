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
        public void Symbol_SetterAndGetterWorksProperly()
        {
            // Arrange
            char expectedSymbol = 'S';

            // Assert
            Assert.AreEqual(expectedSymbol, _stringCharacter.Symbol);
        }

        [Test]
        public void Position_SetterAndGetterWorksProperly()
        {
            // Arrange
            int expectedPosition = 3;

            // Assert
            Assert.AreEqual(expectedPosition, _stringCharacter.Position);
        }
    }
}