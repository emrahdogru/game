using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain.Techs
{
    public abstract class TechObject
    {
        public string Key => this.GetType().Name;
        public abstract string Name { get; }
        public abstract string Description { get; }
    }
}
