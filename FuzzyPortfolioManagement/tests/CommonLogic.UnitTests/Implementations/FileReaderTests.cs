using System;
using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Implementations
{
    [TestFixture]
    public class FileReaderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\TestFile.txt");
        private readonly string _emptyFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\EmptyFile.txt");

        private FileReader _fileReader;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new FileReader();
        }

        [Test]
        public void ReadFileByLines_ThrowsArgumentNullExceptionIfFilePathIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => _fileReader.ReadFileByLines(string.Empty));
        }

        [Test]
        public void ReadFileByLines_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _fileReader.ReadFileByLines(null));
        }

        [Test]
        public void ReadFileByLines_ThrowsFileNotFoundExceptionIfFileNotExists()
        {
            Assert.Throws<FileNotFoundException>(() => _fileReader.ReadFileByLines("notExistingFile.txt"));
        }

        [Test]
        public void ReadFileByLines_ReturnsCorrectListOfStrings()
        {
            // Arrange
            List<string> expectedLines = new List<string> {"line1", "line2", "line3" };

            // Act
            List<string> lines = _fileReader.ReadFileByLines(_filePath);

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(lines, expectedLines));
        }

        [Test]
        public void ReadFileByLines_ReturnsEmptyList()
        {
            // Act
            List<string> lines = _fileReader.ReadFileByLines(_emptyFilePath);

            // Assert
            Assert.IsEmpty(lines);
        }
    }
}