using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.ResourceObjects;
using War.Server.Domain.ObjectSets;

namespace War.Server.Domain.Items.PersonObjects
{
    public class Person : PersonObject
    {

        public override int CargoCapacity { get; } = 10;

        public override string Name { get; } = "Person";

        public override int Mass { get; } = 80;

        public override bool CanCarry { get; } = true;

        public override bool IsCarryable { get; } = true;

        public override int Strength { get; } = 5;

        public override ReceipeDetail Receipe
        {
            get
            {
                _receipe ??= new(30, new() {
                    { ItemObjectSet.Food, 10 }
                });

                return _receipe;
            }
        }
    }
}
