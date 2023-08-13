using JCorePanelBase.Resources;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JCorePanelBase
{
    public static class Steam
    {
        public static async Task<JCSteamGuardResponse> RunSteamWithParams(JCSteamAccountInstance Account, string Params = null)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = GlobalMenager.GetSteamPath();
            processStartInfo.Arguments =  $"-login {Account.AccountInfo.Login} {Account.AccountInfo.Password} " + (Params == null ? "" : Params);
            Process SteamProcess = Process.Start(processStartInfo);
            Thread.Sleep(5000);
            double Res = 0;
            int trys = 0;
            if(Account.AccountInfo.MaFile == null)
            {
                return new JCSteamGuardResponse(true, "MaFile is not loaded.", SteamProcess.Id);
            }
            while (Res < 17)
            {
                if (trys >= 20) { return new JCSteamGuardResponse(false, "Cant input code after 20 trys.", SteamProcess.Id); }
                try
                {
                    Bitmap Screen = ScreenCapturer.Capture(SteamProcess.MainWindowHandle, 0, 0, 705, 430);
                    Screen = Utils.AddImages(Screen, SteamGuard.Screen);
                    Res = Utils.CompareImages(Screen, SteamGuard.EnterGuard);
                    if (Res > 11.4f)
                    {
                        Utils.SendTextToNonActiveWindow(SteamProcess.MainWindowHandle, await Account.AccountInfo.MaFile.GenerateSteamGuardCodeAsync());
                    }
                }
                catch
                { }
                trys++;
                Thread.Sleep(2000);
            }
            return new JCSteamGuardResponse(true, "Success", SteamProcess.Id);
        }
        public static uint GetWindowProcessId(IntPtr windowHandle)
        {
            uint processId;
            Win32.GetWindowThreadProcessId(windowHandle, out processId);
            return processId;
        }
    }
}
