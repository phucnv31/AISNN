using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISIW
{
    public partial class ChupAnh : Form
    {
        bool paint = false;
        bool start = true;
        bool gui = false;
        static Point MousePosDown = new Point(-100, -100);
        static Point MouseCurPos = new Point(-100, -100);
        static Point MouseFinalPos = new Point(-100, -100);
        private Form1 mainForm = null;
        public ChupAnh(Form callingForm)
        {
            mainForm = callingForm as Form1; 
            InitializeComponent();
        }
        private void ChupAnh_DragDrop(object sender, DragEventArgs e)
        {
        }

        private void ChupAnh_MouseDown(object sender, MouseEventArgs e)
        {
            MousePosDown = MousePosition;
            paint = true;
            start = false;
            gui = false;
        }

        private void ChupAnh_MouseUp(object sender, MouseEventArgs e)
        {
            MouseFinalPos = MousePosition;
            paint = false;
            VeKhung();
            Gui(MouseCurPos);
            this.Validate();
            Point[] parr = PointToRect(MouseFinalPos, MousePosDown);
            Size z = new Size();
            z.Width = Math.Abs(MousePosDown.X - MouseFinalPos.X);
            z.Height = Math.Abs(MousePosDown.Y - MouseFinalPos.Y);
            HienAnhCat(parr[0], z);
        }
        private void Chup_Anh(Point p, Size s,bool save)
        {
            Bitmap bm = new Bitmap(s.Width, s.Height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bm);
            //g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            g.CopyFromScreen(p.X, p.Y, 0, 0, s);
            if (!Directory.Exists("Image"))
            {
                Directory.CreateDirectory("Image");
            }
            int i = 0;
            while (File.Exists("Image\\Screenshot" + i + ".png"))
            {
                i++;
            }
            if (save)
            {
                bm.Save("Image\\Screenshot" + i + ".png");
                string Curpath = Application.StartupPath;
                MessageBox.Show("Ảnh lưu thành công tại: " + Curpath + "Image\\Screenshot" + i + ".png");
            }
            else
            {
                Clipboard.SetImage(bm);
            }
            mainForm.SetCheckChupAnh = false;
            g.Dispose();
            bm.Dispose();
        }
        private void VeKhung()
        {
            if (!paint) { MouseCurPos = MouseFinalPos; }
            Pen p = new Pen(Color.WhiteSmoke, 2);
            Graphics paper = this.CreateGraphics();
            if (MousePosDown.X < MouseCurPos.X)
            {
                if (MousePosDown.Y < MouseCurPos.Y)
                {
                    paper.DrawRectangle(p, MousePosDown.X, MousePosDown.Y, Math.Abs(MousePosDown.X - MouseCurPos.X), Math.Abs(MousePosDown.Y - MouseCurPos.Y));
                }
                if (MousePosDown.Y > MouseCurPos.Y)
                {
                    paper.DrawRectangle(p, MousePosDown.X, MouseCurPos.Y, Math.Abs(MousePosDown.X - MouseCurPos.X), Math.Abs(MousePosDown.Y - MouseCurPos.Y));
                }
            }
            if (MousePosDown.X > MouseCurPos.X)
            {
                if (MousePosDown.Y < MouseCurPos.Y)
                {
                    paper.DrawRectangle(p, MouseCurPos.X, MousePosDown.Y, Math.Abs(MousePosDown.X - MouseCurPos.X), Math.Abs(MousePosDown.Y - MouseCurPos.Y));
                }
                if (MousePosDown.Y > MouseCurPos.Y)
                {
                    paper.DrawRectangle(p, MouseCurPos.X, MouseCurPos.Y, Math.Abs(MousePosDown.X - MouseCurPos.X), Math.Abs(MousePosDown.Y - MouseCurPos.Y));
                }
            }
            p.Dispose();
            paper.Dispose();
        }

        private void ChupAnh_MouseMove(object sender, MouseEventArgs e)
        {
            MouseCurPos = MousePosition;
            if (paint)
            {
                VeKhung();
                this.Invalidate();
            }
            else { }
        }

        private void ChupAnh_Paint(object sender, PaintEventArgs e)
        {
            if (!start)
            {
                VeKhung();
                if (!gui)
                {
                    VeKhung();
                }
            }
        }

        private void ChupAnh_Load(object sender, EventArgs e)
        {
            mainForm.SetCheckChupAnh = true;
            ToolTip toolCopy = new ToolTip();
            toolCopy.SetToolTip(btnCopy, "Copy to clipboard");
            ToolTip toolLuu = new ToolTip();
            toolLuu.SetToolTip(btnLuu, "Lưu vào thư mục Image");
            ToolTip toolHuy = new ToolTip();
            toolHuy.SetToolTip(btnHuy, "Hủy thao tác");


        }

        private void ChupAnh_Validated(object sender, EventArgs e)
        {
        }
        private void Gui(Point MouseCurPos)
        {
            gui = true;
            if (MouseFinalPos.Y < Screen.PrimaryScreen.Bounds.Height - btnLuu.Height)
            {
                if (MouseFinalPos.X < Screen.PrimaryScreen.Bounds.Width - (btnLuu.Width + 2 + btnHuy.Width))
                {
                    btnLuu.Location = MouseFinalPos;
                    btnCopy.Location = new Point(btnLuu.Location.X - btnLuu.Width, btnLuu.Location.Y);
                    btnHuy.Location = new Point(MouseFinalPos.X + btnLuu.Width + 2, MouseFinalPos.Y);
                }
                else
                {
                    btnLuu.Location = new Point(MouseFinalPos.X - (btnLuu.Width + 2 + btnHuy.Width), MouseFinalPos.Y);
                    btnCopy.Location = new Point(btnLuu.Location.X - btnLuu.Width, btnLuu.Location.Y);
                    btnHuy.Location = new Point(btnLuu.Location.X + btnLuu.Width + 2, btnLuu.Location.Y);
                }
            }
            else
            {
                if (MouseFinalPos.X < Screen.PrimaryScreen.Bounds.Width - (btnLuu.Width + 2 + btnHuy.Width))
                {
                    btnLuu.Location = new Point(MouseFinalPos.X, MouseFinalPos.Y - btnLuu.Height);
                    btnCopy.Location = new Point(btnLuu.Location.X - btnLuu.Width, btnLuu.Location.Y);
                    btnHuy.Location = new Point(MouseFinalPos.X + btnLuu.Width + 2, MouseFinalPos.Y - btnLuu.Height);
                }
                else
                {
                    btnLuu.Location = new Point(MouseFinalPos.X - (btnLuu.Width + 2 + btnHuy.Width), MouseFinalPos.Y - btnLuu.Height);
                    btnCopy.Location = new Point(btnLuu.Location.X - btnLuu.Width, btnLuu.Location.Y);
                    btnHuy.Location = new Point(btnLuu.Location.X + btnLuu.Width + 2, btnLuu.Location.Y);
                }
            }

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            Size z = new Size();
            z.Width = Math.Abs(MousePosDown.X - MouseFinalPos.X);
            z.Height = Math.Abs(MousePosDown.Y - MouseFinalPos.Y);
            this.Close();
            Chup_Anh(PointToRect(MousePosDown, MouseFinalPos)[0], z,true);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Close();
            mainForm.SetCheckChupAnh = false;
        }
        private void HienAnhCat(Point p, Size s)
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(bm);
            this.Opacity=0;
            //g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            g.CopyFromScreen(p.X, p.Y, 0, 0, s);
            this.Opacity = .7;
            g.Dispose();
            pb.Size = s;
            pb.Image = bm;
            Point[] parr = PointToRect(MousePosDown,MouseFinalPos);
            pb.Location = parr[0];
            pb.Show();
            btnCopy.BringToFront();
            btnLuu.BringToFront();
            btnHuy.BringToFront();
        }
        private Point[] PointToRect(Point p1, Point p2)
        {
            Point[] parr = new Point[2];
            if (p1.X < p2.X)
            {
                parr[0].X = p1.X;
                parr[1].X = p2.X;
            }
            else 
            { 
                parr[0].X = p2.X; 
                parr[1].X = p1.X; 
            }
            if (p1.Y < p2.Y)
            {
                parr[0].Y = p1.Y;
                parr[1].Y = p2.Y;
            }
            else
            {
                parr[0].Y = p2.Y;
                parr[1].Y = p1.Y;
            }

            return parr;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Size z = new Size();
            z.Width = Math.Abs(MousePosDown.X - MouseFinalPos.X);
            z.Height = Math.Abs(MousePosDown.Y - MouseFinalPos.Y);
            this.Close();
            Chup_Anh(PointToRect(MousePosDown, MouseFinalPos)[0], z,false);
        }
    }
}
