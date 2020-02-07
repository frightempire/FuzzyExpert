using System;
using System.Windows;
using System.Windows.Input;
using FuzzyExpert.Profiling.ViewModels;

namespace FuzzyExpert.Profiling.Views
{
    public partial class AddImplicationRuleAction : Window
    {
        public AddImplicationRuleAction(AddImplicationRuleActionModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}