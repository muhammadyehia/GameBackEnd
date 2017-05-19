using System.Collections.Generic;
using FakeGamesApi;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface IFakeGamesApiService
    {
        List<Game> GetGames();
    }
}