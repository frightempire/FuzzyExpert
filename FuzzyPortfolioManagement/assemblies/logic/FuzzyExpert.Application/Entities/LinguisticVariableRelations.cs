using System.Collections.Generic;

namespace FuzzyExpert.Application.Entities
{
    public class LinguisticVariableRelations
    {
        public LinguisticVariableRelations(int linguisticVariableNumber, List<string> relatedUnaryStatementNames)
        {
            LinguisticVariableNumber = linguisticVariableNumber;
            RelatedUnaryStatementNames = relatedUnaryStatementNames;
        }

        public int LinguisticVariableNumber { get; }

        public List<string> RelatedUnaryStatementNames { get; }
    }
}