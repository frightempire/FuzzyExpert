using System;
using FuzzyExpert.Profiling.Views;

namespace FuzzyExpert.WpfClient.ViewModels
{
    public class FuzzyExpertActionsModel
    {
        public ProfilingActions ProfilingActions { get; }

        public FuzzyExpertActionsModel(ProfilingActions profilingActions)
        {
            ProfilingActions = profilingActions ?? throw new ArgumentNullException(nameof(profilingActions));
        }
    }
}