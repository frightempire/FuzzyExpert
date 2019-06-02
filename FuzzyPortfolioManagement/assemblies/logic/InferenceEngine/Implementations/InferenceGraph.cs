using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic.Extensions;
using InferenceEngine.Interfaces;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Implementations
{
    public class InferenceGraph : IInferenceEngine
    {
        private readonly List<IInferenceRule> _rules = new List<IInferenceRule>();
        private readonly List<IInferenceNode> _nodes = new List<IInferenceNode>();

        public void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames)
        {
            if (ifNodeNames == null || ifNodeNames.Count == 0) throw new ArgumentException(nameof(ifNodeNames));
            if (thenNodeNames == null || thenNodeNames.Count == 0) throw new ArgumentException(nameof(thenNodeNames));

            foreach (var nodeName in thenNodeNames) UpdateNodeList(nodeName);
            foreach (var nodeName in ifNodeNames) UpdateNodeList(nodeName);

            List<IInferenceNode> ifNodes = GetNodes(ifNodeNames);
            var rule = new GraphRule(ifNodes, operation, GetNodes(thenNodeNames));
            _rules.Add(rule);

            ifNodes.ForEach(ifn => ifn.AddRelatedRule(rule));
        }

        public void StartInference(List<string> trueNodes, List<string> falseNodes)
        {
            if (trueNodes.Intersect(falseNodes)) throw new ArgumentException("True and false nodes are intersecting.");
            GetNodes(trueNodes).ForEach(tn => tn.UpdateStatus(newStatus: true));
            GetNodes(falseNodes).ForEach(fn => fn.UpdateStatus(newStatus: false));
        }

        private void UpdateNodeList(string nodeName)
        {
            IInferenceNode matchingNode = _nodes.FirstOrDefault(n => n.Name == nodeName);
            if (matchingNode == null) _nodes.Add(new GraphNode(nodeName));
        }

        private List<IInferenceNode> GetNodes(List<string> nodeNames) => 
            _nodes.Where(n => nodeNames.Contains(n.Name)).Select(n => n).ToList();
    }
}