using MongoDB.Bson;
using War.Server.Domain;

namespace War.Server.Models.Summaries
{
    public class UserSummary
    {
        public UserSummary(User user)
        {
            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.FullName = user.FullName;
        }

        public ObjectId Id { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string FullName { get; }
    }
}
