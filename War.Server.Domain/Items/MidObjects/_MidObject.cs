using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Items.MidObjects
{
    public abstract class MidObject : ItemObject
    {
        public override string Type { get; } = nameof(MidObject);
        public override bool CanCarry { get; } = false;
        public override bool IsCarryable { get; } = true;
    }
}
