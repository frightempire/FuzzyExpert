using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyExpert.Core.Entities
{
    public class StatementCombination
    {
        // List of statement combinations - divided by OR
        public StatementCombination(List<UnaryStatement> unaryStatements)
        {
            if (unaryStatements == null) throw new ArgumentNullException(nameof(unaryStatements));
            if (!unaryStatements.Any()) throw new ArgumentNullException(nameof(unaryStatements));

            UnaryStatements = unaryStatements;
        }

        // List of unary statements - divided by AND
        public List<UnaryStatement> UnaryStatements { get; }
    }
}