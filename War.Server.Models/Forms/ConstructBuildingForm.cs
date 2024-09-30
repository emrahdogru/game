using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Forms
{
    public record ConstructBuildingForm
    {
        public required string BuildingKey { get; init; }
        public required int WorkerCount { get; init; }
    }
}
