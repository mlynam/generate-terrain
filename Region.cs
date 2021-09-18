using System;
using System.Numerics;

namespace generate_terrain
{
    public partial class Region
    {
        int m_radius = 1;
        Vector<int> m_center;
        int m_index;
        int m_max_rsquared;
        uint m_color;
        Terrain m_terrain;

        public Region(int bitmap_index, Terrain terrain)
        {
            m_index = bitmap_index;
            m_terrain = terrain;
            m_color = (uint)terrain.RngNext(0xFFFFFF) | (uint)(terrain.RngNext(0xFFFFFF) << 16) | 0x000000FF;
            m_center = ConvertIndexToPosition(m_index);

            m_max_rsquared = RSquared(m_center, Options.TILE_TOP_LEFT);
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Options.TILE_TOP_RIGHT));
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Options.TILE_BOTTOM_RIGHT));
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Options.TILE_BOTTOM_LEFT));
        }

        public int Index => m_index;
        public Vector<int> Center => m_center;
        public float Radius => m_radius;
        public bool IsGrowing => (m_radius * m_radius) < m_max_rsquared;
        public uint Color => m_color;

        public void Grow()
        {
            var target = m_radius + 1 + m_terrain.RngNext(m_terrain.Seed.RegionGrowthRate);
            for (; m_radius < target; ++m_radius)
            {
                Midpoint();
            }
        }

        public override string ToString()
        {
            return "Region".PadRight(10) +
                $"Center: {m_center}, " +
                $"Index: {m_index}, " +
                $"Max R-Squared: {m_max_rsquared}";
        }

        private void DrawPoint(Vector<int> point)
        {
            var target = m_center + point;
            if (m_terrain.Contains(target))
            {
                var index = ConvertPositionToIndex(target);
                if (m_terrain.Tilemap[index] == Options.SEA_FLOOR)
                {
                    m_terrain.Tilemap[index] = m_color;
                }
            }
        }

        private Vector<int> ConvertIndexToPosition(int index)
        {
            var data = new int[4]
            {
                index % Options.MAP_TILE_SIZE,
                index / Options.MAP_TILE_SIZE,
                0,
                0,
            };

            return new Vector<int>(data);
        }

        private int ConvertPositionToIndex(Vector<int> position) => position[1] * Options.MAP_TILE_SIZE + position[0];

        private static int RSquared(Vector<int> left, Vector<int> right)
        {
            var r = Vector.Subtract(left, right);
            var x = r[0];
            var y = r[1];
            return x * x + y * y;
        }
    }
}