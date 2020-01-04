using System.Windows;
using FuzzyExpert.ImplicationRuleSelectorAction.ViewModels;

namespace FuzzyExpert.ImplicationRuleSelectorAction.Panels
{
    public partial class InferenceAction : Window
    {
        public InferenceAction(InferenceActionModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
