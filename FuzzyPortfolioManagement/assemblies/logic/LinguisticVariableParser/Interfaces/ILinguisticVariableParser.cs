using LinguisticVariableParser.Entities;

namespace LinguisticVariableParser.Interfaces
{
    public interface ILinguisticVariableParser
    {
        LinguisticVariableStrings ParseLinguisticVariable(string linguisticVariable);
    }
}
