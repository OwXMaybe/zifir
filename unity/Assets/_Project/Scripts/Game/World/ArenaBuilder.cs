using UnityEngine;
using Zifir.Core;

namespace Zifir.Gameplay
{
    /// <summary>
    /// Phase-1 test arena: bounded rectangle with deterministic obstacle
    /// blocks (seeded, so layouts are reproducible in bug reports). Replaced
    /// by the macro-planned procedural generator (roads/compounds/anomalies)
    /// in phase 2 — this class already owns the "build colliders + visuals
    /// from data" job that generator will reuse.
    /// </summary>
    public static class ArenaBuilder
    {
        private static readonly Color32 FloorColor = new(16, 18, 27, 255);
        private static readonly Color32 WallColor = new(42, 48, 66, 255);

        /// <summary>Builds the arena and returns its parent object.</summary>
        public static GameObject Build(int width, int height, int seed)
        {
            var root = new GameObject("Arena");
            var rng = new System.Random(seed);

            BuildFloor(root.transform, width, height);
            BuildBorder(root.transform, width, height);
            BuildObstacles(root.transform, width, height, rng);
            return root;
        }

        private static void BuildFloor(Transform parent, int w, int h)
        {
            var floor = new GameObject("Floor");
            floor.transform.SetParent(parent, false);
            var sr = floor.AddComponent<SpriteRenderer>();
            sr.sprite = ProceduralSprites.Square(FloorColor);
            sr.sortingOrder = -100;
            floor.transform.localScale = new Vector3(w, h, 1f);
            floor.transform.position = new Vector3(w * 0.5f, h * 0.5f, 0f);
        }

        private static void BuildBorder(Transform parent, int w, int h)
        {
            // Four wall slabs enclosing the play field.
            CreateWall(parent, new Vector2(w * 0.5f, -0.5f), new Vector2(w + 2f, 1f));
            CreateWall(parent, new Vector2(w * 0.5f, h + 0.5f), new Vector2(w + 2f, 1f));
            CreateWall(parent, new Vector2(-0.5f, h * 0.5f), new Vector2(1f, h + 2f));
            CreateWall(parent, new Vector2(w + 0.5f, h * 0.5f), new Vector2(1f, h + 2f));
        }

        private static void BuildObstacles(Transform parent, int w, int h, System.Random rng)
        {
            int count = w * h / 60;
            for (int i = 0; i < count; i++)
            {
                float bw = rng.Next(1, 4);
                float bh = rng.Next(1, 4);
                float x = (float)(rng.NextDouble() * (w - 6) + 3);
                float y = (float)(rng.NextDouble() * (h - 6) + 3);
                // Keep spawn center clear.
                if (Mathf.Abs(x - w * 0.5f) < 4f && Mathf.Abs(y - h * 0.5f) < 4f) continue;
                CreateWall(parent, new Vector2(x, y), new Vector2(bw, bh));
            }
        }

        private static void CreateWall(Transform parent, Vector2 center, Vector2 size)
        {
            var wall = new GameObject("Wall") { layer = LayerMask.NameToLayer("Wall") };
            wall.transform.SetParent(parent, false);
            wall.transform.position = center;

            var sr = wall.AddComponent<SpriteRenderer>();
            sr.sprite = ProceduralSprites.Square(WallColor);
            sr.sortingOrder = 10;
            wall.transform.localScale = new Vector3(size.x, size.y, 1f);

            var col = wall.AddComponent<BoxCollider2D>();
            col.size = Vector2.one; // scale carries the actual size
        }
    }
}
