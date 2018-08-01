using System.Collections.Generic;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.Entities
{
    public class ProductionRule
    {
        public List<UnaryStatement> IfStatement { get; set; }

        public List<LogicalOperation> LogicalOperationsOrder { get; set; }

        public UnaryStatement ThenStatement { get; set; }
    }
}
