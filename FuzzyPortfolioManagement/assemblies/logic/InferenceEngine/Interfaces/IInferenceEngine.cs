using System.Collections.Generic;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceEngine
    {
        void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames);

        List<string> GetInferenceResults(List<string> trueNodes);
    }
}