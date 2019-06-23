using System.Collections.Generic;
using CommonLogic.Implementations;
using CommonLogic.Interfaces;
using DataProvider.Implementations;
using DataProvider.Interfaces;
using FuzzificationEngine.Implementaions;
using FuzzificationEngine.Interfaces;
using InferenceEngine.Implementations;
using InferenceEngine.Interfaces;
using InferenceExpert.Implementations;
using InferenceExpert.Interfaces;
using KnowledgeManager.Helpers;
using KnowledgeManager.Implementations;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Implementations;
using LinguisticVariableParser.Interfaces;
using MembershipFunctionParser.Implementations;
using MembershipFunctionParser.Interfaces;
using ProductionRuleParser.Implementations;
using ProductionRuleParser.Interfaces;
using SimpleInjector;

namespace FuzzyPortfolioManagement.Console.Client.DependencyInjection
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
            _container.Register<IMembershipFunctionParser, MembershipFunctionParser.Implementations.MembershipFunctionParser>(Lifestyle.Singleton);
            _container.Register<IMembershipFunctionCreator, MembershipFunctionCreator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableValidator, LinguisticVariableValidator>(Lifestyle.Singleton);
            _container.Register<ILinguisticVariableParser, LinguisticVariableParser.Implementations.LinguisticVariableParser>(Lifestyle.Singleton);
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
            _container.Register<IExpert, FuzzyExpert>(Lifestyle.Singleton);

            _container.Verify();
            return _container;
        }
    }
}