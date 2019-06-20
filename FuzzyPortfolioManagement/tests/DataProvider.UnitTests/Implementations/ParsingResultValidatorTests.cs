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
                new []{ "init1", "1" },
                new []{ faltedValue }
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
                new []{ "init1", "1" },
                new []{ faltedValue, "1", "not_needed" }
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
                new []{ "init1", "1" },
                new []{ faltedValue, "not_numerical", }
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