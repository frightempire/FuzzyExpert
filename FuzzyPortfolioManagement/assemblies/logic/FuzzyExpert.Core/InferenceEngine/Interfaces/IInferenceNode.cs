using System.Collections.Generic;

namespace FuzzyExpert.Core.InferenceEngine.Interfaces
{
    public interface IInferenceNode
    {
        string Name { get; }

        double ConfidenceFactor { get; }

        List<IInferenceRule> RelatedRules { get; }

        void UpdateConfidenceFactor(double confidenceFactor);
    }
}