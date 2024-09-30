using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;
using War.Server.Domain.MapObjects;

namespace War.Server.Models.Summaries
{
    public class CitySummary(City city)
    {
        public ObjectId Id => city.Id;
        public UserSummary? User => city.User == null ? null : new UserSummary(city.User);
        public string Name => city.Name;
        public Point Location = city.Location;
    }
}
