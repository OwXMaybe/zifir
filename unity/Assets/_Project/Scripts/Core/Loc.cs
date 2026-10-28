using System;
using System.Collections.Generic;

namespace Zifir.Core
{
    /// <summary>
    /// Key-based localization. English is the default and the fallback for any
    /// missing key, so untranslated languages degrade gracefully. Adding a
    /// language = adding one dictionary to <see cref="Tables"/>.
    /// </summary>
    public static class Loc
    {
        public static event Action LanguageChanged;

        private static string _lang = "en";
        private static Dictionary<string, string> _active = English;

        public static string Language => _lang;

        public static void SetLanguage(string code)
        {
            if (string.IsNullOrEmpty(code) || !Tables.ContainsKey(code)) code = "en";
            if (_lang == code) return;
            _lang = code;
            _active = Tables[code];
            SaveSystem.Data.settings.language = code;
            SaveSystem.Save();
            LanguageChanged?.Invoke();
        }

        /// <summary>Translate a key. Falls back to English, then to the key itself.</summary>
        public static string T(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            if (_active.TryGetValue(key, out string v)) return v;
            if (_active != English && English.TryGetValue(key, out string en)) return en;
            return key;
        }

        /// <summary>Initialize from save. Call once at boot.</summary>
        public static void Init()
        {
            string code = SaveSystem.Data.settings.language;
            if (!string.IsNullOrEmpty(code) && Tables.ContainsKey(code))
            {
                _lang = code;
                _active = Tables[code];
            }
        }

        private static readonly Dictionary<string, string> English = new()
        {
            { "game.title", "ZIFIR" },
            { "game.subtitle", "THE KESTREL PROTOCOL" },
            { "menu.startRaid", "START RAID" },
            { "menu.settings", "SETTINGS" },
            { "hud.health", "HP" },
            { "hud.reloading", "RELOADING..." },
            { "hud.extraction", "EXTRACTION" },
            { "settings.language", "LANGUAGE" },
            { "settings.touchSensitivity", "TOUCH SENSITIVITY" },
            { "settings.haptics", "VIBRATION" },
            { "raid.killed", "K.I.A." },
            { "raid.extracted", "EXTRACTION SUCCESSFUL" },
        };

        private static readonly Dictionary<string, string> Turkish = new()
        {
            { "game.title", "ZİFİR" },
            { "game.subtitle", "KESTREL PROTOKOLÜ" },
            { "menu.startRaid", "RAID BAŞLAT" },
            { "menu.settings", "AYARLAR" },
            { "hud.health", "HP" },
            { "hud.reloading", "DOLDURUYOR..." },
            { "hud.extraction", "EXTRACTION" },
            { "settings.language", "DİL" },
            { "settings.touchSensitivity", "DOKUNMATİK HASSASİYET" },
            { "settings.haptics", "TİTREŞİM" },
            { "raid.killed", "K.I.A." },
            { "raid.extracted", "EXTRACTION BAŞARILI" },
        };

        /// <summary>All available languages. Extend by adding a table here.</summary>
        public static readonly Dictionary<string, Dictionary<string, string>> Tables = new()
        {
            { "en", English },
            { "tr", Turkish },
        };
    }
}
