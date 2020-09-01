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

        public void InitializeState(string userName)
        {
            var model = (ProfilingActionsModel) DataContext;
            model.InitializeState();
            model.RefreshProfiles(userName);
        }
    }
}