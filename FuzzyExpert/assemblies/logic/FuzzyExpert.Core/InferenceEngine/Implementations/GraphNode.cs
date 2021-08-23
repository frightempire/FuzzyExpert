using System;
using System.Collections.Generic;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.InferenceEngine.Interfaces;

namespace FuzzyExpert.Core.InferenceEngine.Implementations
{
    public class GraphNode : IInferenceNode
    {
        private readonly List<InferenceResult> _activationOrder;

        public GraphNode(string name, List<InferenceResult> activationOrder)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            _activationOrder = activationOrder ?? throw new ArgumentNullException(nameof(activationOrder));
            Name = name;
        }

        public string Name { get; }

        public double ConfidenceFactor { get; private set; }

        public List<IInferenceRule> RelatedRules { get; } = new List<IInferenceRule>();

        public void UpdateConfidenceFactor(double confidenceFactor)
        {
            if (ConfidenceFactor == 0)
            {
                ConfidenceFactor = confidenceFactor;
                _activationOrder.Add(new InferenceResult(Name, ConfidenceFactor));
            }

            foreach (IInferenceRule rule in RelatedRules) rule.UpdateConfidenceFactor();
        }
    }
}