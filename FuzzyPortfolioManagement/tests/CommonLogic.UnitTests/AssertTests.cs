using System;
using System.IO;
using Base.UnitTests;
using NUnit.Framework;

namespace CommonLogic.UnitTests
{
    [TestFixture]
    public class AssertTests
    {
        [Test]
        public void IsEmpty_ThrowsArgumentNullExceptionIfStringToAssertIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate { ExceptionAssert.IsEmpty(string.Empty); });
        }

        [Test]
        public void IsEmpty_ThrowsArgumentNullExceptionIfStringToAssertIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate { ExceptionAssert.IsEmpty(null); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFileNotExists()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists("notExistingFilePath"); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists(string.Empty); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFilePathIsNull()
        {
            // Act & Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists(null); });
        }

        [Test]
        public void IsNull_ThrowsArgumentNullExceptionIfTestClassInstanceIsNull()
        {
            // Arrange
            TestClass instance = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate { ExceptionAssert.IsNull(instance); });
        }
    }
}
