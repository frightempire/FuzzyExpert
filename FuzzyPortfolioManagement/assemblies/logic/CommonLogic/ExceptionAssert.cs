﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommonLogic
{
    public static class ExceptionAssert
    {
        public static void IsEmpty(string stringToAssert)
        {
            if (string.IsNullOrEmpty(stringToAssert)) throw new ArgumentNullException(nameof(stringToAssert));
        }

        public static void IsEmpty<T>(List<T> list)
        {
            if (!list.Any()) throw new ArgumentNullException(nameof(list));
        }

        public static void IsEmpty<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            if (!dictionary.Any()) throw new ArgumentNullException(nameof(dictionary));
        }

        public static void FileExists(string filePath)
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException(nameof(filePath));
        }

        public static void IsNull<T>(T instance) where T: class
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
        }
    }
}