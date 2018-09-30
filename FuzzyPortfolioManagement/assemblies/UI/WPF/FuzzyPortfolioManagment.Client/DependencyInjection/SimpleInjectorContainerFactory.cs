using CommonLogic.Implementations;
using CommonLogic.Interfaces;
using ProductionRuleManager.Implementations;
using ProductionRuleManager.Interfaces;
using ProductionRulesParser.Implementations;
using ProductionRulesParser.Interfaces;
using ProductionRuleSelectorAction.Panels;
using ProductionRuleSelectorAction.ViewModels;
using SimpleInjector;
using UILogic.Common.Implementations;
using UILogic.Common.Interfaces;

namespace FuzzyPortfolioManagment.Client.DependencyInjection
{
    public class SimpleInjectorContainerFactory
    {
        private readonly Container _container = new Container();

        public Container CreateSimpleInjectorContainer()
        {
            _container.RegisterInstance(typeof(IImplicationRulePreProcessor), new ImplicationRulePreProcessor());
            _container.RegisterInstance(typeof(IImplicationRuleParser), new ImplicationRuleParser());
            _container.Register<IImplicationRuleCreator, ImplicationRuleCreator>();

            _container.RegisterInstance(typeof(IFileReader), new FileReader());
            _container.Register<IImplicationRuleProvider, FileImplicationRuleProvider>();

            _container.Register<IImplicationRuleManager, ImplicationRuleManager>();

            _container.Register<IFileDialogInteractor, ImplicationRuleFileDialogInteractor>();
            _container.Register<ImplicationRuleSelectorActionModel>();

            _container.Register<ImplicationRuleSelectorAction>();

            _container.Verify();
            return _container;
        }
    }
}
