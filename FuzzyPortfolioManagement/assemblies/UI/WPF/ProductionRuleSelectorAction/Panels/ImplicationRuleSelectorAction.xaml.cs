using System.Windows;
using ProductionRuleSelectorAction.ViewModels;

namespace ProductionRuleSelectorAction.Panels
{
    public partial class ImplicationRuleSelectorAction : Window
    {
        public ImplicationRuleSelectorAction(ImplicationRuleSelectorActionModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
