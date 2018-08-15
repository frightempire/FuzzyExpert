using System;
using System.Collections.Generic;
using CommonLogic;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Interfaces;

namespace ProductionRulesParser.Implementations
{
    public class ImplicationRuleParser : IImplicationRuleParser
    {
        private readonly IImplicationRuleHelper _implicationRuleHelper;

        public ImplicationRuleParser(IImplicationRuleHelper implicationRuleHelper)
        {
            ExceptionAssert.IsNull(implicationRuleHelper);

            _implicationRuleHelper = implicationRuleHelper;
        }

        public List<string> DivideImplicationRule(string implicationRule)
        {
            _implicationRuleHelper.ValidateImplicationRule(implicationRule);

            int index = 0;
            string implicationRuleToHandle = _implicationRuleHelper.PreProcessImplicationRule(implicationRule);
            List<object> ruleParts = _implicationRuleHelper.GetRuleParts(implicationRuleToHandle, ref index);

            return new List<string>();
        }

        public ImplicationRule ParseImplicationRule(string implicationRule)
        {
            throw new NotImplementedException();
        }
    }
}