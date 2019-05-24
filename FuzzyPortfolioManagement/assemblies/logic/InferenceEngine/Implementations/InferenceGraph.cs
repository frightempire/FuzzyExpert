using System.Collections.Generic;
using InferenceEngine.Interfaces;

namespace InferenceEngine.Implementations
{
    public class InferenceGraph : IInferenceEngine
    {
        public void AddRule(GraphRule rule)
        {
            throw new System.NotImplementedException();
        }

        public void StartInference(List<GraphNode> trueNodes, List<GraphNode> falseNodes)
        {
            throw new System.NotImplementedException();
        }
    }
}