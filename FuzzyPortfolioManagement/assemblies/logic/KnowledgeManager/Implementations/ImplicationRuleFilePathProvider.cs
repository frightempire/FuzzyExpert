using KnowledgeManager.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class ImplicationRuleFilePathProvider : IImplicationRuleFilePathProvider
    {
        public string FilePath { get; set; }
    }
}