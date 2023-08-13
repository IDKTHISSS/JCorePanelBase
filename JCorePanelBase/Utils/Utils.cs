using Newtonsoft.Json;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public static class Utils
    {
        public static CookieContainer GetCookies(SessionData session)
        {

            CookieContainer cookieContainer_0 = new CookieContainer();
            cookieContainer_0.Add(new Cookie("mobileClientVersion", "777777 3.1.0", "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("mobileClient", "android", "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("steamid", session.SteamID.ToString(), "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("steamLogin", session.SteamID.ToString() + "%7C%7C" + session.AccessToken, "/", ".steamcommunity.com")
            {
                HttpOnly = true
            });
            cookieContainer_0.Add(new Cookie("steamLoginSecure", session.SteamID.ToString() + "%7C%7C" + session.AccessToken, "/", ".steamcommunity.com")
            {
                HttpOnly = true,
                Secure = true
            });
            cookieContainer_0.Add(new Cookie("Steam_Language", "english", "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("dob", "", "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("sessionid", GetRandomHexNumber(32), "/", ".steamcommunity.com"));
            cookieContainer_0.Add(new Cookie("steamCurrencyId", "1", "/", ".steamcommunity.com"));
            return cookieContainer_0;
        }
        private static string GetRandomHexNumber(int digits)
        {
            Random random = new Random();
            byte[] array = new byte[digits / 2];
            random.NextBytes(array);
            string text = string.Concat(array.Select((byte x) => x.ToString("X2")).ToArray());
            if (digits % 2 == 0)
            {
                return text;
            }

            return text + random.Next(16).ToString("X");
        }
        public static Bitmap AddImages(Bitmap background, Bitmap foreground)
        {
            // создаем новый экземпляр Bitmap для результирующего изображения
            Bitmap result = new Bitmap(background.Width, background.Height);

            // создаем новый экземпляр Graphics и настраиваем параметры наложения
            using (Graphics g = Graphics.FromImage(result))
            {
                // наложение изображений
                g.DrawImage(background, new Rectangle(0, 0, background.Width, background.Height));
                g.DrawImage(foreground, new Rectangle(0, 0, foreground.Width, foreground.Height));
            }

            return result;
        }

        public static double CompareImages(Bitmap image1, Bitmap image2)
        {
            // Проверяем, что размеры изображений совпадают
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new ArgumentException("Размеры изображений не совпадают.");
            }

            double mse = 0;
            int width = image1.Width;
            int height = image1.Height;

            // Проходимся по каждому пикселю изображений и вычисляем MSE
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color1 = image1.GetPixel(x, y);
                    Color color2 = image2.GetPixel(x, y);
                    int rDiff = color1.R - color2.R;
                    int gDiff = color1.G - color2.G;
                    int bDiff = color1.B - color2.B;
                    mse += (rDiff * rDiff + gDiff * gDiff + bDiff * bDiff) / (double)(width * height);
                }
            }

            // Вычисляем PSNR
            double psnr = 10 * Math.Log10(255 * 255 / mse);

            // Возвращаем значение PSNR
            return psnr;
        }
        public static void SetTextToClipboard(string text)
        {
            const uint CF_UNICODETEXT = 13;
            // Открытие буфера обмена
            if (Win32.OpenClipboard(IntPtr.Zero))
            {
                // Очистка буфера обмена
                Win32.EmptyClipboard();

                // Получение размера буфера для текста
                int textLength = text.Length;
                int byteCount = (textLength + 1) * 2; // * 2 для UTF-16 кодировки

                // Выделение памяти и копирование текста в буфер обмена
                IntPtr hGlobal = Marshal.StringToHGlobalUni(text);
                IntPtr data = Win32.SetClipboardData(CF_UNICODETEXT, hGlobal);

                // Закрытие буфера обмена
                Win32.CloseClipboard();
            }
        }
        public static void SendTextToNonActiveWindow(IntPtr hwnd, string text)
        {
            const uint WM_KEYDOWN = 0x100;
            const uint WM_KEYUP = 0x101;
            const uint WM_CHAR = 0x102;
            Win32.SetForegroundWindow(hwnd);
            Thread.Sleep(50);
            foreach (char c in text)
            {
                if (c == '{')
                {
                    int end = text.IndexOf('}', text.IndexOf('{'));
                    if (end == -1) continue;
                    string innerText = text.Substring(text.IndexOf('{') + 1, end - text.IndexOf('{') - 1);
                    if (Enum.TryParse(innerText, out ConsoleKey key))
                    {
                        int keyCode = (int)key;
                        Win32.SendMessage(hwnd, WM_KEYDOWN, (IntPtr)keyCode, IntPtr.Zero);
                        Win32.SendMessage(hwnd, WM_KEYUP, (IntPtr)keyCode, IntPtr.Zero);
                    }
                }
                else
                {
                    // Вводим обычный символ
                    Win32.SendMessage(hwnd, WM_CHAR, (IntPtr)c, IntPtr.Zero);
                }
            }
        }

        public static bool IsValidJson(string jsonString)
        {
            try
            {
                JsonConvert.DeserializeObject(jsonString);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
