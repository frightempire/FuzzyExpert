using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommonLogic
{
    public static class ExceptionAssert
    {
        public static void IsEmpty(string stringToAssert)
        {
            if (string.IsNullOrEmpty(stringToAssert))
                throw new ArgumentNullException(nameof(stringToAssert));
        }

        public static void IsEmpty<T>(List<T> list) where T : class
        {
            if (!list.Any())
                throw new ArgumentException(nameof(list));
        }

        public static void FileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(nameof(filePath));
        }

        public static void IsNull<T>(T instance) where T: class
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));
        }
    }
}
