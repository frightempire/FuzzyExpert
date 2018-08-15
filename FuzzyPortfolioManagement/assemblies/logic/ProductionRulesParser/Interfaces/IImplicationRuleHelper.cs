using System.Collections.Generic;

namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRuleHelper
    {
        List<object> GetRuleParts(string implicationRule, ref int index);

        void ValidateImplicationRule(string implicationRule);

        string PreProcessImplicationRule(string implicationRule);
    }
}
