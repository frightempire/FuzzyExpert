using System.Collections.Generic;
using CommonLogic.Entities;
using DataProvider.Entities;

namespace DataProvider.Interfaces
{
    public interface IDataProvider
    {
        Optional<List<InitialData>> GetInitialData();
    }
}