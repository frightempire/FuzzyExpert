using System.Collections.Generic;
using ProductionRuleParser.Entities;

namespace ResultLogging.Interfaces
{
    public interface IInferenceResultLogger
    {
        void LogImplicationRules(Dictionary<int, ImplicationRule> implicationRules);

        void LogInferenceResult(Dictionary<string, double> inferenceResult);
    }
}