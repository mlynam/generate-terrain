using System;
using System.Numerics;

namespace generate_terrain
{
    public class Region
    {
        float m_radius = 0;
        Vector2 m_center;
        int m_index;
        int m_rate;
        float m_max_radius;
        string m_name;
        uint m_color;

        public Region(int bitmap_index, uint color, int rate, string name = null)
        {
            m_color = color;
            m_index = bitmap_index;
            m_name = name ?? "unknown";
            m_rate = rate;
            m_center = new Vector2(
                x: m_index % Options.MAP_TILE_SIZE,
                y: m_index / Options.MAP_TILE_SIZE
            );

            m_max_radius = Vector2.Distance(m_center, TOP_LEFT);
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, TOP_RIGHT));
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, BOTTOM_RIGHT));
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, BOTTOM_LEFT));
        }

        public float MaxRadius => m_max_radius;
        public int Index => m_index;
        public Vector2 Center => m_center;
        public float Radius => m_radius;
        public bool IsGrowing => m_radius < m_max_radius;

        public void Grow(Terrain terrain)
        {
            m_radius += m_rate;

            var circle = MathF.PI * 2;
            var sweep = MathF.PI / Options.MAP_TILE_SIZE / 2;
            var angle = 0f;

            while (angle < circle)
            {
                var x = MathF.Cos(angle);
                var y = MathF.Sin(angle);
                var to = m_center + new Vector2(x, y) * m_radius;

                var r = to - m_center;
                var steps = (int)MathF.Ceiling(r.Length());

                for (int i = 0; i < steps; ++i)
                {
                    var pixel = m_center + Vector2.Normalize(r) * i;
                    if (terrain.Contains(pixel))
                    {
                        var index = (int)pixel.Y * Options.MAP_TILE_SIZE + (int)pixel.X;
                        if (terrain.Tilemap[index] == Options.SEA_FLOOR)
                        {
                            terrain.Tilemap[index] = m_color;
                        }
                    }
                }

                angle += sweep;
            }
        }

        public override string ToString()
        {
            return m_name.Substring(0, 8).PadRight(10) +
                $"Center: {m_center}, " +
                $"Radius: {m_radius}, " +
                $"Index: {m_index}, " +
                $"Color: {m_color.ToString("x")}, " +
                $"Max Radius: {m_max_radius}";
        }

        private static Vector2 TOP_LEFT = new Vector2(0, 0);
        private static Vector2 TOP_RIGHT = new Vector2(Options.MAP_TILE_SIZE, 0);
        private static Vector2 BOTTOM_RIGHT = new Vector2(Options.MAP_TILE_SIZE);
        private static Vector2 BOTTOM_LEFT = new Vector2(0, Options.MAP_TILE_SIZE);
    }
}