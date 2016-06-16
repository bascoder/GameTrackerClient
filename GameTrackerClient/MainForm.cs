using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameTrackerClient
{
    public partial class MainForm : Form
    {
        private readonly TrackerService _trackerService;

        public MainForm()
        {
            InitializeComponent();

            // start tracker service
            _trackerService = new TrackerService();
            _trackerService.Start();
            // on close: stop tracking
            FormClosing += (sender, args) => _trackerService.Stop();
        }
    }
}
