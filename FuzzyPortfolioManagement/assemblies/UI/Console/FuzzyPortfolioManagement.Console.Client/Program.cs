using System.IO;
using FuzzyExpert.Application.Contracts;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Application.InferenceExpert.Interfaces;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;
using FuzzyPortfolioManagement.Console.Client.DependencyInjection;
using SimpleInjector;
using SystemConsole = System.Console;

namespace FuzzyPortfolioManagement.Console.Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            SystemConsole.WriteLine("Specify knowledge base :");
            SystemConsole.WriteLine("Implication rules :");
            string implicationRulesPath = SystemConsole.ReadLine();
            SystemConsole.WriteLine("Linguistic variables :");
            string linguisticVariablesPath = SystemConsole.ReadLine();
            SystemConsole.WriteLine("Specify initial data :");
            string initialDataPath = SystemConsole.ReadLine();
            SystemConsole.WriteLine();

            Container container = new SimpleInjectorContainerFactory().CreateSimpleInjectorContainer();
            SimpleInjectorResolver containerResolver = new SimpleInjectorResolver(container);
            var expert = (IExpert) containerResolver.Resolve(typeof(IExpert));
            var implicationRuleFilePathProvider = (IImplicationRuleFilePathProvider) containerResolver.Resolve(typeof(IImplicationRuleFilePathProvider));
            var linguisticVariableFilePathProvider = (ILinguisticVariableFilePathProvider)containerResolver.Resolve(typeof(ILinguisticVariableFilePathProvider));
            var initialDataFilePathProvider = (IDataFilePathProvider)containerResolver.Resolve(typeof(IDataFilePathProvider));
            var knowledgeBaseManager = (IKnowledgeBaseManager) containerResolver.Resolve(typeof(IKnowledgeBaseManager));
            var resultLogging = (IInferenceResultLogger) containerResolver.Resolve(typeof(IInferenceResultLogger));

            implicationRuleFilePathProvider.FilePath = implicationRulesPath;
            linguisticVariableFilePathProvider.FilePath = linguisticVariablesPath;
            initialDataFilePathProvider.FilePath = initialDataPath;

            File.Delete(resultLogging.LogPath);
            resultLogging.LogImplicationRules(knowledgeBaseManager.GetKnowledgeBase().Value.ImplicationRules);

            ExpertOpinion expertOpinion = expert.GetResult();
            if (expertOpinion.IsSuccess)
            {
                SystemConsole.WriteLine("Inference successful!\n");
                resultLogging.LogInferenceResult(expertOpinion.Result);
            }
            else
            {
                SystemConsole.WriteLine("Inference failed!\n");
                resultLogging.LogInferenceErrors(expertOpinion.ErrorMessages);
            }

            SystemConsole.ReadKey();
        }
    }
}