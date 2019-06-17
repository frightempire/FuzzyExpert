namespace DataProvider.Interfaces
{
    public interface IFileParser<out T>
    {
        T ParseFile(string filePath);
    }
}