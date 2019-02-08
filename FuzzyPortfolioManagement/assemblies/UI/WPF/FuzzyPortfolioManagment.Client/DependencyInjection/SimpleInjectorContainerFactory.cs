using CommonLogic.Implementations;
using CommonLogic.Interfaces;
using KnowledgeManager.Implementations;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Implementations;
using LinguisticVariableParser.Interfaces;
using MembershipFunctionParser.Implementations;
using MembershipFunctionParser.Interfaces;
using ProductionRuleParser.Implementations;
using ProductionRuleParser.Interfaces;
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
            _container.Register<IFileReader, FileReader>();

            _container.Register<IImplicationRuleValidator, ImplicationRuleValidator>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleParser, ImplicationRuleParser>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleCreator, ImplicationRuleCreator>(Lifestyle.Singleton);

            FilePathProvider implicationRulesFilePathProvider = new FilePathProvider();
            _container.Register<IImplicationRuleProvider>(
                () => new FileImplicationRuleProvider(
                    _container.GetInstance<IFileReader>(),
                    implicationRulesFilePathProvider,
                    _container.GetInstance<IImplicationRuleCreator>()),
                Lifestyle.Singleton);

            _container.Register<IImplicationRuleManager, ImplicationRuleManager>(Lifestyle.Singleton);

            _container.Register<IMembershipFunctionValidator, MembershipFunctionValidator>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionParser, MembershipFunctionParser.Implementations.MembershipFunctionParser>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionCreator, MembershipFunctionCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableValidator, LinguisticVariableValidator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableParser, LinguisticVariableParser.Implementations.LinguisticVariableParser>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableCreator, LinguisticVariableCreator>(Lifestyle.Singleton);

            FilePathProvider linguisticVariablesFilePathProvider = new FilePathProvider();
            _container.Register<ILinguisticVariableProvider>(
                () => new FileLinguisticVariableProvider(
                    _container.GetInstance<ILinguisticVariableValidator>(),
                    _container.GetInstance<ILinguisticVariableParser>(),
                    _container.GetInstance<ILinguisticVariableCreator>(),
                    linguisticVariablesFilePathProvider,
                    _container.GetInstance<IFileReader>()),
                Lifestyle.Singleton);

            _container.Register<ILinguisticVariableManager, LinguisticVariableManager>(Lifestyle.Singleton);

            _container.Register<IFileDialogInteractor, ImplicationRuleFileDialogInteractor>();
            _container.Register<ImplicationRuleSelectorActionModel>();
            _container.Register<ImplicationRuleSelectorAction>();

            _container.Verify();
            return _container;
        }
    }
}
