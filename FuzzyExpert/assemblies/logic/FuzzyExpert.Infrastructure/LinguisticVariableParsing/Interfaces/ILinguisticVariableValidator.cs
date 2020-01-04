using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces
{
    public interface ILinguisticVariableValidator
    {
        ValidationOperationResult ValidateLinguisticVariable(string linguisticVariable);
    }
}