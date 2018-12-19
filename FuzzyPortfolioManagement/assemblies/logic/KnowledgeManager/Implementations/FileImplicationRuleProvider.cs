using System.Collections.Generic;
using CommonLogic;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Interfaces;

namespace KnowledgeManager.Implementations
{
    public class FileImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IFileReader _fileReader;
        private readonly IFilePathProvider _filePathProvider;
        private readonly IImplicationRuleCreator _implicationRuleCreator;

        public FileImplicationRuleProvider(
            IFileReader fileReader,
            IFilePathProvider filePathProvider,
            IImplicationRuleCreator implicationRuleCreator)
        {
            ExceptionAssert.IsNull(fileReader);
            ExceptionAssert.IsNull(filePathProvider);
            ExceptionAssert.IsNull(implicationRuleCreator);

            _fileReader = fileReader;
            _filePathProvider = filePathProvider;
            _implicationRuleCreator = implicationRuleCreator;
        }

        public List<ImplicationRule> GetImplicationRules()
        {
            ExceptionAssert.IsNull(_filePathProvider.FilePath);
            ExceptionAssert.FileExists(_filePathProvider.FilePath);

            List<string> implicationRulesFromFile = _fileReader.ReadFileByLines(_filePathProvider.FilePath);

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