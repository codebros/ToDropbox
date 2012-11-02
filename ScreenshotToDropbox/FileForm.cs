using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using Ionic.Zip;
using ToDropbox.Properties;

namespace ToDropbox
{
    public partial class FileForm : Form
    {
        public FileForm(/*Image screenshot*/)
        {
            InitializeComponent();
        }

        private void FileForm_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.icon;
            
            if (Clipboard.ContainsFileDropList())
            {

                var fileDropList = Clipboard.GetFileDropList();
                String now = DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToString("HHmmss");

                if (fileDropList.Count == 1)
                {                    
                    var path = fileDropList[0];
                    FileAttributes attr = File.GetAttributes(path);

                    if ((attr & FileAttributes.Directory) != FileAttributes.Directory)
                    {
                        var filename = Path.GetFileNameWithoutExtension(path) + "-" + now + Path.GetExtension(path);
                        File.Copy(path, Properties.Settings.Default.savefolder + @"\" + filename);
                        String url = Properties.Settings.Default.urlprefix + filename + Properties.Settings.Default.urlsuffix;
                        
                        richTextBox1.Text += "Added file to Dropbox:\n" + filename;
                        Clipboard.SetDataObject(url, true);

                        return;
                    }                    
                }
                
                //Multiple files/directory, zip it
                using (ZipFile zip = new ZipFile())
                {
                    var files = new List<string>();

                    richTextBox1.Text += "Zipped and added multiple files to Dropbox:\n";
                    foreach (var file in fileDropList)
                    {
                        files.Add(file);
                        richTextBox1.Text += file + "\n";
                    }
                    
                    var commonDir = FindCommonDirectoryPath.FindCommonPath("/", files);

                    foreach (var file in files)
                    {
                        zip.AddItem(file, commonDir);
                    }
                    
                    var filename = now + ".zip";                    
                    zip.Save(Properties.Settings.Default.savefolder + @"\" + filename);
                    String url = Properties.Settings.Default.urlprefix + filename + Properties.Settings.Default.urlsuffix;

                    Clipboard.SetDataObject(url, true);
                    return;
                }

            }                        
        }

        private void KeyPressed(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }        
    }

    class FindCommonDirectoryPath
    {
        public static string FindCommonPath(string Separator, List<string> Paths)
        {
            string CommonPath = String.Empty;
            List<string> SeparatedPath = Paths
                .First(str => str.Length == Paths.Max(st2 => st2.Length))
                .Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            foreach (string PathSegment in SeparatedPath.AsEnumerable())
            {
                if (CommonPath.Length == 0 && Paths.All(str => str.StartsWith(PathSegment)))
                {
                    CommonPath = PathSegment;
                }
                else if (Paths.All(str => str.StartsWith(CommonPath + Separator + PathSegment)))
                {
                    CommonPath += Separator + PathSegment;
                }
                else
                {
                    break;
                }
            }

            return CommonPath;
        }
    }
}
