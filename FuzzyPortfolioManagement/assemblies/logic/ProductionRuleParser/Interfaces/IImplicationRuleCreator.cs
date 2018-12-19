using ProductionRuleParser.Entities;

namespace ProductionRuleParser.Interfaces
{
    public interface IImplicationRuleCreator
    {
        ImplicationRuleStrings DivideImplicationRule(string implicationRule);

        ImplicationRule CreateImplicationRuleEntity(ImplicationRuleStrings implicationRuleStrings);
    }
}