using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteamAuth;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System.IO;
using static SteamKit2.DepotManifest;
using static SteamKit2.Internal.CCredentials_GetSteamGuardDetails_Response;
using SteamKit2.Authentication;
using SteamKit2.Internal;
using SteamKit2;
using SessionData = SteamAuth.SessionData;

namespace JCorePanelBase
{
    public class JCSteamAccount
    {
        public string Login;
        public string Password;
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

                        if(steamAccount.Login.ToLower() == newSteamGuardAccount.AccountName.ToLower()) {
                            steamAccount.MaFile = newSteamGuardAccount;
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
            if (MaFile == null) return;
            string Inventory;
            
            try
            {
                Inventory = await SteamWeb.GETRequest("https://steamcommunity.com/my/inventory/json/730/2", MaFile.Session.GetCookies());
            }
            catch(System.Net.WebException ex)
            {
                return;
            }

            if (Utils.IsValidJson(Inventory)) return;

            SteamClient steamClient = new SteamClient();
            steamClient.Connect();
            while (!steamClient.IsConnected)
                await Task.Delay(500);
            CredentialsAuthSession authSession;
            var SteamGuard = MaFile;
            try
            {
                authSession = await steamClient.Authentication.BeginAuthSessionViaCredentialsAsync(new AuthSessionDetails
                {
                    Username = Login,
                    Password = Password,
                    IsPersistentSession = false,
                    PlatformType = EAuthTokenPlatformType.k_EAuthTokenPlatformType_MobileApp,
                    ClientOSType = EOSType.Android9,
                    Authenticator = new UserFormAuthenticator(SteamGuard),
                });
            }
            catch (Exception ex)
            {
                return;
            }
            AuthPollResult pollResponse;
            try
            {
                pollResponse = await authSession.PollingWaitForResultAsync();
            }
            catch (Exception ex)
            {

                return;
            }

            // Build a SessionData object
            SessionData sessionData = new SessionData()
            {
                SteamID = authSession.SteamID.ConvertToUInt64(),
                AccessToken = pollResponse.AccessToken,
                RefreshToken = pollResponse.RefreshToken,
            };
            MaFile.Session = sessionData;
            UpdateSteamGuardAccountData(MaFile);
            
        }
    }
}
