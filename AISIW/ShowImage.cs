using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ANN;
using Accord.Imaging.Converters;
namespace AISIW
{
    public partial class ShowImage : Form
    {
        Bitmap bm = null;
        Point FirstPoint = new Point();
        Point FinalPoint = new Point();
        Point LeftTop = new Point();
        Point RightBottom = new Point();
        bool isPaint = false;
        public ShowImage(Bitmap bmf)
        {
            InitializeComponent();
            bm = bmf;
        }

        private void ShowImage_Load(object sender, EventArgs e)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            LoadImage();
            ImageToArray conv = new ImageToArray(min: 0, max: 1);
            double[] Input;
            conv.Convert(bm, out Input);
            //bm = MakeGrayScale(bm);
            //stopWatch.Stop();
            //var y = stopWatch.ElapsedMilliseconds;
            //var x = ImageToArray(bm);
            //List<double> lst = d.Cast<double>().ToList();
            NeuralNetwork network = new NeuralNetwork(0.00125, new int[] { 2, 4, 1 });
            for (int i = 0; i < 1000; i++)
            {

            }
            var output = network.Run(new double[] { 0, 0 }.ToList());
            MessageBox.Show(output[0].ToString());
            //if (bm.Width + 100 < Screen.PrimaryScreen.Bounds.Width && bm.Height + 100 < Screen.PrimaryScreen.Bounds.Height)
            //{
            //    this.Width = bm.Width + 20;
            //    this.Height = bm.Height + 50;
            //    this.CenterToScreen();
            //    pictureBox1.Width = bm.Width;
            //    pictureBox1.Height = bm.Height;
            //    pictureBox1.Image = bm;
            //}
            //else
            //{
            //    this.Width = bm.Width / 2 + 20;
            //    this.Height = bm.Height / 2 + 40;
            //    this.CenterToScreen();
            //    pictureBox1.Width = bm.Width / 2;
            //    pictureBox1.Height = bm.Height / 2;
            //    pictureBox1.Image = bm;
            //}

        }
        public static Bitmap MakeGrayScale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
        public void LoadImage()
        {
            //string fileNameBM = @"Image\BM.bmp";
            //Bitmap bm = new Bitmap(Image.FromFile(fileNameBM));
            Width = bm.Width;
            Height = bm.Height;
            BackgroundImage = bm;
            pictureBox.Width = bm.Width;
            pictureBox.Height = bm.Height;
        }
        public void DrawRectangle(Point leftTop, Point rightBottom)
        {
            if (isPaint)
            {
                var width = Math.Abs(rightBottom.X - leftTop.X);
                var height = Math.Abs(rightBottom.Y - leftTop.Y);
                //Pen p = new Pen(Color.Red, 2);
                //Graphics paper = this.CreateGraphics();
                //paper.DrawRectangle(p, leftTop.X, leftTop.Y, width, height);
                //p.Dispose();
                //paper.Dispose();
                Bitmap bitmap = new Bitmap(this.bm);
                // Draw the rectangle.
                using (Graphics gr = Graphics.FromImage(bitmap))
                {
                    gr.DrawRectangle(Pens.Red, leftTop.X, leftTop.Y, width, height);
                }
                pictureBox.Image = bitmap;
            }
        }
        public void Draw()
        {
            LeftTop.X = FirstPoint.X <= FinalPoint.X ? FirstPoint.X : FinalPoint.X;
            LeftTop.Y = FirstPoint.Y <= FinalPoint.Y ? FirstPoint.Y : FinalPoint.Y;
            RightBottom.X = FirstPoint.X >= FinalPoint.X ? FirstPoint.X : FinalPoint.X;
            RightBottom.Y = FirstPoint.Y >= FinalPoint.Y ? FirstPoint.Y : FinalPoint.Y;
            DrawRectangle(LeftTop, RightBottom);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            FirstPoint = e.Location;
            isPaint = true;
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            FinalPoint = e.Location;
            Draw();
            this.Validate();
            isPaint = false;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPaint)
            {
                FinalPoint = e.Location;
                Draw();
            }
        }
    }
}
