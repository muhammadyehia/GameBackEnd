using System.Collections.Generic;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Application.Models
{
    public class FilterGamesModel
    {
        public List<GameModel> Games { get; set; }
        public FilterModel Filter { get; set; }
    }
}