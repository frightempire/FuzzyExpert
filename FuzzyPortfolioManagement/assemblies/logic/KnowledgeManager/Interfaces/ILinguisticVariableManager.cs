using System.Collections.Generic;
using CommonLogic.Entities;
using LinguisticVariableParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableManager
    {
        Optional<Dictionary<int, LinguisticVariable>> LinguisticVariables { get; }
    }
}