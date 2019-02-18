using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLogic.Interfaces;

namespace CommonLogic.Implementations
{
    public class FileOperations : IFileOperations
    {
        public List<string> ReadFileByLines(string filePath)
        {
            ExceptionAssert.IsEmpty(filePath);
            ExceptionAssert.FileExists(filePath);

            return File.ReadAllLines(filePath).ToList();
        }

        public void AppendLinesToFile(string filePath, List<string> lines)
        {
            ExceptionAssert.IsEmpty(filePath);
            ExceptionAssert.IsNull(lines);
            ExceptionAssert.IsEmpty(lines);

            File.AppendAllLines(filePath, lines);
        }
    }
}
