using KnowledgeManager.Implementations;
using NUnit.Framework;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleFilePathProviderTests
    {
        private ImplicationRuleFilePathProvider _filePathProvider;

        [SetUp]
        public void SetUp()
        {
            _filePathProvider = new ImplicationRuleFilePathProvider();
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
