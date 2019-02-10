using System.Collections.Generic;
using CommonLogic;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class ImplicationRuleManager : IImplicationRuleManager
    {
        private readonly IImplicationRuleProvider _implicationRuleProvider;

        private Dictionary<int, ImplicationRule> _implicationRules;

        public ImplicationRuleManager(IImplicationRuleProvider implicationRuleProvider)
        {
            ExceptionAssert.IsNull(implicationRuleProvider);
            _implicationRuleProvider = implicationRuleProvider;
        }

        public Dictionary<int, ImplicationRule> ImplicationRules
        {
            get
            {
                if (_implicationRules != null)
                    return _implicationRules;

                List<ImplicationRule> implicationRulesFromProvider = _implicationRuleProvider.GetImplicationRules();

                Dictionary<int, ImplicationRule> implicationRules = new Dictionary<int, ImplicationRule>();
                for (int i = 1; i <= implicationRulesFromProvider.Count; i++)
                {
                    implicationRules.Add(i, implicationRulesFromProvider[i-1]);
                }
                return _implicationRules = implicationRules;
            }
        }
    }
}