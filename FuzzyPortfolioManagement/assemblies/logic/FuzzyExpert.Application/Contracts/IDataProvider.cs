using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Application.Contracts
{
    public interface IDataProvider
    {
        Optional<List<InitialData>> GetInitialData();
    }
}