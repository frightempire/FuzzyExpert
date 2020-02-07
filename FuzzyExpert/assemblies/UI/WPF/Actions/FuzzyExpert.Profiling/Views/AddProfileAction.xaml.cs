using System;
using System.Windows;
using FuzzyExpert.Profiling.ViewModels;

namespace FuzzyExpert.Profiling.Views
{
    public partial class AddProfileAction : Window
    {
        public AddProfileAction(AddProfileActionModel model)
        {
            InitializeComponent();
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            model.CloseAction = Close;
        }

        private void OnCancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}