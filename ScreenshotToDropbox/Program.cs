using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ToDropbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            if (Clipboard.GetDataObject() != null)
            {
                if (!Directory.Exists(Properties.Settings.Default.savefolder))
                {
                    MessageBox.Show("The Save Folder (" + Properties.Settings.Default.savefolder + ") does not exists - check your configuration", "to Dropbox", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                IDataObject data = Clipboard.GetDataObject();

                if (data.GetDataPresent(DataFormats.Bitmap))
                {
                    Application.Run(new BitmapForm());
                }
                else if (data.GetDataPresent(DataFormats.Text))
                {
                    Application.Run(new TextForm());
                }
                else if (Clipboard.ContainsFileDropList())
                {
                    Application.Run(new FileForm());
                }
                else
                {
                    MessageBox.Show("The Data In Clipboard is not supported", "to Dropbox", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("The Clipboard is empty", "Screenshot to Dropbox", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            
        }

        public static IEnumerable<IEnumerable<T>> Transpose<T>(
            this IEnumerable<IEnumerable<T>> source)
        {
            var enumerators = source.Select(e => e.GetEnumerator()).ToArray();
            try
            {
                while (enumerators.All(e => e.MoveNext()))
                {
                    yield return enumerators.Select(e => e.Current).ToArray();
                }
            }
            finally
            {
                Array.ForEach(enumerators, e => e.Dispose());
            }
        }
    }
}
