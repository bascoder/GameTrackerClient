using System.Collections.Generic;
using System.Diagnostics;

namespace GameTrackerClient
{
    public static class ProcessLookup
    {
        /// <summary>
        /// Return list of process names
        /// </summary>
        /// <returns>Collection with process names</returns>
        public static IEnumerable<string> LookupProcesses()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                yield return p.ProcessName;
            }
        }
    }
}
