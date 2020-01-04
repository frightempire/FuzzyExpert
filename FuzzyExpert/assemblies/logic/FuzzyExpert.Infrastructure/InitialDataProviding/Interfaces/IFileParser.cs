namespace FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces
{
    public interface IFileParser<out T>
    {
        T ParseFile(string filePath);
    }
}