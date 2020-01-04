using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class LinguisticVariableFilePathProvider: ILinguisticVariableFilePathProvider
    {
        public string FilePath { get; set; }
    }
}