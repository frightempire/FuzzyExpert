using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Application.InferenceExpert.Entities;
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

        public string LogInferenceResult(Dictionary<int, ImplicationRule> implicationRules, ExpertOpinion expertOpinion, string userName)
        {
            var destinationPath = AppDomain.CurrentDomain.BaseDirectory + $@"\Results\{userName}\InferenceLog-{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}.txt";
            Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

            var rules = implicationRules.Select(rule => $"Implication rule {rule.Key} : {rule.Value}").ToList();
            _fileOperations.AppendLinesToFile(destinationPath, rules);

            if (expertOpinion.IsSuccess)
            {
                var results = expertOpinion.Result.Select(result => $"Node {result.Item1} was enabled with confidence factor {result.Item2}").ToList();
                _fileOperations.AppendLinesToFile(destinationPath, results);
            }
            else
            {
                _fileOperations.AppendLinesToFile(destinationPath, expertOpinion.ErrorMessages);
            }

            return destinationPath;
        }
    }
}