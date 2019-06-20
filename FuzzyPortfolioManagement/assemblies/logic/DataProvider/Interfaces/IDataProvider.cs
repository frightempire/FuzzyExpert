using System.Collections.Generic;
using CommonLogic.Entities;

namespace DataProvider.Interfaces
{
    public interface IDataProvider
    {
        Optional<Dictionary<string, double>> GetInitialData();
    }
}