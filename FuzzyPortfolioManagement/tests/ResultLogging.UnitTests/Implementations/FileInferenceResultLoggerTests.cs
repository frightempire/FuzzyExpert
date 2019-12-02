using System;
using CommonLogic.Interfaces;
using NUnit.Framework;
using ResultLogging.Implementations;
using Rhino.Mocks;

namespace ResultLogging.UnitTests.Implementations
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