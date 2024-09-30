﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.PersonObjects
{
    public class Engineer : PersonObject
    {
        public override int CargoCapacity { get; } = 0;

        public override string Name { get; } = "Engineer";

        public override int Mass { get; } = 80;

        public override bool CanCarry { get; } = false;

        public override bool IsCarryable { get; } = true;


        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(120, new() { { ItemObjectSet.Person, 1 } });
                return _receipe;
            }
        }
    }
}
