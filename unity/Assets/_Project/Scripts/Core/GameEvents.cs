using System;

namespace Zifir.Core
{
    /// <summary>
    /// Static event hub. Systems communicate through events instead of direct
    /// references so no manager becomes a god object and systems stay testable.
    /// </summary>
    public static class GameEvents
    {
        /// <summary>Player took damage. Arg: damage amount after mitigation.</summary>
        public static event Action<float> PlayerDamaged;
        /// <summary>Player health reached zero.</summary>
        public static event Action PlayerDied;
        /// <summary>An enemy died. Arg: credit reward.</summary>
        public static event Action<int> EnemyKilled;
        /// <summary>A weapon fired one shot.</summary>
        public static event Action WeaponFired;

        public static void RaisePlayerDamaged(float dmg) => PlayerDamaged?.Invoke(dmg);
        public static void RaisePlayerDied() => PlayerDied?.Invoke();
        public static void RaiseEnemyKilled(int credits) => EnemyKilled?.Invoke(credits);
        public static void RaiseWeaponFired() => WeaponFired?.Invoke();

        /// <summary>Clears all subscribers. Called on domain reload / raid restart.</summary>
        public static void Clear()
        {
            PlayerDamaged = null;
            PlayerDied = null;
            EnemyKilled = null;
            WeaponFired = null;
        }
    }
}
