using System.Collections.Generic;

namespace CommonLogic.Interfaces
{
    public interface IFileOperations
    {
        List<string> ReadFileByLines(string filePath);

        void AppendLinesToFile(string filePath, List<string> lines);
    }
}