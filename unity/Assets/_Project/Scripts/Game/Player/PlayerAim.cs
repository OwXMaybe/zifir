using UnityEngine;
using Zifir.Core;
using Zifir.GameInput;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Laser aiming: while the aim joystick is held, a laser ray extends from
    /// the player in the joystick's direction, stopping at walls. The closest
    /// visible enemy inside the ray corridor becomes the lock target (sticky —
    /// a wider corridor keeps an acquired target), and the weapon auto-fires
    /// at it. The laser pulses and turns red on lock.
    /// </summary>
    [RequireComponent(typeof(WeaponController))]
    public sealed class PlayerAim : MonoBehaviour
    {
        [Header("Laser")]
        [SerializeField, Min(1f)] private float maxLaserLength = 20f;
        [SerializeField] private Color idleColor = new(0.43f, 0.9f, 1f, 0.75f);
        [SerializeField] private Color lockColor = new(1f, 0.35f, 0.29f, 0.95f);
        [SerializeField, Min(1f)] private float pulseSpeed = 12f;

        [Header("Aim assist (world units)")]
        [SerializeField, Min(0f)] private float acquireCorridor = 0.4f;
        [SerializeField, Min(0f)] private float stickyCorridor = 1.1f;

        private static readonly RaycastHit2D[] CastBuffer = new RaycastHit2D[16];

        private WeaponController _weapon;
        private LineRenderer _laser;
        private int _wallMask;
        private ContactFilter2D _enemyFilter;

        public EnemyHealth LockTarget { get; private set; }

        private void Awake()
        {
            _weapon = GetComponent<WeaponController>();
            _wallMask = LayerMask.GetMask("Wall");
            _enemyFilter = new ContactFilter2D { useTriggers = false };
            _enemyFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
            CreateLaser();
        }

        private void CreateLaser()
        {
            var go = new GameObject("Laser");
            go.transform.SetParent(transform, false);
            _laser = go.AddComponent<LineRenderer>();
            _laser.positionCount = 2;
            _laser.startWidth = 0.07f;
            _laser.endWidth = 0.03f;
            _laser.useWorldSpace = true;
            _laser.sortingOrder = 60;
            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null) _laser.material = new Material(shader);
            _laser.enabled = false;
        }

        private void Update()
        {
            TouchInput input = TouchInput.Instance;
            if (input == null || !input.AimHeld)
            {
                LockTarget = null;
                if (_laser.enabled) _laser.enabled = false;
                return;
            }

            Vector2 origin = transform.position;
            Vector2 dir = input.AimDir.sqrMagnitude > 0.001f ? input.AimDir.normalized : Vector2.right;

            // Laser stops at the first wall (or max length).
            float laserLen = maxLaserLength;
            RaycastHit2D wallHit = Physics2D.Raycast(origin, dir, maxLaserLength, _wallMask);
            if (wallHit.collider != null) laserLen = wallHit.distance;

            LockTarget = SelectTarget(origin, dir, laserLen);

            if (LockTarget != null)
                _weapon.TryFire(LockTarget.AimPoint);

            DrawLaser(origin, dir, laserLen);
        }

        private EnemyHealth SelectTarget(Vector2 origin, Vector2 dir, float laserLen)
        {
            // Sticky: keep the current target while it stays in a wider corridor.
            if (LockTarget != null && LockTarget.Alive &&
                InCorridor(LockTarget.AimPoint, origin, dir, laserLen, stickyCorridor) &&
                HasLineOfSight(origin, LockTarget.AimPoint))
                return LockTarget;

            // Acquire: closest visible enemy inside the narrow corridor.
            int count = Physics2D.CircleCast(origin, acquireCorridor, dir, _enemyFilter, CastBuffer, laserLen);
            EnemyHealth best = null;
            float bestDist = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                var enemy = CastBuffer[i].collider != null
                    ? CastBuffer[i].collider.GetComponentInParent<EnemyHealth>()
                    : null;
                if (enemy == null || !enemy.Alive) continue;
                float d = (enemy.AimPoint - origin).sqrMagnitude;
                if (d < bestDist && HasLineOfSight(origin, enemy.AimPoint))
                {
                    bestDist = d;
                    best = enemy;
                }
            }
            return best;
        }

        private static bool InCorridor(Vector2 point, Vector2 origin, Vector2 dir, float len, float corridor)
        {
            Vector2 to = point - origin;
            float along = Vector2.Dot(to, dir);
            if (along < 0f || along > len + corridor) return false;
            Vector2 closest = origin + dir * Mathf.Clamp(along, 0f, len);
            return (point - closest).sqrMagnitude <= corridor * corridor;
        }

        private bool HasLineOfSight(Vector2 from, Vector2 to)
        {
            return Physics2D.Linecast(from, to, _wallMask).collider == null;
        }

        private void DrawLaser(Vector2 origin, Vector2 dir, float len)
        {
            _laser.enabled = true;
            Vector2 end = origin + dir * len;
            _laser.SetPosition(0, origin);
            _laser.SetPosition(1, end);

            float pulse = 0.7f + 0.3f * Mathf.Sin(Time.time * pulseSpeed);
            Color c = (LockTarget != null ? lockColor : idleColor) * pulse;
            c.a = LockTarget != null ? 0.95f : 0.6f;
            _laser.startColor = c;
            _laser.endColor = new Color(c.r, c.g, c.b, 0.15f);
        }
    }
}
