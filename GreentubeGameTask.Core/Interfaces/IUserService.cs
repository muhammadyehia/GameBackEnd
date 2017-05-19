using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface IUserService
    {
        User AddNewUser(string userName);
        User Login(string userName);
    }
}