using System;
using System.Collections.Generic;
using InferenceEngine.Interfaces;

namespace InferenceEngine.Implementations
{
    public class GraphNode : IInferenceNode
    {
        private readonly List<string> _activationOrder;

        public GraphNode(string name, List<string> activationOrder)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (activationOrder == null) throw new ArgumentNullException(nameof(activationOrder));

            Name = name;
            _activationOrder = activationOrder;
        }

        public string Name { get; }

        public bool Active { get; private set; }

        public List<IInferenceRule> RelatedRules { get; } = new List<IInferenceRule>();

        public void ActivateNode()
        {
            if (!Active)
            {
                Active = true;
                _activationOrder.Add(Name);
            }

            foreach (IInferenceRule rule in RelatedRules) rule.UpdateStatus();
        }
    }
}