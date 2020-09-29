using System;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Entities
{
    public class DefaultSettings
    {
        public DefaultSettings(double confidenceFactorLowerBoundary)
        {
            if (confidenceFactorLowerBoundary <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(confidenceFactorLowerBoundary));
            }

            ConfidenceFactorLowerBoundary = confidenceFactorLowerBoundary;
        }

        public double ConfidenceFactorLowerBoundary { get; }
    }
}