using ProductionRuleParser.Entities;

namespace ProductionRuleParser.Interfaces
{
    public interface IImplicationRuleCreator
    {
        ImplicationRule CreateImplicationRuleEntity(ImplicationRuleStrings implicationRuleStrings);
    }
}