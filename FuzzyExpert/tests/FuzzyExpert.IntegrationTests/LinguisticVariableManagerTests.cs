using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.IntegrationTests
{
    [TestFixture]
    public class LinguisticVariableManagerTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\LinguisticVariables.txt");
        private LinguisticVariableFilePathProvider _filePathProvider;

        private LinguisticVariableManager _linguisticVariableManager;

        [SetUp]
        public void SetUp()
        {
            PrepareLinguisticVariableManager();
        }

        private void PrepareLinguisticVariableManager()
        {
            MembershipFunctionValidator membershipFunctionValidator = new MembershipFunctionValidator(); 
            LinguisticVariableValidator linguisticVariableValidator = new LinguisticVariableValidator(membershipFunctionValidator);
            MembershipFunctionParser membershipFunctionParser = new MembershipFunctionParser();
            LinguisticVariableParser linguisticVariableParser = new LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator);
            _filePathProvider = new LinguisticVariableFilePathProvider {FilePath = _filePath};
            FileOperations fileOperations = new FileOperations();
            FileValidationOperationResultLogger validationOperationResultLogger = new FileValidationOperationResultLogger(fileOperations);

            FileLinguisticVariableProvider linguisticVariableProvider = new FileLinguisticVariableProvider(
                linguisticVariableValidator,
                linguisticVariableParser,
                linguisticVariableCreator,
                _filePathProvider,
                fileOperations,
                validationOperationResultLogger);
            _linguisticVariableManager = new LinguisticVariableManager(linguisticVariableProvider);
        }

        [Test]
        public void LinguisticVariablesGetter_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Arrange
            _filePathProvider.FilePath = string.Empty;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => { var rules = _linguisticVariableManager.LinguisticVariables; });
        }

        [Test]
        public void LinguisticVariablesGetter_ReturnsLinguisticVariablesList()
        {
            // Arrange
            Optional<Dictionary<int, LinguisticVariable>> expectedLinguisticVariables =
                Optional<Dictionary<int, LinguisticVariable>>.For(PrepareExpectedLinguisticVariables());

            // Act
            Optional<Dictionary<int, LinguisticVariable>> actualLinguisticVariables = _linguisticVariableManager.LinguisticVariables;

            // Assert
            Assert.IsTrue(actualLinguisticVariables.IsPresent);
            Assert.AreEqual(expectedLinguisticVariables.Value.Count, actualLinguisticVariables.Value.Count);
            for (int i = 1; i <= actualLinguisticVariables.Value.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariablesAreEqual(expectedLinguisticVariables.Value[i], actualLinguisticVariables.Value[i]));
            }
        }

        private Dictionary<int, LinguisticVariable> PrepareExpectedLinguisticVariables()
        {
            // Water variable
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable = new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData: true);

            // Pressure variable
            MembershipFunctionList secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable = new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData: false);

            return new Dictionary<int, LinguisticVariable>
            {
                {1, firstLinguisticVariable},
                {2, secondLinguisticVariable}
            };
        }
    }
}
