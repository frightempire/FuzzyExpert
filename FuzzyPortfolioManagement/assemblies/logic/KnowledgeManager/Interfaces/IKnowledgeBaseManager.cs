using System.Collections.Generic;
using KnowledgeManager.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseManager
    {
        List<ImplicationRuleRelations> GetImplicationRulesMap();
    }
}