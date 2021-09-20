using System;
using System.Numerics;

namespace generate_terrain
{
    public class Region<T> : RegionData where T : IRegionStepper
    {
        Terrain m_terrain;
        IRegionStepper m_stepper;

        public Region(int bitmap_index, Terrain terrain, T stepper)
        {
            m_index = bitmap_index;
            m_terrain = terrain;
            m_color = (uint)terrain.RngNext(0xFFFFFF) | (uint)(terrain.RngNext(0xFFFFFF) << 16) | 0x000000FF;
            m_center = m_terrain.ConvertTileIndexToPosition(m_index);
            m_stepper = stepper;

            m_max_rsquared = RSquared(m_center, Helpers.TILE_TOP_LEFT);
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_TOP_RIGHT));
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_BOTTOM_RIGHT));
            m_max_rsquared = Math.Max(m_max_rsquared, RSquared(m_center, Helpers.TILE_BOTTOM_LEFT));
        }

        public void Grow()
        {
            for (var target = m_radius + m_terrain.RngNext(m_terrain.Seed.RegionGrowthRate, 1); m_radius < target; m_radius += .5f)
            {
                m_stepper.Step(m_terrain, this);
            }
        }

        public override string ToString()
        {
            return "Region".PadRight(10) +
                $"Center: {m_center}, " +
                $"Index: {m_index}, " +
                $"Max R-Squared: {m_max_rsquared}";
        }

        private static int RSquared(Vector<int> left, Vector<int> right)
        {
            var r = Vector.Subtract(left, right);
            var x = r[0];
            var y = r[1];
            return x * x + y * y;
        }
    }
}