using System;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.ViewModels;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class ProfilingActions : UserControl
    {
        public ProfilingActions(ProfilingActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }

        public void RefreshProfiles()
        {
            ((ProfilingActionsModel) DataContext).RefreshProfiles();
        }
    }
}