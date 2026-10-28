using System.Collections.Generic;
using UnityEngine;

namespace Zifir.Core
{
    /// <summary>
    /// Runtime-generated placeholder-free primitive sprites (solid squares and
    /// circles) matching the game's pixel aesthetic. Cached by color so each
    /// unique sprite is created exactly once (texture memory friendly).
    /// </summary>
    public static class ProceduralSprites
    {
        /// <summary>Pixels per world unit. 1 unit = 1 tile = 16 px, mirroring the original.</summary>
        public const float PPU = 16f;

        private static readonly Dictionary<Color32, Sprite> SquareCache = new();
        private static readonly Dictionary<Color32, Sprite> CircleCache = new();

        /// <summary>1×1 world unit solid square.</summary>
        public static Sprite Square(Color32 color)
        {
            if (SquareCache.TryGetValue(color, out Sprite s)) return s;
            var tex = new Texture2D(16, 16, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };
            var px = new Color32[16 * 16];
            for (int i = 0; i < px.Length; i++) px[i] = color;
            tex.SetPixels32(px);
            tex.Apply(false, true);
            s = Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), PPU);
            SquareCache[color] = s;
            return s;
        }

        /// <summary>1×1 world unit filled circle.</summary>
        public static Sprite Circle(Color32 color)
        {
            if (CircleCache.TryGetValue(color, out Sprite s)) return s;
            const int size = 16;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point
            };
            var px = new Color32[size * size];
            float r = size * 0.5f - 0.5f, cx = (size - 1) * 0.5f, cy = (size - 1) * 0.5f;
            for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float dx = x - cx, dy = y - cy;
                px[y * size + x] = dx * dx + dy * dy <= r * r ? color : new Color32(0, 0, 0, 0);
            }
            tex.SetPixels32(px);
            tex.Apply(false, true);
            s = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), PPU);
            CircleCache[color] = s;
            return s;
        }
    }
}
