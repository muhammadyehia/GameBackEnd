using System;
using System.Collections.Generic;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Entities
{
    public class Game : ICreatedOn
    {
        public long Id { get; set; }


        public string Name { get; set; }
        public double OverAllRate { get; set; }
        public int NumberOfVotes { get; set; }
        public virtual ICollection<UserGameCommentRate> UserGameCommentsRates { get; set; }


        public DateTime CreatedOn { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Game))
                throw new ArgumentException("obj is not an Game");
            var game = obj as Game;
            return game != null && Id.Equals(game.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}