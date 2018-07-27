using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AISIW
{
    class Utilities
    {
        public const int SRCCOPY = 13369376;
        //public static Image CaptureScreen()
        //{
        //    return CaptureWindow(User32.GetDesktopWindow());
        //}

        public static Bitmap CaptureWindow(IntPtr handle, Thread thread)
        {

            Bitmap bm = null;
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Image image = null;
            try
            {
                image = Image.FromHbitmap(hBitmap);
                Gdi32.DeleteObject(hBitmap);
                bm = new Bitmap(image);
            }
            catch
            {
                MessageBox.Show("Lỗi do cửa sổ tìm kiếm bị đóng ! \nVui lòng không tắt cửa sổ tìm kiếm trong quá trình auto !\nMời chọn lại cửa sổ !");
                thread.Abort();
            }
            return bm;
        }
        public static Bitmap CaptureWindow(IntPtr handle)
        {

            Bitmap bm = null;
            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);
            Image image = null;
            image = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);
            bm = new Bitmap(image);
            return bm;
        }
    }
}
