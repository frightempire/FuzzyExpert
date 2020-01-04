using System;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.ResultLogging.Implementations
{
    [TestFixture]
    public class FileValidationOperationResultLoggerTests
    {
        private IFileOperations _fileOperations;
        private FileValidationOperationResultLogger _fileValidationOperationResultLogger;

        [SetUp]
        public void SetUp()
        {
            _fileOperations = MockRepository.GenerateMock<IFileOperations>();
            _fileValidationOperationResultLogger = new FileValidationOperationResultLogger(_fileOperations);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFileOperationsIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new FileValidationOperationResultLogger(null); });
        }
    }
}