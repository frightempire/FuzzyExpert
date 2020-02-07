using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class FileImplicationRuleProviderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "KnowledgeManager\\TestFiles\\TestFile.txt");

        private IFileOperations _fileOperationsMock;
        private IImplicationRuleFilePathProvider _filePathProviderMock;
        private IImplicationRuleValidator _implicationRuleValidatorMock;
        private IImplicationRuleCreator _implicationRuleCreatorMock;
        private INameSupervisor _nameSupervisorMock;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;

        private FileImplicationRuleProvider _fileImplicationRuleProvider;

        [SetUp]
        public void SetUp()
        {
            _fileOperationsMock = MockRepository.GenerateMock<IFileOperations>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();

            _filePathProviderMock = MockRepository.GenerateMock<IImplicationRuleFilePathProvider>();
            _filePathProviderMock.Stub(x => x.FilePath).PropertyBehavior();

            _implicationRuleValidatorMock = MockRepository.GenerateMock<IImplicationRuleValidator>();
            _implicationRuleCreatorMock = MockRepository.GenerateMock<IImplicationRuleCreator>();

            _nameSupervisorMock = MockRepository.GenerateMock<INameSupervisor>();

            _fileImplicationRuleProvider = new FileImplicationRuleProvider(
                _fileOperationsMock,
                _filePathProviderMock,
                _implicationRuleValidatorMock,
                _implicationRuleCreatorMock,
                _nameSupervisorMock,
                _validationOperationResultLoggerMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    null,
                    _filePathProviderMock,
                    _implicationRuleValidatorMock,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    _fileOperationsMock,
                    null,
                    _implicationRuleValidatorMock,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    _fileOperationsMock,
                    _filePathProviderMock,
                    null,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    _fileOperationsMock,
                    _filePathProviderMock,
                    _implicationRuleValidatorMock,
                    null,
                    _nameSupervisorMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    _fileOperationsMock,
                    _filePathProviderMock,
                    _implicationRuleValidatorMock,
                    _implicationRuleCreatorMock,
                    null,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileImplicationRuleProvider(
                    _fileOperationsMock,
                    _filePathProviderMock,
                    _implicationRuleValidatorMock,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock,
                    null);
            });
        }

        [Test]
        public void GetImplicationRules_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Arrange
            _filePathProviderMock.FilePath = "";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileImplicationRuleProvider.GetImplicationRules());
        }

        [Test]
        public void GetImplicationRules_ThrowsFileNotFoundExceptionIfFileDoesntExists()
        {
            // Arrange
            _filePathProviderMock.FilePath = "NotExistingFile.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileImplicationRuleProvider.GetImplicationRules());
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional()
        {
            // Arrange
            _filePathProviderMock.FilePath = _filePath;
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(new List<string>());

            // Act
            Optional<List<ImplicationRule>> expectedImplicationRules = _fileImplicationRuleProvider.GetImplicationRules();

            // Assert
            Assert.IsFalse(expectedImplicationRules.IsPresent);
        }

        [Test]
        public void GetImplicationRules_ReturnsCorrectListOfRules()
        {
            // Arrange
            _filePathProviderMock.FilePath = _filePath;
            _implicationRuleValidatorMock
                .Stub(x => x.ValidateImplicationRule(Arg<string>.Is.Anything))
                .Return(new ValidationOperationResult());

            string firstImplicationRuleString = "IF(A>10)THEN(X=5)";
            string secondImplicationRuleString = "IF(B!=1&C!=2)THEN(X=10)";
            List<string> implicationRulesFromFile = new List<string>
            {
                firstImplicationRuleString, secondImplicationRuleString
            };
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(implicationRulesFromFile);

            // IF (A > 10) THEN (X = 5)
            ImplicationRule firstImplicationRule = new ImplicationRule(
            new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("A", ComparisonOperation.Greater, "10")
                })
            },
            new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("X", ComparisonOperation.Equal, "5")
            }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstImplicationRuleString))
                .Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            ImplicationRule secondImplicationRule = new ImplicationRule(
            new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                    new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                })
            },
            new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("X", ComparisonOperation.Equal, "10")
            }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondImplicationRuleString))
                .Return(secondImplicationRule);

            List<ImplicationRule> expectedImplicationRules = new List<ImplicationRule>
            {
                firstImplicationRule, secondImplicationRule
            };
            Optional<List<ImplicationRule>> expectedOptional = Optional<List<ImplicationRule>>.For(expectedImplicationRules);

            // Act
            Optional<List<ImplicationRule>> actualOptional = _fileImplicationRuleProvider.GetImplicationRules();

            // Assert
            Assert.IsTrue(actualOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
        }

        [Test]
        public void GetImplicationRules_ReturnsOneRuleLessInListOfRulesIfItIsNotValid()
        {
            // Arrange
            _filePathProviderMock.FilePath = _filePath;

            string firstRuleFromFile = "IF(A>10)THEN(X=5)";
            _implicationRuleValidatorMock
                .Stub(x => x.ValidateImplicationRule(firstRuleFromFile))
                .Return(new ValidationOperationResult());
            string secondRuleFromFile = "IF(B!=1&C!=2)THEN(X=10)";
            _implicationRuleValidatorMock
                .Stub(x => x.ValidateImplicationRule(secondRuleFromFile))
                .Return(new ValidationOperationResult());
            string thirdRuleFromFile = "EverythingIsWrongWithThisRule";
            ValidationOperationResult validationOperationResultForThirdRule = new ValidationOperationResult();
            validationOperationResultForThirdRule.AddMessage("Something is wrong with this rule");
            _implicationRuleValidatorMock
                .Stub(x => x.ValidateImplicationRule(thirdRuleFromFile))
                .Return(validationOperationResultForThirdRule);
            List<string> implicationRulesFromFile = new List<string>
            {
                firstRuleFromFile, secondRuleFromFile, thirdRuleFromFile
            };
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(implicationRulesFromFile);

            // IF (A > 10) THEN (X = 5)
            ImplicationRule firstImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("A", ComparisonOperation.Greater, "10")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "5")
                }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstRuleFromFile))
                .Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            ImplicationRule secondImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                        new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "10")
                }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondRuleFromFile))
                .Return(secondImplicationRule);

            List<ImplicationRule> expectedImplicationRules = new List<ImplicationRule>
            {
                firstImplicationRule, secondImplicationRule
            };
            Optional<List<ImplicationRule>> expectedOptional = Optional<List<ImplicationRule>>.For(expectedImplicationRules);

            // Act
            Optional<List<ImplicationRule>> actualOptional = _fileImplicationRuleProvider.GetImplicationRules();

            // Assert
            Assert.IsTrue(actualOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
            _validationOperationResultLoggerMock.AssertWasCalled(x => x.LogValidationOperationResultMessages(validationOperationResultForThirdRule, 3));
        }
    }
}
