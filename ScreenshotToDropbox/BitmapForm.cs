using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ToDropbox.Properties;

namespace ToDropbox
{
    public partial class BitmapForm : Form
    {
        public BitmapForm(/*Image screenshot*/)
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Icon = Resources.icon;
            IDataObject data = Clipboard.GetDataObject();

            if (data.GetDataPresent(DataFormats.Bitmap))
            {
                Image image = (Image)data.GetData(DataFormats.Bitmap, true);
                String now = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                image.Save(Properties.Settings.Default.savefolder + @"\" + now + ".png", System.Drawing.Imaging.ImageFormat.Png);
                String url = Properties.Settings.Default.urlprefix + now + ".png" + Properties.Settings.Default.urlsuffix;

                // Prevent using images internal thumbnail
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                image.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                
                int NewWidth = pictureBox1.Width;
                int MaxHeight = pictureBox1.Height;
                if (image.Width <= NewWidth)
                {
                    NewWidth = image.Width;
                }                

                int NewHeight = image.Height * NewWidth / image.Width;
                if (NewHeight > MaxHeight)
                {
                    // Resize with height instead
                    NewWidth = image.Width * MaxHeight / image.Height;
                    NewHeight = MaxHeight;
                }

                System.Drawing.Image NewImage = image.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

                // Clear handle to original file so that we can overwrite it if necessary
                image.Dispose();

                this.pictureBox1.Width = NewWidth;
                this.pictureBox1.Height = NewHeight;
                this.pictureBox1.Image = NewImage;
                this.Height = NewHeight+50;
                this.Width = NewWidth + 30;

                Clipboard.SetDataObject(url, true);                
            } 
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}
