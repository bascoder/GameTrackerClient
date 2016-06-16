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
            playingState.CurrentGame = game;
            
            Assert.IsNotNull(playingState.CurrentGame);
        }

        [TestMethod]
        public void TestAddTwice()
        {
            // test if datetime stays the same after adding same game multiple times
            Game game = new Game() { Id = 0, Title = "GTA" };
            PlayingState playingState = PlayingState.Instance;
            playingState.CurrentGame = game;

            DateTime startTime1 = playingState.StartTime;
            // sleep a bit before adding and checking datetime
            Thread.Sleep(500);

            playingState.CurrentGame = game;

            DateTime startTime2 = playingState.StartTime;
            Assert.AreEqual(startTime1, startTime2);
            
            Thread.Sleep(500);

            Game gameSame = new Game() { Id = 0, Title = "GTA" };
            playingState.CurrentGame = gameSame;

            
            DateTime startTime3 = playingState.StartTime;

            Assert.AreEqual(startTime2, startTime3);
        }

        [TestMethod]
        public void TestAddNewGame()
        {
            Game game1 = new Game() {Id = 1};
            Game game2 = new Game() {Id = 2};

            PlayingState playingState = PlayingState.Instance;

            playingState.CurrentGame = game1;
            DateTime startTime1 = playingState.StartTime;

            Thread.Sleep(500);

            playingState.CurrentGame = game2;
            DateTime startTime2 = playingState.StartTime;

            Assert.AreNotEqual(startTime1, startTime2);
            Assert.IsTrue(startTime2 > startTime1);
        }
    }
}
