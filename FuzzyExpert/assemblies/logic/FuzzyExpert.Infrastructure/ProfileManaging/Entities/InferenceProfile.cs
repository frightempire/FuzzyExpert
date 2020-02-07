using System.Collections.Generic;
using LiteDB;

namespace FuzzyExpert.Infrastructure.ProfileManaging.Entities
{
    public class InferenceProfile
    {
        [BsonId]
        public string ProfileName { get; set; }

        public string Description { get; set; }

        public List<string> Rules { get; set; }

        public List<string> Variables { get; set; }
    }
}