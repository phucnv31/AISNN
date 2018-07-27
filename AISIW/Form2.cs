using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISIW
{
    public partial class Form2 : Form
    {
        private Form1 mainForm = null;
        public Form2(Form callingForm)
        {
            mainForm = callingForm as Form1; 
            InitializeComponent();
        }
        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            int i=0;
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    i++;
                    listBox1.Items.Add(i+" : "+process.MainWindowTitle);
                    listBox1.Height = listBox1.PreferredHeight;
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            int i = 0;
            bool success = false;
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    i++;
                    if (i + " : " + process.MainWindowTitle == listBox1.SelectedItem.ToString())
                    {
                        string s = process.MainWindowTitle;
                        if (s.Length > 20)
                        {
                            s=s.Substring(0, 20);
                            mainForm.LabelTextWtitle = s + "...";
                        }
                        else
                        {
                            mainForm.LabelTextWtitle = s;
                        }
                        success = true;
                        mainForm.getprocess=process;
                        this.Close();
                    }
                }
            }
            if (!success)
            {
                MessageBox.Show("Cửa Sổ bạn chọn đã tắt hoặc không tồn tại!!\nMời Chọn Lại Cửa Sổ !");
                this.Close();
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            Process[] processlist = Process.GetProcesses();
            int i = 0;
            bool success = false;
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    i++;
                    if (i + " : " + process.MainWindowTitle == listBox1.SelectedItem.ToString())
                    {
                        string s = process.MainWindowTitle;
                        if (s.Length > 20)
                        {
                            s = s.Substring(0, 20);
                            mainForm.LabelTextWtitle = s + "...";
                        }
                        else
                        {
                            mainForm.LabelTextWtitle = s;
                        }
                        success = true;
                        mainForm.getprocess = process;
                        this.Close();
                    }
                }
            }
            if (!success)
            {
                MessageBox.Show("Cửa Sổ bạn chọn đã tắt hoặc không tồn tại!!\nMời Chọn Lại Cửa Sổ !");
                this.Close();
            }
        }
    }
}
