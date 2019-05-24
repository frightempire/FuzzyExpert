using InferenceEngine.Enums;
using InferenceEngine.Interfaces;

namespace InferenceEngine.Implementations
{
    public class GraphNode : IInferenceNode
    {
        public void AddRelatedRule(GraphRule rule)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateStatus(Status newStatus)
        {
            throw new System.NotImplementedException();
        }
    }
}