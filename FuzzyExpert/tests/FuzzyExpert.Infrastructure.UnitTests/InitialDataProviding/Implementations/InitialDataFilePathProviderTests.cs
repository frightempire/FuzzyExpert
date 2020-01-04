using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.InitialDataProviding.Implementations
{
    [TestFixture]
    public class InitialDataFilePathProviderTests
    {
        private InitialDataFilePathProvider _filePathProvider;

        [SetUp]
        public void SetUp()
        {
            _filePathProvider = new InitialDataFilePathProvider();
        }

        [Test]
        public void FilePathSetter_WorksProperly()
        {
            // Arrange
            string expectedFilePath = "file.txt";

            // Act
            _filePathProvider.FilePath = expectedFilePath;

            // Assert
            Assert.AreEqual(expectedFilePath, _filePathProvider.FilePath);
        }

        [Test]
        public void FilePathGetter_ReturnsCorrectValue()
        {
            // Arrange
            string expectedFilePath = "file.txt";
            _filePathProvider.FilePath = expectedFilePath;

            // Act
            string actualFilePath = _filePathProvider.FilePath;

            // Assert
            Assert.AreEqual(expectedFilePath, actualFilePath);
        }
    }
}
