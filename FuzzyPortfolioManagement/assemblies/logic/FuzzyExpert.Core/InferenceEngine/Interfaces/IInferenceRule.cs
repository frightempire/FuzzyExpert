namespace FuzzyExpert.Core.InferenceEngine.Interfaces
{
    public interface IInferenceRule
    {
        double ConfidenceFactor { get; }

        void UpdateConfidenceFactor();
    }
}