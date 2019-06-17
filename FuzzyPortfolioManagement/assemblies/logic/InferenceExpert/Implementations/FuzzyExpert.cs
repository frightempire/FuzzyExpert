using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using DataProvider.Interfaces;
using FuzzificationEngine.Interfaces;
using InferenceEngine.Interfaces;
using InferenceExpert.Interfaces;
using KnowledgeManager.Entities;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;

namespace InferenceExpert.Implementations
{
    public class FuzzyExpert : IExpert
    {
        private readonly IDataProvider _initialDataProvider;
        private readonly IKnowledgeBaseManager _knowledgeManager;
        private readonly IInferenceEngine _inferenceEngine;
        private readonly IFuzzyEngine _fuzzyEngine;

        public FuzzyExpert(
            IDataProvider dataProvider,
            IKnowledgeBaseManager knowledgeBaseManager,
            IInferenceEngine inferenceEngine,
            IFuzzyEngine fuzzyEngine)
        {
            ExceptionAssert.IsNull(dataProvider);
            ExceptionAssert.IsNull(knowledgeBaseManager);
            ExceptionAssert.IsNull(inferenceEngine);
            ExceptionAssert.IsNull(fuzzyEngine);
            _initialDataProvider = dataProvider;
            _knowledgeManager = knowledgeBaseManager;
            _inferenceEngine = inferenceEngine;
            _fuzzyEngine = fuzzyEngine;
        }

        // TODO: Add IsPresent<T> to avoid null checks
        public List<string> GetResult()
        {
            Dictionary<string, double> initialData = _initialDataProvider.GetInitialData();
            KnowledgeBase knowledgeBase = _knowledgeManager.GetKnowledgeBase();

            foreach (var implicationRule in knowledgeBase.ImplicationRules)
            {
                List<string> ifNodeNames = implicationRule.Value.IfStatement
                    .SelectMany(ifs => ifs.UnaryStatements.Select(us => us.Name))
                    .ToList();
                LogicalOperation operation = ifNodeNames.Count == 1 ? LogicalOperation.None : LogicalOperation.And;
                List<string> thenNodeNames = implicationRule.Value.ThenStatement.UnaryStatements
                    .Select(us => us.Name)
                    .ToList();
                _inferenceEngine.AddRule(ifNodeNames, operation, thenNodeNames);
            }

            List<UnaryStatement> ifUnaryStatements = knowledgeBase.ImplicationRules
                .SelectMany(ir => ir.Value.IfStatement.SelectMany(ifs => ifs.UnaryStatements))
                .ToList();

            List<string> activatedNodes = new List<string>();

            foreach (var data in initialData)
            {
                KeyValuePair<int, LinguisticVariable> matchingVariable = knowledgeBase.LinguisticVariables
                    .SingleOrDefault(lv => lv.Value.VariableName == data.Key);
                List<string> relatedStatementNames = knowledgeBase.LinguisticVariablesRelations
                    .SingleOrDefault(lvr => lvr.LinguisticVariableNumber == matchingVariable.Key)
                    ?.RelatedUnaryStatementNames;

                MembershipFunction membershipFunction = _fuzzyEngine.Fuzzify(matchingVariable.Value, data.Value);

                foreach (var name in relatedStatementNames)
                {
                    UnaryStatement matchingStatement = ifUnaryStatements
                        .FirstOrDefault(ius => ius.Name == name && ius.RightOperand == membershipFunction.LinguisticVariableName);
                    if (matchingStatement != null) activatedNodes.Add(name);
                }
            }

            return _inferenceEngine.GetInferenceResults(activatedNodes);
        }
    }
}