using LiteDB;

namespace FuzzyExpert.Infrastructure.DatabaseManagement.Entities
{
    public class User
    {
        [BsonId]
        public string UserName { get; set; }

        // TODO: Change to SecureString
        public string Password { get; set; }
    }
}