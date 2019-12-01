using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using DataProvider.Entities;
using InferenceEngine.Interfaces;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Implementations
{
    public class InferenceGraph : IInferenceEngine
    {
        private readonly Dictionary<string, double> _activationOrder = new Dictionary<string, double>();

        private readonly List<IInferenceRule> _rules = new List<IInferenceRule>();
        private readonly List<IInferenceNode> _nodes = new List<IInferenceNode>();

        public void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames)
        {
            ExceptionAssert.IsNull(ifNodeNames);
            ExceptionAssert.IsNull(thenNodeNames);
            ExceptionAssert.IsEmpty(ifNodeNames);
            ExceptionAssert.IsEmpty(thenNodeNames);

            foreach (var nodeName in thenNodeNames) UpdateNodeList(nodeName);
            foreach (var nodeName in ifNodeNames) UpdateNodeList(nodeName);

            List<IInferenceNode> ifNodes = GetNodes(ifNodeNames);
            var rule = new GraphRule(ifNodes, operation, GetNodes(thenNodeNames));
            _rules.Add(rule);

            ifNodes.ForEach(ifn => ifn.RelatedRules.Add(rule));
        }

        public Dictionary<string, double> GetInferenceResults(List<InitialData> initialData)
        {
            var nodes = GetNodes(initialData.Select(id => id.Name).ToList());
            nodes.ForEach(n => n.UpdateConfidenceFactor(initialData.First(id => id.Name == n.Name).ConfidenceFactor));
            return _activationOrder;
        }

        private void UpdateNodeList(string nodeName)
        {
            IInferenceNode matchingNode = _nodes.FirstOrDefault(n => n.Name == nodeName);
            if (matchingNode == null) _nodes.Add(new GraphNode(nodeName, _activationOrder));
        }

        private List<IInferenceNode> GetNodes(List<string> nodeNames) => _nodes.Where(n => nodeNames.Contains(n.Name)).Select(n => n).ToList();
    }
}