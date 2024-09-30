using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Domain
{
    public interface IGameBoard : IEntity
    {
        double Phase { get; set; }
    }
}
