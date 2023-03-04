using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static TownOfHost.Translator;

namespace TownOfHost
{
    [HarmonyPatch]
    public class ModUpdater
    {
        private static readonly string URL = "https://api.github.com/repos/tukasa0001/TownOfHost";
        public static bool hasUpdate = false;
        public static bool isChecked = false;
        public static Version latestVersion = null;
        public static string latestTitle = null;
        public static string downloadUrl = null;
        public static GenericPopup InfoPopup;

        public static bool CanUpdate = false; // 完善后启动

        [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPrefix]
        [HarmonyPriority(2)]
        public static void Start_Prefix(MainMenuManager __instance)
        {
            if (!CanUpdate) return;
            DeleteOldDLL();
            InfoPopup = UnityEngine.Object.Instantiate(Twitch.TwitchManager.Instance.TwitchPopup);
            InfoPopup.name = "InfoPopup";
            InfoPopup.TextAreaTMP.GetComponent<RectTransform>().sizeDelta = new(2.5f, 2f);
            if (!isChecked) CheckRelease(Main.BetaBuildURL.Value != "").GetAwaiter().GetResult();
            MainMenuManagerPatch.updateButton.SetActive(hasUpdate);
            MainMenuManagerPatch.updateButton.transform.position = MainMenuManagerPatch.template.transform.position + new Vector3(0.25f, 0.75f);
            __instance.StartCoroutine(Effects.Lerp(0.01f, new Action<float>((p) =>
            {
                MainMenuManagerPatch.updateButton.transform
                    .GetChild(0).GetComponent<TMPro.TMP_Text>()
                    .SetText($"{GetString("updateButton")}\n{latestTitle}");
            })));
        }
        public static async Task<bool> CheckRelease(bool beta = false)
        {
            string url = beta ? Main.BetaBuildURL.Value : URL + "/releases/latest";
            try
            {
                string result;
                using (HttpClient client = new())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "TownOfHost Updater");
                    using var response = await client.GetAsync(new Uri(url), HttpCompletionOption.ResponseContentRead);
                    if (!response.IsSuccessStatusCode || response.Content == null)
                    {
                        Logger.Error($"状态代码: {response.StatusCode}", "CheckRelease");
                        return false;
                    }
                    result = await response.Content.ReadAsStringAsync();
                }
                JObject data = JObject.Parse(result);
                if (beta)
                {
                    latestTitle = data["name"].ToString();
                    downloadUrl = data["url"].ToString();
                    hasUpdate = latestTitle != ThisAssembly.Git.Commit;
                }
                else
                {
                    latestVersion = new(data["tag_name"]?.ToString().TrimStart('v'));
                    latestTitle = $"Ver. {latestVersion}";
                    JArray assets = data["assets"].Cast<JArray>();
                    for (int i = 0; i < assets.Count; i++)
                    {
                        if (assets[i]["name"].ToString() == "TownOfHost_Steam.dll" && Constants.GetPlatformType() == Platforms.StandaloneSteamPC)
                        {
                            downloadUrl = assets[i]["browser_download_url"].ToString();
                            break;
                        }
                        if (assets[i]["name"].ToString() == "TownOfHost_Epic.dll" && Constants.GetPlatformType() == Platforms.StandaloneEpicPC)
                        {
                            downloadUrl = assets[i]["browser_download_url"].ToString();
                            break;
                        }
                        if (assets[i]["name"].ToString() == "TownOfHost.dll")
                            downloadUrl = assets[i]["browser_download_url"].ToString();
                    }
                    hasUpdate = latestVersion.CompareTo(Main.version) > 0;
                }
                if (downloadUrl == null)
                {
                    Logger.Error("无法获取下载URL", "CheckRelease");
                    return false;
                }
                isChecked = true;
            }
            catch (Exception ex)
            {
                Logger.Error($"版本检查失败。\n{ex}", "CheckRelease", false);
                return false;
            }
            return true;
        }
        public static void StartUpdate(string url)
        {
            ShowPopup(GetString("updatePleaseWait"));
            if (!BackupDLL())
            {
                ShowPopup(GetString("updateManually"), true);
                return;
            }
            _ = DownloadDLL(url);
            return;
        }
        public static bool BackupDLL()
        {
            try
            {
                File.Move(Assembly.GetExecutingAssembly().Location, Assembly.GetExecutingAssembly().Location + ".bak");
            }
            catch
            {
                Logger.Error("备份失败", "BackupDLL");
                return false;
            }
            return true;
        }
        public static void DeleteOldDLL()
        {
            try
            {
                foreach (var path in Directory.EnumerateFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "*.bak"))
                {
                    Logger.Info($"{Path.GetFileName(path)}删除", "DeleteOldDLL");
                    File.Delete(path);
                }
            }
            catch
            {
                Logger.Error("删除失败", "DeleteOldDLL");
            }
            return;
        }
        public static async Task<bool> DownloadDLL(string url)
        {
            try
            {
                using WebClient client = new();
                client.DownloadProgressChanged += DownloadCallBack;
                client.DownloadFileAsync(new Uri(url), "BepInEx/plugins/TownOfHost.dll");
                while (client.IsBusy) await Task.Delay(1);
                ShowPopup(GetString("updateRestart"), true);
            }
            catch (Exception ex)
            {
                Logger.Error($"下载失败\n{ex}", "DownloadDLL", false);
                ShowPopup(GetString("updateManually"), true);
                return false;
            }
            return true;
        }
        private static void DownloadCallBack(object sender, DownloadProgressChangedEventArgs e)
        {
            ShowPopup($"{GetString("updateInProgress")}\n{e.BytesReceived}/{e.TotalBytesToReceive}({e.ProgressPercentage}%)");
        }
        private static void ShowPopup(string message, bool showButton = false)
        {
            if (InfoPopup != null)
            {
                InfoPopup.Show(message);
                var button = InfoPopup.transform.FindChild("ExitGame");
                if (button != null)
                {
                    button.gameObject.SetActive(showButton);
                    button.GetChild(0).GetComponent<TextTranslatorTMP>().TargetText = StringNames.QuitLabel;
                    button.GetComponent<PassiveButton>().OnClick = new();
                    button.GetComponent<PassiveButton>().OnClick.AddListener((Action)(() => Application.Quit()));
                }
            }
        }
    }
}