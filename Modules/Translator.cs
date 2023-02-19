using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Csv;
using HarmonyLib;

/*
 * Translator from Town Of Host Edited
 * https://github.com/KARPED1EM/TownOfHostEdited/blob/TOHE/Patches/ControlPatch.cs
 */
namespace TownOfHost
{
    public static class Translator
    {
        public static Dictionary<string, Dictionary<int, string>>? TranslateMaps;
        public const string LanguageFolderName = "Language";
        public static void Init()
        {
            Logger.Info("加载语言文件...", "Translator");
            LoadLang();
            Logger.Info("加载语言文件成功", "Translator");
        }
        public static void LoadLang()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("TownOfHost.Resources.string.csv");
            TranslateMaps = new Dictionary<string, Dictionary<int, string>>();

            var options = new CsvOptions()
            {
                HeaderMode = HeaderMode.HeaderPresent,
                AllowNewLineInEnclosedFieldValues = false,
            };
            foreach (var line in CsvReader.ReadFromStream(stream, options))
            {
                if (line.Values[0][0] == '#') continue;
                try
                {
                    Dictionary<int, string> dic = new();
                    for (int i = 1; i < line.ColumnCount; i++)
                    {
                        int id = int.Parse(line.Headers[i]);
                        dic[id] = line.Values[i].Replace("\\n", "\n").Replace("\\r", "\r");
                    }
                    if (!TranslateMaps.TryAdd(line.Values[0], dic))
                        Logger.Warn($"待翻译的 CSV 文件中存在重复项。{line.Index}行: \"{line.Values[0]}\"", "Translator");
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), "Translator");
                }
            }

            // カスタム翻訳ファイルの読み込み
            if (!Directory.Exists(LanguageFolderName)) Directory.CreateDirectory(LanguageFolderName);

            // 翻訳テンプレートの作成
            CreateTemplateFile();
            foreach (var lang in Enum.GetValues(typeof(SupportedLangs)))
            {
                if (File.Exists(@$"./{LanguageFolderName}/{lang}.dat"))
                    LoadCustomTranslation($"{lang}.dat", (SupportedLangs)lang);
            }
        }

        public static string GetString(string s, Dictionary<string, string> replacementDic = null)
        {
            string str = GetString(s);
            if (replacementDic != null)
                foreach (var rd in replacementDic)
                {
                    str = str.Replace(rd.Key, rd.Value);
                }
            return str;
        }

        public static string GetString(string str)
        {
            var res = $"<INVALID:{str}>";

            foreach (var role in NewRole.RoleManager.GetRoles())
            {
                if (str.ToLower().Equals(role.Name.ToLower()) || str.ToLower().Equals(role.DisplayName.ToLower()))
                    // for get role name
                    res = role.DisplayName;

                if (str.ToLower().EndsWith("info") && str.ToLower().StartsWith(role.Name.ToLower()))
                    // for get info
                    res = role.Info;

                if (str.ToLower().EndsWith("info" + "long") && str.ToLower().StartsWith(role.Name.ToLower()))
                    // for get description
                    res = role.Description;
            }

            try
            {
                if (TranslateMaps!.TryGetValue(str, out var dic) && (!dic.TryGetValue((int)SupportedLangs.SChinese, out res) || res == "")) //匹配 str & 无效的 langId 或 res 为空
                {
                    res = $"*{dic[0]}";
                }
            }
            catch (Exception e)
            {
                Logger.Fatal(str, "Translator");
                Logger.Error(e.ToString(), "Translator");
            }
            return res;
        }

        public static void LoadCustomTranslation(string filename, SupportedLangs lang)
        {
            string path = @$"./{LanguageFolderName}/{filename}";
            if (File.Exists(path))
            {
                Logger.Info($"カスタム翻訳ファイル「{filename}」を読み込み", "LoadCustomTranslation");
                using StreamReader sr = new(path, Encoding.GetEncoding("UTF-8"));
                string text;
                string[] tmp = { };
                while ((text = sr.ReadLine() ?? string.Empty) != null)
                {
                    tmp = text.Split(":");
                    if (tmp.Length > 1 && tmp[1] != "")
                    {
                        try
                        {
                            TranslateMaps[tmp[0]][(int)lang] = tmp.Skip(1).Join(delimiter: ":").Replace("\\n", "\n").Replace("\\r", "\r");
                        }
                        catch (KeyNotFoundException)
                        {
                            Logger.Warn($"「{tmp[0]}」は有効なキーではありません。", "LoadCustomTranslation");
                        }
                    }
                }
            }
            else
                Logger.Error($"カスタム翻訳ファイル「{filename}」が見つかりませんでした", "LoadCustomTranslation");
        }

        private static void CreateTemplateFile()
        {
            var text = "";
            foreach (var title in TranslateMaps) text += $"{title.Key}:{GetString(title.Key)}\n";
            File.WriteAllText(@$"./{LanguageFolderName}/template.dat", text);
        }
    }
}