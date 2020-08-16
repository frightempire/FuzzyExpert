using System;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.ResultLogging.Implementations
{
    [TestFixture]
    public class FileResultLoggerTests
    {
        private IFileOperations _fileOperations;
        private FileResultLogger _fileResultLogger;

        [SetUp]
        public void SetUp()
        {
            _fileOperations = MockRepository.GenerateMock<IFileOperations>();
            _fileResultLogger = new FileResultLogger(_fileOperations);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFileOperationsIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new FileResultLogger(null); });
        }
    }
}