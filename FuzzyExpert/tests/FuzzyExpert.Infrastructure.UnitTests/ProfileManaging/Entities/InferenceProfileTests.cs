using System.Collections.Generic;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.ProfileManaging.Entities
{
    [TestFixture]
    public class InferenceProfileTests
    {
        private InferenceProfile _inferenceProfile;

        [SetUp]
        public void SetUp()
        {
            _inferenceProfile = new InferenceProfile();
        }

        [Test]
        public void ProfileName_PropertyExpectedBehavior()
        {
            // Arrange
            string expectedProfileName = "profile_name";

            // Act
            _inferenceProfile.ProfileName = expectedProfileName;

            // Assert
            Assert.AreEqual(expectedProfileName, _inferenceProfile.ProfileName);
        }

        [Test]
        public void Rules_PropertyExpectedBehavior()
        {
            // Arrange
            var expectedRules = new List<string>
            {
                "Rule_1", "Rule_2"
            };

            // Act
            _inferenceProfile.Rules = expectedRules;

            // Assert
            Assert.AreEqual(2, _inferenceProfile.Rules.Count);
            Assert.AreEqual(expectedRules[0], _inferenceProfile.Rules[0]);
            Assert.AreEqual(expectedRules[1], _inferenceProfile.Rules[1]);
        }

        [Test]
        public void Variables_PropertyExpectedBehavior()
        {
            // Arrange
            var expectedVariables = new List<string>
            {
                "Variable_1", "Variable_2"
            };

            // Act
            _inferenceProfile.Variables = expectedVariables;

            // Assert
            Assert.AreEqual(2, _inferenceProfile.Variables.Count);
            Assert.AreEqual(expectedVariables[0], _inferenceProfile.Variables[0]);
            Assert.AreEqual(expectedVariables[1], _inferenceProfile.Variables[1]);
        }

        [Test]
        public void Functions_PropertyExpectedBehavior()
        {
            // Arrange
            var expectedFunctions = new List<string>
            {
                "Function_1", "Function_2"
            };

            // Act
            _inferenceProfile.Functions = expectedFunctions;

            // Assert
            Assert.AreEqual(2, _inferenceProfile.Functions.Count);
            Assert.AreEqual(expectedFunctions[0], _inferenceProfile.Functions[0]);
            Assert.AreEqual(expectedFunctions[1], _inferenceProfile.Functions[1]);
        }
    }
}