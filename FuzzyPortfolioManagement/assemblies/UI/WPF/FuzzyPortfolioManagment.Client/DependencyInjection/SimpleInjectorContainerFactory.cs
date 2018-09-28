using CommonLogic.Implementations;
using CommonLogic.Interfaces;
using ProductionRuleManager.Implementations;
using ProductionRuleManager.Interfaces;
using ProductionRulesParser.Implementations;
using ProductionRulesParser.Interfaces;
using SimpleInjector;

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

            _container.Register<IImplicationRuleManager, ImplicationRuleManager>(Lifestyle.Singleton);

            _container.Verify();
            return _container;
        }
    }
}
