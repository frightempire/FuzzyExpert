using System;
using System.Collections.Generic;
using System.IO;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using KnowledgeManager.Implementations;
using KnowledgeManager.Interfaces;
using NUnit.Framework;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;
using ProductionRuleParser.Interfaces;
using Rhino.Mocks;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class FileImplicationRuleProviderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\TestFile.txt");

        private IFileOperations _fileOperationsMock;
        private IFilePathProvider _filePathProviderMock;
        private IImplicationRuleValidator _implicationRuleValidatorMock;
        private IImplicationRuleParser _implicationRuleParserMock;
        private IImplicationRuleCreator _implicationRuleCreatorMock;
        private INameSupervisor _nameSupervisorMock;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;

        private FileImplicationRuleProvider _fileImplicationRuleProvider;

        [SetUp]
        public void SetUp()
        {
            _fileOperationsMock = MockRepository.GenerateMock<IFileOperations>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();

            _filePathProviderMock = MockRepository.GenerateMock<IFilePathProvider>();
            _filePathProviderMock.Stub(x => x.FilePath).PropertyBehavior();

            _implicationRuleValidatorMock = MockRepository.GenerateMock<IImplicationRuleValidator>();
            _implicationRuleParserMock = MockRepository.GenerateMock<IImplicationRuleParser>();
            _implicationRuleCreatorMock = MockRepository.GenerateMock<IImplicationRuleCreator>();

            _nameSupervisorMock = MockRepository.GenerateMock<INameSupervisor>();

            _fileImplicationRuleProvider = new FileImplicationRuleProvider(
                _fileOperationsMock,
                _filePathProviderMock,
                _implicationRuleValidatorMock,
                _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
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
                    _implicationRuleParserMock,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock,
                    null);
            });
        }

        [Test]
        public void GetImplicationRules_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _fileImplicationRuleProvider.GetImplicationRules());
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

            List<string> implicationRulesFromFile = new List<string>
            {
                "IF(A>10)THEN(X=5)",
                "IF(B!=1&C!=2)THEN(X=10)"
            };
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(implicationRulesFromFile);

            // IF (A > 10) THEN (X = 5)
            ImplicationRuleStrings firstImplicationRuleStrings = new ImplicationRuleStrings("A>10", "X=5");
            _implicationRuleParserMock.Stub(x => x.ExtractStatementParts(implicationRulesFromFile[0]))
                .Return(firstImplicationRuleStrings);
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
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstImplicationRuleStrings))
                .Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            ImplicationRuleStrings secondImplicationRuleStrings = new ImplicationRuleStrings("B!=1&C!=2", "X=10");
            _implicationRuleParserMock.Stub(x => x.ExtractStatementParts(implicationRulesFromFile[1]))
                .Return(secondImplicationRuleStrings);
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
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondImplicationRuleStrings))
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
            ImplicationRuleStrings firstImplicationRuleStrings = new ImplicationRuleStrings("A>10", "X=5");
            _implicationRuleParserMock.Stub(x => x.ExtractStatementParts(implicationRulesFromFile[0]))
                .Return(firstImplicationRuleStrings);
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
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstImplicationRuleStrings))
                .Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            ImplicationRuleStrings secondImplicationRuleStrings = new ImplicationRuleStrings("B!=1&C!=2", "X=10");
            _implicationRuleParserMock.Stub(x => x.ExtractStatementParts(implicationRulesFromFile[1]))
                .Return(secondImplicationRuleStrings);
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
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondImplicationRuleStrings))
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
