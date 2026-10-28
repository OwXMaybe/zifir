using UnityEngine;
using Zifir.Config;
using Zifir.Core;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Owns the equipped weapon: fire cadence, deterministic recoil pattern
    /// (learnable, mirrors the original), spread, magazine and reload. Bullets
    /// come from a shared pool — zero allocations while shooting.
    /// </summary>
    public sealed class WeaponController : MonoBehaviour
    {
        [SerializeField, Min(0f)] private float recoilRecoverySeconds = 0.35f;
        [SerializeField, Min(0.5f)] private float muzzleOffset = 0.6f;

        private WeaponDef _weapon;
        private ObjectPool<Bullet> _pool;
        private float _fireTimer;
        private float _reloadTimer;
        private float _recoilRecoverTimer;
        private int _recoilIndex;

        public WeaponDef Weapon => _weapon;
        public int MagazineCurrent { get; private set; }
        public bool Reloading => _reloadTimer > 0f;

        public void Configure(WeaponDef weapon, ObjectPool<Bullet> pool)
        {
            _weapon = weapon;
            _pool = pool;
            MagazineCurrent = weapon != null ? weapon.magazineSize : 0;
            _recoilIndex = 0;
            _fireTimer = 0f;
            _reloadTimer = 0f;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _fireTimer -= dt;

            if (_reloadTimer > 0f)
            {
                _reloadTimer -= dt;
                if (_reloadTimer <= 0f && _weapon != null)
                    MagazineCurrent = _weapon.magazineSize;
            }

            _recoilRecoverTimer -= dt;
            if (_recoilRecoverTimer <= 0f && _recoilIndex > 0)
            {
                _recoilIndex--;
                _recoilRecoverTimer = 0.06f;
            }
        }

        /// <summary>
        /// Attempt one shot toward the target point. Honors cadence, reload and
        /// magazine; auto-reloads on empty. Returns true if a shot left the barrel.
        /// </summary>
        public bool TryFire(Vector2 targetPoint)
        {
            if (_weapon == null || _pool == null) return false;
            if (_fireTimer > 0f || Reloading) return false;
            if (MagazineCurrent <= 0)
            {
                StartReload();
                return false;
            }

            MagazineCurrent--;
            _fireTimer = _weapon.FireInterval;

            Vector2 origin = transform.position;
            Vector2 toTarget = targetPoint - origin;
            float baseAngle = Mathf.Atan2(toTarget.y, toTarget.x);

            float drift = _weapon.RecoilAt(_recoilIndex);
            _recoilIndex++;
            _recoilRecoverTimer = recoilRecoverySeconds;

            for (int i = 0; i < _weapon.pelletsPerShot; i++)
            {
                float ang = baseAngle + drift + Random.Range(-_weapon.spreadRadians, _weapon.spreadRadians);
                Vector2 dir = new(Mathf.Cos(ang), Mathf.Sin(ang));
                Bullet b = _pool.Get();
                b.Fire(origin + dir * muzzleOffset, dir, _weapon.bulletSpeed,
                       _weapon.damage, _weapon.range, _weapon.tracerColor, _pool.Release);
            }

            GameEvents.RaiseWeaponFired();
            Haptics.Light();
            return true;
        }

        public void StartReload()
        {
            if (_weapon == null || Reloading || MagazineCurrent >= _weapon.magazineSize) return;
            _reloadTimer = _weapon.reloadSeconds;
        }
    }
}
