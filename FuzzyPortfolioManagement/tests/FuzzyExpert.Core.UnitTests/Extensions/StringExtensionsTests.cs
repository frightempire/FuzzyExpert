using System.Collections.Generic;
using FuzzyExpert.Core.Extensions;
using NUnit.Framework;

namespace FuzzyExpert.Core.UnitTests.Extensions
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