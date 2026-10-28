using System;

namespace Zifir.Core
{
    /// <summary>
    /// Serializable save schema. Mirrors the HTML original's v5 save so feature
    /// parity is trackable. JsonUtility-friendly: fields only, no dictionaries.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;

        // Progression
        public int level = 1;
        public int xp;
        public int gems;
        public int credits;
        public int sector;
        public int maxSector;
        public int storyStep;
        public int ngPlus;

        // Stats
        public int raids;
        public int extracts;
        public int kills;
        public int bestRaid;

        // Unlocks (parallel arrays instead of dictionaries for JsonUtility)
        public string activeOperator = "sentinel";
        public string[] unlockedOperators = { "sentinel" };
        public string[] ownedWeapons = { "pistol", "carbine", "knife" };

        // Settings
        public SettingsData settings = new SettingsData();
    }

    [Serializable]
    public class SettingsData
    {
        public float sfxVolume = 1f;
        public float musicVolume = 1f;
        public float screenShake = 1f;
        public bool damageNumbers = true;
        public bool crtFilter;
        public bool flashEffects = true;
        public bool haptics = true;
        /// <summary>Touch sensitivity multiplier, 0.5–1.5.</summary>
        public float touchSensitivity = 1f;
        /// <summary>Language code. English is the default for new players.</summary>
        public string language = "en";
        public string difficulty = "stalker";
    }
}
