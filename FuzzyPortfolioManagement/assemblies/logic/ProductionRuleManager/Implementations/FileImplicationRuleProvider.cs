﻿using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Interfaces;
using ProductionRuleManager.Interfaces;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Interfaces;

namespace ProductionRuleManager.Implementations
{
    public class FileImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IFileReader _fileReader;
        private readonly IImplicationRuleCreator _implicationRuleCreator;

        public FileImplicationRuleProvider(
            IFileReader fileReader,
            IImplicationRuleCreator implicationRuleCreator)
        {
            ExceptionAssert.IsNull(fileReader);
            ExceptionAssert.IsNull(implicationRuleCreator);

            _fileReader = fileReader;
            _implicationRuleCreator = implicationRuleCreator;
        }

        public string FilePath { get; set; }

        public List<ImplicationRule> GetImplicationRules()
        {
            ExceptionAssert.IsNull(FilePath);
            ExceptionAssert.FileExists(FilePath);

            List<string> implicationRulesFromFile = _fileReader.ReadFileByLines(FilePath);

            List<ImplicationRule> implicationRules = new List<ImplicationRule>();
            implicationRulesFromFile.ForEach(irff =>
            {
                ImplicationRuleStrings implicationRuleStrings =
                    _implicationRuleCreator.DivideImplicationRule(irff);
                ImplicationRule implicationRule =
                    _implicationRuleCreator.CreateImplicationRuleEntity(implicationRuleStrings);
                implicationRules.Add(implicationRule);
            });

            return implicationRules;
        }
    }
}