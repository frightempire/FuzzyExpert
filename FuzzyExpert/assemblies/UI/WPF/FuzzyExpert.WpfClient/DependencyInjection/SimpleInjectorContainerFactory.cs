using System.Collections.Generic;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Core.FuzzificationEngine.Implementations;
using FuzzyExpert.Core.FuzzificationEngine.Interfaces;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Interfaces;
using FuzzyExpert.Inferencing.ViewModels;
using FuzzyExpert.Inferencing.Views;
using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Helpers;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using FuzzyExpert.Infrastructure.ProfileManaging.Implementations;
using FuzzyExpert.Infrastructure.ProfileManaging.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using FuzzyExpert.Profiling.ViewModels;
using FuzzyExpert.Profiling.Views;
using FuzzyExpert.WpfClient.ViewModels;
using FuzzyExpert.WpfClient.Views;
using SimpleInjector;

namespace FuzzyExpert.WpfClient.DependencyInjection
{
    public class SimpleInjectorContainerFactory
    {
        private readonly Container _container = new Container();

        public Container CreateSimpleInjectorContainer()
        {
            _container.Register<IProfileRepository, ProfileRepository>(Lifestyle.Singleton);

            // KnowledgeBaseManager
            _container.Register<IFileOperations, FileOperations>(Lifestyle.Singleton);
            _container.Register<IValidationOperationResultLogger, FileValidationOperationResultLogger>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleValidator, ImplicationRuleValidator>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleParser, ImplicationRuleParser>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleCreator, ImplicationRuleCreator>(Lifestyle.Singleton);
            _container.Register<INameProvider, UniqueNameProvider>(Lifestyle.Singleton);
            _container.Register<INameSupervisor, NameSupervisor>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleProvider, DatabaseImplicationRuleProvider>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleManager, ImplicationRuleManager>(Lifestyle.Singleton);

            _container.Register<IMembershipFunctionValidator, MembershipFunctionValidator>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionParser, MembershipFunctionParser>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionCreator, MembershipFunctionCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableValidator, LinguisticVariableValidator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableParser, LinguisticVariableParser>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableCreator, LinguisticVariableCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableProvider, DatabaseLinguisticVariableProvider>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableManager, LinguisticVariableManager>(Lifestyle.Singleton);

            _container.Register<IKnowledgeBaseValidator, KnowledgeBaseValidator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableRelationsInitializer, LinguisticVariableRelationsInitializer>(Lifestyle.Singleton);
            _container.Register<IKnowledgeBaseManager, KnowledgeBaseManager>(Lifestyle.Singleton);

            // CsvDataProvider
            _container.Register<IFileParser<List<string[]>>, CsvFileParser>(Lifestyle.Singleton);
            _container.Register<IParsingResultValidator, ParsingResultValidator>(Lifestyle.Singleton);
            _container.Register<IDataFilePathProvider, InitialDataFilePathProvider>(Lifestyle.Singleton);
            _container.Register<IDataProvider, CsvDataProvider>(Lifestyle.Singleton);

            // FuzzyExpert
            _container.Register<IInferenceEngine, InferenceGraph>(Lifestyle.Singleton);
            _container.Register<IFuzzyEngine, FuzzyEngine>(Lifestyle.Singleton);
            _container.Register<IExpert, Application.InferenceExpert.Implementations.FuzzyExpert>(Lifestyle.Singleton);

            // Models and ViewModels
            _container.Register<InferencingActionsModel>(Lifestyle.Singleton);
            _container.Register<InferencingActions>(Lifestyle.Singleton);
            _container.Register<ProfilingActionsModel>(Lifestyle.Singleton);
            _container.Register<ProfilingActions>(Lifestyle.Singleton);
            _container.Register<AddProfileActionModel>(Lifestyle.Singleton);
            _container.Register<AddProfileAction>(Lifestyle.Singleton);
            _container.Register<FuzzyExpertActionsModel>(Lifestyle.Singleton);
            _container.Register<FuzzyExpertActions>(Lifestyle.Singleton);

            // Logging
            _container.Register<IInferenceResultLogger, FileInferenceResultLogger>(Lifestyle.Singleton);

            _container.Verify();
            return _container;
        }
    }
}