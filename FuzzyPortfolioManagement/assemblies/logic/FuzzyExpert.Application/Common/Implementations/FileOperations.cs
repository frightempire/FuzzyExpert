using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzyExpert.Application.Common.Interfaces;

namespace FuzzyExpert.Application.Common.Implementations
{
    public class FileOperations : IFileOperations
    {
        public List<string> ReadFileByLines(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (!File.Exists(filePath)) throw new FileNotFoundException(nameof(filePath));

            return File.ReadAllLines(filePath).ToList();
        }

        public void AppendLinesToFile(string filePath, List<string> lines)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            if (!lines.Any()) throw new ArgumentNullException(nameof(filePath));

            File.AppendAllLines(filePath, lines);
        }
    }
}