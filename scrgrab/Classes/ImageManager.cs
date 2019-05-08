using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace scrgrab.Classes
{
    /// <summary>
    /// Manage list of images
    /// </summary>
    class ImageManager
    {
        private const String c_image_db = "image.db";
        private String m_folder;
        private List<ImageInfo> m_images = new List<ImageInfo>();
        private int m_index = -1;

        /// <summary>
        /// access to images property
        /// </summary>
        public List<ImageInfo> Images { get { return m_images; } }

        /// <summary>
        /// nested class to hold details of each image
        /// </summary>
        public class ImageInfo
        {
            public String Filename { get { return Path + "\\" + Name; } }
            public String Path { get; set; }
            public DateTime Time { get; set; }
            public String Name { get; set; }

            public override String ToString()
            {
                return this.Time.ToString("dd MMM yyyy HH:mm");
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="folder">String</param>
        public ImageManager(String folder)
        {
            this.m_folder = folder;
        }

        /// <summary>
        /// static enumerate method to run the enumeration in a thread
        /// </summary>
        /// <param name="folder">String</param>
        public static void enumerate(String folder)
        {
            ImageManager im = new ImageManager(folder);
            Task.Factory.StartNew(() => im.enumerateToFile());
        }

        /// <summary>
        /// enumerate folder/file list to a file
        /// </summary>
        public void enumerateToFile()
        {
            List<String> list = new List<string>();

            var files = from enumfolder in enumerateFolders()
                        from file in enumerateFiles(enumfolder)
                        orderby Path.GetFileName(file)
                        select new { @Folder = enumfolder, @File = Path.GetFileName(file) };

            foreach (var file in files)
            {
                list.Add(String.Format("{0}$_${1}", file.Folder, file.File));
            }

            File.WriteAllLines(Path.Combine(m_folder, c_image_db), list);
        }

        /// <summary>
        /// get a list of available images for the given date
        /// </summary>
        /// <param name="date">DateTime</param>
        public void getImages(DateTime date)
        {
            m_images.Clear();

            if (!File.Exists(Path.Combine(m_folder, c_image_db)))
                enumerateToFile();

            List<String> files = File.ReadAllLines(Path.Combine(m_folder, c_image_db)).ToList<String>();

            foreach (String file in files)
            {
                String[] split = file.Split(new String[] { "$_$" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length > 1)
                {
                    ImageInfo inf = new ImageInfo();
                    inf.Path = split[0];
                    inf.Name = split[1];
                    // the left 8 characters are date, next 6 are HH:mm:ss
                    inf.Time = DateTime.ParseExact(inf.Name.Substring(0, 14), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    if (inf.Time.Date == date.Date)
                        m_images.Add(inf);
                }
            }

            // finally sort by datetime
            m_images.Sort(delegate (ImageInfo c1, ImageInfo c2) { return c1.Time.CompareTo(c2.Time); });
        }

        public int indexOfByTime(int hour)
        {
            for (int x = 0; x < m_images.Count; x++)
                if (m_images[x].Time.Hour == hour)
                    return x;
            return -1;
        }

        public ImageInfo nextImage()
        {
            if (m_images.Count < 0)
                return null;
            m_index = (m_index == -1) ? 0 : (m_index < m_images.Count - 1) ? m_index + 1 : m_index;
            return m_images[m_index];
        }

        public ImageInfo prevImage()
        {
            if (m_images.Count < 0)
                return null;
            m_index = (m_index == -1) ? 0 : (m_index > 0) ? m_index - 1 : m_index;
            return m_images[m_index];
        }


        private List<String> enumerateFolders()
        {
            try
            {
                return Directory.GetDirectories(m_folder, "*", SearchOption.AllDirectories).ToList<String>();
            }
            catch (UnauthorizedAccessException)
            {
                return new List<String>();
            }
        }

        private List<String> enumerateFiles(String folder)
        {
            return Directory.EnumerateFiles(folder).ToList<String>();
        }
    }
}
