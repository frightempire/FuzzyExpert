using System;
using System.Windows.Controls;
using FuzzyExpert.Inferencing.ViewModels;

namespace FuzzyExpert.Inferencing.Views
{
    public partial class InferencingActions : UserControl
    {
        public InferencingActions(InferencingActionsModel model)
        {
            DataContext = model ?? throw new ArgumentNullException(nameof(model));
            InitializeComponent();
        }
    }
}