using System;
using System.Web.Http;
using GreentubeGameTask.Application.Models;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Application.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult LoginOrRegister(UserDto user)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                UserModel result;
                var systemUser = _userService.Login(user.Name);
                if (systemUser != null)
                {
                    result = new UserModel {Id = systemUser.Id, Name = systemUser.Name};
                    return Ok(result);
                }
                systemUser = _userService.AddNewUser(user.Name);
                if (systemUser.Id == 0) return BadRequest("Faild to register user ");
                result = new UserModel {Id = systemUser.Id, Name = systemUser.Name};
                return Ok(result);
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