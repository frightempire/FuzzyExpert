using System;

namespace FuzzyExpert.Core.Entities
{
    public class InitialData
    {
        public InitialData(string name, double value, double confidenceFactor)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            Value = value;
            ConfidenceFactor = confidenceFactor;
        }

        public string Name { get; }

        public double Value { get; }

        public double ConfidenceFactor { get; }
    }
}