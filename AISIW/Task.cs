using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AISIW
{
    class Task
    {
        private int x;

        public int X
        {
            get { return x; }
            set { x = value; }
        }
        private int y;

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        private Form1 frm = null;

        public Form1 Frm
        {
            get { return frm; }
            set { frm = value; }
        }
        private Thread threadtask = null;

        public Thread Threadtask
        {
            get { return threadtask; }
            set { threadtask = value; }
        }
        private int repeat;
        public int Repeat
        {
            get { return repeat; }
            set { repeat = value; }
        }
        private string imageName;

        public string ImageName
        {
            get { return imageName; }
            set { imageName = value; }
        }
        private string imageLocal;

        public string ImageLocal
        {
            get { return imageLocal; }
            set { imageLocal = value; }
        }
        private int clicks;

        public int Clicks
        {
            get { return clicks; }
            set { clicks = value; }
        }
        private int delayClick;

        public int DelayClick
        {
            get { return delayClick; }
            set { delayClick = value; }
        }
        private int delaySearch;

        public int DelaySearch
        {
            get { return delaySearch; }
            set { delaySearch = value; }
        }
        private string typeClick;

        public string TypeClick
        {
            get { return typeClick; }
            set { typeClick = value; }
        }
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private int sendTimes;

        public int SendTimes
        {
            get { return sendTimes; }
            set { sendTimes = value; }
        }
        private int delaySend;

        public int DelaySend
        {
            get { return delaySend; }
            set { delaySend = value; }
        }
        private bool isAutoClick;

        public bool IsAutoClick
        {
            get { return isAutoClick; }
            set { isAutoClick = value; }
        }
        private bool isSendText;

        public bool IsSendText
        {
            get { return isSendText; }
            set { isSendText = value; }
        }
        private bool isSendKey;

        public bool IsSendKey
        {
            get { return isSendKey; }
            set { isSendKey = value; }
        }
        private bool isClickXY;

        public bool IsClickXY
        {
            get { return isClickXY; }
            set { isClickXY = value; }
        }
        public Task()
        {
            x = -1;
            y = -1;
            repeat = 0;
            imageName = "";
            imageLocal = "";
            clicks = 0;
            delayClick = 0;
            delaySearch = 0;
            typeClick = "Double Left";
            key = "";
            text = "";
            sendTimes = 0;
            delaySend = 0;
            isAutoClick = true;
            isSendText = false;
            isSendKey = false;
            IsClickXY = false;
        }
        public Task(int X,int Y,int Repeat,string iName, string iLocal, int Clicks, int DelayClick, int DelaySearch, string TypeClick, string Key, string Text, int SendTimes, int DelaySend, bool IsAutoClick, bool IsSendKey, bool IsSendText,bool IsClickXY)
        {
            x = X;
            y = Y;
            repeat = Repeat;
            imageName = iName;
            imageLocal = iLocal;
            clicks = Clicks;
            delayClick = DelayClick;
            delaySearch = DelaySearch;
            typeClick = TypeClick;
            key = Key;
            text = Text;
            sendTimes = SendTimes;
            delaySend = DelaySend;
            isAutoClick = IsAutoClick;
            isClickXY = IsClickXY;
        }
        public string toStringT(){
            string t = "";
            t = "X:"+x+"\nY:"+y+"\nImage Name:" + imageName + "\nImage Local:" + imageLocal + "\nClick:" + clicks + "\nDelayClick:" + delayClick + "\nDelaySearch:" + delaySearch + "\nTypeClick:" + typeClick + "\nKey:" + key + "\nText:" + text + "\nSendTimes:" + sendTimes + "\nDelaySend:" + delaySend + "\nIsAutoClick:" + isAutoClick + "\nIsSendKey:" + isSendKey + "\nIsSendText:" + isSendText+"\nRepeat:"+repeat;
            return t;
        }
    }
}
