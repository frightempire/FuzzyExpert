using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.InferenceEngine.Interfaces;

namespace FuzzyExpert.Core.InferenceEngine.Implementations
{
    public class GraphRule : IInferenceRule
    {
        private readonly List<IInferenceNode> _ifNodes;
        private readonly LogicalOperation _operation;
        private readonly List<IInferenceNode> _thenNodes;

        public GraphRule(List<IInferenceNode> ifNodes, LogicalOperation operation, List<IInferenceNode> thenNodes)
        {
            if (ifNodes == null || ifNodes.Count == 0) throw new ArgumentNullException(nameof(ifNodes));
            if (thenNodes == null || thenNodes.Count == 0) throw new ArgumentNullException(nameof(thenNodes));

            _ifNodes = ifNodes;
            _operation = operation;
            _thenNodes = thenNodes;
        }

        public double ConfidenceFactor { get; private set; }

        public void UpdateConfidenceFactor()
        {
            int activeNodesCount = _ifNodes.Count(ifn => ifn.ConfidenceFactor != 0);
            if (activeNodesCount != _ifNodes.Count) return;

            double confidenceFactor = ReevaluateConfidenceFactor();

            if (confidenceFactor > ConfidenceFactor && ConfidenceFactor != 0) return;

            ConfidenceFactor = confidenceFactor;
            _thenNodes.ForEach(tn => tn.UpdateConfidenceFactor(ConfidenceFactor));
        }

        private double ReevaluateConfidenceFactor()
        {
            switch (_operation)
            {
                case LogicalOperation.And:
                    return _ifNodes.Select(ifn => ifn.ConfidenceFactor).Min();
                case LogicalOperation.Or:
                    return _ifNodes.Select(ifn => ifn.ConfidenceFactor).Max();
                case LogicalOperation.None:
                    return _ifNodes.Select(ifn => ifn.ConfidenceFactor).First();
                default:
                    throw new ArgumentException($"Operation {_operation} is not recognized.");
            }
        }
    }
}