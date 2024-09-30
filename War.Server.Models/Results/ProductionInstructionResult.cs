using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static War.Server.Domain.MapObjects.City.BuildingContainerSequental;

namespace War.Server.Models.Results
{
    public class ProductionInstructionResult(ProductionInstruction pi)
    {
        public ObjectId Id => pi.Id;
        public int CompletedAmount => pi.CompletedAmount;
        public int Amount => pi.Amount;

        public string ItemKey => pi.Item.Key;

        public int RemainingAmount => pi.RemainingAmount;

        public int DurationPerItem => Convert.ToInt32((pi.EndDate - pi.StartDate).TotalSeconds / pi.Amount);

        public int RemainingDurationForCurrentItem => ((CompletedAmount + 1) * DurationPerItem) - Convert.ToInt32((DateTime.UtcNow - pi.StartDate).TotalSeconds);

        public bool IsStarted => pi.StartDate <= DateTime.UtcNow;
    }
}
