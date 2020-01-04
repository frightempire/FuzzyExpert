using System.ComponentModel;
using System.Windows;
using FuzzyExpert.ImplicationRuleSelectorAction.ViewModels;

namespace FuzzyExpert.ImplicationRuleSelectorAction.Panels
{
    public partial class DataSelectorAction : Window
    {
        public DataSelectorAction(DataSelectorActionModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // TODO: figure out how to move it to view model
        private void DoneButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;            
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}