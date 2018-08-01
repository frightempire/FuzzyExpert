﻿using System;
using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using NUnit.Framework;

namespace CommonLogic.UnitTests
{
    [TestFixture]
    public class FileReaderTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\TestFile.txt");

        private FileReader _fileReader;

        [SetUp]
        public void SetUp()
        {
            _fileReader = new FileReader(_filePath);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFilePathIsEmpty()
        {
            Assert.Throws<ArgumentNullException>(() => new FileReader(string.Empty));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FileReader(null));
        }

        [Test]
        public void Constructor_ThrowsFileNotFoundExceptionIfFileNotExists()
        {
            Assert.Throws<FileNotFoundException>(() => new FileReader("notExistingFile.txt"));
        }

        [Test]
        public void ReadFileByLines()
        {
            // Arrange
            List<string> expectedLines = new List<string> {"line1", "line2", "line3" };

            // Act
            List<string> lines = _fileReader.ReadFileByLines();

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(lines, expectedLines));
        }

    }
}
