using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Services
{
    public class CommentRateService : ICommentRateService
    {
        private readonly IGameService _gameService;

        private readonly IUnitOfWork _unitOfWork;

        public CommentRateService(IUnitOfWork unitOfWork, IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.AutoDetectChange = false;
            _unitOfWork.ValidateOnSaveEnabled = false;
            _gameService = gameService;
        }

        public bool AddGameComment(long userId, long gameId, string comment)
        {
            try
            {
                var userGameCommentRate = GetUserGameCommentRateByGameIdAndUserId(userId, gameId);
                var result = false;
                if (userGameCommentRate == null)
                {
                    userGameCommentRate = new UserGameCommentRate {Comment = comment, GameId = gameId, UserId = userId};
                    _unitOfWork.UserGameCommentRateCommands.Add(userGameCommentRate);
                    result = _unitOfWork.Commit() > 0;
                }
                if (string.IsNullOrWhiteSpace(userGameCommentRate.Comment))
                {
                    userGameCommentRate.Comment = comment;
                    _unitOfWork.UserGameCommentRateCommands.Update(userGameCommentRate);
                    result = _unitOfWork.Commit() > 0;
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

        public bool AddGameRate(long userId, long gameId, int rate, out double overAllRate)
        {
            try
            {
                overAllRate = 0;
                var userGameCommentRate = GetUserGameCommentRateByGameIdAndUserId(userId, gameId);
                var result = false;
                if (userGameCommentRate == null)
                {
                    userGameCommentRate = new UserGameCommentRate {Rate = rate, GameId = gameId, UserId = userId};
                    _unitOfWork.UserGameCommentRateCommands.Add(userGameCommentRate);
                    result = _unitOfWork.Commit() > 0;
                }
                if (userGameCommentRate.Rate == 0)
                {
                    userGameCommentRate.Rate = rate;
                    _unitOfWork.UserGameCommentRateCommands.Update(userGameCommentRate);
                    result = _unitOfWork.Commit() > 0;
                }
                if (result)
                {
                    overAllRate = GetGameOverAllRating(gameId);
                    result = _gameService.UpdateGameOverAllRate(GetGameOverAllRating(gameId), gameId);
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

        private double GetGameOverAllRating(long gameId)
        {
            try
            {
                var query = new List<Expression<Func<UserGameCommentRate, bool>>>
                {
                    c => c.GameId == gameId,
                    c => c.Rate > 0
                };
                return _unitOfWork.UserGameCommentRateQueries.Query(query).Average(c => c.Rate);
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        private UserGameCommentRate GetUserGameCommentRateByGameIdAndUserId(long userId, long gameId)
        {
            try
            {
                var query = new List<Expression<Func<UserGameCommentRate, bool>>>
                {
                    c => c.GameId == gameId & c.UserId == userId
                };
                return _unitOfWork.UserGameCommentRateQueries.Query(query).FirstOrDefault();
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