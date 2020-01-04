using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableCreator
    {
        LinguisticVariable CreateLinguisticVariableEntity(LinguisticVariableStrings linguisticVariableStrings);
    }
}