using InferenceEngine.Enums;
using InferenceEngine.Implementations;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceNode
    {
        void AddRelatedRule(GraphRule rule);

        void UpdateStatus(Status newStatus);
    }
}