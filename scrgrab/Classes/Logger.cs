using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace scrgrab.Classes
{
    class Logger:IDisposable
    {

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Logger() {
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

        private string m_folder;
        /// <summary>
        /// standard logger with a working folder
        /// </summary>
        /// <param name="folder"></param>
        public Logger(string folder):base() { m_folder = folder; }

        public void Log(string value)
        {
            //string file = this.GetType().Assembly.Location;
            //string app = System.IO.Path.GetFileNameWithoutExtension(file);
            string filename = Path.Combine(m_folder, "log.txt");
            value = String.Format("{0}: {1}", DateTime.Now.ToString(), value);
            if (File.Exists(filename))
            {
                using (StreamWriter sw = File.AppendText(filename))
                {
                    sw.WriteLine(value);
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(filename))
                {
                    sw.WriteLine(value);
                }
            }
        }

        /// <summary>
        /// Static member to simplify calling the Log method
        /// </summary>
        /// <param name="folder">location of the log file</param>
        /// <param name="value">log value</param>
        public static void Log(string folder, string value)
        {
            using (Logger log = new Logger(folder))
            {
                log.Log(value);
            }
        }
    }
}
