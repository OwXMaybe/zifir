using UnityEngine;
using UnityEngine.UI;
using Zifir.Core;
using Zifir.Gameplay;
using Zifir.GameInput;

namespace Zifir.UI
{
    /// <summary>
    /// Code-built HUD: health/stamina bars, ammo readout, and the two virtual
    /// joystick visuals (glassy base ring + knob) that track TouchInput state.
    /// Canvas scales with screen size and respects the device safe area.
    /// </summary>
    public sealed class HUD : MonoBehaviour
    {
        private PlayerHealth _playerHealth;
        private PlayerController _playerController;
        private WeaponController _weapon;

        private RectTransform _safeRoot;
        private Image _healthFill;
        private Image _staminaFill;
        private Text _ammoText;

        private RectTransform _moveBase, _moveKnob, _aimBase, _aimKnob;
        private Image _aimBaseImage, _aimKnobImage;
        private PlayerAim _aim;

        private static readonly Color Glass = new(0.55f, 0.7f, 1f, 0.22f);
        private static readonly Color GlassKnob = new(0.55f, 0.7f, 1f, 0.4f);
        private static readonly Color AimGlass = new(0.43f, 0.88f, 1f, 0.22f);
        private static readonly Color AimLocked = new(1f, 0.45f, 0.35f, 0.45f);

        public void Bind(PlayerHealth health, PlayerController controller, WeaponController weapon, PlayerAim aim)
        {
            _playerHealth = health;
            _playerController = controller;
            _weapon = weapon;
            _aim = aim;
        }

        private void Awake()
        {
            BuildCanvas();
        }

        private void BuildCanvas()
        {
            var canvasGo = new GameObject("Canvas");
            canvasGo.transform.SetParent(transform, false);
            var canvas = canvasGo.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            scaler.matchWidthOrHeight = 0.5f;
            canvasGo.AddComponent<GraphicRaycaster>();

            // Safe-area container: everything anchors inside the notch-free zone.
            _safeRoot = new GameObject("SafeArea").AddComponent<RectTransform>();
            _safeRoot.SetParent(canvasGo.transform, false);
            ApplySafeArea();

            BuildBars();
            BuildAmmo();
            _moveBase = BuildJoystick("MoveJoystick", Glass, GlassKnob, out _moveKnob, out _, out _);
            _aimBase = BuildJoystick("AimJoystick", AimGlass, GlassKnob, out _aimKnob, out _aimBaseImage, out _aimKnobImage);
        }

        private void ApplySafeArea()
        {
            Rect safe = Screen.safeArea;
            _safeRoot.anchorMin = new Vector2(safe.xMin / Screen.width, safe.yMin / Screen.height);
            _safeRoot.anchorMax = new Vector2(safe.xMax / Screen.width, safe.yMax / Screen.height);
            _safeRoot.offsetMin = Vector2.zero;
            _safeRoot.offsetMax = Vector2.zero;
        }

        private void BuildBars()
        {
            _healthFill = BuildBar(new Vector2(20, -20), new Color(0.29f, 0.87f, 0.5f));
            _staminaFill = BuildBar(new Vector2(20, -42), new Color(0.43f, 0.9f, 1f));
        }

        private Image BuildBar(Vector2 anchoredPos, Color fillColor)
        {
            var bg = NewRect("BarBg", _safeRoot);
            var bgImage = bg.gameObject.AddComponent<Image>();
            bgImage.color = new Color(0.05f, 0.07f, 0.12f, 0.6f);
            bg.anchorMin = bg.anchorMax = new Vector2(0f, 1f);
            bg.pivot = new Vector2(0f, 1f);
            bg.anchoredPosition = anchoredPos;
            bg.sizeDelta = new Vector2(240, 16);

            var fill = NewRect("Fill", bg);
            var fillImage = fill.gameObject.AddComponent<Image>();
            fillImage.color = fillColor;
            fill.anchorMin = Vector2.zero;
            fill.anchorMax = Vector2.one;
            fill.offsetMin = new Vector2(2, 2);
            fill.offsetMax = new Vector2(-2, -2);
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            return fillImage;
        }

        private void BuildAmmo()
        {
            var rect = NewRect("Ammo", _safeRoot);
            _ammoText = rect.gameObject.AddComponent<Text>();
            _ammoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            _ammoText.fontSize = 30;
            _ammoText.alignment = TextAnchor.LowerRight;
            _ammoText.color = new Color(0.91f, 0.93f, 0.97f, 0.95f);
            rect.anchorMin = rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(1f, 0f);
            rect.anchoredPosition = new Vector2(-24, 24);
            rect.sizeDelta = new Vector2(300, 44);
        }

        private RectTransform BuildJoystick(string name, Color baseColor, Color knobColor,
            out RectTransform knob, out Image baseImage, out Image knobImage)
        {
            var baseRect = NewRect(name, _safeRoot);
            baseImage = baseRect.gameObject.AddComponent<Image>();
            baseImage.color = baseColor;
            baseImage.sprite = UISprites.Circle;
            baseRect.sizeDelta = new Vector2(170, 170);
            baseRect.gameObject.SetActive(false);

            knob = NewRect("Knob", baseRect);
            knobImage = knob.gameObject.AddComponent<Image>();
            knobImage.color = knobColor;
            knobImage.sprite = UISprites.Circle;
            knob.sizeDelta = new Vector2(70, 70);
            return baseRect;
        }

        private static RectTransform NewRect(string name, Transform parent)
        {
            var rect = new GameObject(name).AddComponent<RectTransform>();
            rect.SetParent(parent, false);
            return rect;
        }

        private void Update()
        {
            if (_playerHealth != null)
                _healthFill.fillAmount = _playerHealth.Current / _playerHealth.Max;
            if (_playerController != null)
                _staminaFill.fillAmount = _playerController.Stamina / _playerController.StaminaMax;
            if (_weapon != null && _weapon.Weapon != null)
                _ammoText.text = _weapon.Reloading
                    ? Loc.T("hud.reloading")
                    : $"{_weapon.MagazineCurrent} / {_weapon.Weapon.magazineSize}";

            UpdateJoystickVisuals();
        }

        private void UpdateJoystickVisuals()
        {
            TouchInput input = TouchInput.Instance;
            if (input == null) return;

            bool moveOn = input.MoveActive && input.Move.sqrMagnitude > 0.0001f;
            _moveBase.gameObject.SetActive(moveOn);
            if (moveOn)
            {
                PlaceOnScreen(_moveBase, input.MoveOriginScreen);
                _moveKnob.anchoredPosition = input.Move * 50f;
            }

            _aimBase.gameObject.SetActive(input.AimHeld);
            if (input.AimHeld)
            {
                PlaceOnScreen(_aimBase, input.AimOriginScreen);
                _aimKnob.anchoredPosition = input.AimDir * input.AimMagnitude * 50f;
                bool locked = _aim != null && _aim.LockTarget != null;
                _aimBaseImage.color = locked ? AimLocked : AimGlass;
            }
        }

        private void PlaceOnScreen(RectTransform rect, Vector2 screenPos)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _safeRoot, screenPos, null, out Vector2 local);
            rect.anchoredPosition = local;
        }
    }

    /// <summary>Runtime-generated smooth circle sprite for UI (cached).</summary>
    internal static class UISprites
    {
        private static Sprite _circle;

        public static Sprite Circle
        {
            get
            {
                if (_circle != null) return _circle;
                const int size = 64;
                var tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
                var px = new Color32[size * size];
                float r = size * 0.5f - 1f, c = (size - 1) * 0.5f;
                for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float dx = x - c, dy = y - c;
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    byte a = d <= r - 1f ? (byte)255 : d <= r ? (byte)Mathf.RoundToInt((r - d) * 255f) : (byte)0;
                    px[y * size + x] = new Color32(255, 255, 255, a);
                }
                tex.SetPixels32(px);
                tex.Apply(false, true);
                _circle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
                return _circle;
            }
        }
    }
}
