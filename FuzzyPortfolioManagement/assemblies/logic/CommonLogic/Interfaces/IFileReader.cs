using System.Collections.Generic;

namespace ProductionRulesParser.Interfaces
{
    public interface IFileReader
    {
        List<string> ReadFileByLines();
    }
}
