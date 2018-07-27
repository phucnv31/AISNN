using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISIW
{
    public partial class ShowImage : Form
    {
        Bitmap bm = null;
        public ShowImage(Bitmap bmf)
        {
            InitializeComponent();
            bm = bmf;
        }

        private void ShowImage_Load(object sender, EventArgs e)
        {
            if (bm.Width + 100 < Screen.PrimaryScreen.Bounds.Width && bm.Height+100< Screen.PrimaryScreen.Bounds.Height)
            {
                this.Width = bm.Width + 20;
                this.Height = bm.Height + 50;
                this.CenterToScreen();
                pictureBox1.Width = bm.Width;
                pictureBox1.Height = bm.Height;
                pictureBox1.Image = bm;
            }
            else
            {
                this.Width = bm.Width/2 + 20;
                this.Height = bm.Height/2 + 40;
                this.CenterToScreen();
                pictureBox1.Width = bm.Width/2;
                pictureBox1.Height = bm.Height/2;
                pictureBox1.Image = bm;
            }
            
        }
    }
}
