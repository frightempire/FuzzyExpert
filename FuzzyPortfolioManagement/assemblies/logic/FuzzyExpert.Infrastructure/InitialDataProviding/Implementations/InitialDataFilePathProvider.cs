using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Implementations
{
    public class InitialDataFilePathProvider: IDataFilePathProvider
    {
        public string FilePath { get; set; }
    }
}