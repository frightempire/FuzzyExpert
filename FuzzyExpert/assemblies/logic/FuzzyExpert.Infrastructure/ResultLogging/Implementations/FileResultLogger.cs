using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.ResultLogging.Implementations
{
    public class FileResultLogger : IResultLogger
    {
        private readonly IFileOperations _fileOperations;

        public FileResultLogger(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        public string ResultLogPath => AppDomain.CurrentDomain.BaseDirectory + @"\ResultLog.txt";

        public string ValidationLogPath => AppDomain.CurrentDomain.BaseDirectory + @"\ValidationLog.txt";

        public void LogImplicationRules(Dictionary<int, ImplicationRule> implicationRules)
        {
            List<string> rules = implicationRules.Select(rule => $"Implication rule {rule.Key} : {rule.Value}").ToList();
            _fileOperations.AppendLinesToFile(ResultLogPath, rules);
        }

        public void LogInferenceResult(Dictionary<string, double> inferenceResult)
        {
            List<string> results = inferenceResult.Select(result => $"Node {result.Key} was enabled with confidence factor {result.Value}").ToList();
            _fileOperations.AppendLinesToFile(ResultLogPath, results);
        }

        public void LogInferenceErrors(List<string> inferenceErrors)
        {
            _fileOperations.AppendLinesToFile(ResultLogPath, inferenceErrors);
        }

        public void LogValidationErrors(List<string> validationErrors)
        {
            _fileOperations.AppendLinesToFile(ValidationLogPath, validationErrors);
        }
    }
}