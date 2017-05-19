using System.Collections.Generic;

namespace GreentubeGameTask.Core.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserGameCommentRate> UserGameCommentsRates { get; set; }
    }
}