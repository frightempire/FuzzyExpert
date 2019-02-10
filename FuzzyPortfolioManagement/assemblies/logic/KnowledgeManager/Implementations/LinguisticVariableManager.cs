using System.Collections.Generic;
using CommonLogic;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class LinguisticVariableManager : ILinguisticVariableManager
    {
        private readonly ILinguisticVariableProvider _linguisticVariableProvider;

        private Dictionary<int, LinguisticVariable> _linguisticVariables;

        public LinguisticVariableManager(ILinguisticVariableProvider linguisticVariableProvider)
        {
            ExceptionAssert.IsNull(linguisticVariableProvider);
            _linguisticVariableProvider = linguisticVariableProvider;
        }

        public Dictionary<int, LinguisticVariable> LinguisticVariables
        {
            get
            {
                if (_linguisticVariables != null)
                    return _linguisticVariables;

                List<LinguisticVariable> linguisticVariablesFromProvider = _linguisticVariableProvider.GetLinguisticVariables();

                Dictionary<int, LinguisticVariable> linguisticVariables = new Dictionary<int, LinguisticVariable>();
                for (int i = 1; i <= linguisticVariablesFromProvider.Count; i++)
                {
                    linguisticVariables.Add(i, linguisticVariablesFromProvider[i - 1]);
                }
                return _linguisticVariables = linguisticVariables;
            }
        }
    }
}