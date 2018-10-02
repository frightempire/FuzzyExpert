using System.Collections.Generic;
using CommonLogic;
using ProductionRuleManager.Interfaces;
using ProductionRulesParser.Entities;

namespace ProductionRuleManager.Implementations
{
    public class FileImplicationRuleManager : IImplicationRuleManager
    {
        private readonly IImplicationRuleProvider _implicationRuleProvider;

        private List<ImplicationRule> _implicationRules;

        public FileImplicationRuleManager(IImplicationRuleProvider implicationRuleProvider)
        {
            ExceptionAssert.IsNull(implicationRuleProvider);
            _implicationRuleProvider = implicationRuleProvider;
        }

        public List<ImplicationRule> ImplicationRules => _implicationRules ?? (_implicationRules = _implicationRuleProvider.GetImplicationRules());
    }
}