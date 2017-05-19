using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _unitOfWork.AutoDetectChange = false;
            _unitOfWork.ValidateOnSaveEnabled = false;
        }

        public User AddNewUser(string userName)
        {
            try
            {
                var user = new User {Name = userName};
                _unitOfWork.UserCommands.Add(user);
                _unitOfWork.Commit();
                return user;
            }
            catch (Exception ex)
            {
                // log information here
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        public User Login(string userName)
        {
            try
            {
                var query = new List<Expression<Func<User, bool>>>
                {
                    c => c.Name.ToLower() == userName.ToLower()
                };
                return _unitOfWork.UserQueries.Query(query).FirstOrDefault();
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