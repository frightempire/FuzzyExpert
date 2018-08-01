using System;
using System.IO;
using NUnit.Framework;

namespace CommonLogic.UnitTests
{
    [TestFixture]
    public class AssertTests
    {
        [Test]
        public void IsNotEmpty_ThrowsArgumentNullExceptionIfStringToAssertIsEmpty()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(delegate { ExceptionAssert.IsNotEmpty(string.Empty); });
        }

        [Test]
        public void IsNotEmpty_ThrowsArgumentNullExceptionIfStringToAssertIsNull()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(delegate { ExceptionAssert.IsNotEmpty(null); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFileNotExists()
        {
            // Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists("notExistingFilePath"); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists(string.Empty); });
        }

        [Test]
        public void FileExists_ThrowsFileNotFoundExceptionIfFilePathIsNull()
        {
            // Assert
            Assert.Throws<FileNotFoundException>(delegate { ExceptionAssert.FileExists(null); });
        }
    }
}
