namespace GreentubeGameTask.Core.Interfaces
{
    public interface ICommentRateService
    {
        bool AddGameComment(long userId, long gameId, string comment);
        bool AddGameRate(long userId, long gameId, int rate, out double overAllRate);
    }
}