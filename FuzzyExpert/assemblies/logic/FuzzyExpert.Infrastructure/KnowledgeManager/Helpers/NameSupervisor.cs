using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Common.Interfaces;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;

namespace FuzzyExpert.Infrastructure.KnowledgeManager.Helpers
{
    public class NameSupervisor : INameSupervisor
    {
        private readonly INameProvider _nameProvider;

        public NameSupervisor(INameProvider nameProvider)
        {
            _nameProvider = nameProvider ?? throw new ArgumentNullException(nameof(nameProvider));
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