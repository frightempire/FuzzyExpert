using System.Collections.Generic;
using InferenceExpert.Entities;
using NUnit.Framework;

namespace InferenceExpert.UnitTests.Entities
{
    [TestFixture]
    public class ExpertOpinionTests
    {
        private ExpertOpinion _expertOpinion;

        [SetUp]
        public void SetUp()
        {
            _expertOpinion = new ExpertOpinion();
        }

        [Test]
        public void Constructor_SetsDefaultValues()
        {
            // Assert
            Assert.IsTrue(_expertOpinion.IsSuccess);
            Assert.AreEqual(0, _expertOpinion.ErrorMessages.Count);
            Assert.AreEqual(0, _expertOpinion.Result.Count);
        }

        [Test]
        public void AddErrorMessage_AddsMessageToErrorMessagesList()
        {
            // Arrange
            string errorMessage = "Oh no";

            // Act
            _expertOpinion.AddErrorMessage(errorMessage);

            // Assert
            Assert.IsFalse(_expertOpinion.IsSuccess);
            Assert.AreEqual(1, _expertOpinion.ErrorMessages.Count);
            Assert.AreEqual(errorMessage, _expertOpinion.ErrorMessages[0]);
        }

        [Test]
        public void AddErrorMessages_AddsMessagesToErrorMessagesList()
        {
            // Arrange
            string firstErrorMessage = "Oh no";
            string secondErrorMessage = "He's coming back";
            List<string> errorMessages = new List<string> {firstErrorMessage, secondErrorMessage};

            // Act
            _expertOpinion.AddErrorMessages(errorMessages);

            // Assert
            Assert.IsFalse(_expertOpinion.IsSuccess);
            Assert.AreEqual(2, _expertOpinion.ErrorMessages.Count);
            Assert.AreEqual(firstErrorMessage, _expertOpinion.ErrorMessages[0]);
            Assert.AreEqual(secondErrorMessage, _expertOpinion.ErrorMessages[1]);
        }

        [Test]
        public void AddResult_AddsResultToResultDictionary()
        {
            // Arrange
            double expectedValue = 50;
            string expectedKey = "A1";
            var result = new KeyValuePair<string, double>(expectedKey, expectedValue);

            // Act
            _expertOpinion.AddResult(result);

            // Assert
            Assert.IsTrue(_expertOpinion.IsSuccess);
            Assert.AreEqual(1, _expertOpinion.Result.Count);
            Assert.AreEqual(expectedValue, _expertOpinion.Result[expectedKey]);
        }

        [Test]
        public void AddResult_AddMessagesToErrorMessagesList()
        {
            // Arrange
            double firstExpectedValue = 50;
            string firstExpectedKey = "A1";
            double secondExpectedValue = 100;
            string secondExpectedKey = "B1";
            var results = new Dictionary<string, double>
            {
                {firstExpectedKey, firstExpectedValue},
                {secondExpectedKey, secondExpectedValue}
            };

            // Act
            _expertOpinion.AddResults(results);

            // Assert
            Assert.IsTrue(_expertOpinion.IsSuccess);
            Assert.AreEqual(2, _expertOpinion.Result.Count);
            Assert.AreEqual(firstExpectedValue, _expertOpinion.Result[firstExpectedKey]);
            Assert.AreEqual(secondExpectedValue, _expertOpinion.Result[secondExpectedKey]);
        }
    }
}