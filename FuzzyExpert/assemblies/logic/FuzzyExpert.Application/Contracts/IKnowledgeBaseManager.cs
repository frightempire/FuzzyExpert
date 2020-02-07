using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Application.Contracts
{
    public interface IKnowledgeBaseManager
    {
        Optional<KnowledgeBase> GetKnowledgeBase(string profileName);
    }
}