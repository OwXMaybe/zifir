using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Zifir.Core;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Zifir.GameInput
{
    /// <summary>
    /// Mobile-first input: dynamic dual virtual joysticks driven by the new
    /// Input System's EnhancedTouch API.
    ///
    /// Left half  → movement joystick (spawns under the finger, base follows).
    /// Right half → 360° aim joystick; the laser always follows its direction.
    ///
    /// In the editor (no touch), WASD + mouse provide a dev fallback so the
    /// game stays testable, but the shipped experience is touch-only.
    /// </summary>
    public sealed class TouchInput : MonoBehaviour
    {
        public static TouchInput Instance { get; private set; }

        [Header("Joystick geometry (fractions of min(screen w,h) — resolution independent)")]
        [SerializeField, Range(0.02f, 0.3f)] private float fullDeflectionFraction = 0.09f;
        [SerializeField, Range(0.001f, 0.05f)] private float deadZoneFraction = 0.012f;
        [SerializeField, Range(0.05f, 0.4f)] private float baseFollowFraction = 0.13f;
        [SerializeField, Range(0f, 1f)] private float leftZoneMax = 0.44f;

        // Movement joystick state
        private int _moveTouchId = -1;
        private Vector2 _moveOrigin;
        /// <summary>Analog movement, magnitude 0..1.</summary>
        public Vector2 Move { get; private set; }
        public bool MoveActive => _moveTouchId != -1 || Move.sqrMagnitude > 0f;
        public Vector2 MoveOriginScreen => _moveOrigin;

        // Aim joystick state
        private int _aimTouchId = -1;
        private Vector2 _aimOrigin;
        /// <summary>True while the aim finger is down — laser is visible and autofire armed.</summary>
        public bool AimHeld { get; private set; }
        /// <summary>Normalized aim direction (world/screen aligned; y up).</summary>
        public Vector2 AimDir { get; private set; } = Vector2.right;
        /// <summary>Aim stick deflection 0..1 (drives UI knob).</summary>
        public float AimMagnitude { get; private set; }
        public Vector2 AimOriginScreen => _aimOrigin;

        private float Sensitivity => Mathf.Clamp(SaveSystem.Data.settings.touchSensitivity, 0.5f, 1.5f);
        private float MinScreen => Mathf.Min(Screen.width, Screen.height);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            EnhancedTouchSupport.Enable();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            ReadTouches();
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Touch.activeTouches.Count == 0) ReadDesktopFallback();
#endif
        }

        private void ReadTouches()
        {
            // Release tracking for ended touches first.
            foreach (Touch t in Touch.activeTouches)
            {
                if (t.phase is UnityEngine.InputSystem.TouchPhase.Ended or UnityEngine.InputSystem.TouchPhase.Canceled)
                {
                    if (t.touchId == _moveTouchId) ReleaseMove();
                    if (t.touchId == _aimTouchId) ReleaseAim();
                }
            }

            foreach (Touch t in Touch.activeTouches)
            {
                switch (t.phase)
                {
                    case UnityEngine.InputSystem.TouchPhase.Began:
                        OnTouchBegan(t);
                        break;
                    case UnityEngine.InputSystem.TouchPhase.Moved:
                    case UnityEngine.InputSystem.TouchPhase.Stationary:
                        OnTouchMoved(t);
                        break;
                }
            }

            // Safety: if a tracked touch vanished without an Ended phase, release it.
            if (_moveTouchId != -1 && !TouchAlive(_moveTouchId)) ReleaseMove();
            if (_aimTouchId != -1 && !TouchAlive(_aimTouchId)) ReleaseAim();
        }

        private static bool TouchAlive(int id)
        {
            foreach (Touch t in Touch.activeTouches)
                if (t.touchId == id) return true;
            return false;
        }

        private void OnTouchBegan(Touch t)
        {
            Vector2 pos = t.screenPosition;
            bool leftSide = pos.x < Screen.width * leftZoneMax;
            if (leftSide && _moveTouchId == -1)
            {
                _moveTouchId = t.touchId;
                _moveOrigin = pos;
                Move = Vector2.zero;
            }
            else if (!leftSide && _aimTouchId == -1)
            {
                _aimTouchId = t.touchId;
                _aimOrigin = pos;
                AimHeld = true;
                AimMagnitude = 0f; // direction persists from last aim until dragged
            }
        }

        private void OnTouchMoved(Touch t)
        {
            Vector2 pos = t.screenPosition;
            if (t.touchId == _moveTouchId)
            {
                Vector2 delta = pos - _moveOrigin;
                float dist = delta.magnitude;
                float follow = baseFollowFraction * MinScreen;
                if (dist > follow) _moveOrigin = pos - delta.normalized * follow;

                float dead = deadZoneFraction * MinScreen;
                float full = fullDeflectionFraction * MinScreen / Sensitivity;
                float mag = dist <= dead ? 0f : Mathf.Clamp01(dist / full);
                Move = mag > 0f ? delta.normalized * mag : Vector2.zero;
            }
            else if (t.touchId == _aimTouchId)
            {
                Vector2 delta = pos - _aimOrigin;
                float dist = delta.magnitude;
                float follow = baseFollowFraction * MinScreen;
                if (dist > follow) _aimOrigin = pos - delta.normalized * follow;

                float dead = deadZoneFraction * MinScreen;
                float full = fullDeflectionFraction * MinScreen / Sensitivity;
                if (dist > dead) AimDir = delta.normalized; // smooth 360°
                AimMagnitude = Mathf.Clamp01(dist / full);
            }
        }

        private void ReleaseMove()
        {
            _moveTouchId = -1;
            Move = Vector2.zero;
        }

        private void ReleaseAim()
        {
            _aimTouchId = -1;
            AimHeld = false;
            AimMagnitude = 0f;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        /// <summary>Editor-only dev controls: WASD move, hold mouse button to aim at cursor.</summary>
        private void ReadDesktopFallback()
        {
            var kb = Keyboard.current;
            var mouse = Mouse.current;
            if (kb != null)
            {
                Vector2 mv = Vector2.zero;
                if (kb.wKey.isPressed) mv.y += 1f;
                if (kb.sKey.isPressed) mv.y -= 1f;
                if (kb.aKey.isPressed) mv.x -= 1f;
                if (kb.dKey.isPressed) mv.x += 1f;
                if (_moveTouchId == -1) Move = mv.normalized;
            }
            if (mouse != null && _aimTouchId == -1)
            {
                AimHeld = mouse.leftButton.isPressed;
                if (AimHeld && Camera.main != null)
                {
                    Vector3 world = Camera.main.ScreenToWorldPoint(mouse.position.ReadValue());
                    Vector2 dir = (Vector2)world - (Vector2)Camera.main.transform.position;
                    // Aim relative to screen center ≈ player position (camera follows player).
                    if (dir.sqrMagnitude > 0.01f) AimDir = dir.normalized;
                    AimMagnitude = 1f;
                }
            }
        }
#endif
    }
}
