using DataProvider.Interfaces;

namespace DataProvider.Implementations
{
    public class InitialDataFilePathProvider: IDataFilePathProvider
    {
        public string FilePath { get; set; }
    }
}