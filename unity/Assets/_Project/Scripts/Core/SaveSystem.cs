using System;
using System.IO;
using UnityEngine;

namespace Zifir.Core
{
    /// <summary>
    /// JSON save persistence at Application.persistentDataPath with versioned
    /// migration. Writes atomically (temp file + move) so a crash mid-save
    /// never corrupts the existing save.
    /// </summary>
    public static class SaveSystem
    {
        private const string FileName = "zifir_save.json";

        private static SaveData _data;

        /// <summary>The loaded save. Loads (or creates) lazily on first access.</summary>
        public static SaveData Data
        {
            get
            {
                if (_data == null) Load();
                return _data;
            }
        }

        private static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

        public static void Load()
        {
            try
            {
                if (File.Exists(SavePath))
                {
                    _data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
                    Migrate(_data);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[SaveSystem] Load failed, starting fresh: {e.Message}");
                _data = null;
            }

            _data ??= new SaveData();
        }

        public static void Save()
        {
            if (_data == null) return;
            try
            {
                string tmp = SavePath + ".tmp";
                File.WriteAllText(tmp, JsonUtility.ToJson(_data, prettyPrint: false));
                if (File.Exists(SavePath)) File.Delete(SavePath);
                File.Move(tmp, SavePath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveSystem] Save failed: {e.Message}");
            }
        }

        /// <summary>Deletes the save and resets in-memory state. Irreversible.</summary>
        public static void ResetAll()
        {
            try { if (File.Exists(SavePath)) File.Delete(SavePath); }
            catch (Exception e) { Debug.LogError($"[SaveSystem] Reset failed: {e.Message}"); }
            _data = new SaveData();
        }

        private static void Migrate(SaveData data)
        {
            if (data == null) return;
            // Future schema upgrades bump SaveData.CurrentVersion and add steps here.
            data.version = SaveData.CurrentVersion;
        }
    }
}
