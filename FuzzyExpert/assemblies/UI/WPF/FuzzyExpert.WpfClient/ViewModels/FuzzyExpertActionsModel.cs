using System;
using FuzzyExpert.Inferencing.Views;
using FuzzyExpert.Profiling.Views;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class FuzzyExpertActionsModel
    {
        public ProfilingActions ProfilingActions { get; }

        public InferencingActions InferencingActions { get; }

        public FuzzyExpertActionsModel(ProfilingActions profilingActions, InferencingActions inferencingActions)
        {
            ProfilingActions = profilingActions ?? throw new ArgumentNullException(nameof(profilingActions));
            InferencingActions = inferencingActions ?? throw new ArgumentNullException(nameof(inferencingActions));
        }
    }
}