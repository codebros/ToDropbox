using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ToDropbox.Properties;

namespace ToDropbox
{
    public partial class TextForm : Form
    {
        public TextForm(/*Image screenshot*/)
        {
            InitializeComponent();
        }

        private void TextForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.icon;

            IDataObject data = Clipboard.GetDataObject();

            if (data.GetDataPresent(DataFormats.Text) || data.GetDataPresent(DataFormats.Html) || data.GetDataPresent(DataFormats.Rtf))
            {
                String text = (String)data.GetData(DataFormats.Text, true);
                String now = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToString("HHmmss");
                String url = Properties.Settings.Default.urlprefix + now + ".txt" + Properties.Settings.Default.urlsuffix;
                using (StreamWriter outfile = new StreamWriter(Properties.Settings.Default.savefolder + @"\" + now + ".txt"))
                {
                    outfile.Write(text);
                }

                richTextBox1.Text = text;
                Clipboard.SetDataObject(url, true);
            }

            if (data.GetDataPresent(DataFormats.Rtf))
            {

            }
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }

    }
}
