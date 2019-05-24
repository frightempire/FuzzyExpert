using System.Collections.Generic;
using InferenceEngine.Implementations;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceEngine
    {
        void AddRule(GraphRule rule);

        void StartInference(List<GraphNode> trueNodes, List<GraphNode> falseNodes);
    }
}
