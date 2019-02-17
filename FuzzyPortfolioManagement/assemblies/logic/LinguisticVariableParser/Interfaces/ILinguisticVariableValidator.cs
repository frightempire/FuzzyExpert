using CommonLogic.Entities;

namespace LinguisticVariableParser.Interfaces
{
    public interface ILinguisticVariableValidator
    {
        ValidationOperationResult ValidateLinguisticVariable(string linguisticVariable);
    }
}