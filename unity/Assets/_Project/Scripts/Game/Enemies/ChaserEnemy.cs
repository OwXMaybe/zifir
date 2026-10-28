using UnityEngine;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Phase-1 enemy: pursues the player when in range and deals contact
    /// damage on a cooldown. Wall-aware (physics body). The full enemy zoo
    /// (soldier/hound/drone/spitter) replaces this in phase 2 while reusing
    /// EnemyHealth and the same movement base.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(EnemyHealth))]
    public sealed class ChaserEnemy : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float moveSpeed = 3.2f;
        [SerializeField, Min(0.5f)] private float aggroRange = 9f;
        [SerializeField, Min(0.1f)] private float contactRange = 0.8f;
        [SerializeField, Min(0f)] private float contactDamage = 12f;
        [SerializeField, Min(0.1f)] private float attackCooldown = 0.8f;

        private Rigidbody2D _body;
        private EnemyHealth _health;
        private Transform _target;
        private float _nextAttackTime;

        public void SetTarget(Transform target) => _target = target;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _health = GetComponent<EnemyHealth>();
            _body.gravityScale = 0f;
            _body.freezeRotation = true;
            _body.interpolation = RigidbodyInterpolation2D.Interpolate;
        }

        private void FixedUpdate()
        {
            if (!_health.Alive || _target == null)
            {
                _body.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 toTarget = (Vector2)_target.position - _body.position;
            float dist = toTarget.magnitude;

            if (dist > aggroRange)
            {
                _body.linearVelocity = Vector2.zero;
                return;
            }

            if (dist > contactRange)
            {
                _body.linearVelocity = toTarget.normalized * moveSpeed;
            }
            else
            {
                _body.linearVelocity = Vector2.zero;
                TryAttack();
            }
        }

        private void TryAttack()
        {
            if (Time.time < _nextAttackTime || _target == null) return;
            _nextAttackTime = Time.time + attackCooldown;
            var hp = _target.GetComponent<PlayerHealth>();
            if (hp != null) hp.Damage(contactDamage);
        }
    }
}
