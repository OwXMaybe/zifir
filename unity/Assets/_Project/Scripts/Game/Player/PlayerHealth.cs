using UnityEngine;
using Zifir.Core;

namespace Zifir.Gameplay
{
    /// <summary>Player hit points with brief post-hit invulnerability.</summary>
    public sealed class PlayerHealth : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float maxHealth = 130f;
        [SerializeField, Min(0f)] private float invulnerabilitySeconds = 0.35f;

        private float _invulnUntil;

        public float Max => maxHealth;
        public float Current { get; private set; }
        public bool IsDead => Current <= 0f;

        public void Configure(float max)
        {
            maxHealth = max;
            Current = max;
        }

        private void Awake()
        {
            Current = maxHealth;
        }

        public void Damage(float amount)
        {
            if (IsDead || Time.time < _invulnUntil || amount <= 0f) return;
            _invulnUntil = Time.time + invulnerabilitySeconds;
            Current = Mathf.Max(0f, Current - amount);
            GameEvents.RaisePlayerDamaged(amount);
            Haptics.Heavy();
            if (Current <= 0f) GameEvents.RaisePlayerDied();
        }

        public void Heal(float amount)
        {
            if (IsDead || amount <= 0f) return;
            Current = Mathf.Min(maxHealth, Current + amount);
        }
    }
}
