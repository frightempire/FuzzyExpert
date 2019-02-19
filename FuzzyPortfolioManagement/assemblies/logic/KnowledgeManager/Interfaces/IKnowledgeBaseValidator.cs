using CommonLogic.Entities;

namespace KnowledgeManager.Interfaces
{
    public interface IKnowledgeBaseValidator
    {
        ValidationOperationResult ValidateLinguisticVariablesNames();
    }
}
