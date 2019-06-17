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

        public GraphRule(List<IInferenceNode> ifNodes, LogicalOperation operation, List<IInferenceNode> thenNodes)
        {
            if (ifNodes == null || ifNodes.Count == 0) throw new ArgumentNullException(nameof(ifNodes));
            if (thenNodes == null || thenNodes.Count == 0) throw new ArgumentNullException(nameof(thenNodes));

            _ifNodes = ifNodes;
            _thenNodes = thenNodes;

            MinMax = CalculateNeededNumberOfActivatedNodes(operation, ifNodes.Count);
        }

        public bool? Status { get; private set; }

        public Tuple<int, int> MinMax { get; }

        public void UpdateStatus()
        {
            if (Status == true) return;

            Status = ReavaluateStatus();
            if (Status != true) return;

            _thenNodes.ForEach(tn => tn.ActivateNode());
        }

        private bool? ReavaluateStatus()
        {
            int activeNodesCount = _ifNodes.Count(ifn => ifn.Active);
            return activeNodesCount >= MinMax.Item1 && activeNodesCount <= MinMax.Item2;
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
                    if (ifNodesCount != 1) throw new ArgumentException($"Operation is {operation}; if nodes count is {ifNodesCount} it should be 1.");
                    min_max = new Tuple<int, int>(1, 1);
                    break;
                default:
                    throw new ArgumentException($"Operaiton {operation} is not recognized.");
            }
            return min_max;
        }
    }
}