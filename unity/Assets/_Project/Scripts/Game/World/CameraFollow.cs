using UnityEngine;
using Zifir.GameInput;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Smooth-damped camera with a small look-ahead toward the aim direction —
    /// the player sees more of what they're shooting at (mirrors the original's
    /// camera feel).
    /// </summary>
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField, Min(0.01f)] private float smoothTime = 0.12f;
        [SerializeField, Min(0f)] private float aimLookAhead = 1.6f;

        private Vector3 _velocity;

        public void SetTarget(Transform t) => target = t;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desired = target.position;
            TouchInput input = TouchInput.Instance;
            if (input != null && input.AimHeld)
                desired += (Vector3)(input.AimDir * aimLookAhead);

            desired.z = transform.position.z;
            transform.position = Vector3.SmoothDamp(transform.position, desired, ref _velocity, smoothTime);
        }
    }
}
