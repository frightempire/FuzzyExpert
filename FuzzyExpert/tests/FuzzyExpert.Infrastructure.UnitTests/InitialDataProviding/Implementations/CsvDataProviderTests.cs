using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.InitialDataProviding.Implementations
{
    [TestFixture]
    public class CsvDataProviderTests
    {
        private readonly string _csvFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "InitialDataProviding\\TestFiles\\InitialData.csv");

        private IFileParser<List<string[]>> _csvParserMock;
        private IParsingResultValidator _validatorMock;
        private IDataFilePathProvider _filePathProviderMock;
        private IValidationOperationResultLogger _validationOperationResultLoggerMock;
        private CsvDataProvider _csvDataProvider;

        [SetUp]
        public void SetUp()
        {
            _csvParserMock = MockRepository.GenerateMock<IFileParser<List<string[]>>>();
            _validatorMock = MockRepository.GenerateMock<IParsingResultValidator>();
            _filePathProviderMock = MockRepository.GenerateMock<IDataFilePathProvider>();
            _validationOperationResultLoggerMock = MockRepository.GenerateMock<IValidationOperationResultLogger>();
            _csvDataProvider = new CsvDataProvider(_csvParserMock, _validatorMock, _filePathProviderMock, _validationOperationResultLoggerMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_IfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(null, _validatorMock, _filePathProviderMock, _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(_csvParserMock, null, _filePathProviderMock, _validationOperationResultLoggerMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CsvDataProvider(_csvParserMock, _validatorMock, null, _validationOperationResultLoggerMock);
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
            var expectedValidationResult = ValidationOperationResult.Fail(new List<string> { "something is not right" });
            _validatorMock.Stub(x => x.Validate(Arg<List<string[]>>.Is.Anything)).Return(expectedValidationResult);

            // Act
            var initialData = _csvDataProvider.GetInitialData();

            // Assert
            Assert.IsFalse(initialData.IsPresent);
            _validationOperationResultLoggerMock.AssertWasCalled(x => x.LogValidationOperationResultMessages(expectedValidationResult));
        }

        [Test]
        public void GetInitialData_ReturnsDictionary_IfValidationSucceeded()
        {
            // Arrange
            _filePathProviderMock.Stub(x => x.FilePath).Return(_csvFilePath);
            var expectedParsingResult = new List<string[]>
            {
                new []{ "I1_1", "55", "0.1" },
                new []{ "I1_2", "10.5", "0.1" },
                new []{ "Init3", "0.55", "0.1" },
                new []{ "Init4", "1", "0.1" },
                new []{ "Init5", "2", "0.1" }
            };
            _csvParserMock.Stub(x => x.ParseFile(_csvFilePath)).Return(expectedParsingResult);
            var expectedValidationResult = ValidationOperationResult.Success();
            _validatorMock.Stub(x => x.Validate(Arg<List<string[]>>.Is.Anything)).Return(expectedValidationResult);
            var expectedData = new List<InitialData>
            {
                new InitialData("I1_1", 55, 0.1),
                new InitialData("I1_2", 10.5, 0.1),
                new InitialData("Init3", 0.55, 0.1),
                new InitialData("Init4", 1, 0.1),
                new InitialData("Init5", 2, 0.1)
            };
            var expectedResult = Optional<List<InitialData>>.For(expectedData);

            // Act
            var actualResult = _csvDataProvider.GetInitialData();

            // Assert
            Assert.IsTrue(actualResult.IsPresent);
            Assert.AreEqual(expectedResult.Value.Count, actualResult.Value.Count);
            for (var i = 0; i < actualResult.Value.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.InitialDatasAreEqual(expectedResult.Value[i], actualResult.Value[i]));
            }
            _validationOperationResultLoggerMock.AssertWasNotCalled(x => x.LogValidationOperationResultMessages(expectedValidationResult));
        }
    }
}