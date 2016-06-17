using System;
using System.Threading;
using GameTrackerClient;
using GameTrackerClient.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTrackerClientTest
{
    [TestClass]
    public class PlayingStateTest
    {
        [TestMethod]
        public void TestAdd()
        {
            Game game = new Game() {Id = 0, Title = "GTA"};
            PlayingState playingState = PlayingState.Instance;
            playingState.UpdateGame(game);

            Assert.IsNotNull(playingState.CurrentState.Game);
        }

        [TestMethod]
        public void TestAddTwice()
        {
            // test if datetime stays the same after adding same game multiple times
            Game game = new Game() {Id = 0, Title = "GTA"};
            PlayingState playingState = PlayingState.Instance;
            playingState.UpdateGame(game);

            DateTime startTime1 = playingState.CurrentState.StartTime;
            // sleep a bit before adding and checking datetime
            Thread.Sleep(500);

            playingState.UpdateGame(game);

            DateTime startTime2 = playingState.CurrentState.StartTime;
            Assert.AreEqual(startTime1, startTime2);

            Thread.Sleep(500);

            Game gameSame = new Game() {Id = 0, Title = "GTA"};
            playingState.UpdateGame(gameSame);


            DateTime startTime3 = playingState.CurrentState.StartTime;

            Assert.AreEqual(startTime2, startTime3);
        }

        [TestMethod]
        public void TestAddNewGame()
        {
            Game game1 = new Game() {Id = 1};
            Game game2 = new Game() {Id = 2};

            PlayingState playingState = PlayingState.Instance;

            playingState.UpdateGame(game1);
            DateTime startTime1 = playingState.CurrentState.StartTime;

            Thread.Sleep(500);

            playingState.UpdateGame(game2);
            DateTime startTime2 = playingState.CurrentState.StartTime;

            Assert.AreNotEqual(startTime1, startTime2);
            Assert.IsTrue(startTime2 > startTime1);
        }

        [TestMethod]
        public void TestRemoveGame()
        {
            Game game1 = new Game() {Id = 1};

            PlayingState playingState = PlayingState.Instance;
            
            playingState.UpdateGame(game1);
            Assert.AreEqual(playingState.CurrentState.Game, game1);

            PlayingState.State prev = playingState.RemoveGame();
            Assert.AreEqual(prev.Game, game1);
        }

        [TestMethod]
        public void TestTimePlaying()
        {
            Game game = new Game();
            PlayingState playingState = PlayingState.Instance;
            playingState.UpdateGame(game);

            TimeSpan ts1 = playingState.CurrentState.PlayingTime;
            Thread.Sleep(500);
            TimeSpan ts2 = playingState.CurrentState.PlayingTime;

            Assert.IsTrue(ts2 > ts1);
        }
    }
}