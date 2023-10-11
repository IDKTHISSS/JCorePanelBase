
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace JCorePanelBase
{
    public static class Steam
    {
        public static async Task<JCSteamGuardResponse> RunSteamWithParams(JCSteamAccountInstance Account, string Params = null, string CustomSteamPath = null)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = CustomSteamPath == null ? GlobalMenager.GetSteamPath() : CustomSteamPath;
            processStartInfo.Arguments = $"-login {Account.AccountInfo.Login} {Account.AccountInfo.Password} " + (Params == null ? "" : Params);
            Process SteamProcess = Process.Start(processStartInfo);
            Thread.Sleep(5000);
            if (Account.AccountInfo.MaFile == null)
            {
                return new JCSteamGuardResponse(true, "MaFile is not loaded.", SteamProcess.Id);
            }
            Thread.Sleep(10000);
            Process[] processes = Utils.GetProcessesByParentPID(SteamProcess.Id, "steamwebhelper");
            while (!NeedInput(processes))
            {
                Thread.Sleep(500);
            }
            Thread.Sleep(500);
            Console.WriteLine("Found Login Window");
            SetWindows(processes);
            InputSimulator simulator = new InputSimulator();
            simulator.Keyboard.TextEntry(Account.AccountInfo.Login);
            simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
            simulator.Keyboard.TextEntry(Account.AccountInfo.Password);
            simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
            Console.WriteLine("Enter SteamGuard Window");
            Thread.Sleep(2500);
            while (IsVisible(processes))
            {
                SetWindows(processes);
                simulator.Keyboard.TextEntry(Account.AccountInfo.MaFile.GenerateSteamGuardCode());
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                Thread.Sleep(5000);
            }
            return new JCSteamGuardResponse(true, "Success", SteamProcess.Id);
        }



        private static bool NeedInput(Process[] processes)
        {
            foreach (var process in processes)
            {
                if (Utils.HasQRCode(ScreenCapturer.Capture(process.MainWindowHandle, 0, 0, 844, 527)))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsVisible(Process[] processes)
        {
            foreach (var process in processes)
            {
                if (Win32.IsWindowVisible(process.MainWindowHandle))
                {
                    return true;
                }
            }
            return false;
        }
        private static void SetWindows(Process[] processes)
        {
            foreach (var process in processes)
            {
                Win32.SetForegroundWindow(process.MainWindowHandle);
            }
        }
    }
}
