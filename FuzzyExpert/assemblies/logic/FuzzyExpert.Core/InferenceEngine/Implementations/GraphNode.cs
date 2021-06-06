using System;
using System.Collections.Generic;
using FuzzyExpert.Core.InferenceEngine.Interfaces;

namespace FuzzyExpert.Core.InferenceEngine.Implementations
{
    public class GraphNode : IInferenceNode
    {
        private readonly List<Tuple<string, double>> _activationOrder;

        public GraphNode(string name, List<Tuple<string, double>> activationOrder)
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
                _activationOrder.Add(new Tuple<string, double>(Name, ConfidenceFactor));
            }

            foreach (IInferenceRule rule in RelatedRules) rule.UpdateConfidenceFactor();
        }
    }
}