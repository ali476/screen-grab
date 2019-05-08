using Microsoft.Win32;
using scrgrab.Classes;
using scrgrab.Timer;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace scrgrab
{
    /// <summary>
    /// Application's main form
    /// </summary>
    public partial class formMain : Form
    {
        private bool closedFromMenu = false;
        private bool firstStart = true;
        private BDSTimer bdstimer;
        // system events
        private PowerModeChangedEventHandler m_powerHandler;

        /// <summary>
        /// Application's main form constructor
        /// </summary>
        public formMain()
        {
            InitializeComponent();

            for (int i = 0; i < 24; i++)
            {
                comboHFrom.Items.Add(i);
                comboHTo.Items.Add(i);
            }
            comboMFrom.Items.AddRange(new object[] { 0, 15, 30, 45 });
            comboMTo.Items.AddRange(new object[] { 0, 15, 30, 45 });

            textHistory.Text = Configuration.History.ToString();
            textInterval.Text = Configuration.Interval.ToString();
            textWorkingFolder.Text = Configuration.WorkingFolder;

            Configuration.GetOperationalFrom(out int h, out int m);
            comboHFrom.SelectedItem = h;
            comboMFrom.SelectedItem = m;

            Configuration.GetOperationalTo(out h, out m);
            comboHTo.SelectedItem = h;
            comboMTo.SelectedItem = m;

            Icon appIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            this.Icon = appIcon;
            this.notifyIcon.Icon = appIcon;
            notifyIcon.BalloonTipIcon = ToolTipIcon.None;

            bdstimer = new BDSTimer(TimerFiredEvent, Configuration.StartTime, Configuration.EndTime, TimeSpan.FromMinutes(Configuration.Interval));
            bdstimer.Start();
            firstStart = false;

            m_powerHandler = new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
            SystemEvents.PowerModeChanged += m_powerHandler;
        }

        void TimerFiredEvent()
        {
            CaptureScreen();
            ImageManager.Enumerate(Configuration.WorkingFolder);
        }

        void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                Logger.Log(Configuration.WorkingFolder, "Power suspended");
                bdstimer.Stop();
            }
            else if (e.Mode == PowerModes.Resume)
            {
                Logger.Log(Configuration.WorkingFolder, "Power resumed");
                bdstimer.Start();
            }
        }

        private void CaptureScreen()
        {
            Capture c = new Capture(Configuration.Filename);
            c.capture(screens: Classes.Capture.Parse(Configuration.Screen));
        }

        private void MinimiseToTray()
        {
            if (!firstStart)
                firstStart = false;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(500);
            this.ShowInTaskbar = false;
            this.Hide();
            SaveConfiguration();
        }

        private void RestoreFromTray()
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.Show();
        }

        private void SaveConfiguration()
        {
            int i = 0;
            Configuration.WorkingFolder = textWorkingFolder.Text;

            if (int.TryParse(textHistory.Text, out i))
                Configuration.History = i;

            if (int.TryParse(textInterval.Text, out i))
                Configuration.Interval = i;

            Configuration.SetOperationalFrom((int)comboHFrom.SelectedItem, (int)comboMFrom.SelectedItem);
            Configuration.SetOperationalTo((int)comboHTo.SelectedItem, (int)comboMTo.SelectedItem);
            Configuration.Log();
            // re-set the timer
            bdstimer.Lock();
            bdstimer.StartTime = Configuration.StartTime;
            bdstimer.EndTime = Configuration.EndTime;
            bdstimer.Interval = TimeSpan.FromMinutes(Configuration.Interval);
            bdstimer.Unlock();
            bdstimer.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.SelectedPath = Configuration.WorkingFolder;
            DialogResult result = fbd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                textWorkingFolder.Text = fbd.SelectedPath;
                SaveConfiguration();
            }
        }

        private void configuration_Changed(object sender, EventArgs e)
        {
            if (!firstStart)
                SaveConfiguration();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            this.ActiveControl = labelWorkingFolder;
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
                MinimiseToTray();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            RestoreFromTray();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closedFromMenu)
            {
                MinimiseToTray();
                e.Cancel = true;
                return;
            }
            // if we're really closing then tidy up the system event handler
            SystemEvents.PowerModeChanged -= m_powerHandler;
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            closedFromMenu = true;
            Close();
        }

        private void LogNow_Clicked(object sender, EventArgs e)
        {
            CaptureScreen();
        }

        private void ButtonView_Click(object sender, EventArgs e)
        {
            bdstimer.Stop();
            Viewer vu = new Viewer();
            vu.ShowDialog(this);
            bdstimer.Start();
        }
    }
}