using Newtonsoft.Json;
using SteamAuth;
using SteamKit2;
using SteamKit2.Authentication;
using SteamKit2.Internal;
using System;
using System.IO;
using System.Threading.Tasks;
using SessionData = SteamAuth.SessionData;

namespace JCorePanelBase
{
    public class JCSteamAccount
    {
        public string Login;
        public string Password;
        public SessionData Session;
        public SteamGuardAccount MaFile;

        public void UpdateSteamGuardAccountData(SteamGuardAccount newSteamGuardAccount)
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"))) return;

            string[] files = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts"));
            foreach (string file in files)
            {
                string extension = Path.GetExtension(file);
                if (extension == ".jcfile")
                {
                    string json = File.ReadAllText(file);

                    try
                    {
                        JCSteamAccount steamAccount = JsonConvert.DeserializeObject<JCSteamAccount>(json);

                        if (steamAccount.Login.ToLower() == newSteamGuardAccount.AccountName.ToLower())
                        {
                            steamAccount.MaFile = newSteamGuardAccount;
                            steamAccount.Session = newSteamGuardAccount.Session;
                            File.WriteAllText(file, JsonConvert.SerializeObject(steamAccount).ToString());
                        }
                    }
                    catch (JsonException)
                    {

                    }
                }
            }
        }

        public async Task CheckSession()
        {
            if (Session != null)
            {
                string Inventory;

                try
                {
                    Inventory = await SteamWeb.GETRequest("https://steamcommunity.com/my/inventory/json/730/2", Session.GetCookies());
                }
                catch (System.Net.WebException ex)
                {
                    return;
                }

                if (Utils.IsValidJson(Inventory)) return;
            }
            SteamClient steamClient = new SteamClient();
            steamClient.Connect();
            while (!steamClient.IsConnected)
                await Task.Delay(500);
            CredentialsAuthSession authSession;
            var SteamGuard = MaFile;
            authSession = await steamClient.Authentication.BeginAuthSessionViaCredentialsAsync(new AuthSessionDetails
            {
                Username = Login,
                Password = Password,
                IsPersistentSession = false,
                PlatformType = EAuthTokenPlatformType.k_EAuthTokenPlatformType_MobileApp,
                ClientOSType = EOSType.Android9,
                Authenticator = new UserFormAuthenticator(SteamGuard),
            });

            AuthPollResult pollResponse;
            try
            {
                pollResponse = await authSession.PollingWaitForResultAsync();
            }
            catch (Exception ex)
            {

                return;
            }

            SessionData sessionData = new SessionData()
            {
                SteamID = authSession.SteamID.ConvertToUInt64(),
                AccessToken = pollResponse.AccessToken,
                RefreshToken = pollResponse.RefreshToken,
            };
            MaFile.Session = sessionData;
            Session = sessionData;
            UpdateSteamGuardAccountData(MaFile);

        }
    }
}
