using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using GameTrackerClient.model;
using Newtonsoft.Json;

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
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly int _timeout;
        private IDictionary<string, Game> _mappingDictionary;

        public TrackerService(int timeout = 60000)
        {
            _timeout = timeout;
            Init();
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

        private void Init()
        {
            try
            {
                LoadMapping();
            }
            catch (IOException e)
            {
                Log.Error(e);
                throw new GameTrackerException("Could not read mapping file", e);
            }
            catch (JsonException e)
            {
                Log.Error(e);
                throw new GameTrackerException("Json mapping file has been corrupted", e);
            }
        }

        private void LoadMapping()
        {
            string mappingPath = Properties.Resources.mapping_path;
            using (StreamReader file = File.OpenText(mappingPath))
            {
                JsonSerializer serializer = new JsonSerializer();
                _mappingDictionary =
                    (Dictionary<string, Game>) serializer.Deserialize(file, typeof(Dictionary<string, Game>));
                Log.Debug(string.Format("Mapping file read with {0} entries", _mappingDictionary.Count));
            }
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
                if (_mappingDictionary.ContainsKey(process))
                {
                    Game playedGame = _mappingDictionary[process];
                    PlayingState.Instance.CurrentGame = playedGame;
                    Log.Info("Player is playing " + playedGame.Title);
                }
            }
            Thread.Sleep(_timeout);
        }
    }
}
