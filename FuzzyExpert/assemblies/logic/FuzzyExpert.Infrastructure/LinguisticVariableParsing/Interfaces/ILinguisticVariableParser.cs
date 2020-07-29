using System.Collections.Generic;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableParser
    {
        List<LinguisticVariableStrings> ParseLinguisticVariable(string linguisticVariable);
    }
}