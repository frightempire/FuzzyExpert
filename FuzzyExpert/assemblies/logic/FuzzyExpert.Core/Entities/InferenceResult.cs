namespace FuzzyExpert.Core.Entities
{
    public class InferenceResult
    {
        public InferenceResult(string nodeName, double confidenceFactor)
        {
            NodeName = nodeName;
            ConfidenceFactor = confidenceFactor;
        }

        public string NodeName { get; }

        public double ConfidenceFactor { get; }
    }

    public class DeFuzzifiedInferenceResult : InferenceResult
    {
        public DeFuzzifiedInferenceResult(string nodeName, double confidenceFactor, double defuzzifiedValue) 
            : base(nodeName, confidenceFactor)
        {
            DefuzzifiedValue = defuzzifiedValue;
        }

        public double DefuzzifiedValue { get; }
    }
}