using System.Collections.Generic;

namespace DataProvider.Interfaces
{
    public interface IDataProvider
    {
        Dictionary<string, double> GetInitialData();
    }
}