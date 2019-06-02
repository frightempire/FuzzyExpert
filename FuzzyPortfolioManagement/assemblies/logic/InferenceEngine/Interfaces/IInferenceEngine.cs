using System.Collections.Generic;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceEngine
    {
        void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames);

        void StartInference(List<string> trueNodes, List<string> falseNodes);
    }
}