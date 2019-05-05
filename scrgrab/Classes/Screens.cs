using System;

namespace scrgrab.Classes
{
    /// <summary>
    /// Enumeration deining the types of screen
    /// </summary>
    public partial class Capture
    {
        /// <summary>
        /// Types of screen
        /// </summary>
        public enum Screens
        {
            /// <summary>
            /// All screens
            /// </summary>
            All,
            /// <summary>
            /// Only the primary screen
            /// </summary>
            Primary
        }

        /// <summary>
        /// Parse a string to it's equivalent Screens enum or return a default if the string cannot be parsed
        /// </summary>
        /// <param name="value">the value to parse</param>
        /// <returns>parsed value or a default</returns>
        public static Screens Parse(string value)
        {
            Screens result;
            if (Enum.TryParse(value, true, out result))
                return result;
            return Screens.All;
        }
    }
}
