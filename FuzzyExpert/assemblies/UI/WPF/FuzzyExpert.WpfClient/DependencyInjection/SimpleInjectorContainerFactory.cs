using System.Collections.Generic;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Core.FuzzificationEngine.Implementations;
using FuzzyExpert.Core.FuzzificationEngine.Interfaces;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Interfaces;
using FuzzyExpert.ImplicationRuleSelectorAction.Panels;
using FuzzyExpert.ImplicationRuleSelectorAction.ViewModels;
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
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using SimpleInjector;

namespace FuzzyExpert.WpfClient.DependencyInjection
{
    public class SimpleInjectorContainerFactory
    {
        private readonly Container _container = new Container();

        public Container CreateSimpleInjectorContainer()
        {
            // KnowledgeBaseManager
            _container.Register<IFileOperations, FileOperations>(Lifestyle.Singleton);
            _container.Register<IValidationOperationResultLogger, FileValidationOperationResultLogger>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleValidator, ImplicationRuleValidator>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleParser, ImplicationRuleParser>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleCreator, ImplicationRuleCreator>(Lifestyle.Singleton);
            _container.Register<INameProvider, UniqueNameProvider>(Lifestyle.Singleton);
            _container.Register<INameSupervisor, NameSupervisor>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleFilePathProvider, ImplicationRuleFilePathProvider>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleProvider, FileImplicationRuleProvider>(Lifestyle.Singleton);
            _container.Register<IImplicationRuleManager, ImplicationRuleManager>(Lifestyle.Singleton);

            _container.Register<IMembershipFunctionValidator, MembershipFunctionValidator>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionParser, MembershipFunctionParser>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionCreator, MembershipFunctionCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableValidator, LinguisticVariableValidator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableParser, LinguisticVariableParser>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableCreator, LinguisticVariableCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableFilePathProvider, LinguisticVariableFilePathProvider>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableProvider, FileLinguisticVariableProvider>(Lifestyle.Singleton);
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
            _container.Register<IExpert, FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert>(Lifestyle.Singleton);

            _container.Register<DataSelectorActionModel>(Lifestyle.Singleton);
            _container.Register<DataSelectorAction>(Lifestyle.Singleton);
            _container.Register<InferenceActionModel>(Lifestyle.Singleton);
            _container.Register<InferenceAction>(Lifestyle.Singleton);

            // Logging
            _container.Register<IInferenceResultLogger, FileInferenceResultLogger>(Lifestyle.Singleton);

            _container.Verify();
            return _container;
        }
    }
}