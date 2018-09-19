using System.Collections.Generic;
using CommonLogic;
using ProductionRuleManager.Interfaces;
using ProductionRulesParser.Entities;

namespace ProductionRuleManager.Implementations
{
    public class ImplicationRuleManager : IImplicationRuleManager
    {
        private readonly IImplicationRuleProvider _implicationRuleProvider;

        private List<ImplicationRule> _implicationRules;

        public ImplicationRuleManager(IImplicationRuleProvider implicationRuleProvider)
        {
            ExceptionAssert.IsNull(implicationRuleProvider);
            _implicationRuleProvider = implicationRuleProvider;
        }

        public List<ImplicationRule> ImplicationRules => 
            _implicationRules ?? (_implicationRules = _implicationRuleProvider.GetImplicationRules());
    }
}