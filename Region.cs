using System;
using System.Numerics;

namespace generate_terrain
{
    public partial class Region
    {
        float m_radius = 0;
        Vector2 m_center;
        int m_index;
        int m_rate;
        float m_max_radius;
        string m_name;
        uint m_color;
        Terrain m_terrain;

        public Region(int bitmap_index, uint color, int rate, Terrain terrain, string name = null)
        {
            m_terrain = terrain;
            m_color = color;
            m_index = bitmap_index;
            m_name = name ?? "unknown";
            m_rate = rate;
            m_center = ConvertIndexToPosition(m_index);
            m_max_radius = Vector2.Distance(m_center, TOP_LEFT);
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, TOP_RIGHT));
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, BOTTOM_RIGHT));
            m_max_radius = MathF.Max(m_max_radius, Vector2.Distance(m_center, BOTTOM_LEFT));

            InitializeNeighbor(bitmap_index);
        }

        public float MaxRadius => m_max_radius;
        public int Index => m_index;
        public Vector2 Center => m_center;
        public float Radius => m_radius;
        public bool IsGrowing => m_radius < m_max_radius;
        public uint Color => m_color;

        public void Grow()
        {
            m_radius += m_rate;

            Neighbor();
            // Sweep();
        }

        public override string ToString()
        {
            return m_name.Substring(0, 8).PadRight(10) +
                $"Center: {m_center}, " +
                $"Radius: {m_radius}, " +
                $"Index: {m_index}, " +
                $"Color: {m_color.ToString("x").PadLeft(8, '0')}, " +
                $"Max Radius: {m_max_radius}";
        }

        private Vector2 ConvertIndexToPosition(int index) => new Vector2(
            x: index % Options.MAP_TILE_SIZE,
            y: index / Options.MAP_TILE_SIZE
        );

        private int ConvertPositionToIndex(Vector2 position) => (int)position.Y * Options.MAP_TILE_SIZE + (int)position.X;

        private static Vector2 TOP_LEFT = new Vector2(0, 0);
        private static Vector2 TOP_RIGHT = new Vector2(Options.MAP_TILE_SIZE, 0);
        private static Vector2 BOTTOM_RIGHT = new Vector2(Options.MAP_TILE_SIZE);
        private static Vector2 BOTTOM_LEFT = new Vector2(0, Options.MAP_TILE_SIZE);
    }
}