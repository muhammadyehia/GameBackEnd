using System;
using GreentubeGameTask.Core.Entities;

namespace GreentubeGameTask.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IQueries<User> UserQueries { get; }
        ICommands<User> UserCommands { get; }
        IQueries<Game> GameQueries { get; }
        ICommands<Game> GameCommands { get; }
        IQueries<UserGameCommentRate> UserGameCommentRateQueries { get; }
        ICommands<UserGameCommentRate> UserGameCommentRateCommands { get; }

        bool AutoDetectChange { set; }
        bool ValidateOnSaveEnabled { set; }
        int Commit();
        void BulkCommit();
    }
}