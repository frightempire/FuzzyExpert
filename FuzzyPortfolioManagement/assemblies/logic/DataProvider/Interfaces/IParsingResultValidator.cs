using System.Collections.Generic;
using CommonLogic.Entities;

namespace DataProvider.Interfaces
{
    public interface IParsingResultValidator
    {
        ValidationOperationResult Validate(List<string[]> parsignResult);
    }
}