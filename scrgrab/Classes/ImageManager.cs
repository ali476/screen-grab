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
    partial class ImageManager
    {
        private const String c_image_db = "image.db";
        private String m_folder;
        private int m_index = -1;

        /// <summary>
        /// access to images property
        /// </summary>
        public List<ImageInfo> Images { get; } = new List<ImageInfo>();

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
        public static void Enumerate(String folder)
        {
            ImageManager im = new ImageManager(folder);
            Task.Factory.StartNew(() => im.EnumerateToFile());
        }

        /// <summary>
        /// enumerate folder/file list to a file
        /// </summary>
        public void EnumerateToFile()
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
        public void GetImages(DateTime date)
        {
            Images.Clear();

            if (!File.Exists(Path.Combine(m_folder, c_image_db)))
                EnumerateToFile();

            List<String> files = File.ReadAllLines(Path.Combine(m_folder, c_image_db)).ToList<String>();

            foreach (String file in files)
            {
                String[] split = file.Split(new String[] { "$_$" }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length > 1)
                {
                    ImageInfo inf = new ImageInfo() { Path = split[0], Name = split[1] };
                    inf.ParseDateTime(split[1]);
                    if (inf.DateOf(date))
                        Images.Add(inf);
                }
            }

            // finally sort by datetime
            Images.Sort(delegate (ImageInfo c1, ImageInfo c2) { return c1.ImageDateTime.CompareTo(c2.ImageDateTime); });
        }

        /// <summary>
        /// Returns the index of the first image matching the specified time (regarless of image date) or -1 if none found
        /// </summary>
        /// <param name="hour"></param>
        /// <returns>int</returns>
        public int IndexOfByTime(int hour)
        {
            for (int x = 0; x < Images.Count; x++)
                if (Images[x].ImageDateTime.Hour == hour)
                    return x;
            return -1;
        }

        /// <summary>
        /// <see cref="ImageInfo"/> of the next, or last image
        /// </summary>
        /// <returns><see cref="ImageInfo"/></returns>
        public ImageInfo NextImage()
        {
            if (Images.Count < 0)
                return null;
            m_index = (m_index == -1) ? 0 : (m_index < Images.Count - 1) ? m_index + 1 : m_index;
            return Images[m_index];
        }

        /// <summary>
        /// <see cref="ImageInfo"/> of the previous, or first image
        /// </summary>
        /// <returns><see cref="ImageInfo"/></returns>
        public ImageInfo PrevImage()
        {
            if (Images.Count < 0)
                return null;
            m_index = (m_index == -1) ? 0 : (m_index > 0) ? m_index - 1 : m_index;
            return Images[m_index];
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
