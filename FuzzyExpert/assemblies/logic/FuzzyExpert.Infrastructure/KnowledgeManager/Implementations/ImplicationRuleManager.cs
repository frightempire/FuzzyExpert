using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class ImplicationRuleManager : IImplicationRuleManager
    {
        private readonly IImplicationRuleProvider _implicationRuleProvider;

        private Optional<Dictionary<int, ImplicationRule>> _implicationRules = Optional<Dictionary<int, ImplicationRule>>.Empty();

        public ImplicationRuleManager(IImplicationRuleProvider implicationRuleProvider)
        {
            _implicationRuleProvider = implicationRuleProvider ?? throw new ArgumentNullException(nameof(implicationRuleProvider));
        }

        public Optional<Dictionary<int, ImplicationRule>> ImplicationRules
        {
            get
            {
                if (_implicationRules.IsPresent)
                {
                    return _implicationRules;
                }

                Optional<List<ImplicationRule>> implicationRulesFromProvider = _implicationRuleProvider.GetImplicationRules();
                if (!implicationRulesFromProvider.IsPresent)
                {
                    return Optional<Dictionary<int, ImplicationRule>>.Empty();
                }

                Dictionary<int, ImplicationRule> implicationRules = new Dictionary<int, ImplicationRule>();
                for (int i = 1; i <= implicationRulesFromProvider.Value.Count; i++)
                {
                    implicationRules.Add(i, implicationRulesFromProvider.Value[i-1]);
                }
                return _implicationRules = Optional<Dictionary<int, ImplicationRule>>.For(implicationRules);
            }
        }
    }
}