using System;
using System.IO;

namespace CommonLogic
{
    public static class ExceptionAssert
    {
        public static void IsNotEmpty(string stringToAssert)
        {
            if (string.IsNullOrEmpty(stringToAssert))
                throw new ArgumentNullException(nameof(stringToAssert));
        }

        public static void FileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(nameof(filePath));
        }
    }
}
