using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain.Items.Attributes;

namespace War.Server.Domain.Items.ResourceObjects
{
    public abstract class ResourceObject : Items.ItemObject, IResourceAttribute
    {
        public override string Type { get; } = nameof(ResourceObject);
        public override int Strength { get; } = 10;

        public override bool CanCarry { get; } = false;
        public override bool IsCarryable { get; } = true;

        public override ReceipeDetail Receipe { get; } = new(5, []);
    }
}
