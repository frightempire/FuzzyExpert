namespace ProductionRulesParser.Interfaces
{
    public interface IImplicationRulePreProcessor
    {
        void ValidateImplicationRule(string implicationRule);

        string PreProcessImplicationRule(string implicationRule);
    }
}