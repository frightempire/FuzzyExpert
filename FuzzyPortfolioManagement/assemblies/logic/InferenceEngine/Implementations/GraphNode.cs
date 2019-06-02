using System.Collections.Generic;
using InferenceEngine.Interfaces;

namespace InferenceEngine.Implementations
{
    public class GraphNode : IInferenceNode
    {
        private readonly List<IInferenceRule> _relatedInferenceRules = new List<IInferenceRule>();

        public GraphNode(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public bool? Status { get; private set; }

        public void AddRelatedRule(IInferenceRule rule) => _relatedInferenceRules.Add(rule);

        public void UpdateStatus(bool? newStatus)
        {
            Status = newStatus;
            foreach (IInferenceRule rule in _relatedInferenceRules) rule.UpdateStatus(Name, Status);
        }
    }
}