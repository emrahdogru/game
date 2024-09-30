using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Forms
{
    public record AddToProductionQueueForm
    {
        public required ObjectId BuildingContainer { get; set; }
        public required string ItemKey { get; set; }
        public required int Amount { get; set; }
    }
}
