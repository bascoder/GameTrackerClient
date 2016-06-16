using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameTrackerClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameTrackerClientTest
{
    [TestClass]
    public class ProcessLookupTest
    {
        [TestMethod]
        public void TestList()
        {
            IEnumerable<string> processes = ProcessLookup.LookupProcesses();
            Process[] localLookup =  Process.GetProcesses();

            foreach (var process in localLookup)
            {
                Assert.IsTrue(processes.Contains(process.ProcessName));
            }
        }
    }
}
