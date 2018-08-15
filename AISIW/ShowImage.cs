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
        public ShowImage(Bitmap bmf)
        {
            InitializeComponent();
            bm = bmf;
        }

        private void ShowImage_Load(object sender, EventArgs e)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            
            ImageToArray conv = new ImageToArray(min: 0, max: 1);
            double[] Input;
            conv.Convert(bm,out Input);
            //bm = MakeGrayScale(bm);
            //stopWatch.Stop();
            //var y = stopWatch.ElapsedMilliseconds;
            //var x = ImageToArray(bm);
            //List<double> lst = d.Cast<double>().ToList();
            NeuralNetwork network = new NeuralNetwork(0.00125, new int[] { 2,4,1});
            for (int i = 0; i < 1000000; i++)
            {
                network.Train(new double[] { 1, 0 }.ToList(), new double[] { 1 }.ToList());
                network.Train(new double[] { 0, 1 }.ToList(), new double[] { 1 }.ToList());
                network.Train(new double[] { 1, 1 }.ToList(), new double[] { 1 }.ToList());
                //network.Train(new double[] { 0, 0 }.ToList(), new double[] { 0 }.ToList());
            }
            var output = network.Run(new double[] { 0, 0 }.ToList());
            MessageBox.Show(output[0].ToString());
            if (bm.Width + 100 < Screen.PrimaryScreen.Bounds.Width && bm.Height + 100 < Screen.PrimaryScreen.Bounds.Height)
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
                this.Width = bm.Width / 2 + 20;
                this.Height = bm.Height / 2 + 40;
                this.CenterToScreen();
                pictureBox1.Width = bm.Width / 2;
                pictureBox1.Height = bm.Height / 2;
                pictureBox1.Image = bm;
            }

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
    }
}
