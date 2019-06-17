using System.Collections.Generic;
using System.IO;
using DataProvider.Implementations;
using NUnit.Framework;

namespace DataProvider.UnitTests.Implementations
{
    [TestFixture]
    public class CsvFileParserTests
    {
        private readonly string _csvFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\InitialData.csv");

        private CsvFileParser _csvFileParser;

        [SetUp]
        public void SetUp()
        {
            _csvFileParser = new CsvFileParser();
        }

        [Test]
        public void ParseFile_ThrowsFileNotFound_IfFileDoesntExist()
        {
            // Arrange
            string wrongFileName = "non_existing_file_name.csv";
            
            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => { _csvFileParser.ParseFile(wrongFileName); });
            Assert.Throws<FileNotFoundException>(() => { _csvFileParser.ParseFile(null); });
            Assert.Throws<FileNotFoundException>(() => { _csvFileParser.ParseFile(string.Empty); });
        }

        [Test]
        public void ParseFile_ReturnCollectionOfStrings()
        {
            // Assert
            List<string[]> expectedResult = new List<string[]>
            {
                new []{ "I1_1", "55" },
                new []{ "I1_2", "10,5" },
                new []{ "Init3", "0,55" },
                new []{ "Init4", "1" },
                new []{ "Init5", "2" }
            };

            // Act
            List<string[]> actualResult = _csvFileParser.ParseFile(_csvFilePath);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}