using UnityEngine;
using Zifir.Config;
using Zifir.GameInput;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Physics-driven movement: analog speed from the virtual joystick,
    /// full-deflection sprint with a stamina budget, smooth acceleration /
    /// deceleration for mobile feel. Facing follows the aim joystick when
    /// aiming, otherwise the movement direction.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0.1f)] private float moveSpeed = 5.4f;
        [SerializeField, Min(1f)] private float sprintMultiplier = 1.55f;
        [SerializeField, Range(0.5f, 1f)] private float sprintThreshold = 0.94f;
        [SerializeField, Min(1f)] private float acceleration = 60f;

        [Header("Stamina")]
        [SerializeField, Min(1f)] private float staminaMax = 100f;
        [SerializeField, Min(0f)] private float staminaDrain = 22f;
        [SerializeField, Min(0f)] private float staminaRegen = 14f;

        private Rigidbody2D _body;
        private Vector2 _targetVelocity;

        public float Stamina { get; private set; }
        public float StaminaMax => staminaMax;
        public bool Sprinting { get; private set; }
        /// <summary>Current facing angle in radians (0 = +x).</summary>
        public float FacingRadians { get; private set; }

        public void Configure(OperatorDef op)
        {
            if (op == null) return;
            moveSpeed = op.moveSpeed;
            sprintMultiplier = op.sprintMultiplier;
            staminaMax = op.staminaMax;
            staminaDrain = op.staminaDrainPerSecond;
            staminaRegen = op.staminaRegenPerSecond;
            Stamina = staminaMax;
        }

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _body.gravityScale = 0f;
            _body.freezeRotation = true;
            _body.interpolation = RigidbodyInterpolation2D.Interpolate;
            _body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            Stamina = staminaMax;
        }

        private void Update()
        {
            TouchInput input = TouchInput.Instance;
            if (input == null)
            {
                _targetVelocity = Vector2.zero;
                return;
            }

            Vector2 move = input.Move;
            float mag = Mathf.Clamp01(move.magnitude);

            Sprinting = mag >= sprintThreshold && Stamina > 1f;
            float speed = moveSpeed * (Sprinting ? sprintMultiplier : 1f) * mag;
            _targetVelocity = mag > 0f ? move.normalized * speed : Vector2.zero;

            Stamina = Mathf.Clamp(
                Stamina + (Sprinting ? -staminaDrain : staminaRegen) * Time.deltaTime,
                0f, staminaMax);

            // Facing: aim direction wins; otherwise face where we walk.
            if (input.AimHeld)
                FacingRadians = Mathf.Atan2(input.AimDir.y, input.AimDir.x);
            else if (mag > 0.05f)
                FacingRadians = Mathf.Atan2(move.y, move.x);
        }

        private void FixedUpdate()
        {
            _body.linearVelocity = Vector2.MoveTowards(
                _body.linearVelocity, _targetVelocity, acceleration * Time.fixedDeltaTime);
        }
    }
}
