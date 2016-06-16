using System;
using System.Reflection;
using GameTrackerClient.model;

namespace GameTrackerClient
{
    /// <summary>
    ///     Thread safe singleton containing playing state
    /// </summary>
    public sealed class PlayingState
    {
        #region singleton implementation

        private static volatile PlayingState _instance;
        private static readonly object Lock = new object();

        private readonly object _stateLock = new object();

        private PlayingState()
        {
        }

        public static PlayingState Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                            _instance = new PlayingState();
                    }
                }

                return _instance;
            }
        }

        #endregion

        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Game _currentGame;
        private DateTime _startTime;

        public event EventHandler UpdateEvent;

        public Game CurrentGame
        {
            get
            {
                lock (_stateLock)
                {
                    return _currentGame;
                }
            }
            set
            {
                lock (_stateLock)
                {
                    if (!value.Equals(_currentGame))
                    {
                        _currentGame = value;
                        _startTime = DateTime.Now;
                        Log.Info("New game playing " + _currentGame);
                        OnRaiseEvent();
                    }
                }
            }
        }

        public DateTime StartTime
        {
            get
            {
                lock (_stateLock)
                {
                    return _startTime;
                }
            }
        }

        public TimeSpan CurrentPlayTime
        {
            get
            {
                lock (_stateLock)
                {
                    return DateTime.Now - _startTime;
                }
            }
        }

        private void OnRaiseEvent()
        {
            UpdateEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
