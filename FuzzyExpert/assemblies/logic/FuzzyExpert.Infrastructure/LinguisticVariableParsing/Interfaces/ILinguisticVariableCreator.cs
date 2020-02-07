using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableCreator
    {
        LinguisticVariable CreateLinguisticVariableEntity(string linguisticVariable);
    }
}