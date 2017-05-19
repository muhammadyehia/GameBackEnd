using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IFakeGamesApiService _fakeGamesApiService;
        private readonly IUnitOfWork _unitOfWork;

        public GameService(IUnitOfWork unitOfWork, IFakeGamesApiService fakeGamesApiService)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.AutoDetectChange = false;
            _unitOfWork.ValidateOnSaveEnabled = false;
            _fakeGamesApiService = fakeGamesApiService;
        }

        public int UpdateNewGamesFromApitoLocalDb()
        {
            try
            {
                var apiGameList = _fakeGamesApiService.GetGames();
                if (apiGameList == null || !apiGameList.Any()) return 0;
                var gameList =
                    apiGameList.Select(g => new Game {Name = g.Name, Id = g.Id, CreatedOn = DateTime.Now}).ToList();
                var gamesIds = gameList.Select(i => i.Id).ToList();
                var query = new List<Expression<Func<Game, bool>>> {c => gamesIds.Contains((int) c.Id)};
                var gamesInDb = _unitOfWork.GameQueries.Query(query);
                var gamesNotInDb = new HashSet<Game>(gameList.Except(gamesInDb));
                if (gamesNotInDb.Any())
                {
                    _unitOfWork.GameCommands.AddBulk(gamesNotInDb);
                    _unitOfWork.BulkCommit();
                }
                return gamesNotInDb.Count;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        public IEnumerable<Game> GetFilterdGamesIncludeCommentsWithUsers(FilterModel filterModel)
        {
            try
            {
                Func<IQueryable<Game>, IOrderedQueryable<Game>> orderBy;
                var includes = new List<Expression<Func<Game, object>>>
                {
                    u => u.UserGameCommentsRates,
                    g => g.UserGameCommentsRates.Select(c => c.User)
                };

                if (filterModel.SortParameter == ConfigurationManager.AppSettings["AlphabeticSortParameter"])
                {
                    if (filterModel.SortDirection)
                        orderBy = c => c.OrderBy(d => d.Name);
                    else
                        orderBy = c => c.OrderByDescending(d => d.Name);
                }
                else if (filterModel.SortParameter == ConfigurationManager.AppSettings["RateSortParameter"])
                {
                    if (filterModel.SortDirection)
                        orderBy = c => c.OrderBy(d => d.OverAllRate);
                    else
                        orderBy = c => c.OrderByDescending(d => d.OverAllRate);
                }
                else
                {
                    if (filterModel.SortDirection)
                        orderBy = c => c.OrderBy(d => d.CreatedOn);
                    else
                        orderBy = c => c.OrderByDescending(d => d.CreatedOn);
                }
                long itemsCount;
                var result = _unitOfWork.GameQueries.GetPage(out itemsCount, filterModel.PageSize,
                    filterModel.SkipRecords,
                    orderBy: orderBy, includes: includes);
                filterModel.TotalRecords = itemsCount;
                filterModel.TotalPages = (filterModel.TotalRecords - 1)/filterModel.PageSize + 1;
                if (itemsCount > 0)
                {
                    var apiCallDate = DateTime.Now;

                    if (string.IsNullOrEmpty(filterModel.ApiCallDate))
                    {
                        apiCallDate = apiCallDate.AddDays(-1);
                    }

                    else if (
                        !DateTime.TryParseExact(filterModel.ApiCallDate,
                            ConfigurationManager.AppSettings["DateFormat"], CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out apiCallDate))
                    {
                        apiCallDate = DateTime.Now.AddDays(-1);
                    }

                    if ((DateTime.Now - apiCallDate).TotalDays >= 1)
                    {
                        if (CanGetNewGamesFromApi())
                        {
                            var gamesAdded = UpdateNewGamesFromApitoLocalDb();
                            filterModel.TotalRecords += gamesAdded;
                            filterModel.TotalPages = (filterModel.TotalRecords - 1)/filterModel.PageSize + 1;
                        }
                        filterModel.ApiCallDate = DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"],
                            CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        filterModel.ApiCallDate = apiCallDate.ToString(ConfigurationManager.AppSettings["DateFormat"],
                            CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    long totalRecords;
                    var newGames = AddGamesFromApiAndGetFirstTen(out totalRecords);
                    filterModel.TotalRecords = totalRecords;
                    filterModel.TotalPages = (filterModel.TotalRecords - 1)/filterModel.PageSize + 1;
                    filterModel.ApiCallDate = DateTime.Now.ToString(ConfigurationManager.AppSettings["DateFormat"]);
                    return newGames;
                }
                return result.AsEnumerable();
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }


        public Game GetGameById(long gameId)
        {
            try
            {
                var query = new List<Expression<Func<Game, bool>>> {c => c.Id == gameId};
                var game = _unitOfWork.GameQueries.Query(query).FirstOrDefault();
                return game;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        public bool UpdateGameOverAllRate(double overAllRate, long gameId)
        {
            try
            {
                var game = GetGameById(gameId);
                if (game == null) return false;
                game.OverAllRate = overAllRate;
                game.NumberOfVotes += 1;
                _unitOfWork.GameCommands.Update(game);
                return _unitOfWork.Commit() > 0;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        private IEnumerable<Game> AddGamesFromApiAndGetFirstTen(out long totalRecords)
        {
            try
            {
                var gameList = _fakeGamesApiService.GetGames();
                var firstTenGames = new List<Game>();
                if (gameList != null && gameList.Any())
                {
                    firstTenGames = gameList.Take(10).Select(g => new Game {Name = g.Name, Id = g.Id}).ToList();
                    totalRecords = gameList.Count;
                    SaveGamesFromApi(gameList);
                }
                else
                {
                    totalRecords = 0;
                }
                return firstTenGames;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        private void SaveGamesFromApi(IEnumerable<FakeGamesApi.Game> gameList)
        {
            try
            {
                _unitOfWork.GameCommands.AddBulk(
                    gameList.Select(g => new Game {Name = g.Name, Id = g.Id, CreatedOn = DateTime.Now}));
                _unitOfWork.BulkCommit();
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        public bool CanGetNewGamesFromApi()
        {
            try
            {
                var result = false;
                Func<IQueryable<Game>, IOrderedQueryable<Game>> orderBy = o => o.OrderByDescending(c => c.CreatedOn);
                var newestGame = _unitOfWork.GameQueries.Query(orderBy: orderBy).FirstOrDefault();
                if (newestGame != null)
                {
                    result = (DateTime.Now - newestGame.CreatedOn).TotalDays >= 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }
    }
}