using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoIt;
using System.IO;
using System.Runtime.InteropServices;
namespace AISIW
{
    
    public partial class Form1 : Form
    {
        Readini r = new Readini(@"AISIW\load config.ini");
        static bool stop = false;
        static Thread currentThreadAuto = null;
        static Thread currentThreadUpdateGUI = null;
        Process process = null;
        //static string thongbao = "";
        ImageList imageList1 = new ImageList();
        int imageIndex = 0;
        ThongTin thongtin = new ThongTin();
        List<string> imageListText = new List<string>();
        private List<Task> listTask = new List<Task>();
        private List<Image_to_Search> listImage_S = new List<Image_to_Search>();
        public Form1()
        {
            InitializeComponent();
            //Boolean success = Form1.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x0000, 0x42);//Set hotkey as 'b'
            Boolean success = RegisterHotKey(this.Handle, 123, Constants.CTRL + Constants.ALT, (int)Keys.Z);
            if (success == true)
            {
            }
            else
            {
                MessageBox.Show("Lỗi phím tắt chụp ảnh !");
            }
        }
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        //[DllImport("user32.dll")]
        //public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                ShowChupAnh();
                //MessageBox.Show("Chụp Ảnh Mới");//You can replace this statement with your desired response to the Hotkey.
            }
            base.WndProc(ref m);
        }
        public void Form1_Load(object sender, EventArgs e)
        {
            //load
            if (File.Exists(@"AISIW\load config.ini"))
            {
                for (int i = 0; i < Convert.ToInt32(r.ReadValue("AISIW ", "List Count:")); i++)
                {
                    Task t = new Task();
                    t.ImageName = r.ReadValue("Task " + i + "", "ImageN:");
                    t.ImageLocal = r.ReadValue("Task " + i + "", "ImageL:");
                    t.Clicks = Convert.ToInt32(r.ReadValue("Task " + i + "", "Clicks:"));
                    t.DelayClick = Convert.ToInt32(r.ReadValue("Task " + i + "", "Delay:"));
                    t.DelaySearch = Convert.ToInt32(r.ReadValue("Task " + i + "", "Delay Search:"));
                    t.TypeClick = r.ReadValue("Task " + i + "", "Type Click:");
                    t.Key = r.ReadValue("Task " + i + "", "Key:");
                    t.Text = r.ReadValue("Task " + i + "", "Text:");
                    t.SendTimes = Convert.ToInt32(r.ReadValue("Task " + i + "", "Send Times:"));
                    t.DelaySend = Convert.ToInt32(r.ReadValue("Task " + i + "", "Delay Send:"));
                    t.IsAutoClick = Convert.ToBoolean(r.ReadValue("Task " + i + "", "IsAutoClick:"));
                    t.IsSendKey = Convert.ToBoolean(r.ReadValue("Task " + i + "", "IsSendKey:"));
                    t.IsSendText = Convert.ToBoolean(r.ReadValue("Task " + i + "", "IsSendText:"));
                    listTask.Add(t);
                }
                LoadTask_toListbox();
            }

            //#load
            ToolTip toolDelaySearch = new ToolTip();
            ToolTip toolDelay = new ToolTip();
            ToolTip toolText = new ToolTip();
            ToolTip toolKey = new ToolTip();
            ToolTip toolToiUu = new ToolTip();
            ToolTip toolBaoDong = new ToolTip();
            ToolTip toolClickXY = new ToolTip();
            ToolTip toolRepeat = new ToolTip();
            ToolTip toolRepeat2 = new ToolTip();
            ToolTip toolpickwin = new ToolTip();
            ToolTip toolpickimage = new ToolTip();
            ToolTip toolcapture = new ToolTip();
            toolpickwin.SetToolTip(btnpickw, "Chọn cửa sổ muốn auto.");
            toolpickimage.SetToolTip(btnAddImage, "Chọn ảnh muốn tìm kiếm.");
            toolcapture.SetToolTip(btncapture, "Chụp ảnh nhanh.");
            toolRepeat.SetToolTip(lblrepeat, "Lặp lại các tác vụ trong danh sách tác vụ nhiều lần.");
            toolRepeat2.SetToolTip(pb, "Lặp lại các tác vụ trong danh sách tác vụ nhiều lần.");
            toolClickXY.SetToolTip(cbclickToaDo, "Click theo tọa độ biết trước, nếu không chọn thì mặc định tìm tọa độ bằng hình ảnh.");
            toolBaoDong.SetToolTip(cbBaoDong, "Báo động khi tìm ảnh quá lâu");
            toolToiUu.SetToolTip(cbToiUu, "Cho phép tái sử dụng tọa độ ảnh cho lần lặp(repeat) sau");
            toolDelaySearch.SetToolTip(label7, "Khoảng thời gian giãn cách giữa 2 lần tìm ảnh.");
            toolDelay.SetToolTip(label4, "Khoảng thời gian giãn cách giữa 2 lần thực hiện click.");
            toolText.SetToolTip(radioText, "Gửi một đoạn văn bản đến cửa sổ đã chọn.");
            toolKey.SetToolTip(radioKey, "Gửi một phím đến cửa sổ đã chọn.");
            cbtypeclick.SelectedIndex = 0;
            cbkey.SelectedIndex = 0;
        }

        private void btnpickw_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2(this);
            frm2.ShowDialog();
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.BMP;*.JPG;*.PNG";
            openfile.FilterIndex = 1;
            openfile.Multiselect = true;
            DialogResult result = openfile.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] sarr = openfile.FileNames;
                foreach (string s in sarr)
                {
                    Image image = Image.FromFile(s);
                    imageList1.Images.Add(image);
                    listView1.SmallImageList = imageList1;
                    ListViewItem listItem = new ListViewItem();
                    listItem.ImageIndex = imageIndex;
                    listItem.Text = " " + s.Substring(s.LastIndexOf("\\") + 1);
                    imageListText.Add(listItem.Text);
                    listView1.Items.Add(listItem);
                    //set thuoc tinh image va add vao list
                    Image_to_Search imgS = new Image_to_Search();
                    imgS.ImageName = s.Substring(s.LastIndexOf("\\") + 1);
                    imgS.ImageLocal = s;
                    listImage_S.Add(imgS);
                    imageIndex++;
                }
            }
        }
        public string LabelTextWtitle
        {
            get { return lblwtitle.Text; }
            set { lblwtitle.Text = value; }
        }
        bool checkChupAnh = false;
        public bool SetCheckChupAnh
        {
            get { return checkChupAnh; }
            set { checkChupAnh = value; }
        }
        public Process getprocess
        {
            get { return process; }
            set { process = value; }
        }
        private void btnstart_Click(object sender, EventArgs e)
        {
            if (listTask.Count > 0)
            {
                listTask[0].Repeat = Convert.ToInt32(numericRepeat.Value);
                if (process != null)
                {
                    stop = false;
                    control_button("start");
                    listTask[0].Frm = this;
                    Thread threadauto = new Thread(Auto);
                    currentThreadAuto = threadauto;
                    listTask[0].Threadtask = threadauto;
                    threadauto.Start(listTask);
                    currentThreadUpdateGUI = new Thread(UpdateGUI);
                    currentThreadUpdateGUI.Start();
                }
                else
                {
                    MessageBox.Show("Bạn Chưa Chọn Cửa Sổ !!!");
                }
            }
            else
            {
                MessageBox.Show("Chưa có tác vụ !!!");
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btncapture_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nhấn tổ hợp phím Ctrl+Alt+Z để chụp\nClick giữ và kéo chuột để chọn vùng muốn cắt.");
        }

        private void btnaddtask_click_Click(object sender, EventArgs e)
        {
            if (cbclickToaDo.Checked)
            {
                listTask.Add(Load_inputTask("IsClickXY", 0));
            }
            else
            {
                //neu da chon anh thi add tac vu vao list auto
                if (listView1.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        listTask.Add(Load_inputTask("IsAutoClick", i));
                    }
                }
                else
                {
                    MessageBox.Show("Chọn ảnh trước !!!");
                }
            }

            //load task vao listbox
            LoadTask_toListbox();
        }
        public void LoadTask_toListbox()
        {
            listboxTask.Items.Clear();
            for (int i = 0; i < listTask.Count; i++)
            {
                if (listTask[i].IsAutoClick)
                {
                    string s = (i + 1) + "=>" + listTask[i].ImageName + " | Clicks:" + listTask[i].Clicks + " | Type:" + listTask[i].TypeClick + " | Delay:" + listTask[i].DelayClick + "ms | Delay Search:" + listTask[i].DelaySearch + "ms";
                    listboxTask.Items.Add(s);
                }
                if (listTask[i].IsClickXY)
                {
                    string s = (i + 1) + "=>Click XY:X=" + listTask[i].X + ",Y=" + listTask[i].Y + " | Clicks:" + listTask[i].Clicks + " | Type:" + listTask[i].TypeClick + " | Delay:" + listTask[i].DelayClick + "ms | Delay Search:" + listTask[i].DelaySearch + "ms";
                    listboxTask.Items.Add(s);
                }
                if (listTask[i].IsSendKey)
                {
                    string s = (i + 1) + "=> Send Key: " + listTask[i].Key + " | Số Lần:" + listTask[i].SendTimes + " | Delay Send:" + listTask[i].DelaySend + "ms";
                    listboxTask.Items.Add(s);
                }
                if (listTask[i].IsSendText)
                {
                    string subtext = listTask[i].Text;
                    if (subtext.Length > 10) { subtext = subtext.Substring(0, 10) + "..."; }
                    string s = (i + 1) + "=> Send Text: " + subtext + " | Số Lần:" + listTask[i].SendTimes + " | Delay Send:" + listTask[i].DelaySend + "ms";
                    listboxTask.Items.Add(s);
                }

            }
        }
        private void btndeltask_click_Click(object sender, EventArgs e)
        {
            del_task();
        }
        private void del_task()
        {
            if (listboxTask.SelectedItems.Count > 0)
            {

                for (int i = listboxTask.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    listTask.RemoveAt(listboxTask.SelectedIndices[i]);
                    //listboxTask.Items.RemoveAt(listboxTask.SelectedIndices[i]);

                }
                LoadTask_toListbox();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn tác vụ cần xóa !");
            }
        }
        private void btninsertClick_Click(object sender, EventArgs e)
        {
            if (listboxTask.SelectedItems.Count > 0)
            {
                if (cbclickToaDo.Checked)
                {
                    listTask.Insert(listboxTask.SelectedIndex, Load_inputTask("IsClickXY", 0));
                    LoadTask_toListbox();
                }
                else
                {
                    int count = listView1.SelectedItems.Count;
                    if (count == 0) { MessageBox.Show("Bạn chưa chọn ảnh !"); }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            listTask.Insert(listboxTask.SelectedIndex + i, Load_inputTask("IsAutoClick", i));
                        }
                        LoadTask_toListbox();
                    }
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn vị trí chèn !");
            }
        }
        private Task Load_inputTask(string s, int i)
        {
            Task task = new Task();
            if (s == "IsAutoClick")
            {
                task.ImageName = listImage_S[listView1.SelectedIndices[i]].ImageName;
                task.ImageLocal = listImage_S[listView1.SelectedIndices[i]].ImageLocal;
                task.Clicks = Convert.ToInt32(numericClicks.Value);
                task.DelayClick = Convert.ToInt32(numericDelayClick.Value);
                task.DelaySearch = Convert.ToInt32(numericDelaySearch.Value);
                task.TypeClick = cbtypeclick.Text;
                task.IsAutoClick = true;
                task.IsSendKey = false;
                task.IsSendText = false;
                task.IsClickXY = false;
            }
            if (s == "IsClickXY")
            {
                task.X = Convert.ToInt32(numericX.Value);
                task.Y = Convert.ToInt32(numericY.Value);
                task.Clicks = Convert.ToInt32(numericClicks.Value);
                task.DelayClick = Convert.ToInt32(numericDelayClick.Value);
                task.DelaySearch = Convert.ToInt32(numericDelaySearch.Value);
                task.TypeClick = cbtypeclick.Text;
                task.IsAutoClick = false;
                task.IsSendKey = false;
                task.IsSendText = false;
                task.IsClickXY = true;
            }
            if (s == "IsSendKey")
            {
                task.DelaySend = Convert.ToInt32(numericDelaySend.Value);
                task.SendTimes = Convert.ToInt32(numericSend.Value);
                task.Key = cbkey.Text;
                task.IsAutoClick = false;
                task.IsSendKey = true;
                task.IsSendText = false;
                task.IsClickXY = false;
            }
            if (s == "IsSendText")
            {
                task.DelaySend = Convert.ToInt32(numericDelaySend.Value);
                task.SendTimes = Convert.ToInt32(numericSend.Value);
                task.Text = txtText.Text;
                task.IsAutoClick = false;
                task.IsSendKey = false;
                task.IsSendText = true;
                task.IsClickXY = false;
            }
            return task;
        }

        private void btnaddtaskSend_Click(object sender, EventArgs e)
        {
            if (radioKey.Checked)
            {
                listTask.Add(Load_inputTask("IsSendKey", 0));
            }
            if (radioText.Checked)
            {
                listTask.Add(Load_inputTask("IsSendText", 0));
            }
            LoadTask_toListbox();
        }

        private void btninsertSend_Click(object sender, EventArgs e)
        {
            if (listboxTask.SelectedItems.Count > 0)
            {
                if (radioKey.Checked)
                    listTask.Insert(listboxTask.SelectedIndex, Load_inputTask("IsSendKey", 0));
                if (radioText.Checked)
                    listTask.Insert(listboxTask.SelectedIndex, Load_inputTask("IsSendText", 0));
                LoadTask_toListbox();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn vị trí chèn !");
            }
        }
        public void Auto(object obj)
        {
            List<Task> listTauto = (List<Task>)obj;
            if (listTauto[0].Repeat == 0)
            {
                while (true)
                {
                    for (int loop = 0; loop < listTauto.Count; loop++)
                    {
                        //auto theo listTauto[loop]
                        thongtin.TaskIndex = loop;
                        if (listTauto[loop].IsAutoClick)
                        {
                            Point p = new Point(-1, -1);
                            while (p.X == -1)
                            {
                                //trong khi chua tim dc => tim tiep
                                if (stop == true) { currentThreadAuto.Abort(); }
                                p = Image_searchEX(listTauto[loop].ImageLocal, Utilities.CaptureWindow(process.MainWindowHandle, currentThreadAuto), 85);
                                //MessageBox.Show(listTauto[loop].ImageName+":"+ p.ToString());
                            }
                            for (int m = 0; m < listTauto[loop].Clicks; m++)
                            {
                                AutoItX.ControlClick(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].TypeClick, 1, p.X, p.Y);
                                Thread.Sleep(listTauto[loop].DelayClick);
                            }
                            Thread.Sleep(listTauto[loop].DelaySearch);
                        }
                        if (listTauto[loop].IsClickXY)
                        {
                            for (int m = 0; m < listTauto[loop].Clicks; m++)
                            {
                                AutoItX.ControlClick(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].TypeClick, 1, listTauto[loop].X, listTauto[loop].Y);
                                Thread.Sleep(listTauto[loop].DelayClick);
                            }
                            Thread.Sleep(listTauto[loop].DelaySearch);
                        }
                        if (listTauto[loop].IsSendText)
                        {
                            for (int m = 0; m < listTauto[loop].SendTimes; m++)
                            {
                                if (stop == true) { currentThreadAuto.Abort(); }
                                AutoItX.ControlSend(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].Text);
                                Thread.Sleep(listTauto[loop].DelaySend);
                            }

                        }
                        if (listTauto[loop].IsSendKey)
                        {
                            for (int m = 0; m < listTauto[loop].SendTimes; m++)
                            {
                                if (stop == true) { currentThreadAuto.Abort(); }
                                AutoItX.ControlSend(process.MainWindowHandle, process.MainWindowHandle, "{" + listTauto[loop].Key + "}");
                                Thread.Sleep(listTauto[loop].DelaySend);
                            }
                        }
                    }
                }
            }
            else
            {
                for (int rp = 0; rp < listTauto[0].Repeat; rp++)
                {
                    for (int loop = 0; loop < listTauto.Count; loop++)
                    {
                        //auto theo listTauto[loop]
                        thongtin.TaskIndex = loop;
                        if (listTauto[loop].IsAutoClick)
                        {
                            Point p = new Point(-1, -1);
                            while (p.X == -1)
                            {
                                //trong khi chua tim dc => tim tiep
                                if (stop == true) { currentThreadAuto.Abort(); }
                                p = Image_searchEX(listTauto[loop].ImageLocal, Utilities.CaptureWindow(process.MainWindowHandle, currentThreadAuto), 85);
                                //MessageBox.Show(listTauto[loop].ImageName+":"+ p.ToString());
                            }
                            for (int m = 0; m < listTauto[loop].Clicks; m++)
                            {
                                AutoItX.ControlClick(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].TypeClick, 1, p.X, p.Y);
                                Thread.Sleep(listTauto[loop].DelayClick);
                            }
                            Thread.Sleep(listTauto[loop].DelaySearch);
                        }
                        if (listTauto[loop].IsClickXY)
                        {
                            for (int m = 0; m < listTauto[loop].Clicks; m++)
                            {
                                AutoItX.ControlClick(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].TypeClick, 1, listTauto[loop].X, listTauto[loop].Y);
                                Thread.Sleep(listTauto[loop].DelayClick);
                            }
                            Thread.Sleep(listTauto[loop].DelaySearch);
                        }
                        if (listTauto[loop].IsSendText)
                        {
                            for (int m = 0; m < listTauto[loop].SendTimes; m++)
                            {
                                if (stop == true) { currentThreadAuto.Abort(); }
                                AutoItX.ControlSend(process.MainWindowHandle, process.MainWindowHandle, listTauto[loop].Text);
                                Thread.Sleep(listTauto[loop].DelaySend);
                            }

                        }
                        if (listTauto[loop].IsSendKey)
                        {
                            for (int m = 0; m < listTauto[loop].SendTimes; m++)
                            {
                                if (stop == true) { currentThreadAuto.Abort(); }
                                AutoItX.ControlSend(process.MainWindowHandle, process.MainWindowHandle, "{" + listTauto[loop].Key + "}");
                                Thread.Sleep(listTauto[loop].DelaySend);
                            }
                        }
                    }
                }
            }
            Action actionControlbtn = () => control_button("stop");
            this.Invoke(actionControlbtn);
            Action actionbtn = () => btnstop.Enabled = false;
            this.Invoke(actionbtn);
            Action actionbtn2 = () => btnstart.Enabled = true;
            this.Invoke(actionbtn2);
            Action actionthreadAbort = () => currentThreadUpdateGUI.Abort();
            this.Invoke(actionthreadAbort);
            Action actionlbl = () => lbltientrinh.Text = "Tất cả tác vụ đã hoàn thành !";
            this.Invoke(actionlbl);
        }
        public static Point PcountToPoint(int pcount, int width)
        {
            Point p = new Point();
            p.X = pcount % width;
            p.Y = pcount / width;
            return p;
        }
        public static IntPtr WinGetHandle(string wName)
        {
            IntPtr hWnd = IntPtr.Zero;

            foreach (Process pList in Process.GetProcesses())
                if (pList.MainWindowTitle.Contains(wName))
                    hWnd = pList.MainWindowHandle;

            return hWnd;
        }
        unsafe Point Image_searchEX(string bmlocalT, Bitmap bmM, double dochinhxac)
        {
            double dcx = dochinhxac / 100.0;
            Point Pimage = new Point();
            Bitmap bmT = new Bitmap(bmlocalT);
            Rectangle rec = new Rectangle(0, 0, bmT.Width, bmT.Height);
            BitmapData bmTData = bmT.LockBits(rec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideT = bmTData.Stride;
            int nOffsetT = strideT - bmT.Width * 3;
            byte* pT = (byte*)bmTData.Scan0;
            byte* pTcopy = pT;
            Rectangle recM = new Rectangle(0, 0, bmM.Width, bmM.Height);
            BitmapData bmMData = bmM.LockBits(recM, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int strideM = bmMData.Stride;
            int nOffsetM = strideM - bmM.Width * 3;
            byte* pM = (byte*)bmMData.Scan0;
            int x, y, PcountM = 0;
            int yscan = bmM.Height - bmT.Height;
            int xscan = bmM.Width - bmT.Width;
            int sobyte1hangT = bmT.Width * 3;
            int sobyte1hangM_andOff = bmM.Width * 3 + nOffsetM;
            //=>them
            int PcountT = 0;
            bool br = false;
            Point pxDcChon = new Point(-1, -1);
            Point pxDcChon2 = new Point(-1, -1);
            Point pxDcChon3 = new Point(-1, -1);
            for (int iT = 0; iT < bmT.Height - 1; iT++)
            {

                for (int jT = 0; jT < bmT.Width - 1; jT++)
                {
                    PcountT = iT * bmT.Width + jT;
                    if (PcountT < bmT.Width * bmT.Height - 1)
                    {
                        if ((pTcopy + PcountT * 3)[0] != (pTcopy + (PcountT + 1) * 3)[0] && (pTcopy + PcountT * 3)[1] != (pTcopy + (PcountT + 1) * 3)[1] && (pTcopy + PcountT * 3)[2] != (pTcopy + (PcountT + 1) * 3)[2])
                        {
                            pxDcChon2 = PcountToPoint(PcountT, bmT.Width);
                            if (ssPX_khac(pTcopy + PcountT * 3, pTcopy + (PcountT + 2) * 3) && ssPX_khac(pTcopy + (PcountT + 1) * 3, pTcopy + (PcountT + 2) * 3))
                            {
                                pxDcChon3 = PcountToPoint(PcountT, bmT.Width);
                                if (pxDcChon3.X + 10 <= bmT.Width)
                                {
                                    br = true;
                                    break;
                                }
                                pxDcChon3 = new Point(-1, -1);
                            }
                        }
                    }

                }
                if (br) { break; }
                pTcopy += nOffsetT;
            }
            if (pxDcChon2.X == -1)
                pxDcChon = new Point(0, 0);
            else
            {
                pxDcChon = pxDcChon2;
                if (pxDcChon3.X != -1)
                {
                    pxDcChon = pxDcChon3;
                }
            }
            byte* p_10px = (byte*)bmTData.Scan0 + pxDcChon.Y * strideT + (pxDcChon.X) * 3;
            for (y = 0; y < bmM.Height; y++)
            {
                for (x = 0; x < bmM.Width; x++)
                {
                    pT = (byte*)bmTData.Scan0;
                    int hei2 = bmT.Height / 2;
                    int wid2 = bmT.Width / 2;
                    PcountM = (y * bmM.Width + x);
                    int _chinhxac = 0;
                    for (int k = 0; k < 10; k++)
                    {
                        if ((p_10px + k * 3)[0] == (pM + k * 3)[0] && (p_10px + k * 3)[1] == (pM + k * 3)[1] && (p_10px + k * 3)[2] == (pM + k * 3)[2])
                        {
                            _chinhxac++;
                        }
                    }
                    if (_chinhxac / 10.0 >= 0.8)
                    {
                        int _chinhxac2 = 0;
                        int PcountT1 = 0;
                        for (int iy = 0; iy < bmT.Height; iy++)
                        {

                            for (int jx = 0; jx < bmT.Width; jx++)
                            {
                                PcountT1 = iy * bmT.Width + jx;
                                if ((pT + PcountT1 * 3)[0] == (pM + ((iy - pxDcChon.Y) * strideM) + (jx - pxDcChon.X) * 3)[0])
                                {
                                    _chinhxac2++;
                                }
                            }
                            pT += nOffsetT;
                        }
                        if (_chinhxac2 / (bmT.Width * bmT.Height * 1.0) >= dcx)
                        {
                            //MessageBox.Show("Chinh xac :" + (_chinhxac2 / (bmT.Width * bmT.Height * 1.0) * 100).ToString() + " %");
                            bmM.UnlockBits(bmMData);
                            bmT.UnlockBits(bmTData);
                            PcountM = PcountM + (hei2 - pxDcChon.Y) * bmM.Width + (wid2 - pxDcChon.X);
                            Pimage = PcountToPoint(PcountM, bmM.Width);
                            bmT.Dispose();
                            bmM.Dispose();
                            return Pimage;
                        }
                    }
                    pM += 3;
                }
                pM += nOffsetM;
            }
            //=>them
            //them<=  
            Pimage.X = -1;
            Pimage.Y = -1;
            bmM.UnlockBits(bmMData);
            bmT.UnlockBits(bmTData);
            bmT.Dispose();
            bmM.Dispose();
            return Pimage;
        }
        unsafe bool ssPX_khac(byte* p1, byte* p2)
        {
            if (p1[0] != p2[0] && p1[1] != p2[1] && p1[2] != p2[2])
                return true;
            return false;
        }
        private void btnstop_Click(object sender, EventArgs e)
        {
            stop = true;
            control_button("stop");
            currentThreadAuto.Abort();
            currentThreadUpdateGUI.Abort();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (currentThreadAuto != null && currentThreadUpdateGUI != null)
            {
                currentThreadAuto.Abort();
                currentThreadUpdateGUI.Abort();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists("AISIW"))
                Directory.CreateDirectory("AISIW");
            if (listTask.Count > 0)
            {
                try
                {
                    File.Delete(@"AISIW\load config.ini");
                    for (int i = 0; i < listTask.Count; i++)
                    {
                        r.WriteValue("Task " + i + "", "ImageN:", listTask[i].ImageName);
                        r.WriteValue("Task " + i + "", "ImageL:", listTask[i].ImageLocal);
                        r.WriteValue("Task " + i + "", "Clicks:", listTask[i].Clicks.ToString());
                        r.WriteValue("Task " + i + "", "Delay:", listTask[i].DelayClick.ToString());
                        r.WriteValue("Task " + i + "", "Delay Search:", listTask[i].DelaySearch.ToString());
                        r.WriteValue("Task " + i + "", "Type Click:", listTask[i].TypeClick);
                        r.WriteValue("Task " + i + "", "Key:", listTask[i].Key);
                        r.WriteValue("Task " + i + "", "Text:", listTask[i].Text);
                        r.WriteValue("Task " + i + "", "Send Times:", listTask[i].SendTimes.ToString());
                        r.WriteValue("Task " + i + "", "Delay Send:", listTask[i].DelaySend.ToString());
                        r.WriteValue("Task " + i + "", "IsAutoClick:", listTask[i].IsAutoClick.ToString());
                        r.WriteValue("Task " + i + "", "IsSendKey:", listTask[i].IsSendKey.ToString());
                        r.WriteValue("Task " + i + "", "IsSendText:", listTask[i].IsSendText.ToString());
                    }
                    r.WriteValue("AISIW ", "List Count:", listTask.Count.ToString());
                    r.WriteValue("AISIW ", "Repeat:", Convert.ToInt32(numericRepeat.Value).ToString());
                    MessageBox.Show("Lưu thành công !", "Thông báo !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("Lỗi không thể lưu file load config.ini\nCó thể file đó đang được sử dụng bởi một chương trình khác !");
                }
                
            }
        }

        private void btndelImage_Click(object sender, EventArgs e)
        {
            del_image();
        }
        private void del_image()
        {
            for (int i = listView1.SelectedIndices.Count - 1; i >= 0; i--)
            {
                imageList1.Images.RemoveAt(listView1.SelectedIndices[i]);
                imageListText.RemoveAt(listView1.SelectedIndices[i]);
                listImage_S.RemoveAt(listView1.SelectedIndices[i]);
                //giam image index
                imageIndex--;
            }
            load_ImageToListV();
        }
        private void load_ImageToListV()
        {
            listView1.Clear();
            for (int i = 0; i < imageIndex; i++)
            {
                ListViewItem listItem = new ListViewItem();
                listItem.ImageIndex = i;
                listItem.Text = imageListText[i];
                listView1.Items.Add(listItem);
            }
        }
        private void UpdateGUI()
        {
            while (true)
            {

                Action action = () => listboxTask.ClearSelected();
                //listboxTask.SelectedIndex = thongtin.TaskIndex;
                //listboxTask.
                this.Invoke(action);
                Action action1 = () => listboxTask.SetSelected(thongtin.TaskIndex, true);
                this.Invoke(action1);
                if (listTask[thongtin.TaskIndex].IsAutoClick)
                {
                    Action action2 = () => lbltientrinh.Text = "Đang tìm ảnh: " + listTask[thongtin.TaskIndex].ImageName;
                    this.Invoke(action2);
                }
                if (listTask[thongtin.TaskIndex].IsSendKey)
                {
                    Action action2 = () => lbltientrinh.Text = "Đang send key: " + listTask[thongtin.TaskIndex].Key;
                    this.Invoke(action2);
                }
                if (listTask[thongtin.TaskIndex].IsSendText)
                {
                    Action action2 = () => lbltientrinh.Text = "Đang send text: " + listTask[thongtin.TaskIndex].Text;
                    this.Invoke(action2);
                }
                Thread.Sleep(200);
            }

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        public void control_button(string s)
        {
            if (s == "start")
            {
                btnAddImage.Enabled = false;
                btnpickw.Enabled = false;
                btnaddtask_click.Enabled = false;
                btnaddtaskSend.Enabled = false;
                btndelImage.Enabled = false;
                btndeltask_click.Enabled = false;
                btninsertClick.Enabled = false;
                btninsertSend.Enabled = false;
                btnstart.Enabled = false;
                btnstop.Enabled = true;
            }
            if (s == "stop")
            {
                btnAddImage.Enabled = true;
                btnpickw.Enabled = true;
                btnaddtask_click.Enabled = true;
                btnaddtaskSend.Enabled = true;
                btndelImage.Enabled = true;
                btndeltask_click.Enabled = true;
                btninsertClick.Enabled = true;
                btninsertSend.Enabled = true;
                btnstart.Enabled = true;
                btnstop.Enabled = false;
                lbltientrinh.Text = "Stop !!!";
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng chưa hoàn thành!");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = "notepad.exe";
            notePad.StartInfo.Arguments = @"AISIW\help.txt";
            notePad.Start();
        }
        private void ShowChupAnh()
        {
            if (!checkChupAnh)
            {
                Form frm = new ChupAnh(this);
                frm.Width = Screen.PrimaryScreen.Bounds.Width;
                frm.Height = Screen.PrimaryScreen.Bounds.Height;
                frm.BackColor = Color.Black;
                frm.Opacity = .7;
                frm.StartPosition = FormStartPosition.CenterScreen;
                frm.Show();
                frm.Activate();
                frm.Focus();
            }
            
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                del_image();
            }
        }

        private void listboxTask_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                del_task();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng chưa hoàn thành!");
        }

        private void btntest_Click(object sender, EventArgs e)
        {
            if(process!=null)
            {
                Bitmap bm = Utilities.CaptureWindow(process.MainWindowHandle);
                Form frm = new ShowImage(bm);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Bạn chưa chọn cửa sổ để test!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng chưa hoàn thành !");
        }
    }
    public static class Constants
    {
        public const int NOMOD = 0x0000;
        public const int ALT = 0x0001;
        public const int CTRL = 0x0002;
        public const int SHIFT = 0x0004;
        public const int WIN = 0x0008;

        public const int WM_HOTKEY_MSG_ID = 0x0312;
    }
}
