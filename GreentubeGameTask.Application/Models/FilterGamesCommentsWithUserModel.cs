using System.Collections.Generic;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Application.Models
{
    public class FilterGamesCommentsWithUserModel
    {
        public List<GameCommentsWithUserModel> GamesCommentsWithUser { get; set; }
        public FilterModel Filter { get; set; }
    }
}