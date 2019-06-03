using System.Collections.Generic;

namespace KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseManager
    {
        List<string> GetImplicationRulesMap();
    }
}