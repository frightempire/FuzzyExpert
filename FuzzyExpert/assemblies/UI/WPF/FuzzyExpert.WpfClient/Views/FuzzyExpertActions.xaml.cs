using System;
using System.Windows;
using FuzzyExpert.WpfClient.ViewModels;

namespace FuzzyExpert.WpfClient.Views
{
    public partial class FuzzyExpertActions : Window
    {
        public FuzzyExpertActions(FuzzyExpertActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }
    }
}