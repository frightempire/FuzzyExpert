using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLogic.Interfaces;

namespace CommonLogic.Implementations
{
    public class FileReader : IFileReader
    {
        private readonly string _filePath;

        public FileReader(string filePath)
        {
            ExceptionAssert.IsEmpty(filePath);
            ExceptionAssert.FileExists(filePath);

            _filePath = filePath;
        }

        public List<string> ReadFileByLines()
        {
            return File.ReadAllLines(_filePath).ToList();
        }
    }
}
