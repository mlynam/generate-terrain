using System;
using System.Numerics;

namespace generate_terrain
{
    public class Plate<T> : IPlate where T : IPlateStepper
    {
        float m_radius = 0;
        Vector2 m_center;
        int m_index;
        float m_max_rsquared;
        uint m_color;
        Terrain m_terrain;
        IPlateStepper m_stepper;
        int m_density;

        public Plate(int bitmap_index, Terrain terrain, T stepper)
        {
            m_index = bitmap_index;
            m_density = terrain.RngNext(255, 1);
            m_terrain = terrain;
            m_color = Helpers.AddBlue(Terrain.SEA_FLOOR, m_density);
            m_center = m_terrain.ConvertTileIndexToPosition(m_index);
            m_stepper = stepper;

            m_max_rsquared = RSquared(m_center, Helpers.TILE_TOP_LEFT);
            m_max_rsquared = MathF.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_TOP_RIGHT));
            m_max_rsquared = MathF.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_BOTTOM_RIGHT));
            m_max_rsquared = MathF.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_BOTTOM_LEFT));
        }

        public int Index => m_index;
        public Vector2 Center => m_center;
        public float Radius => m_radius;
        public bool IsGrowing => (m_radius * m_radius) < m_max_rsquared;
        public uint Color => m_color;
        public int DensityFactor => m_density;

        public void Grow()
        {
            for (var target = m_radius + m_terrain.RngNext(m_terrain.Seed.RegionGrowthRate, 1); m_radius < target; m_radius += .5f)
            {
                m_stepper.Step(this);
            }
        }

        public void DrawRegionPoint(Vector2 point)
        {
            var i = m_terrain.ConvertPositionToTileIndex(point);
            if (m_terrain.Contains(point) && m_terrain.Tilemap[i] == Terrain.SEA_FLOOR)
            {
                var adjacent = new Vector2[]
                {
                    point + Helpers.UP,
                    point + Helpers.DOWN,
                    point + Helpers.LEFT,
                    point + Helpers.RIGHT,
                };

                foreach (var position in adjacent)
                {
                    var j = m_terrain.ConvertPositionToTileIndex(position);
                    if (m_terrain.Contains(position) && m_terrain.Tilemap[j] != Terrain.SEA_FLOOR && m_terrain.Tilemap[j] != m_color)
                    {
                        // Ineligible point
                        return;
                    }
                }

                if (m_terrain.Tilemap[i] == Terrain.SEA_FLOOR)
                {
                    m_terrain.Tilemap[i] = m_color;
                }
            }
        }

        public override string ToString()
        {
            return "Region".PadRight(10) +
                $"Center: {m_center}, " +
                $"Index: {m_index}, " +
                $"Max R-Squared: {m_max_rsquared}";
        }

        private static float RSquared(Vector2 left, Vector2 right)
        {
            var r = left - right;
            var x = r.X;
            var y = r.Y;
            return x * x + y * y;
        }
    }
}