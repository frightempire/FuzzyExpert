using System.Windows;
using CommonLogic.Implementations;
using ProductionRuleSelectorAction.ViewModels;
using UILogic.Common.Implementations;

namespace ProductionRuleSelectorAction.Panels
{
    public partial class ImplicationRuleSelectorAction : Window
    {
        public ImplicationRuleSelectorAction()
        {
            InitializeComponent();
            DataContext = new ImplicationRuleSelectorActionModel(new ImplicationRuleFileDialogInteractor(), new FileReader());
        }
    }
}
