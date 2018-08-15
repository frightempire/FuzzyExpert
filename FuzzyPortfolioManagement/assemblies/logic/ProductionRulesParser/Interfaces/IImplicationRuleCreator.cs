using System;
using System.Collections.Generic;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleCreator
    {
        List<ImplicationRule> CreateImplicationRules(string implicationRuleString);
    }

    public class ImplicationRuleCreator : IImplicationRuleCreator
    {
        public ImplicationRuleCreator()
        {
            
        }

        public List<ImplicationRule> CreateImplicationRules(string implicationRuleString)
        {
            throw new NotImplementedException();
        }
    }
}
