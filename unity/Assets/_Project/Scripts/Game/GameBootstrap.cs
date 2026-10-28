using UnityEngine;
using Zifir.Config;
using Zifir.Core;
using Zifir.Gameplay;
using Zifir.GameInput;
using Zifir.UI;

namespace Zifir
{
    /// <summary>
    /// Composition root. Builds the whole vertical slice from code at startup —
    /// no scene wiring or prefabs to break. Press Play in any scene and the
    /// game constructs itself: systems, arena, player, enemies, camera, HUD.
    /// </summary>
    public static class GameBootstrap
    {
        private const int ArenaWidth = 36;
        private const int ArenaHeight = 24;
        private const int ArenaSeed = 20940;
        private const int EnemyCount = 6;
        private const int BulletPrewarm = 48;

        private static bool _built;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Boot()
        {
            if (_built) return;
            _built = true;

            SaveSystem.Load();
            Loc.Init();
            Application.targetFrameRate = 60;

            var systems = new GameObject("Systems");
            Object.DontDestroyOnLoad(systems);
            systems.AddComponent<TouchInput>();
            systems.AddComponent<AppLifecycle>();

            ArenaBuilder.Build(ArenaWidth, ArenaHeight, ArenaSeed);

            GameObject player = BuildPlayer(new Vector2(ArenaWidth * 0.5f, ArenaHeight * 0.5f));
            SetupCamera(player.transform);
            SpawnEnemies(player.transform);

            var hudGo = new GameObject("HUD");
            var hud = hudGo.AddComponent<HUD>();
            hud.Bind(player.GetComponent<PlayerHealth>(),
                     player.GetComponent<PlayerController>(),
                     player.GetComponent<WeaponController>(),
                     player.GetComponent<PlayerAim>());
        }

        private static GameObject BuildPlayer(Vector2 position)
        {
            var player = new GameObject("Player") { layer = LayerMask.NameToLayer("Player") };
            player.transform.position = position;

            var sr = player.AddComponent<SpriteRenderer>();
            OperatorDef op = OperatorLibrary.CreateSentinel();
            sr.sprite = ProceduralSprites.Circle(new Color32(122, 176, 255, 255));
            sr.sortingOrder = 50;

            player.AddComponent<Rigidbody2D>();
            var col = player.AddComponent<CircleCollider2D>();
            col.radius = 0.45f;

            var controller = player.AddComponent<PlayerController>();
            controller.Configure(op);

            var health = player.AddComponent<PlayerHealth>();
            health.Configure(op.maxHealth);

            var bulletParent = new GameObject("Bullets").transform;
            var pool = new ObjectPool<Bullet>(() => Bullet.CreateInstance(bulletParent), BulletPrewarm);

            var weapon = player.AddComponent<WeaponController>();
            weapon.Configure(WeaponLibrary.CreateCarbine(), pool);

            player.AddComponent<PlayerAim>();
            return player;
        }

        private static void SetupCamera(Transform target)
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera") { tag = "MainCamera" };
                cam = camGo.AddComponent<Camera>();
            }
            cam.orthographic = true;
            cam.orthographicSize = 11.25f; // 360 px of the original at 16 PPU
            cam.backgroundColor = new Color(0.02f, 0.024f, 0.047f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.transform.position = new Vector3(target.position.x, target.position.y, -10f);

            var follow = cam.GetComponent<CameraFollow>();
            if (follow == null) follow = cam.gameObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
        }

        private static void SpawnEnemies(Transform playerTransform)
        {
            var rng = new System.Random(ArenaSeed + 1);
            for (int i = 0; i < EnemyCount; i++)
            {
                float x = (float)(rng.NextDouble() * (ArenaWidth - 8) + 4);
                float y = (float)(rng.NextDouble() * (ArenaHeight - 8) + 4);
                if (Vector2.Distance(new Vector2(x, y), playerTransform.position) < 6f) continue;

                var enemy = new GameObject("Enemy") { layer = LayerMask.NameToLayer("Enemy") };
                enemy.transform.position = new Vector2(x, y);

                var sr = enemy.AddComponent<SpriteRenderer>();
                sr.sprite = ProceduralSprites.Circle(new Color32(255, 90, 74, 255));
                sr.sortingOrder = 45;

                enemy.AddComponent<Rigidbody2D>();
                var col = enemy.AddComponent<CircleCollider2D>();
                col.radius = 0.42f;

                var health = enemy.AddComponent<EnemyHealth>();
                health.Configure(40f, 12);

                var ai = enemy.AddComponent<ChaserEnemy>();
                ai.SetTarget(playerTransform);
            }
        }
    }

    /// <summary>
    /// Google Play background behavior: auto-save on pause/focus loss so no
    /// progress is ever lost when Android suspends the app.
    /// </summary>
    public sealed class AppLifecycle : MonoBehaviour
    {
        private void OnApplicationPause(bool paused)
        {
            if (paused) SaveSystem.Save();
        }

        private void OnApplicationQuit()
        {
            SaveSystem.Save();
        }
    }
}
