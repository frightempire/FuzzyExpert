using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Entities
{
    public class Settings
    {
        [BsonId]
        public int Id { get; set; }

        public string UserName { get; set; }

        public double ConfidenceFactorLowerBoundary { get; set; }
    }
}