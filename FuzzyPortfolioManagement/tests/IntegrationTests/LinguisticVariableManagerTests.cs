using System;
using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using KnowledgeManager.Implementations;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Implementations;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class LinguisticVariableManagerTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\LinguisticVariables.txt");
        private FilePathProvider _filePathProvider;

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
            MembershipFunctionParser.Implementations.MembershipFunctionParser membershipFunctionParser = 
                new MembershipFunctionParser.Implementations.MembershipFunctionParser();
            LinguisticVariableParser.Implementations.LinguisticVariableParser linguisticVariableParser = 
                new LinguisticVariableParser.Implementations.LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator);
            _filePathProvider = new FilePathProvider {FilePath = _filePath};
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
        public void LinguisticVariablesGetter_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            // Arrange
            _filePathProvider.FilePath = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { var rules = _linguisticVariableManager.LinguisticVariables; });
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
            Dictionary<int, LinguisticVariable> expectedLinguisticVariables = PrepareExpectedLinguisticVariables();

            // Act
            Dictionary<int, LinguisticVariable> actuaLinguisticVariables = _linguisticVariableManager.LinguisticVariables;

            // Assert
            Assert.AreEqual(expectedLinguisticVariables.Count, actuaLinguisticVariables.Count);
            for (int i = 1; i <= actuaLinguisticVariables.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariablesAreEqual(expectedLinguisticVariables[i], actuaLinguisticVariables[i]));
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

            // Pressure vatiable
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
