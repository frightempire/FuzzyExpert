using System;
using System.Collections.Generic;
using FuzzyExpert.Application.InferenceExpert.Entities;
using NUnit.Framework;

namespace FuzzyExpert.Application.UnitTests.InferenceExpert.Entities
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
        public void AddResult_AddMessagesToErrorMessagesList()
        {
            // Arrange
            double firstExpectedValue = 50;
            string firstExpectedKey = "A1";
            double secondExpectedValue = 100;
            string secondExpectedKey = "B1";
            var results = new List<Tuple<string, double>>
            {
                new Tuple<string, double>(firstExpectedKey, firstExpectedValue),
                new Tuple<string, double>(secondExpectedKey, secondExpectedValue)
            };

            // Act
            _expertOpinion.AddResults(results);

            // Assert
            Assert.IsTrue(_expertOpinion.IsSuccess);
            Assert.AreEqual(2, _expertOpinion.Result.Count);
            Assert.AreEqual(firstExpectedValue, _expertOpinion.Result[0].Item2);
            Assert.AreEqual(secondExpectedValue, _expertOpinion.Result[1].Item2);
        }
    }
}