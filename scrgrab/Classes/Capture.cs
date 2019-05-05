using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace scrgrab.Classes
{
    /// <summary>
    /// Capture a screen image
    /// </summary>
    public partial class Capture
    {
        private String m_filename_template = "";

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="filename"></param>
        public Capture(string filename) => this.m_filename_template = filename;

        /// <summary>
        /// capture the current screen image and save as the filename that was provided when this object was created.
        /// </summary>
        public void capture(Screens screens)
        {
            // always capture the primary screen, unless specifically asked for Secondary Screen
            switch (screens)
            {
                case Screens.Primary:
                    capture(Screen.PrimaryScreen, GetImageFilename(m_filename_template,Screen.PrimaryScreen));
                    break;
                default:
                    foreach (Screen screen in Screen.AllScreens)
                    {
                        capture(screen: screen, filename: GetImageFilename(m_filename_template, screen));
                    }
                    break;
            }
        }

        private string GetImageFilename(string filename_template, Screen screen)
        {
            // remove unwated characters
            // source: https://stackoverflow.com/questions/7411438/remove-characters-from-c-sharp-string
            return string.Format("{0}_{1}.png",
                filename_template, new string(screen.DeviceName.Where(x => char.IsWhiteSpace(x) || char.IsLetterOrDigit(x)).ToArray()));
        }

        private void capture(Screen screen, string filename)
        {
            using (Bitmap bmp = new Bitmap(screen.Bounds.Width,
                                            screen.Bounds.Height,
                                            System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0, bmp.Size,
                                     CopyPixelOperation.SourceCopy);

                    bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }
    }
}
