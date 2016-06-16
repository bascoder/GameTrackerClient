using System.Threading;

namespace GameTrackerClient
{
    /// <summary>
    ///     Tracker service reads process list every {timeout} seconds
    /// </summary>
    public class TrackerService
    {
        private Thread _looperThread;
        private volatile bool _stopped = true;

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _timeout;

        public TrackerService(int timeout = 60000)
        {
            _timeout = timeout;
        }

        /// <summary>
        ///     Start internal looper thread
        /// </summary>
        public void Start()
        {
            if (!_stopped || _looperThread != null)
            {
                throw new GameTrackerException("Illegal state: thread has already started");
            }
            _stopped = false;

            _looperThread = new Thread(Looper);
            _looperThread.Start();
        }

        /// <summary>
        ///     Interrupt internal looper thread
        /// </summary>
        public void Stop()
        {
            _looperThread.Interrupt();
            _stopped = true;
            _looperThread = null;
        }

        /// <summary>
        ///     Join internal looper thread
        /// </summary>
        public void StopWithoutInterrupt()
        {
            _stopped = true;
            // allow to join within timeout + 1 second
            _looperThread.Join(_timeout + 1000);
            _looperThread = null;
        }

        private void Looper()
        {
            Log.Info("TrackerService thread has started");
            while (!_stopped)
            {
                try
                {
                    LooperBody();
                }
                catch (ThreadInterruptedException)
                {
                    // bail out
                    break;
                }
            }
            Log.Info("TrackerService thread has stopped");
        }

        private void LooperBody()
        {
            var processes = ProcessLookup.LookupProcesses();
            foreach (string process in processes)
            {
                Log.Debug(process);
            }
            Thread.Sleep(_timeout);
        }
    }
}