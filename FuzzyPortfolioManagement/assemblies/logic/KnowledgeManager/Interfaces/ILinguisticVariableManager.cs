using System.Collections.Generic;
using LinguisticVariableParser.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface ILinguisticVariableManager
    {
        List<LinguisticVariable> LinguisticVariables { get; }
    }
}