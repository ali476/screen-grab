using System;

namespace scrgrab.Classes
{
    partial class ImageManager
    {
        /// <summary>
        /// nested class to hold details of each image
        /// </summary>
        public class ImageInfo
        {
            public String Filename { get { return System.IO.Path.Combine(Path,Name); } }
            public String Path { get; set; }
            public DateTime ImageDateTime { get; set; }
            public String Name { get; set; }

            /// <summary>
            /// Parse the value of image's date and time
            /// </summary>
            /// <param name="value"></param>
            public void ParseDateTime(string value)
            {
                // expecting left 8 characters as yyyymmdd and the next 6 as hhmmss
                if (DateTime.TryParseExact(value.Length >= 14 ? value.Substring(0, 14) : null,
                    "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime dateTime))
                    ImageDateTime = dateTime;
            }

            /// <summary>
            /// Check if date of <see cref="ImageDateTime"/> matches date of <paramref name="dateTime"/>
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public bool DateOf(DateTime dateTime)
            {
                return ImageDateTime.Date.CompareTo(dateTime.Date) == 0;
            }

            public override String ToString()
            {
                return this.ImageDateTime.ToString("dd MMM yyyy HH:mm");
            }
        }
    }
}
