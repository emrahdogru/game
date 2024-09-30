using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using War.Server.Domain;

namespace War.Server.Models.Summaries
{
    public class UserProfileSummary : UserSummary
    {
        public UserProfileSummary(User user)
            : base(user)
        {
            Language = user.Language;
        }

        public Languages Language { get; }
    }
}
