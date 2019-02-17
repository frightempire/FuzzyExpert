using CommonLogic.Entities;

namespace ProductionRuleParser.Interfaces
{
    public interface IImplicationRuleValidator
    {
        ValidationOperationResult ValidateImplicationRule(string implicationRule);
    }
}