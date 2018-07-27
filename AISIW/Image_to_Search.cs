using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AISIW
{
    class Image_to_Search
    {
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
        public Image_to_Search()
        {
            imageLocal = "";
            imageName = "";
        }
        public Image_to_Search(string ImageName,string ImageLocal)
        {
            imageLocal = ImageName;
            imageName = ImageLocal;
        }

    }
}
