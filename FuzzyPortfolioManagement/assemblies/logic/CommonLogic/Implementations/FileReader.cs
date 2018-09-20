using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLogic.Interfaces;

namespace CommonLogic.Implementations
{
    public class FileReader : IFileReader
    {
        public List<string> ReadFileByLines(string filePath)
        {
            ExceptionAssert.IsEmpty(filePath);
            ExceptionAssert.FileExists(filePath);

            return File.ReadAllLines(filePath).ToList();
        }
    }
}
