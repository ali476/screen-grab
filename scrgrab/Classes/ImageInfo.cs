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
            public string Filename { get { return System.IO.Path.Combine(Path, Name); } }
            public string Path { get; set; }
            public DateTime ImageDateTime { get; set; }
            public string Name { get; set; }

            /// <summary>
            /// Check if the image is of the given date
            /// </summary>
            /// <param name="date"></param>
            /// <returns>bool</returns>
            public bool DateOf(DateTime date) => ImageDateTime.Date.CompareTo(date.Date) == 0;

            /// <summary>
            /// Set the value of <see cref="ImageDateTime"/> from the supplied string value provided it is of the correct format and length
            /// </summary>
            /// <param name="value"></param>
            public void SetDateFromString(string value)
            {
                // the left 8 characters are yyyymmdd, next 6 are HHmmss
                if (DateTime.TryParseExact(value.Length >= 14 ? value.Substring(0, 14) : null,
                    "yyyyMMddHHmmss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDatetime))
                    ImageDateTime = parsedDatetime;
            }

            public override String ToString()
            {
                return this.ImageDateTime.ToString("dd MMM yyyy HH:mm");
            }

        }
    }
}
