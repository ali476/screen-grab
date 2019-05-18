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
        private bool m_closedFromMenu = false;
        private bool m_firstStart = true;
        private BDSTimer m_bdstimer;
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

            m_bdstimer = new BDSTimer(TimerFiredEvent, Configuration.StartTime, Configuration.EndTime, TimeSpan.FromMinutes(Configuration.Interval));
            m_bdstimer.Start();
            m_firstStart = false;

            m_powerHandler = new PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
            SystemEvents.PowerModeChanged += m_powerHandler;
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvent_SessionEndingHandler);
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
                m_bdstimer.Stop();
            }
            else if (e.Mode == PowerModes.Resume)
            {
                Logger.Log(Configuration.WorkingFolder, "Power resumed");
                m_bdstimer.Start();
            }
        }

        void SystemEvent_SessionEndingHandler(object sender, SessionEndingEventArgs e)
        {
            Shutdown();
        }

        private void CaptureScreen()
        {
            Capture c = new Capture(Configuration.Filename);
            c.capture(screens: Classes.Capture.Parse(Configuration.Screen));
        }

        private void MinimiseToTray()
        {
            if (!m_firstStart)
                m_firstStart = false;
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
            m_bdstimer.Lock();
            m_bdstimer.StartTime = Configuration.StartTime;
            m_bdstimer.EndTime = Configuration.EndTime;
            m_bdstimer.Interval = TimeSpan.FromMinutes(Configuration.Interval);
            m_bdstimer.Unlock();
            m_bdstimer.Start();
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

        private void Shutdown()
        {
            m_closedFromMenu = true;
            Close();
        }

        private void configuration_Changed(object sender, EventArgs e)
        {
            if (!m_firstStart)
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
            if (!m_closedFromMenu)
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
            Shutdown();
        }

        private void LogNow_Clicked(object sender, EventArgs e)
        {
            CaptureScreen();
        }

        private void ButtonView_Click(object sender, EventArgs e)
        {
            m_bdstimer.Stop();
            Viewer vu = new Viewer();
            vu.ShowDialog(this);
            m_bdstimer.Start();
        }
    }
}