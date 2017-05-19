using System;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Entities
{
    public class UserGameCommentRate : ICreatedOn
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long GameId { get; set; }
        public string Comment { get; set; }

        public int Rate { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}