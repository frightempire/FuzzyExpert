using System;
using System.Collections.Generic;
using System.Linq;
using InferenceEngine.Interfaces;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Implementations
{
    public class GraphRule : IInferenceRule
    {
        private readonly List<IInferenceNode> _ifNodes;
        private readonly List<IInferenceNode> _thenNodes;
        private readonly Tuple<int, int> _min_max;

        public GraphRule(List<IInferenceNode> ifNodes, LogicalOperation operation, List<IInferenceNode> thenNodes)
        {
            _ifNodes = ifNodes;
            _thenNodes = thenNodes;

            _min_max = CalculateNeededNumberOfActivatedNodes(operation, ifNodes.Count);
        }

        public bool? Status { get; private set; }

        public void UpdateStatus(string nodeName, bool? newStatus)
        {
            if (Status == true) return;

            Status = ReavaluateStatus();
            if (Status != true) return;

            _thenNodes.ForEach(tn => tn.UpdateStatus(true));
        }

        private bool? ReavaluateStatus()
        {
            int activeNodesCount = _ifNodes.Count(ifn => ifn.Status == true);
            return activeNodesCount >= _min_max.Item1 && activeNodesCount <= _min_max.Item2;
        }

        private Tuple<int, int> CalculateNeededNumberOfActivatedNodes(LogicalOperation operation, int ifNodesCount)
        {
            Tuple<int, int> min_max;
            switch (operation)
            {
                case LogicalOperation.And:
                    min_max = new Tuple<int, int>(ifNodesCount, ifNodesCount);
                    break;
                case LogicalOperation.Or:
                    min_max = new Tuple<int, int>(1, ifNodesCount);
                    break;
                case LogicalOperation.None:
                    if (ifNodesCount != 1) throw new ArgumentException($"Operation is {operation}; if nodes count is {ifNodesCount} and should be 1.");
                    min_max = new Tuple<int, int>(1, 1);
                    break;
                default:
                    throw new ArgumentException($"Operaiton {operation} is not recognized.");
            }
            return min_max;
        }
    }
}