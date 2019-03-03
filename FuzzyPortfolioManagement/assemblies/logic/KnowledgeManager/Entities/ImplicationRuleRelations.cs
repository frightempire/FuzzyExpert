using System.Collections.Generic;
using CommonLogic;

namespace KnowledgeManager.Entities
{
    public class ImplicationRuleRelations
    {
        public ImplicationRuleRelations(
            int implicationRuleNumber,
            List<ImplicationRulesConnection> antecedentRuleNumbers,
            List<ImplicationRulesConnection> decendentRuleNumbers,
            List<int> linguisticVariableNumbers)
        {
            ExceptionAssert.IsNull(antecedentRuleNumbers);
            ExceptionAssert.IsNull(decendentRuleNumbers);
            ExceptionAssert.IsNull(linguisticVariableNumbers);
            ExceptionAssert.IsEmpty(linguisticVariableNumbers);

            ImplicationRuleNumber = implicationRuleNumber;
            AntecedentRuleNumbers = antecedentRuleNumbers;
            DecendentRuleNumbers = decendentRuleNumbers;
            LinguisticVariableNumbers = linguisticVariableNumbers;
        }

        public int ImplicationRuleNumber { get; }

        public List<ImplicationRulesConnection> AntecedentRuleNumbers { get; }

        public List<ImplicationRulesConnection> DecendentRuleNumbers { get; }

        public List<int> LinguisticVariableNumbers { get; }
    }
}