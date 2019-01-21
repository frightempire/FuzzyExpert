using LinguisticVariableParser.Entities;

namespace LinguisticVariableParser.Interfaces
{
    public interface ILinguisticVariableParser
    {
        LinguisticVariable ParseLinguisticVariable(string linguisticVariable);
    }
}
