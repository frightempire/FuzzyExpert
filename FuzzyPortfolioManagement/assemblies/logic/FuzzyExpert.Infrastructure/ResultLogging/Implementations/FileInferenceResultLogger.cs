using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.ResultLogging.Interfaces;

namespace FuzzyExpert.Infrastructure.ResultLogging.Implementations
{
    public class FileInferenceResultLogger : IInferenceResultLogger
    {
        private readonly IFileOperations _fileOperations;

        public FileInferenceResultLogger(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        public string LogPath => AppDomain.CurrentDomain.BaseDirectory + @"\ResultLog.txt";

        public void LogImplicationRules(Dictionary<int, ImplicationRule> implicationRules)
        {
            List<string> rules = implicationRules.Select(rule => $"Implication rule {rule.Key} : {rule.Value}").ToList();
            _fileOperations.AppendLinesToFile(LogPath, rules);
        }

        public void LogInferenceResult(Dictionary<string, double> inferenceResult)
        {
            List<string> results = inferenceResult.Select(result => $"Node {result.Key} was enabled with confidence factor {result.Value}").ToList();
            _fileOperations.AppendLinesToFile(LogPath, results);
        }

        public void LogInferenceErrors(List<string> inferenceErrors)
        {
            _fileOperations.AppendLinesToFile(LogPath, inferenceErrors);
        }
    }
}