﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Forms
{
    public record CancelProductionInQueue
    {
        public required ObjectId BuildingContainer { get; init; }
        public required ObjectId InstructionId { get; init; }
    }
}