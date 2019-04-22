using System;
using System.Drawing;
using System.Windows.Forms;

namespace scrgrab.Classes
{
    /// <summary>
    /// Capture a screen image
    /// </summary>
    public class Capture
    {
        private String filename = "";

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="filename"></param>
        public Capture(String filename)
        {
            this.filename = filename;
        }

        /// <summary>
        /// capture the current screen image and save as the filename that was provided when this object was created.
        /// </summary>
        public void capture()
        {
            using (Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height,
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
