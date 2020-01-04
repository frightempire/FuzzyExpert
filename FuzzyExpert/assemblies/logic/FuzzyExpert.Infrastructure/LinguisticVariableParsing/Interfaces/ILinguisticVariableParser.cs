using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableParser
    {
        LinguisticVariableStrings ParseLinguisticVariable(string linguisticVariable);
    }
}