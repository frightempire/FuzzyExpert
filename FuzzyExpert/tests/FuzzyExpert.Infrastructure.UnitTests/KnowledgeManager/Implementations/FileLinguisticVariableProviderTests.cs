using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class FileLinguisticVariableProviderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "KnowledgeManager\\TestFiles\\TestFile.txt");

        private ILinguisticVariableValidator _linguisticVariableValidatorMock;
        private ILinguisticVariableParser _linguisticVariableParserMock;
        private ILinguisticVariableCreator _linguisticVariableCreatorMock;
        private ILinguisticVariableFilePathProvider _filePathProviderMock;
        private IFileOperations _fileOperationsMock;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;
        private FileLinguisticVariableProvider _fileLinguisticVariableProvider;

        [SetUp]
        public void SetUp()
        {
            _linguisticVariableValidatorMock = MockRepository.GenerateMock<ILinguisticVariableValidator>();
            _linguisticVariableParserMock = MockRepository.GenerateMock<ILinguisticVariableParser>();
            _linguisticVariableCreatorMock = MockRepository.GenerateMock<ILinguisticVariableCreator>();
            _filePathProviderMock = MockRepository.GenerateMock<ILinguisticVariableFilePathProvider>();
            _filePathProviderMock.Stub(x => x.FilePath).PropertyBehavior();
            _fileOperationsMock = MockRepository.GenerateMock<IFileOperations>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();
            _fileLinguisticVariableProvider = new FileLinguisticVariableProvider(
                _linguisticVariableValidatorMock,
                _linguisticVariableCreatorMock,
                _filePathProviderMock,
                _fileOperationsMock,
                _validationOperationResultLoggerMock);
        }

        [Test]
        public void Constructor_ExpectedBehavior()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileLinguisticVariableProvider(
                    null,
                    _linguisticVariableCreatorMock,
                    _filePathProviderMock,
                    _fileOperationsMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileLinguisticVariableProvider(
                    _linguisticVariableValidatorMock,
                    null,
                    _filePathProviderMock,
                    _fileOperationsMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileLinguisticVariableProvider(
                    _linguisticVariableValidatorMock,
                    _linguisticVariableCreatorMock,
                    null,
                    _fileOperationsMock,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileLinguisticVariableProvider(
                    _linguisticVariableValidatorMock,
                    _linguisticVariableCreatorMock,
                    _filePathProviderMock,
                    null,
                    _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new FileLinguisticVariableProvider(
                    _linguisticVariableValidatorMock,
                    _linguisticVariableCreatorMock,
                    _filePathProviderMock,
                    _fileOperationsMock,
                    null);
            });
        }

        [Test]
        public void GetLinguisticVariables_ThrowsFileNotFoundExceptionIfFilePathIsNull()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileLinguisticVariableProvider.GetLinguisticVariables());
        }

        [Test]
        public void GetLinguisticVariables_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Arrange
            _filePathProviderMock.FilePath = "";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileLinguisticVariableProvider.GetLinguisticVariables());
        }

        [Test]
        public void GetLinguisticVariables_ThrowsFileNotFoundExceptionIfFileDoesNotExists()
        {
            // Arrange
            _filePathProviderMock.FilePath = "NotExistingFile.txt";

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => _fileLinguisticVariableProvider.GetLinguisticVariables());
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional()
        {
            // Arrange
            _filePathProviderMock.FilePath = _filePath;
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(new List<string>());

            // Act
            Optional<List<LinguisticVariable>> linguisticVariables = _fileLinguisticVariableProvider.GetLinguisticVariables();

            // Assert
            Assert.IsFalse(linguisticVariables.IsPresent);
        }

        [Test]
        public void GetLinguisticVariables_ReturnsCorrectListOfRules()
        {
            // Arrange
            _linguisticVariableValidatorMock
                .Stub(x => x.ValidateLinguisticVariable(Arg<string>.Is.Anything))
                .Return(new ValidationOperationResult());

            _filePathProviderMock.FilePath = _filePath;

            string firstLinguisticVariableStringFromFile =
                "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string secondLinguisticVariableStringFromFile =
                "Pressure:Derivative:[Low:Trapezoidal:(20,50,50,60)|High:Trapezoidal:(80,100,100,150)]";
            List<string> linguisticVariablesFromFile = new List<string>
            {
                firstLinguisticVariableStringFromFile, secondLinguisticVariableStringFromFile
            };
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(linguisticVariablesFromFile);

            // Water variable
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable = new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData:true);

            List<MembershipFunctionStrings> firstMembershipFunctionStrings = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<double> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<double> {50, 60, 60, 80})
            };
            LinguisticVariableStrings firstLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", firstMembershipFunctionStrings);

            // Pressure variable
            MembershipFunctionList secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable = new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData:false);

            List<MembershipFunctionStrings> secondMembershipFunctionStrings = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Low", "Trapezoidal", new List<double> {20, 50, 50, 60}),
                new MembershipFunctionStrings("High", "Trapezoidal", new List<double> {80, 100, 100, 150})
            };
            LinguisticVariableStrings secondLinguisticVariableStrings = new LinguisticVariableStrings("Pressure", "Derivative", secondMembershipFunctionStrings);

            List<LinguisticVariable> expectedLinguisticVariables = new List<LinguisticVariable>
            {
                firstLinguisticVariable, secondLinguisticVariable
            };
            Optional<List<LinguisticVariable>> expectedOptional = Optional<List<LinguisticVariable>>.For(expectedLinguisticVariables);

            // Stubs
            _linguisticVariableParserMock
                .Stub(x => x.ParseLinguisticVariable(firstLinguisticVariableStringFromFile))
                .Return(firstLinguisticVariableStrings);
            _linguisticVariableParserMock
                .Stub(x => x.ParseLinguisticVariable(secondLinguisticVariableStringFromFile))
                .Return(secondLinguisticVariableStrings);

            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntity(firstLinguisticVariableStringFromFile))
                .Return(firstLinguisticVariable);
            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntity(secondLinguisticVariableStringFromFile))
                .Return(secondLinguisticVariable);

            // Act
            Optional<List<LinguisticVariable>> actualOptional = _fileLinguisticVariableProvider.GetLinguisticVariables();

            // Assert
            Assert.IsTrue(expectedOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
        }

        [Test]
        public void GetLinguisticVariables_ReturnsOneRuleLessIfItIsNotValid()
        {
            // Arrange
            _filePathProviderMock.FilePath = _filePath;

            string firstLinguisticVariableStringFromFile =
                "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string secondLinguisticVariableStringFromFile =
                "Pressure:Derivative:[Low:Trapezoidal:(20,50,50,60)|High:Trapezoidal:(80,100,100,150)]";
            string thirdLinguisticVariableStringFromFile =
                "EverythingIsWrongWithThirdLinguisticVariable";

            _linguisticVariableValidatorMock
                .Stub(x => x.ValidateLinguisticVariable(firstLinguisticVariableStringFromFile))
                .Return(new ValidationOperationResult());
            _linguisticVariableValidatorMock
                .Stub(x => x.ValidateLinguisticVariable(secondLinguisticVariableStringFromFile))
                .Return(new ValidationOperationResult());
            ValidationOperationResult validationOperationResultForThirdVariable = new ValidationOperationResult();
            validationOperationResultForThirdVariable.AddMessage("Something is wrong with third linguistic variable");
            _linguisticVariableValidatorMock
                .Stub(x => x.ValidateLinguisticVariable(thirdLinguisticVariableStringFromFile))
                .Return(validationOperationResultForThirdVariable);

            List<string> linguisticVariablesFromFile = new List<string>
            {
                firstLinguisticVariableStringFromFile, secondLinguisticVariableStringFromFile, thirdLinguisticVariableStringFromFile
            };
            _fileOperationsMock.Stub(x => x.ReadFileByLines(Arg<string>.Is.Anything)).IgnoreArguments().Return(linguisticVariablesFromFile);

            // Water variable
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable = new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData: true);

            List<MembershipFunctionStrings> firstMembershipFunctionStrings = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<double> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<double> {50, 60, 60, 80})
            };
            LinguisticVariableStrings firstLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", firstMembershipFunctionStrings);

            // Pressure variable
            MembershipFunctionList secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable = new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData: false);

            List<MembershipFunctionStrings> secondMembershipFunctionStrings = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Low", "Trapezoidal", new List<double> {20, 50, 50, 60}),
                new MembershipFunctionStrings("High", "Trapezoidal", new List<double> {80, 100, 100, 150})
            };
            LinguisticVariableStrings secondLinguisticVariableStrings = new LinguisticVariableStrings("Pressure", "Derivative", secondMembershipFunctionStrings);

            List<LinguisticVariable> expectedLinguisticVariables = new List<LinguisticVariable>
            {
                firstLinguisticVariable, secondLinguisticVariable
            };
            Optional<List<LinguisticVariable>> expectedOptional = Optional<List<LinguisticVariable>>.For(expectedLinguisticVariables);

            // Stubs
            _linguisticVariableParserMock
                .Stub(x => x.ParseLinguisticVariable(firstLinguisticVariableStringFromFile))
                .Return(firstLinguisticVariableStrings);
            _linguisticVariableParserMock
                .Stub(x => x.ParseLinguisticVariable(secondLinguisticVariableStringFromFile))
                .Return(secondLinguisticVariableStrings);

            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntity(firstLinguisticVariableStringFromFile))
                .Return(firstLinguisticVariable);
            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntity(secondLinguisticVariableStringFromFile))
                .Return(secondLinguisticVariable);

            // Act
            Optional<List<LinguisticVariable>> actualOptional = _fileLinguisticVariableProvider.GetLinguisticVariables();

            // Assert
            Assert.IsTrue(expectedOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
            _validationOperationResultLoggerMock.AssertWasCalled(x => x.LogValidationOperationResultMessages(validationOperationResultForThirdVariable, 3));
        }
    }
}