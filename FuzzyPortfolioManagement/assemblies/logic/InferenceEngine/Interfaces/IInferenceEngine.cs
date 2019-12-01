using System.Collections.Generic;
using DataProvider.Entities;
using ProductionRuleParser.Enums;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceEngine
    {
        void AddRule(List<string> ifNodeNames, LogicalOperation operation, List<string> thenNodeNames);

        Dictionary<string, double> GetInferenceResults(List<InitialData> initialData);
    }
}