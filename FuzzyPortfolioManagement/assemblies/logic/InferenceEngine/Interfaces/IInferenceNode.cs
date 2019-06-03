using System.Collections.Generic;

namespace InferenceEngine.Interfaces
{
    public interface IInferenceNode
    {
        string Name { get; }

        bool Active { get; }

        List<IInferenceRule> RelatedRules { get; }

        void ActivateNode();
    }
}