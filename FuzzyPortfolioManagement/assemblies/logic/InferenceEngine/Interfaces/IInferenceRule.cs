namespace InferenceEngine.Interfaces
{
    public interface IInferenceRule
    {
        bool? Status { get; }

        void UpdateStatus(string nodeName, bool? newStatus);
    }
}