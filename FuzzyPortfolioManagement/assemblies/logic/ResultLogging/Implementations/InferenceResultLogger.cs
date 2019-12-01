using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using CommonLogic.Interfaces;
using ProductionRuleParser.Entities;
using ResultLogging.Interfaces;

namespace ResultLogging.Implementations
{
    public class InferenceResultLogger : IInferenceResultLogger
    {
        private readonly IFileOperations _fileOperations;

        public InferenceResultLogger(IFileOperations fileOperations)
        {
            ExceptionAssert.IsNull(fileOperations);
            _fileOperations = fileOperations;
        }

        public void LogImplicationRules(Dictionary<int, ImplicationRule> implicationRules)
        {
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + @"\ResultLog.txt";
            List<string> rules = implicationRules.Select(rule => $"Implication rule {rule.Key} : {rule.Value}").ToList();
            _fileOperations.AppendLinesToFile(pathToFile, rules);
        }

        public void LogInferenceResult(Dictionary<string, double> inferenceResult)
        {
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + @"\ResultLog.txt";
            List<string> results = inferenceResult.Select(result => $"Node {result.Key} was enabled with confidence factor {result.Value}").ToList();
            _fileOperations.AppendLinesToFile(pathToFile, results);
        }
    }
}