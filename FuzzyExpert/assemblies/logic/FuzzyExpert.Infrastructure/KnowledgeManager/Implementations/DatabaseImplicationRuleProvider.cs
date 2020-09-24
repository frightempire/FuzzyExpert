using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class DatabaseImplicationRuleProvider : IImplicationRuleProvider
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IImplicationRuleCreator _implicationRuleCreator;

        public DatabaseImplicationRuleProvider(
            IProfileRepository fileOperations,
            IImplicationRuleCreator implicationRuleCreator)
        {
            _profileRepository = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
            _implicationRuleCreator = implicationRuleCreator ?? throw new ArgumentNullException(nameof(implicationRuleCreator));
        }

        public Optional<List<ImplicationRule>> GetImplicationRules(string profileName)
        {
            var profile = _profileRepository.GetProfileByName(profileName);
            if (!profile.IsPresent || profile.Value.Rules == null || !profile.Value.Rules.Any())
            {
                return Optional<List<ImplicationRule>>.Empty();
            }

            List<ImplicationRule> implicationRules = new List<ImplicationRule>();
            foreach (var rule in profile.Value.Rules)
            {
                ImplicationRule implicationRule = _implicationRuleCreator.CreateImplicationRuleEntity(rule);
                implicationRules.Add(implicationRule);
            }

            List<ImplicationRule> separatedImplicationRules = DivideComplexImplicationRules(implicationRules);
            return Optional<List<ImplicationRule>>.For(separatedImplicationRules);
        }

        private List<ImplicationRule> DivideComplexImplicationRules(List<ImplicationRule> implicationRules)
        {
            List<ImplicationRule> grownRuleList = new List<ImplicationRule>();

            foreach (var implicationRule in implicationRules)
            {
                if (implicationRule.IfStatement.Count > 1)
                {
                    foreach (var statementCombination in implicationRule.IfStatement)
                    {
                        ImplicationRule dividedImplicationRule =
                            new ImplicationRule(new List<StatementCombination> {statementCombination},
                                implicationRule.ThenStatement);
                        grownRuleList.Add(dividedImplicationRule);
                    }
                }
                else
                {
                    grownRuleList.Add(implicationRule);
                }
            }

            return grownRuleList;
        }
    }
}