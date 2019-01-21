using System.Collections.Generic;
using CommonLogic.Extensions;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void RemoveUnwantedCharacters_RemovesOneCharacterType()
        {
            // Arrange
            List<char> charactersToRemove = new List<char> {' '};
            string stringToProcess = "Test -:- string";
            string expectedString = "Test-:-string";

            // Act
            string actualString = stringToProcess.RemoveUnwantedCharacters(charactersToRemove);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }

        [Test]
        public void RemoveUnwantedCharacters_RemovesTwoCharacterType()
        {
            // Arrange
            List<char> charactersToRemove = new List<char> { ' ', '-' };
            string stringToProcess = "Test -:- string";
            string expectedString = "Test:string";

            // Act
            string actualString = stringToProcess.RemoveUnwantedCharacters(charactersToRemove);

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }
    }
}