using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using ImgPlacer.Models;

namespace ImgPlacer.Utils
{
    /// <summary>
    /// アプリの設定情報を保持し、Json で保存／読み込みするクラス。
    /// 設定ファイルはアプリの実行ディレクトリに保存されます。
    /// </summary>
    public class AppSettings
    {
        private const string SettingsFileName = "AppSettings.json";

        // 必要に応じてプロパティを追加してください（サンプルをいくつか用意）
        public string LastOpenedFolder { get; set; }

        public double CanvasWidth { get; set; }

        public List<TemplateText> Templates { get; set; }

        private static JsonSerializerOptions JsonOptions => new ()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
        };

        public static string GetConfigPath()
        {
            // 実行ディレクトリ直下
            var baseDir = AppContext.BaseDirectory;
            return Path.Combine(baseDir, SettingsFileName);
        }

        public static bool Exists()
        {
            return File.Exists(GetConfigPath());
        }

        public static AppSettings LoadOrDefault()
        {
            var path = GetConfigPath();
            try
            {
                if (!File.Exists(path))
                {
                    return new AppSettings();
                }

                var json = File.ReadAllText(path, Encoding.UTF8);
                var settings = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
                return settings ?? new AppSettings();
            }
            catch
            {
                // 壊れたファイルなどのケースでは新規で返す
                return new AppSettings();
            }
        }

        public static bool TryLoad(out AppSettings settings)
        {
            var path = GetConfigPath();
            try
            {
                if (!File.Exists(path))
                {
                    settings = new AppSettings();
                    return false;
                }

                var json = File.ReadAllText(path, Encoding.UTF8);
                var result = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
                if (result == null)
                {
                    settings = new AppSettings();
                    return false;
                }

                settings = result;
                return true;
            }
            catch
            {
                settings = new AppSettings();
                return false;
            }
        }

        public void Save()
        {
            var path = GetConfigPath();
            var json = JsonSerializer.Serialize(this, JsonOptions);

            // できるだけ安全に書き込み（テンポラリ→置き換え）
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var tempPath = path + ".tmp";
            File.WriteAllText(tempPath, json, new UTF8Encoding(false)); // UTF-8 (BOMなし)
            if (File.Exists(path))
            {
                File.Replace(tempPath, path, null);
            }
            else
            {
                File.Move(tempPath, path!);
            }
        }
    }
}