namespace InferenceEngine.Interfaces
{
    public interface IInferenceNode
    {
        string Name { get; }

        bool? Status { get; }

        void AddRelatedRule(IInferenceRule rule);

        void UpdateStatus(bool? newStatus);
    }
}