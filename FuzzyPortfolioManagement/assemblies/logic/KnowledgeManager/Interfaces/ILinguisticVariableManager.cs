using System.Collections.Generic;
using LinguisticVariableParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableManager
    {
        Dictionary<int, LinguisticVariable> LinguisticVariables { get; }
    }
}