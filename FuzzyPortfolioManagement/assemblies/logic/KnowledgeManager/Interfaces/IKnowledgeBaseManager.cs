using CommonLogic.Entities;
using KnowledgeManager.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseManager
    {
        Optional<KnowledgeBase> GetKnowledgeBase();
    }
}