using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War.Server.Models.Forms
{
    public interface ILoginForm
    {
        string Email { get; init; }
        string Password { get; init; }   
    }
}
