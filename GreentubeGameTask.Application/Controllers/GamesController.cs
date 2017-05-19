using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GreentubeGameTask.Application.Models;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Application.Controllers
{
    public class GamesController : ApiController
    {
        private readonly ICommentRateService _commentRateService;
        private readonly IGameService _gameService;


        public GamesController(IGameService gameService, ICommentRateService commentRateService)
        {
            _gameService = gameService;
            _commentRateService = commentRateService;
        }

        [HttpGet]
        [Route("GetGamesCommentsRates")]
        public IHttpActionResult GetGamesIncludedCommentsWithUserNamesAndRates(int currentPage, int pageSize,
            bool sortDirection, string sortParameter, string apiCallDate = "")
        {
            try
            {
                var filterModel = new FilterModel
                {
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    SortDirection = sortDirection,
                    SortParameter = sortParameter,
                    ApiCallDate = apiCallDate
                };
                var gameListIncludeCommentsWithUsers = _gameService.GetFilterdGamesIncludeCommentsWithUsers(filterModel);
                if (gameListIncludeCommentsWithUsers != null)
                {
                    var projectedGameListIncludeCommentsWithUsers = gameListIncludeCommentsWithUsers
                        .Select(
                            g =>
                            {
                                if (g.UserGameCommentsRates != null)
                                {
                                    return new GameCommentsWithUserModel
                                    {
                                        Game =
                                            new GameModel
                                            {
                                                Id = g.Id,
                                                Name = g.Name,
                                                OverAllRate = g.OverAllRate,
                                                NumberOfVotes = g.NumberOfVotes
                                            },
                                        UsersCommentsWithRate =
                                            g.UserGameCommentsRates.Select(
                                                c =>
                                                    new UserCommentRateModel
                                                    {
                                                        UserName = c.User.Name,
                                                        Comment = c.Comment,
                                                        Rate = c.Rate
                                                    }).ToList()
                                    };
                                }
                                return new GameCommentsWithUserModel
                                {
                                    Game =
                                        new GameModel
                                        {
                                            Id = g.Id,
                                            Name = g.Name,
                                            OverAllRate = g.OverAllRate,
                                            NumberOfVotes = g.NumberOfVotes
                                        },
                                    UsersCommentsWithRate = new List<UserCommentRateModel>()
                                };
                            })
                        .ToList();
                    var result = new FilterGamesCommentsWithUserModel
                    {
                        Filter = filterModel,
                        GamesCommentsWithUser = projectedGameListIncludeCommentsWithUsers
                    };
                    return Ok(result);
                }
                return Ok();
            }
            catch (Exception)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                return BadRequest("Some thing is wrong happend");
            }
        }

        [HttpPost]
        [Route("addcomment")]
        public HttpResponseMessage AddGameComment(GameCommentDto gameCommentDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _commentRateService.AddGameComment(gameCommentDto.UserId, gameCommentDto.GameId,
                        gameCommentDto.Comment);
                    if (result)
                        return new HttpResponseMessage(HttpStatusCode.OK);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            catch (Exception)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some thing is wrong happend");
            }
        }

        [HttpPost]
        [Route("addrate")]
        public IHttpActionResult AddGameRate(GameRateDto gameRateDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    double overAllRate;
                    var result = _commentRateService.AddGameRate(gameRateDto.UserId, gameRateDto.GameId,
                        gameRateDto.Rate, out overAllRate);
                    if (result)
                        return Ok(overAllRate);
                }
                return BadRequest();
            }
            catch (Exception)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                return BadRequest("Some thing is wrong happend");
            }
        }
    }
}