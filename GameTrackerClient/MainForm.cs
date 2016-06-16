using System.Windows.Forms;

namespace GameTrackerClient
{
    public partial class MainForm : Form
    {
        private readonly TrackerService _trackerService;
        private readonly PlayingState _playingState;

        private delegate void UpdateDelegate();

        private readonly UpdateDelegate _delegateImpl;

        public MainForm()
        {
            InitializeComponent();

            _delegateImpl = new UpdateDelegate(UpdateLabel);

            // Setup event handler
            _playingState = PlayingState.Instance;
            _playingState.UpdateEvent += (sender, args) =>
            {
                if (labelCurrentGame.InvokeRequired)
                {
                    labelCurrentGame.BeginInvoke(_delegateImpl);
                }
            };

            // start tracker service
            _trackerService = new TrackerService();
            _trackerService.Start();
            // on close: stop tracking
            FormClosing += (sender, args) => _trackerService.Stop();
        }

        private void UpdateLabel()
        {
            labelCurrentGame.Text = _playingState.CurrentGame.Title;
        }
    }
}