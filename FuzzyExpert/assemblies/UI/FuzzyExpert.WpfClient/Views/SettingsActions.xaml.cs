using System;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.ViewModels;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class SettingsActions : UserControl
    {
        public SettingsActions(SettingsActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }

        public void InitializeState(string userName)
        {
            var model = (SettingsActionsModel)DataContext;
            model.InitializeState();
            model.RefreshSettings(userName);
        }
    }
}