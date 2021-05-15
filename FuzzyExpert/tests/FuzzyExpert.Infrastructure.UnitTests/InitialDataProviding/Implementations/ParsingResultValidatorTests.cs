using System.Collections.Generic;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.InitialDataProviding.Implementations
{
    [TestFixture]
    public class ParsingResultValidatorTests
    {
        private ParsingResultValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new ParsingResultValidator();
        }

        [Test]
        public void Validate_ReturnsFalse_IfFileIsEmpty()
        {
            // Arrange
            List<string[]> input = new List<string[]>();

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(result.Messages.Count, 1);
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesDoesNotHaveEnoughInfo()
        {
            // Arrange
            string wrongValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ wrongValue, "0.1" },
                new []{ "init3", "2", "0.1" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(wrongValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesHaveTooMuchInfo()
        {
            // Arrange
            string wrongValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ wrongValue, "1", "0.1", "not_needed" },
                new []{ "init3", "2", "0.1" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(wrongValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesCouldNotBeParsed()
        {
            // Arrange
            string faltedValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ faltedValue, "not_numerical", "0.1" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(faltedValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfThirdValueIsNotInRange()
        {
            // Arrange
            string wrongValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ wrongValue, "2", "10" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.Successful);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(wrongValue));
        }
    }
}