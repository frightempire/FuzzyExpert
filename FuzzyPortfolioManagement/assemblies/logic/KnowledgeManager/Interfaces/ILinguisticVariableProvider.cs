using LinguisticVariableParser.Entities;
using System.Collections.Generic;
using CommonLogic.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableProvider
    {
        Optional<List<LinguisticVariable>> GetLinguisticVariables();
    }
}