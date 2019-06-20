using System.Collections.Generic;
using CommonLogic.Entities;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Entities
{
    [TestFixture]
    public class ValidationOperationResultTests
    {
        private ValidationOperationResult _validationOperationResult;

        [SetUp]
        public void SetUp()
        {
            _validationOperationResult = new ValidationOperationResult();
        }

        [Test]
        public void Constructor_InitiatesEmptyMessagesList()
        {
            // Assert
            Assert.AreEqual(0, _validationOperationResult.Messages.Count);
        }

        [Test]
        public void Constructor_InitiatesIsSuccessTrue()
        {
            // Assert
            Assert.AreEqual(true, _validationOperationResult.IsSuccess);
        }

        [Test]
        public void AddMessage_AddsMessageToMessageListAndSwitchesIsSuccessValue()
        {
            // Arrange
            string messageToAdd = "test message";
            List<string> expectedMessageList = new List<string> {messageToAdd};

            // Act
            _validationOperationResult.AddMessage(messageToAdd);

            // Assert
            Assert.AreEqual(expectedMessageList, _validationOperationResult.Messages);
            Assert.AreEqual(false, _validationOperationResult.IsSuccess);
        }

        [Test]
        public void AddMessages_AddsMessagesToMessageListAndSwitchesIsSuccessValue()
        {
            // Arrange
            string firstMessageToAdd = "test message 1";
            string secondMessageToAdd = "test message 2";
            List<string> expectedMessageList = new List<string> { firstMessageToAdd, secondMessageToAdd };

            // Act
            _validationOperationResult.AddMessages(expectedMessageList);

            // Assert
            Assert.AreEqual(expectedMessageList, _validationOperationResult.Messages);
            Assert.AreEqual(false, _validationOperationResult.IsSuccess);
        }

        [Test]
        public void GetMessages_ReturnsMessagesList()
        {
            // Arrange
            string firstMessageToAdd = "test message 1";
            string secondMessageToAdd = "test message 2";
            List<string> expectedMessageList = new List<string> {firstMessageToAdd, secondMessageToAdd};
            _validationOperationResult.AddMessage(firstMessageToAdd);
            _validationOperationResult.AddMessage(secondMessageToAdd);

            // Act
            List<string> actualMessageList = _validationOperationResult.Messages;

            // Assert
            Assert.AreEqual(expectedMessageList, actualMessageList);
        }
    }
}
