using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeGamesApi;
using FluentAssertions;
using GreentubeGameTask.Core.Interfaces;
using GreentubeGameTask.Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GreentubeGameTask.Core.Test
{
    [TestClass]
    public class GameServiceTest
    {
        internal List<Game> FakeGamesApiGames;
        internal GameService GameService;
        internal Mock<IFakeGamesApiService> MockFakeGamesApiService;
        internal Mock<ICommands<Entities.Game>> MockGameCommands;
        internal Mock<IQueries<Entities.Game>> MockGameQueries;
        internal Mock<IUnitOfWork> MockUnitOfWork;

        public GameServiceTest()
        {
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockGameQueries = new Mock<IQueries<Entities.Game>>();
            MockGameCommands = new Mock<ICommands<Entities.Game>>();
            MockFakeGamesApiService = new Mock<IFakeGamesApiService>();
        }

        [TestInitialize]
        public void Initialize()
        {
            FakeGamesApiGames = new List<Game>
            {
                new Game
                {
                    Id = 1,
                    Name = "Schnapsen"
                },
                new Game
                {
                    Id = 2,
                    Name = "Bauernschnapsen"
                },
                new Game
                {
                    Id = 3,
                    Name = "Backgammon"
                },
                new Game
                {
                    Id = 4,
                    Name = "4 to win"
                },
                new Game
                {
                    Id = 7,
                    Name = "Boule"
                },
                new Game
                {
                    Id = 8,
                    Name = "Crazy Eights"
                },
                new Game
                {
                    Id = 9,
                    Name = "Penalty King"
                },
                new Game
                {
                    Id = 14,
                    Name = "Ulti"
                },
                new Game
                {
                    Id = 15,
                    Name = "Zuminga"
                },
                new Game
                {
                    Id = 16,
                    Name = "Tarock 20"
                }
            };

            MockFakeGamesApiService.Setup(m => m.GetGames()).Returns(FakeGamesApiGames);
            MockUnitOfWork.SetupGet(p => p.GameCommands).Returns(MockGameCommands.Object);
        }

        [TestMethod]
        public void UpdateNewGamesFromApitoLocalDb_WhenNewGamesReturnedAndLocalDataBaseIsEmpty_ShouldAdded()
        {
            // arrange

            MockGameQueries.Setup(
                m =>
                    m.Query(It.IsAny<List<Expression<Func<Entities.Game, bool>>>>(),
                        It.IsAny<Func<IQueryable<Entities.Game>, IOrderedQueryable<Entities.Game>>>(),
                        It.IsAny<List<Expression<Func<Entities.Game, object>>>>()))
                .Returns(new List<Entities.Game>().AsQueryable());
            MockUnitOfWork.SetupGet(p => p.GameQueries).Returns(MockGameQueries.Object);

            // act
            GameService = new GameService(MockUnitOfWork.Object, MockFakeGamesApiService.Object);
            var result = GameService.UpdateNewGamesFromApitoLocalDb();

            // assert
            result.Should().Be(10);
        }

        [TestMethod]
        public void UpdateNewGamesFromApitoLocalDb_WhenNewGamesReturnedAndLocalDataBaseIsNotEmpty_ShouldAdded()
        {
            // arrange

            MockGameQueries.Setup(
                m =>
                    m.Query(It.IsAny<List<Expression<Func<Entities.Game, bool>>>>(),
                        It.IsAny<Func<IQueryable<Entities.Game>, IOrderedQueryable<Entities.Game>>>(),
                        It.IsAny<List<Expression<Func<Entities.Game, object>>>>()))
                .Returns(
                    FakeGamesApiGames.Take(3).Select(g => new Entities.Game {Id = g.Id, Name = g.Name}).AsQueryable());
            MockUnitOfWork.SetupGet(p => p.GameQueries).Returns(MockGameQueries.Object);
            // act
            GameService = new GameService(MockUnitOfWork.Object, MockFakeGamesApiService.Object);
            var result = GameService.UpdateNewGamesFromApitoLocalDb();

            // assert
            result.Should().Be(7);
        }

        [TestMethod]
        public void UpdateNewGamesFromApitoLocalDb_WhenNoNewGamesReturned_ShouldNotAdd()
        {
            // arrange

            MockGameQueries.Setup(
                m =>
                    m.Query(It.IsAny<List<Expression<Func<Entities.Game, bool>>>>(),
                        It.IsAny<Func<IQueryable<Entities.Game>, IOrderedQueryable<Entities.Game>>>(),
                        It.IsAny<List<Expression<Func<Entities.Game, object>>>>()))
                .Returns(FakeGamesApiGames.Select(g => new Entities.Game {Id = g.Id, Name = g.Name}).AsQueryable());
            MockUnitOfWork.SetupGet(p => p.GameQueries).Returns(MockGameQueries.Object);
            // act
            GameService = new GameService(MockUnitOfWork.Object, MockFakeGamesApiService.Object);
            var result = GameService.UpdateNewGamesFromApitoLocalDb();

            // assert
            result.Should().Be(0);
        }

        [TestMethod]
        public void UpdateNewGamesFromApitoLocalDb_WhenGamesReturned_ShouldNotAdd()
        {
            // arrange
            MockFakeGamesApiService.Setup(m => m.GetGames()).Returns(new List<Game>());
            MockGameQueries.Setup(
                m =>
                    m.Query(It.IsAny<List<Expression<Func<Entities.Game, bool>>>>(),
                        It.IsAny<Func<IQueryable<Entities.Game>, IOrderedQueryable<Entities.Game>>>(),
                        It.IsAny<List<Expression<Func<Entities.Game, object>>>>()))
                .Returns(FakeGamesApiGames.Select(g => new Entities.Game {Id = g.Id, Name = g.Name}).AsQueryable());
            MockUnitOfWork.SetupGet(p => p.GameQueries).Returns(MockGameQueries.Object);
            // act
            GameService = new GameService(MockUnitOfWork.Object, MockFakeGamesApiService.Object);
            var result = GameService.UpdateNewGamesFromApitoLocalDb();

            // assert
            result.Should().Be(0);
        }
    }
}