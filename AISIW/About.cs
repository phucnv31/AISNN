using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISIW
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            this.linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.facebook.com/AISIW-Auto-Image-Search-inactive-Windows-140059496575215/?fref=ts");

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.facebook.com/nguyen.phuc.33865");
        }

        private void About_Load(object sender, EventArgs e)
        {
            string path = @"AISIW\about.txt";
            if (File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamReader rd = new StreamReader(fs, Encoding.UTF8);
                string str = rd.ReadToEnd();
                textBox1.Text = str;
                rd.Close();
                fs.Close();
            }
            

        }
    }
}
