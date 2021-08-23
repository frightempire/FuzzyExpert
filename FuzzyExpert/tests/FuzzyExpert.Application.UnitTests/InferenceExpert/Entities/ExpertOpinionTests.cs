using System;
using System.Collections.Generic;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Core.Entities;
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
            double firstExpectedConfidenceFactor = 50;
            string firstExpectedNodeName = "A1";
            double firstExpectedDeFuzzifiedValue = 2;
            double secondExpectedConfidenceFactor = 100;
            string secondExpectedNodeName = "B1";
            double secondExpectedDeFuzzifiedValue = 3;
            var results = new List<DeFuzzifiedInferenceResult>
            {
                new DeFuzzifiedInferenceResult(firstExpectedNodeName, firstExpectedConfidenceFactor, firstExpectedDeFuzzifiedValue),
                new DeFuzzifiedInferenceResult(secondExpectedNodeName, secondExpectedConfidenceFactor, secondExpectedDeFuzzifiedValue)
            };

            // Act
            _expertOpinion.AddResults(results);

            // Assert
            Assert.IsTrue(_expertOpinion.IsSuccess);
            Assert.AreEqual(2, _expertOpinion.Result.Count);
            Assert.AreEqual(firstExpectedConfidenceFactor, _expertOpinion.Result[0].ConfidenceFactor);
            Assert.AreEqual(secondExpectedConfidenceFactor, _expertOpinion.Result[1].ConfidenceFactor);
        }
    }
}