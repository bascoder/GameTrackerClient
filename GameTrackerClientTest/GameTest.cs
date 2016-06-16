using System;
using GameTrackerClient.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTrackerClientTest
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void TestEquals()
        {
            Game empty = new Game();
            Assert.AreEqual(new Game(), empty);

            Game gta = new Game() {Title = "gta"};
            Game gta2 = new Game() {Title = "gta"};

            Assert.AreEqual(gta, gta2);
        }

        [TestMethod]
        public void TestUnequal()
        {
            Game gta = new Game() { Title = "gta", Icon = "hello.ico" };
            Game gta2 = new Game() { Title = "gta", Id = 1};

            Assert.AreNotEqual(gta, gta2);
            
            Game needForSpeed = new Game() {Title = "Need for speed"};
            Game callOfDuty = new Game() {Title = "Call of Duty"};

            Assert.AreNotEqual(needForSpeed, callOfDuty);
            Assert.AreNotEqual(needForSpeed, gta);
            Assert.AreNotEqual(callOfDuty, gta);
        }
    }
}
