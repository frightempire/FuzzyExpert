using System;
using System.Collections.Generic;
using System.IO;
using CommonLogic.Entities;
using CommonLogic.Interfaces;
using DataProvider.Implementations;
using DataProvider.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace DataProvider.UnitTests.Implementations
{
    [TestFixture]
    public class CsvDataProviderTests
    {
        private readonly string _csvFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\InitialData.csv");

        private IFileParser<List<string[]>> _csvParserMock;
        private IParsingResultValidator _validatorMock;
        private IFilePathProvider _filePathProviderMock;
        private IValidationOperationResultLogger _validationOperationResultLogerMock;
        private CsvDataProvider _csvDataProvider;

        [SetUp]
        public void SetUp()
        {
            _csvParserMock = MockRepository.GenerateMock<IFileParser<List<string[]>>>();
            _validatorMock = MockRepository.GenerateMock<IParsingResultValidator>();
            _filePathProviderMock = MockRepository.GenerateMock<IFilePathProvider>();
            _validationOperationResultLogerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();
            _csvDataProvider = new CsvDataProvider(_csvParserMock, _validatorMock, _filePathProviderMock, _validationOperationResultLogerMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_IfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(null, _validatorMock, _filePathProviderMock, _validationOperationResultLogerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(_csvParserMock, null, _filePathProviderMock, _validationOperationResultLogerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(_csvParserMock, _validatorMock, null, _validationOperationResultLogerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(_csvParserMock, _validatorMock, _filePathProviderMock, null);
            });
        }

        [Test]
        public void GetInitialData_ThrowsFileNotFoundException_IfFilePathIsEmpty()
        {
            // Arrange
            _filePathProviderMock.Stub(x => x.FilePath).Return("non_existing_file.csv");

            // Assert
            Assert.Throws<FileNotFoundException>(() => { _csvDataProvider.GetInitialData(); });
        }

        [Test]
        public void GetInitialData_ReturnsEmptyOptional_IfValidationFails()
        {
            // Arrange
            _filePathProviderMock.Stub(x => x.FilePath).Return(_csvFilePath);
            ValidationOperationResult expectedValidationResult = new ValidationOperationResult();
            expectedValidationResult.AddMessage("something is not right");
            _validatorMock.Stub(x => x.Validate(Arg<List<string[]>>.Is.Anything)).Return(expectedValidationResult);

            // Act
            Optional<Dictionary<string, double>> initialData = _csvDataProvider.GetInitialData();

            // Assert
            Assert.IsFalse(initialData.IsPresent);
            _validationOperationResultLogerMock.AssertWasCalled(x => x.LogValidationOperationResultMessages(expectedValidationResult));
        }

        [Test]
        public void GetInitialData_ReturnsDictionary_IfValidationSucceded()
        {
            // Arrange
            _filePathProviderMock.Stub(x => x.FilePath).Return(_csvFilePath);
            List<string[]> expectedParsingResult = new List<string[]>
            {
                new []{ "I1_1", "55" },
                new []{ "I1_2", "10,5" },
                new []{ "Init3", "0,55" },
                new []{ "Init4", "1" },
                new []{ "Init5", "2" }
            };
            _csvParserMock.Stub(x => x.ParseFile(_csvFilePath)).Return(expectedParsingResult);
            ValidationOperationResult expectedValidationResult = new ValidationOperationResult();
            _validatorMock.Stub(x => x.Validate(Arg<List<string[]>>.Is.Anything)).Return(expectedValidationResult);
            Dictionary<string, double> expectedData = new Dictionary<string, double>
            {
                { "I1_1", 55 },
                { "I1_2", 10.5 },
                { "Init3", 0.55 },
                { "Init4", 1 },
                { "Init5", 2 },
            };
            Optional<Dictionary<string, double>> expectedResult = Optional<Dictionary<string, double>>.For(expectedData);

            // Act
            Optional<Dictionary<string, double>> actualResult = _csvDataProvider.GetInitialData();

            // Assert
            Assert.IsTrue(actualResult.IsPresent);
            Assert.AreEqual(expectedResult.Value, actualResult.Value);
            _validationOperationResultLogerMock.AssertWasNotCalled(x => x.LogValidationOperationResultMessages(expectedValidationResult));
        }
    }
}