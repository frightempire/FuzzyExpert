using LinguisticVariableParser.Entities;
using System.Collections.Generic;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableProvider
    {
        List<LinguisticVariable> GetLinguisticVariables();
    }
}