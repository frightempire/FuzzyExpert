using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using FuzzyExpert.WpfClient.Models;
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

        public void SetProfiles(ObservableCollection<InferenceProfileModel> profiles)
        {
            ((ProfilingActionsModel) DataContext).Profiles = profiles;
        }
    }
}