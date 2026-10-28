using UnityEngine;
using Zifir.Core;

namespace Zifir.Gameplay
{
    /// <summary>Enemy hit points, hit flash, and death → kill event.</summary>
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float maxHealth = 40f;
        [SerializeField, Min(0)] private int creditReward = 12;
        [SerializeField, Min(0f)] private float hitFlashSeconds = 0.08f;

        private SpriteRenderer _renderer;
        private Color _baseColor;
        private float _flashUntil;

        public bool Alive { get; private set; } = true;
        /// <summary>Where bullets and the laser should aim (center of mass).</summary>
        public Vector2 AimPoint => transform.position;

        public void Configure(float hp, int credits)
        {
            maxHealth = hp;
            creditReward = credits;
            Current = hp;
        }

        public float Current { get; private set; }

        private void Awake()
        {
            Current = maxHealth;
            _renderer = GetComponentInChildren<SpriteRenderer>();
            if (_renderer != null) _baseColor = _renderer.color;
        }

        private void Update()
        {
            if (_renderer != null && _flashUntil > 0f && Time.time >= _flashUntil)
            {
                _renderer.color = _baseColor;
                _flashUntil = 0f;
            }
        }

        public void Damage(float amount, Vector2 hitPoint)
        {
            if (!Alive || amount <= 0f) return;
            Current -= amount;

            if (_renderer != null)
            {
                _renderer.color = Color.white;
                _flashUntil = Time.time + hitFlashSeconds;
            }

            if (Current <= 0f) Die();
        }

        private void Die()
        {
            Alive = false;
            GameEvents.RaiseEnemyKilled(creditReward);
            Haptics.Medium();
            Destroy(gameObject);
        }
    }
}
