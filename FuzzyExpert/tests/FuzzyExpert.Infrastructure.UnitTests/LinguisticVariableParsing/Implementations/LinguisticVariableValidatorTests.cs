using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.LinguisticVariableParsing.Implementations
{
    [TestFixture]
    public class LinguisticVariableValidatorTests
    {
        private LinguisticVariableValidator _linguisticVariableValidator;

        [SetUp]
        public void SetUp()
        {
            _linguisticVariableValidator = new LinguisticVariableValidator();
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfThereAreWhitespacesInIt()
        {
            // Arrange
            var linguisticVariable = "Water : Initial : [Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);
            
            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfThereIsNotEnoughBrackets()
        {
            // Arrange
            var linguisticVariable = "[Water]:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfOpeningBracketIsIncorrect()
        {
            // Arrange
            var linguisticVariable = "]Water]:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfClosingBracketIsIncorrect()
        {
            // Arrange
            var linguisticVariable = "[Water]:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)[";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfFirstColumnIsBeforeClosingBracket()
        {
            // Arrange
            var linguisticVariable = "[Water:]Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsValidationResultWithError_IfFirstColumnIsAfterOpeningBracket()
        {
            // Arrange
            var linguisticVariable = "[Water]:Initial[:ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
        }

        [Test]
        public void ValidateLinguisticVariables_ReturnsSuccessValidationResult()
        {
            // Arrange
            var linguisticVariable = "[Water,Iron]:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";

            // Act
            var validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariables(linguisticVariable);

            // Assert
            Assert.IsTrue(validationOperationResult.Successful);
        }
    }
}