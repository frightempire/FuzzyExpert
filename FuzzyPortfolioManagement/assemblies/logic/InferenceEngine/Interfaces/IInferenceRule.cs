using InferenceEngine.Enums;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceRule
    {
        void UpdateStatus(string nodeName, Status newStatus);

        bool IsActive();
    }
}