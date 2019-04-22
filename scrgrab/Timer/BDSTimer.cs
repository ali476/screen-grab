using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
        private bool m_pending; // set when called from StartPending
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
        /// start the timer; this starts the timer using the operational values 
        /// previously provided when the class was first created or subsequently set via properties. If Start() is
        /// not within operational times, then it calls StartPending() to set the correct due time for the next operational time start
        /// </summary>
        public void Start()
        {
            if (!m_locked)
            {
                // if already running, first call stop
                if (Started)
                    Stop();
                // decide how to start
                if (!IsOperational())
                    StartPending();
                else
                {
                    m_pending = false;
                    m_timer.Interval = m_interval.TotalMilliseconds;
                    m_timer.Elapsed += BDSTimerCallback;
                    m_timer.AutoReset = true;
                    m_timer.Enabled = true;
                }
            }
        }

        /// <summary>
        /// start pending, set the timer's due time (i.e. the time it first fires) to the next operstational start time
        /// </summary>
        public void StartPending()
        {
            if (!m_locked)
            {
                if (Started)
                    Stop();
                // calculate number of seconds elapsed between now and next due time
                m_pending = true;
                m_timer.Interval = GetNextOperationalStart();
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
                m_pending = false;
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
        private double GetNextOperationalStart()
        {
            // next_due = time-to-midnight + start-time
            TimeSpan time_to_midnight = new TimeSpan(24, 0, 0).Subtract(DateTime.Now.TimeOfDay);
            return (time_to_midnight + StartTime).TotalMilliseconds;
        }

        private void BDSTimerCallback(Object source, System.Timers.ElapsedEventArgs e)
        {
            // fire application-supplied call back
            m_onTimer?.Invoke();

            // if no longer operational, stop the timer and start a new pending timer
            if (!IsOperational())
            {
                Stop();
                StartPending();
            }
        }

    }
}
