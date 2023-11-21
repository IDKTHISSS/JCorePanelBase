using System;

namespace JCorePanelBase
{

    // Ваша статическая функция, которую вы хотите вызвать из DLL
    public static class GlobalMenager
    {
        public static Action<string> ShowDialog;
        public static Action<string, string, Action<string>> ShowInput;
        public static Action<string, Action<bool>> ShowConfirm;
        public static Func<string, string> GetProperty;
        public static Func<string> GetSteamPath;
    }
}
