using System;
using System.Windows.Forms;
using GameTrackerClient.model;
using GameTrackerClient.Properties;

namespace GameTrackerClient
{
    public partial class MainForm : Form
    {
        private const int UpdateInterval = 1000;

        private readonly TrackerService _trackerService = new TrackerService(UpdateInterval * 30);
        private readonly PlayingState _playingState = PlayingState.Instance;

        private delegate void UpdateDelegate();

        public MainForm()
        {
            InitializeComponent();

            SetupPlayingState();
            SetupTrackerService();
            SetupTimer();
        }

        private void SetupTimer()
        {
            var timer = new Timer {Interval = UpdateInterval};
            timer.Tick += (sender, args) => { UpdateLabel(); };
            timer.Enabled = true;
        }

        private void SetupTrackerService()
        {
            // start tracker service
            _trackerService.Start();
            // on close: stop tracking
            FormClosing += (sender, args) => _trackerService.Stop();
        }

        private void SetupPlayingState()
        {
            _playingState.UpdateEvent += (sender, args) =>
            {
                if (labelCurrentGame.InvokeRequired)
                {
                    labelCurrentGame.BeginInvoke(new UpdateDelegate(UpdateLabel));
                }
            };
        }

        private void UpdateLabel()
        {
            Game game = _playingState.CurrentGame;
            if (game == null)
            {
                labelCurrentGame.Text = Resources.MainForm_UpdateLabel_none;
                return;
            }

            TimeSpan playTime = _playingState.CurrentPlayTime;
            if (playTime.TotalMinutes >= 1.0)
            {
                string timeString = playTime.TotalHours > 1.0
                    ? playTime.Hours + " " + Resources.MainForm_UpdateLabel_hours
                    : playTime.Minutes + " " + Resources.MainForm_UpdateLabel_minutes;
                labelCurrentGame.Text = string.Format("{0} ({1})", game.Title, timeString);
            }
            else
            {
                labelCurrentGame.Text = string.Format("{0} (< 0 {1})", game.Title, Resources.MainForm_UpdateLabel_minutes);
            }
        }
    }
}
