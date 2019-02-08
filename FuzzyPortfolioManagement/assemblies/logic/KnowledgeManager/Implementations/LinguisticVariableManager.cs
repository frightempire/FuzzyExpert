using System.Collections.Generic;
using CommonLogic;
using KnowledgeManager.Interfaces;
using LinguisticVariableParser.Entities;

namespace KnowledgeManager.Implementations
{
    public class LinguisticVariableManager : ILinguisticVariableManager
    {
        private readonly ILinguisticVariableProvider _linguisticVariableProvider;

        private List<LinguisticVariable> _linguisticVariables;

        public LinguisticVariableManager(ILinguisticVariableProvider linguisticVariableProvider)
        {
            ExceptionAssert.IsNull(linguisticVariableProvider);
            _linguisticVariableProvider = linguisticVariableProvider;
        }

        public List<LinguisticVariable> LinguisticVariables => _linguisticVariables ??
                                                               (_linguisticVariables = _linguisticVariableProvider.GetLinguisticVariables());
    }
}