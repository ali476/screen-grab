﻿using System;
using System.Globalization;
using Microsoft.Win32;

namespace scrgrab.Classes
{
    /// <summary>
    /// static configuration class
    /// </summary>
    public static class Configuration
    {
        //const String root = "HKEY_CURRENT_USER";
        const string key = "Software\\BDS Consulting\\scrgrab";
        const string folder = "WorkingFolder";
        const string history = "History";
        const string interval = "Interval";
        const string opFrom = "OperationStart";
        const string opTo = "OperationEnd";

        /// <summary>
        /// readonly - calculates the current filename
        /// </summary>
        public static String Filename { get => GetFilename(); }

        /// <summary>
        /// the length of time before old images are deleted
        /// </summary>
        public static int History { get => GetHistory(); set => SetHistory(value); }

        /// <summary>
        /// the interval, in minutes, between each event
        /// </summary>
        public static int Interval { get => GetInterval(); set => SetInterval(value); }

        /// <summary>
        /// Start time
        /// </summary>
        public static TimeSpan StartTime { get => GetStartTime(); }

        /// <summary>
        ///  end time
        /// </summary>
        public static TimeSpan EndTime { get => GetEndTime(); }

        /// <summary>
        /// the working folder
        /// </summary>
        public static String WorkingFolder { get => GetWorkingFolder(); set => SetWorkingFolder(value); }

        /// <summary>
        /// calculates the time when an event should start operation. Date part is always set to 1/1/2000
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns>DateTime</returns>
        public static void GetOperationalFrom(out int hour, out int minute)
        {
            // 0800 hrs
            hour = 8;
            minute = 0;
            long t = hour * 3600 + 0 + 0;
            // read the registry
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key))
                if (rk != null)
                    t = (int)rk.GetValue(opFrom, t);
            // convert values
            TimeSpan ts = TimeSpan.FromSeconds(t);
            hour = ts.Hours;
            minute = ts.Minutes;
            //return new DateTime(2000, 1, 1, hour, minute, 0);
        }

        /// <summary>
        /// sets the time when an event should start operation. Date part is always set to 1/1/2000
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public static void SetOperationalFrom(int hour, int minute)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key,true);
            if (rk == null)
            {
                Registry.CurrentUser.CreateSubKey(@key);
                rk = Registry.CurrentUser.OpenSubKey(@key, true);
            }
            rk.SetValue(opFrom, hour * 3600 + minute * 60, RegistryValueKind.DWord);
            rk.Close();
        }

        /// <summary>
        /// calculates the time when an event should end operation. Date part is always set to 1/1/2000
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns>DateTime</returns>
        public static void GetOperationalTo(out int hour, out int minute)
        {
            // 1830 hrs
            hour = 18;
            minute = 30;
            long t = hour * 3600 + minute * 60 + 0;
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key))
                if (rk != null)
                    t = (int)rk.GetValue(opTo, t);
            // convert values
            TimeSpan ts = TimeSpan.FromSeconds(t);
            hour = ts.Hours;
            minute = ts.Minutes;
        }

        /// <summary>
        /// Sets the time when an event should end operation. Date part is always set to 1/1/2000
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        public static void SetOperationalTo(int hour, int minute)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key,true);
            if (rk == null)
            {
                Registry.CurrentUser.CreateSubKey(@key);
                rk = Registry.CurrentUser.OpenSubKey(@key, true);
            }
            rk.SetValue(opTo, hour * 3600 + minute * 60, RegistryValueKind.DWord);
            rk.Close();
        }

        /// <summary>
        /// log the contents
        /// </summary>
        public static void Log()
        {
            string value = string.Format("Start: {0}, End: {1}, Interval: {2}",
                Configuration.StartTime.ToString(), Configuration.EndTime.ToString(), Configuration.Interval.ToString());
            Logger.Log(Configuration.WorkingFolder, value);
        }

        private static String GetFilename()
        {
            String fn;
            int wk = 0;
            DateTime dt = DateTime.Now;

            // calculate week number
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            wk = cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            var dow = cal.GetDayOfWeek(dt);
            fn = String.Format("{0}\\Week {1}\\{2}", GetWorkingFolder(), wk, dow);
            // check and create folder
            System.IO.Directory.CreateDirectory(fn);
            // finish the filename template
            fn += "\\{0}.png";
            do
            {
                fn = String.Format(fn, DateTime.Now.ToString("yyyyMMddHHmmss"));
            } while (System.IO.File.Exists(fn));
            return fn;
        }

        private static int GetHistory()
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key))
                if (rk != null)
                    return (int)rk.GetValue(history, 30);
            return 30;
        }

        private static int GetInterval()
        {
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key))
                if (rk != null)
                    return (int)rk.GetValue(interval, 15);
            return 15;
        }

        private static String GetWorkingFolder()
        {
            String regValue = null;
            String f = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            using (RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key))
                if (rk != null)
                {
                    regValue = (String)rk.GetValue(folder, f);
                }
            return String.IsNullOrEmpty(regValue) ? f : regValue;
        }

        private static void SetHistory(int value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key, true);
            if (rk == null)
            {
                Registry.CurrentUser.CreateSubKey(@key);
                rk = Registry.CurrentUser.OpenSubKey(@key, true);
            }
            rk.SetValue(history, value, RegistryValueKind.DWord);
            rk.Close();
        }

        private static void SetInterval(int value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key,true);
            if (rk == null)
            {
                Registry.CurrentUser.CreateSubKey(@key);
                rk = Registry.CurrentUser.OpenSubKey(@key, true);
            }
            rk.SetValue(interval, value, RegistryValueKind.DWord);
            rk.Close();
        }

        private static void SetWorkingFolder(String value)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@key,true);
            if (rk == null)
            {
                Registry.CurrentUser.CreateSubKey(@key);
                rk = Registry.CurrentUser.OpenSubKey(@key, true);
            }
            rk.SetValue(folder, value, RegistryValueKind.String);
            rk.Close();
        }

        private static TimeSpan GetStartTime()
        {
            int h, m;
            GetOperationalFrom(out h, out m);
            return TimeSpan.FromSeconds(h * 3600 + m * 60);
        }
        private static TimeSpan GetEndTime()
        {
            int h, m;
            GetOperationalTo(out h, out m);
            return TimeSpan.FromSeconds(h * 3600 + m * 60);
        }

    }
}
