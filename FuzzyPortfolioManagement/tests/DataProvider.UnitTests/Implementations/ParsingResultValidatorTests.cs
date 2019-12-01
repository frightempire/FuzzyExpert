using System.Collections.Generic;
using CommonLogic.Entities;
using DataProvider.Implementations;
using NUnit.Framework;

namespace DataProvider.UnitTests.Implementations
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
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Messages.Count, 1);
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesDoesntHaveEnoughInfo()
        {
            // Arrange
            string faltedValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ faltedValue, "0.1" },
                new []{ "init3", "2", "0.1" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(faltedValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesHaveTooMuchInfo()
        {
            // Arrange
            string faltedValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ faltedValue, "1", "0.1", "not_needed" },
                new []{ "init3", "2", "0.1" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(faltedValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfOneOfValuesCouldntBeParsed()
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
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(faltedValue));
        }

        [Test]
        public void Validate_ReturnsFalse_IfThirdValueIsNotInRange()
        {
            // Arrange
            string faltedValue = "init2";
            List<string[]> input = new List<string[]>
            {
                new []{ "init1", "1", "0.1" },
                new []{ faltedValue, "2", "10" }
            };

            // Act
            ValidationOperationResult result = _validator.Validate(input);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(result.Messages.Count, 1);
            Assert.IsTrue(result.Messages[0].Contains(faltedValue));
        }
    }
}