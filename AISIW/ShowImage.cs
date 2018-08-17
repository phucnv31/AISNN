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
using System.IO;
using AForge.Imaging.Filters;

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
        DirectoryInfo folderImage = new DirectoryInfo("Image");
        DirectoryInfo folderImageTmp = new DirectoryInfo("ImageTmp");
        FileInfo[] FI;
        Image ImageSelected;
        int imageSelectIndex = 0;
        Readini readIni = new Readini(@"ImageTmp\imageInfo.ini");
        public ShowImage(Bitmap bmf)
        {
            InitializeComponent();
            bm = bmf;
        }

        private void ShowImage_Load(object sender, EventArgs e)
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            FI = folderImage.GetFiles().Where(x => x.FullName.EndsWith(".bmp") || x.FullName.EndsWith(".png") || x.FullName.EndsWith(".jpg")).ToArray();
            var fileNames = FI.Select(x => x.Name);
            listViewImage.Columns.Add("Name");
            listViewImage.Columns[0].Width = listViewImage.Width;
            for (int i = 0; i < fileNames.Count(); i++)
            {
                listViewImage.Items.Add(fileNames.ToArray()[i]);
            }
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
            //Width = bm.Width;
            //Height = bm.Height;
            //BackgroundImage = bm;
            //pictureBox.Width = bm.Width;
            //pictureBox.Height = bm.Height;
        }
        public void DrawRectangle(Point leftTop, Point rightBottom)
        {
            if (isPaint)
            {
                var width = Math.Abs(rightBottom.X - leftTop.X);
                var height = Math.Abs(rightBottom.Y - leftTop.Y);
                if (pictureBox.Image != null)
                {
                    Bitmap bm = new Bitmap(ImageSelected, ImageSelected.Width, ImageSelected.Height);
                    using (Graphics gr = Graphics.FromImage(bm))
                    {
                        gr.DrawRectangle(Pens.Red, leftTop.X, leftTop.Y, width, height);
                    }
                    pictureBox.Image = bm;
                    this.Invalidate();
                }
            }
        }
        public void Draw()
        {
            if (ImageSelected == null)
                return;
            LeftTop = new Point(Math.Min(FirstPoint.X, FinalPoint.X), Math.Min(FirstPoint.Y, FinalPoint.Y));
            RightBottom = new Point(Math.Max(FirstPoint.X, FinalPoint.X), Math.Max(FirstPoint.Y, FinalPoint.Y));
            LeftTop = new Point(Math.Max(pictureBox.Location.X, LeftTop.X), Math.Max(pictureBox.Location.Y, LeftTop.Y));
            Point maxRightBottom = new Point(pictureBox.Location.X + ImageSelected.Width, pictureBox.Location.Y + ImageSelected.Height);
            RightBottom = new Point(Math.Min(maxRightBottom.X, RightBottom.X), Math.Min(maxRightBottom.Y, RightBottom.Y));
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

        private void buttonOk_Click(object sender, EventArgs e)
        {
            var P = ConvertLocationToP(LeftTop, RightBottom, new Point(0, 0), new Point(ImageSelected.Width, ImageSelected.Height));
            string value = string.Join("|", P);
            readIni.WriteValue("ImageInfo", FI[imageSelectIndex].Name, value);
            if (!File.Exists(folderImageTmp.FullName + @"\" + FI[imageSelectIndex].Name))
            {
                ImageSelected.Save(folderImageTmp.FullName + @"\" + FI[imageSelectIndex].Name);
            }
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            if (listViewImage.SelectedIndices.Count > 0)
            {
                DecreaseIndex();
                SelectIndexChange(imageSelectIndex);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (listViewImage.SelectedIndices.Count > 0)
            {
                IncreaseIndex();
                SelectIndexChange(imageSelectIndex);
            }
        }

        private void listViewImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewImage.SelectedIndices.Count > 0)
            {
                imageSelectIndex = listViewImage.SelectedIndices[0];
                SelectIndexChange(listViewImage.SelectedIndices[0]);
                labelIndex.Text = (imageSelectIndex + 1).ToString() + "/" + FI.Count();
            }
        }
        void SelectIndexChange(int selectIndex)
        {
            if (listViewImage.SelectedIndices.Count > 0)
            {
                var fileSelect = FI.ElementAtOrDefault(listViewImage.SelectedIndices[0]);
                if (fileSelect != null)
                {
                    ImageSelected = Image.FromFile(fileSelect.FullName);
                    if (ImageSelected.Width > pictureBox.Width || ImageSelected.Height > pictureBox.Height)
                    {
                        ResizeBilinear filter = new ResizeBilinear(pictureBox.Width, pictureBox.Height);
                        // apply the filter
                        Bitmap newImage = filter.Apply(new Bitmap(ImageSelected));
                        ImageSelected = newImage;
                    }
                    pictureBox.Image = ImageSelected;
                }
            }
        }
        void IncreaseIndex()
        {
            listViewImage.Items[imageSelectIndex].Selected = false;
            imageSelectIndex = imageSelectIndex + 1 < FI.Count() ? imageSelectIndex + 1 : 0;
            listViewImage.Items[imageSelectIndex].Selected = true;
            listViewImage.Select();
            labelIndex.Text = (imageSelectIndex + 1).ToString() + "/" + FI.Count();
        }
        void DecreaseIndex()
        {
            listViewImage.Items[imageSelectIndex].Selected = false;
            imageSelectIndex = imageSelectIndex > 0 ? imageSelectIndex - 1 : FI.Count() - 1;
            listViewImage.Items[imageSelectIndex].Selected = true;
            listViewImage.Select();
            labelIndex.Text = (imageSelectIndex + 1).ToString() + "/" + FI.Count();
        }
        double[] ConvertLocationToP(Point leftTop, Point rightBottom, Point imageLocation, Point imageRightBottom)
        {
            var width = rightBottom.X - leftTop.X;
            var height = rightBottom.Y - leftTop.Y;
            var widthImage = imageRightBottom.X - imageLocation.X;
            var heightImage = imageRightBottom.Y - imageLocation.Y;
            Point middlePoint = new Point(leftTop.X + (width / 2), leftTop.Y + (height / 2));
            double[] P = new double[5];
            P[0] = 1;
            P[1] = middlePoint.X * 1.0 / imageRightBottom.X;
            P[2] = middlePoint.Y * 1.0 / imageRightBottom.Y;
            P[3] = width * 1.0 / widthImage;
            P[4] = height * 1.0 / heightImage;
            return P;
        }

        private void buttonTrain_Click(object sender, EventArgs e)
        {
            FileInfo[] fis = folderImageTmp.GetFiles().Where(x => x.FullName.EndsWith(".bmp") || x.FullName.EndsWith(".jpg") || x.FullName.EndsWith(".png")).ToArray();
            TrainImage(fis[1]);
        }
        void TrainImage(FileInfo fi)
        {
            string value = readIni.ReadValue("ImageInfo", fi.Name);
            string[] P = value.Split('|');
            double[] output = new double[P.Count()];
            for (int i = 0; i < P.Count(); i++)
            {
                output[i] = double.Parse(P[i]);
            }
            Bitmap bitmap = new Bitmap(Image.FromFile(fi.FullName));
            ImageToArray conv = new ImageToArray(min: 0, max: 1);
            double[] input;
            conv.Convert(bitmap, out input);
            labelTrain.Text = "0/10";
            System.Threading.Tasks.Task.Run(() =>
            {
                NeuralNetwork network = new NeuralNetwork(0.00125, new int[] { bitmap.Width * bitmap.Height, 6, 5, 5 });
                for (int i = 0; i < 10; i++)
                {
                    network.Train(input.ToList(), output.ToList());
                    this.Invoke(new Action(
                        delegate ()
                        {
                            labelTrain.Text = (i+1).ToString() + "/" + 10;
                        }));
                }
            });
        }
    }
}
