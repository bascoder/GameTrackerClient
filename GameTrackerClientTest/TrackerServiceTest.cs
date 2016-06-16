using System;
using GameTrackerClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTrackerClientTest
{
    [TestClass]
    public class TrackerServiceTest
    {
        [TestMethod]
        public void TestStartStopMultiple()
        {
            TrackerService service = new TrackerService();
            service.Start();
            service.Stop();
            service.Start();
            service.Stop();
        }

        [TestMethod]
        public void TestStartStopQuiet()
        {
            TrackerService service = new TrackerService(1000);
            service.Start();
            service.StopWithoutInterrupt();
        }

        [TestMethod]
        public void TestStartTwiceThrow()
        {
            TrackerService service = new TrackerService();
            service.Start();
            Exception e = null;
            try
            {
                service.Start();
            }
            catch (GameTrackerException ex)
            {
                e = ex;
            }
            
            Assert.IsNotNull(e);
        }
    }
}
