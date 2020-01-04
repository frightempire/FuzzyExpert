using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Implementations
{
    public class LinguisticVariableRelationsInitializer : ILinguisticVariableRelationsInitializer
    {
        public List<LinguisticVariableRelations> FormRelations(
            Dictionary<int, ImplicationRule> implicationRules,
            Dictionary<int, LinguisticVariable> linguisticVariables)
        {
            List<LinguisticVariableRelations> relations = new List<LinguisticVariableRelations>();

            List<UnaryStatement> ifUnaryStatements = implicationRules.SelectMany(ir => ir.Value.IfStatement.SelectMany(ifs => ifs.UnaryStatements)).ToList();
            List<UnaryStatement> thenUnaryStatements = implicationRules.SelectMany(ir => ir.Value.ThenStatement.UnaryStatements).ToList();
            List<UnaryStatement> allUnaryStatements = new List<UnaryStatement>();
            allUnaryStatements.AddRange(ifUnaryStatements);
            allUnaryStatements.AddRange(thenUnaryStatements);

            foreach (KeyValuePair<int, LinguisticVariable> linguisticVariable in linguisticVariables)
            {
                List<string> relatedStatements = new List<string>();
                foreach (UnaryStatement unaryStatement in allUnaryStatements)
                {
                    if (linguisticVariable.Value.VariableName == unaryStatement.LeftOperand &&
                        !relatedStatements.Contains(unaryStatement.Name))
                    {
                        relatedStatements.Add(unaryStatement.Name);
                    }
                }
                relations.Add(new LinguisticVariableRelations(linguisticVariable.Key, relatedStatements));
            }

            return relations;
        }
    }
}