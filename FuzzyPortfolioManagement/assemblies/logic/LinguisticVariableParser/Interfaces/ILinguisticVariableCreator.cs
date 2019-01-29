using LinguisticVariableParser.Entities;

namespace LinguisticVariableParser.Interfaces
{
    public interface ILinguisticVariableCreator
    {
        LinguisticVariable CreateLinguisticVariableEntity(LinguisticVariableStrings linguisticVariableStrings);
    }
}