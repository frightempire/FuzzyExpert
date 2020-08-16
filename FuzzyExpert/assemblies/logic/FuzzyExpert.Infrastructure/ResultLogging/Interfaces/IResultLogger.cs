using System.Collections.Generic;
using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.ResultLogging.Interfaces
{
    public interface IResultLogger
    {
        string ResultLogPath { get; }

        string ValidationLogPath { get; }

        void LogImplicationRules(Dictionary<int, ImplicationRule> implicationRules);

        void LogInferenceResult(Dictionary<string, double> inferenceResult);

        void LogInferenceErrors(List<string> inferenceErrors);

        void LogValidationErrors(List<string> validationErrors);
    }
}