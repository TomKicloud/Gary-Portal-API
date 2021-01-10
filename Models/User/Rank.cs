using System;
using System.Collections.Generic;

namespace GaryPortalAPI.Models
{
    public class Rank
    {
        public int RankId { get; set; }
        public string RankName { get; set; }
        public int RankAccessLevel { get; set; }

        public virtual ICollection<UserRanks> UserAmigoRanks { get; set; }
        public virtual ICollection<UserRanks> UserPositivityRanks { get; set; }
    }
}
