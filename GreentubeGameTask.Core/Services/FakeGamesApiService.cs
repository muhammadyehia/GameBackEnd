using System.Collections.Generic;
using FakeGamesApi;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Services
{
    public class FakeGamesApiService : IFakeGamesApiService
    {
        public List<Game> GetGames()
        {
            return new FakeGamesApi.FakeGamesApi().GetGames();
        }
    }
}