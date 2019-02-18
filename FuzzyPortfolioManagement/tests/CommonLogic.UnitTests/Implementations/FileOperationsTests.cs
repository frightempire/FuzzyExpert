using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Base.UnitTests;
using CommonLogic.Implementations;
using NUnit.Framework;

namespace CommonLogic.UnitTests.Implementations
{
    [TestFixture]
    public class FileOperationsTests
    {
        private readonly string _readFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\ReadTestFile.txt");
        private readonly string _emptyFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\EmptyFile.txt");
        private readonly string _writeFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\WriteTestFile.txt");

        private FileOperations _fileOperations;

        [SetUp]
        public void SetUp()
        {
            _fileOperations = new FileOperations();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_writeFilePath))
                File.Delete(_writeFilePath);
        }

        [Test]
        public void ReadFileByLines_ThrowsArgumentNullExceptionIfFilePathIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => _fileOperations.ReadFileByLines(string.Empty));
        }

        [Test]
        public void ReadFileByLines_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => _fileOperations.ReadFileByLines(null));
        }

        [Test]
        public void ReadFileByLines_ThrowsFileNotFoundExceptionIfFileNotExists()
        {
            Assert.Throws<FileNotFoundException>(() => _fileOperations.ReadFileByLines("notExistingFile.txt"));
        }

        [Test]
        public void ReadFileByLines_ReturnsCorrectListOfStrings()
        {
            // Arrange
            List<string> expectedLines = new List<string> {"line1", "line2", "line3" };

            // Act
            List<string> lines = _fileOperations.ReadFileByLines(_readFilePath);

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(lines, expectedLines));
        }

        [Test]
        public void ReadFileByLines_ReturnsEmptyList()
        {
            // Act
            List<string> lines = _fileOperations.ReadFileByLines(_emptyFilePath);

            // Assert
            Assert.IsEmpty(lines);
        }

        [Test]
        public void AppendLinesToFile_ThrowsArgumentNullExceptionIfFilePathIsEmpty()
        {
            // Arrange
            List<string> testStrings = new List<string> {"1", "2"};

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _fileOperations.AppendLinesToFile(string.Empty, testStrings));
        }

        [Test]
        public void AppendLinesToFile_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            // Arrange
            List<string> testStrings = new List<string> { "1", "2" };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _fileOperations.AppendLinesToFile(null, testStrings));
        }

        [Test]
        public void AppendLinesToFile_ThrowsArgumentNullExceptionIfLinesAreEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _fileOperations.AppendLinesToFile(_writeFilePath, new List<string>()));
        }

        [Test]
        public void AppendLinesToFile_ThrowsArgumentNullExceptionIfLinesAreNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _fileOperations.AppendLinesToFile(_writeFilePath, null));
        }

        [Test]
        public void AppendLinesToFile_WritesLinesToFile()
        {
            // Arrange
            List<string> expectedLines = new List<string> { "first line", "second line" };

            // Act
            _fileOperations.AppendLinesToFile(_writeFilePath, expectedLines);
            List<string> actualLines = File.ReadAllLines(_writeFilePath).ToList();

            // Assert
            Assert.AreEqual(expectedLines, actualLines);
        }

        [Test]
        public void AppendLinesToFile_AppendsLinesToFile()
        {
            // Arrange
            List<string> expectedLines = new List<string> { "first line", "second line" };
            int expectedLinesCount = expectedLines.Count * 2;

            // Act
            _fileOperations.AppendLinesToFile(_writeFilePath, expectedLines);
            _fileOperations.AppendLinesToFile(_writeFilePath, expectedLines);
            List<string> actualLines = File.ReadAllLines(_writeFilePath).ToList();
            int actualLinesCount = actualLines.Count;

            // Assert
            Assert.AreEqual(expectedLinesCount, actualLinesCount);
            Assert.AreEqual(expectedLines[0], actualLines[expectedLinesCount - 2]);
            Assert.AreEqual(expectedLines[1], actualLines[expectedLinesCount - 1]);
        }
    }
}