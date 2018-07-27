using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISIW
{
    class ThongTin
    {
        private int taskIndex;

        public int TaskIndex
        {
            get { return taskIndex; }
            set { taskIndex = value; }
        }
        public ThongTin()
        {
            taskIndex = 0;
        }
        public ThongTin(int TaskIndex)
        {
            taskIndex = TaskIndex;
        }
    }
}
