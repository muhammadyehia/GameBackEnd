using System.Data.Entity;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;

namespace GreentubeGameTask.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(DbContext context, ICommands<Game> gameCommands, IQueries<Game> gameQueries,
            ICommands<User> userCommands, IQueries<User> userQueries,
            ICommands<UserGameCommentRate> userGameCommentRateCommands,
            IQueries<UserGameCommentRate> userGameCommentRateQueries)
        {
            _context = context;
            GameCommands = gameCommands;
            GameQueries = gameQueries;
            UserCommands = userCommands;
            UserGameCommentRateCommands = userGameCommentRateCommands;
            UserGameCommentRateQueries = userGameCommentRateQueries;
            UserQueries = userQueries;
        }

        public bool AutoDetectChange
        {
            set { _context.Configuration.AutoDetectChangesEnabled = value; }
        }

        public ICommands<Game> GameCommands { get; }

        public IQueries<Game> GameQueries { get; }

        public ICommands<User> UserCommands { get; }

        public ICommands<UserGameCommentRate> UserGameCommentRateCommands { get; }

        public IQueries<UserGameCommentRate> UserGameCommentRateQueries { get; }
        public IQueries<User> UserQueries { get; }

        public bool ValidateOnSaveEnabled
        {
            set { _context.Configuration.ValidateOnSaveEnabled = value; }
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void BulkCommit()
        {
            _context.BulkSaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}