using UnityEngine;
using Zifir.Core;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Pooled projectile. Moves by stepped raycasts (no rigidbody) so fast
    /// bullets never tunnel through walls, and no physics allocations occur.
    /// </summary>
    public sealed class Bullet : MonoBehaviour
    {
        private static readonly RaycastHit2D[] HitBuffer = new RaycastHit2D[8];

        private Vector2 _velocity;
        private float _damage;
        private float _remainingRange;
        private int _hitMask;
        private System.Action<Bullet> _release;

        public void Fire(Vector2 origin, Vector2 direction, float speed, float damage,
                         float range, Color tracer, System.Action<Bullet> release)
        {
            transform.position = origin;
            _velocity = direction.normalized * speed;
            _damage = damage;
            _remainingRange = range;
            _release = release;
            _hitMask = LayerMask.GetMask("Enemy", "Wall");

            float ang = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, ang);

            var sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = tracer;
        }

        private void Update()
        {
            float stepLen = _velocity.magnitude * Time.deltaTime;
            if (stepLen <= 0f) { Despawn(); return; }

            Vector2 pos = transform.position;
            Vector2 dir = _velocity.normalized;

            int hits = Physics2D.RaycastNonAlloc(pos, dir, HitBuffer, stepLen, _hitMask);
            for (int i = 0; i < hits; i++)
            {
                RaycastHit2D hit = HitBuffer[i];
                if (hit.collider == null) continue;

                var enemy = hit.collider.GetComponentInParent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.Damage(_damage, hit.point);
                    Despawn();
                    return;
                }
                // Wall
                Despawn();
                return;
            }

            _remainingRange -= stepLen;
            if (_remainingRange <= 0f) { Despawn(); return; }
            transform.position = pos + dir * stepLen;
        }

        private void Despawn()
        {
            _release?.Invoke(this);
        }

        /// <summary>Creates a bullet instance for pooling (thin 2px tracer sprite).</summary>
        public static Bullet CreateInstance(Transform parent)
        {
            var go = new GameObject("Bullet");
            go.transform.SetParent(parent, false);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = ProceduralSprites.Square(new Color32(255, 255, 255, 255));
            sr.sortingOrder = 40;
            go.transform.localScale = new Vector3(0.45f, 0.12f, 1f);
            return go.AddComponent<Bullet>();
        }
    }
}
