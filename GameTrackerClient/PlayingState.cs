using System;
using System.Reflection;
using System.Resources;
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

        public event EventHandler UpdateEvent;

        private State _currentState;

        public State CurrentState
        {
            get
            {
                lock (_stateLock)
                {
                    return _currentState;
                }
            }
        }

        public State UpdateGame(Game newGame)
        {
            lock (_stateLock)
            {
                State previousState = _currentState;
                State newState = new State();

                if (!newGame.Equals(previousState.Game))
                {
                    newState.Game = newGame;
                    Log.Info("New game playing " + newGame);

                    _currentState = newState;
                    OnRaiseEvent();
                }

                return previousState;
            }
        }

        public State RemoveGame()
        {
            lock (_stateLock)
            {
                State previousState = _currentState;
                _currentState = new State();
                return previousState;
            }
        }

        private void OnRaiseEvent()
        {
            UpdateEvent?.Invoke(this, EventArgs.Empty);
        }

        public struct State
        {
            private Game _game;

            public Game Game
            {
                get { return _game; }
                set
                {
                    if (value != null && !value.Equals(_game))
                    {
                        _game = value;
                        StartTime = DateTime.Now;
                    }
                }
            }

            public TimeSpan PlayingTime
            {
                get
                {
                    if (_game != null)
                        return DateTime.Now - StartTime;
                    return new TimeSpan(0);
                }
            }

            public DateTime StartTime { get; private set; }
        }
    }
}
