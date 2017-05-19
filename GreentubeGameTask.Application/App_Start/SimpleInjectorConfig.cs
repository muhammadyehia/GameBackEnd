using System.Data.Entity;
using System.Web.Http;
using GreentubeGameTask.Core.Entities;
using GreentubeGameTask.Core.Interfaces;
using GreentubeGameTask.Core.Services;
using GreentubeGameTask.Infrastructure;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;

namespace GreentubeGameTask.Application
{
    public class SimpleInjectorConfig
    {
        public static void RegisterComponents()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Register<DbContext, UserGameContext>(Lifestyle.Scoped);
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register<ICommands<User>, Commands<User>>(Lifestyle.Scoped);
            container.Register<IQueries<User>, Queries<User>>(Lifestyle.Scoped);
            container.Register<IFakeGamesApiService, FakeGamesApiService>(Lifestyle.Transient);
            container.Register<IUserService, UserService>(Lifestyle.Transient);
            container.Register<ICommands<Game>, Commands<Game>>(Lifestyle.Scoped);
            container.Register<IQueries<Game>, Queries<Game>>(Lifestyle.Scoped);
            container.Register<IGameService, GameService>(Lifestyle.Transient);
            container.Register<ICommands<UserGameCommentRate>, Commands<UserGameCommentRate>>(Lifestyle.Scoped);
            container.Register<IQueries<UserGameCommentRate>, Queries<UserGameCommentRate>>(Lifestyle.Scoped);
            container.Register<ICommentRateService, CommentRateService>(Lifestyle.Transient);
            container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
            container.Verify(VerificationOption.VerifyAndDiagnose);
            GlobalConfiguration.Configuration.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}