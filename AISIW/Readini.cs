using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AISIW
{
    class Readini
    {
        private string path = "";

        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        //dung lenh da co trong kernel32 cua windows
        [DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filepath);

        [DllImport("kernel32")]

        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filepath);

        public Readini(string thePath)
        { 
        this.path = thePath;
        }
        /// doc thong tin trong file ini 
        public string ReadValue(string section, string key)

        {
            StringBuilder tmp = new StringBuilder(255); 

            long i = GetPrivateProfileString(section, key, "", tmp, 255, this.path);

            return tmp.ToString();
        }
        /// Ghi thong tin vao file ini 
        public void WriteValue(string section, string key, string values)

        {
            WritePrivateProfileString(section, key, values, this.path);
        }


        }
}
