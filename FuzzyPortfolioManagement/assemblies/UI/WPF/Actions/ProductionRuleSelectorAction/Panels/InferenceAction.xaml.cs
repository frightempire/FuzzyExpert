using System.Windows;
using ProductionRuleSelectorAction.ViewModels;

namespace ProductionRuleSelectorAction.Panels
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
