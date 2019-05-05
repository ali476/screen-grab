using scrgrab.Classes;
using System;

namespace scrgrab.Timer
{
    /// <summary>
    /// Multi-media timer
    /// </summary>
    class BDSTimer : IDisposable
    {
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        public bool Disposed { get { return disposedValue; } }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }
                m_timer.Dispose();
                m_timer = null;
                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BDSTimer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Data
        private bool m_locked = false; // set while timer is locked for update
        private TimeSpan m_start_time = TimeSpan.FromSeconds(8 * 3600); // default start time of 0800hrs
        private TimeSpan m_end_time = TimeSpan.FromSeconds(18 * 3600 + 30 * 60); // default end time of 1830hrs
        private TimeSpan m_interval = TimeSpan.FromSeconds(15 * 60); // default interval of 15 mins
        private System.Timers.Timer m_timer = new System.Timers.Timer();
        private OnTimerEvent m_onTimer = null; // application supplied on-timer event
        #endregion

        public delegate void OnTimerEvent();

        #region Public Properties
        public TimeSpan EndTime { get => m_end_time; set => SetEndTime(value); }
        public TimeSpan StartTime { get => m_start_time; set => SetStartTime(value); }
        public TimeSpan Interval { get => m_interval; set => SetInterval(value); }
        public bool Started => m_timer.Enabled;
        public OnTimerEvent OnTimer { get => m_onTimer; set => m_onTimer = value; }
        #endregion

        /// <summary>
        /// Standard constructor
        /// </summary>
        public BDSTimer()
        {
        }

        /// <summary>
        /// Parameterised constructor. Once constructed, use Start() and Stop() to manage the timer
        /// </summary>
        /// <param name="ontimer">application supplied on-timer event</param>
        /// <param name="start_seconds">start time expressed in seconds since midnight</param>
        /// <param name="end_seconds">end time expressed in seconds since midnight</param>
        /// <param name="interval_seconds">trigger time (timer event) in milliseconds</param>
        public BDSTimer(OnTimerEvent ontimer, TimeSpan start_seconds, TimeSpan end_seconds, TimeSpan interval_seconds) : base()
        {
            m_onTimer = ontimer;
            m_start_time = start_seconds;
            m_end_time = end_seconds;
            m_interval = interval_seconds;
        }

        /// <summary>
        /// check to see if the timer is still within the operational times
        /// </summary>
        /// <returns></returns>
        public bool IsOperational()
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            return (TimeSpan.Compare(now, m_start_time) > -1) && (TimeSpan.Compare(now, m_end_time) < 1);
        }

        /// <summary>
        /// indicate the timer object will not be reset while properties are changed
        /// </summary>
        public void Lock()
        {
            m_locked = true;
        }

        public void Unlock()
        {
            m_locked = false;
        }

        /// <summary>
        /// start the timer; this calls GetNextStartTime to calculate the length of the timer interval
        /// which could be either the value of Interval or, if Now is after the EndTime, a calulated value 
        /// until the next StartTime
        /// </summary>
        public void Start()
        {
            if (!m_locked)
            {
                if (Started)
                    Stop();

                m_timer.Interval = GetNextStartTime();
                m_timer.Elapsed += BDSTimerCallback;
                m_timer.AutoReset = true;
                m_timer.Enabled = true;
            }
        }

        /// <summary>
        /// stops the timer by deleting it from the timer queue
        /// </summary>
        public void Stop()
        {
            if (!m_locked && Started)
            {
                m_timer.Enabled = false;
                m_timer.Elapsed -= BDSTimerCallback;
            }
        }

        #region Property Accessors
        private void SetInterval(TimeSpan value)
        {
            if (TimeSpan.Compare(m_interval, value) != 0)
            {
                Stop();
                m_interval = value;
                Start();
            }
        }

        private void SetEndTime(TimeSpan value)
        {
            if (TimeSpan.Compare(m_end_time, value) != 0)
            {
                Stop();
                m_end_time = value;
                Start();
            }
        }

        private void SetStartTime(TimeSpan value)
        {
            if (TimeSpan.Compare(m_start_time, value) != 0)
            {
                Stop();
                m_start_time = value;
                Start();
            }
        }
        #endregion

        /// <summary>
        /// calculate number of milliseconds between now and the next time timer is due to become operational
        /// </summary>
        /// <returns></returns>
        private double GetNextStartTime()
        {
            TimeSpan result = m_interval;
            if (!IsOperational())
            {
                // next_due = time-to-midnight + start-time
                TimeSpan time_to_midnight = new TimeSpan(24, 0, 0).Subtract(DateTime.Now.TimeOfDay);
                result = time_to_midnight + StartTime;
            }
            Logger.Log(Configuration.WorkingFolder, "Next start: " + result.ToString());
            return result.TotalMilliseconds;
        }

        private void BDSTimerCallback(Object source, System.Timers.ElapsedEventArgs e)
        {
            // fire application-supplied call back
            m_onTimer?.Invoke();
        }

    }
}
