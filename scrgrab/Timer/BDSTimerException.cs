using scrgrab.Classes;
using System;
using System.Runtime.Serialization;

namespace scrgrab.Timer
{
    /// <summary>
    /// Custom exception for DBSTimer
    /// </summary>
    [Serializable]
    public class BDSTimerException : Exception
    {
        /// <summary>
        /// standard ctor
        /// </summary>
        public BDSTimerException()
        {
        }

        /// <summary>
        /// standard handler for BDSException when no inner-exception is specified
        /// </summary>
        /// <param name="message">the error message for this exception</param>
        public BDSTimerException(string message) : base(message)
        {
            Logger.Log(Configuration.WorkingFolder, "Exception -> " + message);
        }

        /// <summary>
        /// standard handler for BDSException when there is an inner-exception specified
        /// </summary>
        /// <param name="message">the error message for this exception</param>
        /// <param name="innerException">inner exception</param>
        public BDSTimerException(string message, Exception innerException) : base(message, innerException)
        {
            Logger.Log(Configuration.WorkingFolder,
                string.Format("Exception -> {0}{1}", message, innerException == null ? "" : innerException.Message));
        }

        /// <summary>
        /// implemented by base
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BDSTimerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
