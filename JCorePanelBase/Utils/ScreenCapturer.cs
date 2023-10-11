using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

            // Создаем новое изображение с форматом пикселей, который поддерживает редактирование
            Bitmap croppedBmp = new Bitmap(cropWidth, cropHeight, PixelFormat.Format32bppRgb);

            // Создаем объект Graphics для нового изображения
            using (Graphics graphics = Graphics.FromImage(croppedBmp))
            {
                // Копируем обрезанную часть из оригинального изображения
                graphics.DrawImage(bmp, new Rectangle(0, 0, cropWidth, cropHeight), cropRect, GraphicsUnit.Pixel);
            }

            // Возвращаем обрезанное изображение
            return croppedBmp;
        }
    }
}
