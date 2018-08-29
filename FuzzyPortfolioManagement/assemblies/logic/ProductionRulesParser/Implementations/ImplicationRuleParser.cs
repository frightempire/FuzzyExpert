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

            string preProcessedImplicationRule = _implicationRuleHelper.PreProcessImplicationRule(implicationRule);



            List<string> simplifiedRules = _implicationRuleHelper.GetStatementParts(ref preProcessedImplicationRule);

            return new List<string>();
        }

        public ImplicationRule CreateImplicationRuleEntity(string implicationRule)
        {
            throw new NotImplementedException();
        }
    }
}