using System.Collections.Generic;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface IGameService
    {
        IEnumerable<Game> GetFilterdGamesIncludeCommentsWithUsers(FilterModel filterModel);
        Game GetGameById(long gameId);
        int UpdateNewGamesFromApitoLocalDb();
        bool UpdateGameOverAllRate(double overAllRate, long gameId);
    }
}