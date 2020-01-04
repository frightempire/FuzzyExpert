using System.Collections.Generic;
using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces
{
    public interface IParsingResultValidator
    {
        ValidationOperationResult Validate(List<string[]> parsingResult);
    }
}