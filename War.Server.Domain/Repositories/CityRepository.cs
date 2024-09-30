using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.MapObjects;

namespace War.Server.Domain.Repositories
{
    public class CityRepository : Repository<City>
    {
        public static IQueryable<City> GetUserCities(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            return GetAll().Where(x => x.UserId == user.Id);
        }
    }
}
