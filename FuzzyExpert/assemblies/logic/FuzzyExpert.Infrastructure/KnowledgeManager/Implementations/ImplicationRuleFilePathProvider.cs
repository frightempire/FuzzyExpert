using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class ImplicationRuleFilePathProvider : IImplicationRuleFilePathProvider
    {
        public string FilePath { get; set; }
    }
}