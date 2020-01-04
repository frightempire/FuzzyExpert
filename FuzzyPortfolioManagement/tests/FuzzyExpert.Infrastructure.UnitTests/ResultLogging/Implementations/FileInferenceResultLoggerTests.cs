using System;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.ResultLogging.Implementations
{
    [TestFixture]
    public class FileInferenceResultLoggerTests
    {
        private IFileOperations _fileOperations;
        private FileInferenceResultLogger _fileInferenceResultLogger;

        [SetUp]
        public void SetUp()
        {
            _fileOperations = MockRepository.GenerateMock<IFileOperations>();
            _fileInferenceResultLogger = new FileInferenceResultLogger(_fileOperations);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFileOperationsIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new FileInferenceResultLogger(null); });
        }
    }
}