using System;
using System.Collections.Generic;
using System.Linq;
using CommonLogic;
using CommonLogic.Interfaces;
using KnowledgeManager.Interfaces;
using ProductionRuleParser.Entities;

namespace KnowledgeManager.Helpers
{
    public class NameSupervisor : INameSupervisor
    {
        private readonly INameProvider _nameProvider;

        public NameSupervisor(INameProvider nameProvider)
        {
            ExceptionAssert.IsNull(nameProvider);
            _nameProvider = nameProvider;
        }

        public void AssignNames(List<UnaryStatement> unaryStatements)
        {
            List<Tuple<string, string>> cachedNames = new List<Tuple<string, string>>();
            foreach (var statement in unaryStatements)
            {
                Tuple<string, string> correspondingName = cachedNames.FirstOrDefault(n => n.Item1 == statement.ToString());
                if (correspondingName == null)
                {
                    string name = _nameProvider.GetName();
                    statement.Name = name;
                    cachedNames.Add(new Tuple<string, string>(statement.ToString(), name));
                }
                else
                {
                    statement.Name = correspondingName.Item2;
                }
            }
        }
    }
}