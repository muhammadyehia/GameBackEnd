using System.Collections.Generic;

namespace GreentubeGameTask.Application.Models
{
    public class GameCommentsWithUserModel
    {
        public GameModel Game { get; set; }
        public List<UserCommentRateModel> UsersCommentsWithRate { get; set; }
    }
}