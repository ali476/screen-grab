using System;
using System.Windows.Forms;

namespace scrgrab.Classes
{
    /// <summary>
    /// Image Viewer form
    /// </summary>
    public partial class Viewer : Form
    {
        private ImageManager m_imageManager = new ImageManager(Configuration.WorkingFolder);

        /// <summary>
        /// Image viewer form default constructor
        /// </summary>
        public Viewer()
        {
            InitializeComponent();

            cbxTime.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 });

            datePicker.Value = DateTime.Now;
        }

        private void showImage(Keys key)
        {
            int x = cbxImages.SelectedIndex
                , y = cbxImages.Items.Count - 1;

            if (y < 1)
                return;

            if (key == Keys.Left)
            {
                x = x > 0 ? x - 1 : 0;
            }
            else if (key == Keys.Right)
            {
                x = x < y - 1 ? x + 1 : y;
            }
            cbxImages.SelectedIndex = x;
        }

        private void showImage(ImageManager.ImageInfo image)
        {
            if (image == null)
            {
                pictureBox.Image = null;
                labelTitle.Text = "";
                return;
            }
            pictureBox.LoadAsync(image.Filename);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            labelTitle.Text = "Image taken on " + image.ToString();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            m_imageManager.GetImages(datePicker.Value);

            // set the time combo to first hour
            int hr, mn;
            Configuration.GetOperationalFrom(out hr, out mn);

            if (m_imageManager.Images.Count > 0)
            {
                hr = m_imageManager.Images[0].ImageDateTime.Hour;
            }

            // list available images
            cbxImages.Items.Clear();
            cbxImages.Items.AddRange((ImageManager.ImageInfo[])m_imageManager.Images.ToArray());

            cbxTime.SelectedIndex = cbxTime.Items.IndexOf(hr);

        }

        private void cbxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxTime.SelectedIndex > -1)
            {
                int hr = (int)cbxTime.Items[cbxTime.SelectedIndex];

                // find an image for this time and display it
                int x = m_imageManager.IndexOfByTime(hr);
                if (x > -1)
                    cbxImages.SelectedIndex = x;
            }
        }

        private void cbxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            showImage(cbxImages.SelectedIndex == -1 ? null : (ImageManager.ImageInfo)cbxImages.Items[cbxImages.SelectedIndex]);
        }

        private void Viewer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                showImage(Keys.Left);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right)
            {
                showImage(Keys.Right);
                e.Handled = true;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            showImage(Keys.Right);
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            showImage(Keys.Left);
        }
    }
}
