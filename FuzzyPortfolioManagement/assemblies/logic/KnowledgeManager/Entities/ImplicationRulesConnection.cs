namespace KnowledgeManager.Entities
{
    public class ImplicationRulesConnection
    {
        public ImplicationRulesConnection(int connectedRuleNumber, bool isReached = false)
        {
            ConnectedRuleNumber = connectedRuleNumber;
            IsReached = isReached;
        }

        public int ConnectedRuleNumber { get; }

        public bool IsReached { get; set; }
    }
}