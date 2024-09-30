using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Repositories
{
    public class UserRepository : Repository<User>
    {
        public static IQueryable<Player> GetPlayers(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return PlayerRepository.GetAll().Where(x => x.UserId == user.Id);
        }

        public static User? FindByEmail(string email)
        {
            email = email.Trim().ToLower(System.Globalization.CultureInfo.GetCultureInfo("en-us"));
            return GetAll().FirstOrDefault(x => x.Email == email);
        }
    }
}
