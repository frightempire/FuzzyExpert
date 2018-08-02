using System;
using ProductionRulesParser.Entities;

namespace ProductionRulesParser.Interfaces
{
    public interface IProductionRuleCreator
    {
        ProductionRule CreateProductionRule(string productionRuleString);
    }

    public class ProductionRuleCreator : IProductionRuleCreator
    {
        public ProductionRule CreateProductionRule(string productionRuleString)
        {
            throw new NotImplementedException();
        }
    }
}
