using UnityEngine;

namespace Zifir.Config
{
    /// <summary>
    /// Designer-tunable weapon definition. All balance lives here, editable in
    /// the Inspector — no magic numbers in gameplay code. Distances/speeds are
    /// in world units (1 unit = 1 tile = 16 px of the original game).
    /// </summary>
    [CreateAssetMenu(menuName = "Zifir/Weapon", fileName = "Weapon")]
    public class WeaponDef : ScriptableObject
    {
        [Header("Identity")]
        public string id = "pistol";
        public string displayName = "VS-9 WHISPER";
        public Color tracerColor = new(0.62f, 0.85f, 1f);

        [Header("Damage")]
        [Min(0f)] public float damage = 13f;
        [Min(1f)] public float roundsPerMinute = 300f;
        [Min(1)] public int pelletsPerShot = 1;

        [Header("Magazine")]
        [Min(1)] public int magazineSize = 15;
        [Min(0f)] public float reloadSeconds = 1.1f;

        [Header("Ballistics")]
        [Min(0f)] public float bulletSpeed = 32f;
        [Min(0f)] public float range = 10.6f;
        [Range(0f, 0.5f)] public float spreadRadians = 0.028f;
        public bool fullAuto;

        [Header("Recoil (deterministic pattern, mirrors the original)")]
        [Range(0f, 0.5f)] public float recoilPerStep = 0.012f;
        [Tooltip("Horizontal drift per consecutive shot; index clamps at the end.")]
        public float[] recoilPattern = { 0f, 0.2f, 0.5f, 0.9f, 1.3f, 1.6f, 1.8f, 1.7f, 1.4f, 1.0f, 0.5f, 0f, -0.5f, -1.0f, -1.4f, -1.6f, -1.5f, -1.2f, -0.8f, -0.4f };

        /// <summary>Seconds between shots.</summary>
        public float FireInterval => 60f / roundsPerMinute;

        /// <summary>Recoil drift (radians) for the given consecutive-shot index.</summary>
        public float RecoilAt(int shotIndex)
        {
            if (recoilPattern == null || recoilPattern.Length == 0) return 0f;
            int i = Mathf.Clamp(shotIndex, 0, recoilPattern.Length - 1);
            return recoilPattern[i] * recoilPerStep;
        }
    }

    /// <summary>
    /// Code factory for the built-in arsenal. Phase 1 ships pistol + carbine;
    /// an editor tool can bake these into .asset files for hand-tuning later.
    /// </summary>
    public static class WeaponLibrary
    {
        public static WeaponDef CreatePistol()
        {
            var w = ScriptableObject.CreateInstance<WeaponDef>();
            w.name = "Weapon_Pistol";
            w.id = "pistol";
            w.displayName = "VS-9 WHISPER";
            w.damage = 13f; w.roundsPerMinute = 300f; w.magazineSize = 15;
            w.reloadSeconds = 1.1f; w.bulletSpeed = 32f; w.range = 10.6f;
            w.spreadRadians = 0.028f; w.fullAuto = false; w.recoilPerStep = 0.012f;
            w.tracerColor = new Color(0.62f, 0.85f, 1f);
            return w;
        }

        public static WeaponDef CreateCarbine()
        {
            var w = ScriptableObject.CreateInstance<WeaponDef>();
            w.name = "Weapon_Carbine";
            w.id = "carbine";
            w.displayName = "PULSE CARBINE MK.2";
            w.damage = 11f; w.roundsPerMinute = 660f; w.magazineSize = 30;
            w.reloadSeconds = 1.6f; w.bulletSpeed = 37.5f; w.range = 16.25f;
            w.spreadRadians = 0.045f; w.fullAuto = true; w.recoilPerStep = 0.02f;
            w.tracerColor = new Color(1f, 0.82f, 0.48f);
            return w;
        }
    }
}
