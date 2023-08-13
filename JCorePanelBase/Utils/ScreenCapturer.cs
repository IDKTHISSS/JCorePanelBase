using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public static class ScreenCapturer
    {
        public static Bitmap Capture(IntPtr hwnd, int startX, int startY, int endX, int endY)
        {
            // Получаем размеры окна
            RECT rect;
            Win32.GetWindowRect(hwnd, out rect);
            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            // Получаем контекст устройства для вывода на экран
            IntPtr hdcScreen = Win32.GetDC(IntPtr.Zero);
            IntPtr hdcBitmap = Win32.CreateCompatibleDC(hdcScreen);
            IntPtr hBitmap = Win32.CreateCompatibleBitmap(hdcScreen, width, height);

            // Связываем Bitmap с контекстом устройства для вывода на экран
            Win32.SelectObject(hdcBitmap, hBitmap);

            // Копируем изображение в контекст устройства для вывода на экран
            Win32.PrintWindow(hwnd, hdcBitmap, 0);

            // Создаем Bitmap для сохранения обрезанного изображения
            Bitmap bmp = Image.FromHbitmap(hBitmap);

            // Высвобождаем ресурсы
            Win32.DeleteObject(hBitmap);
            Win32.ReleaseDC(IntPtr.Zero, hdcScreen);
            Win32.DeleteDC(hdcBitmap);

            // Обрезаем изображение до нужных размеров
            int cropWidth = endX - startX;
            int cropHeight = endY - startY;
            Rectangle cropRect = new Rectangle(startX, startY, cropWidth, cropHeight);
            Bitmap croppedBmp = new Bitmap(cropWidth, cropHeight, bmp.PixelFormat);
            Graphics graphics = Graphics.FromImage(croppedBmp);
            graphics.DrawImage(bmp, new Rectangle(0, 0, cropWidth, cropHeight), cropRect, GraphicsUnit.Pixel);
            graphics.Dispose();

            // Возвращаем обрезанное изображение
            return croppedBmp;
        }
    }
}
