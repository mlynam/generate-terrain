using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace generate_terrain
{
    public class Terrain
    {
        List<Region> m_regions = new List<Region>();
        uint[] m_tile = new uint[Options.MAP_TILE_SIZE * Options.MAP_TILE_SIZE];
        uint[] m_bitmap;
        int m_size;
        int m_deadzone;
        Random m_rng;
        Seed m_seed;

        public Terrain(Seed seed)
        {
            var size = seed.Size;
            if (size != 1)
            {
                throw new NotImplementedException("Tiling is not implemented");
            }

            m_seed = seed;
            m_size = size * Options.MAP_TILE_SIZE;
            m_rng = new Random(seed.Pack());
            m_bitmap = new uint[size * m_tile.Length];
            m_deadzone = m_tile.Length / (seed.RegionCount + 1) / seed.RegionCount;

            int region_size = (m_tile.Length - m_deadzone * (seed.RegionCount + 1)) / seed.RegionCount;
            int offset = m_deadzone / 2;

            for (int i = 0; i < seed.RegionCount; i++)
            {
                var next = m_rng.Next(region_size);
                var region = new Region(
                    bitmap_index: offset + next,
                    terrain: this
                );

                m_tile[region.Index] = region.Color;
                offset += (region_size + m_deadzone);

                m_regions.Add(region);
            }

            Array.Fill<uint>(m_tile, Options.SEA_FLOOR);
        }

        public void DrawFaultLines()
        {
            while (m_regions.Any(region => region.IsGrowing))
            {
                foreach (var region in m_regions.Where(region => region.IsGrowing))
                {
                    region.Grow();
                }
            }

            // TODO: reflect faultines into bitmap
            Array.Copy(m_tile, m_bitmap, m_tile.Length);
        }

        public bool Contains(Vector<int> point) =>
            Vector.GreaterThanOrEqualAll(point, Options.TILE_TOP_LEFT) &&
            Vector.GreaterThanAll(Options.TILE_BOTTOM_RIGHT, point);

        public int Size => m_size;
        public int Deadzone => m_deadzone;
        public uint[] Bitmap => m_bitmap;
        public uint[] Tilemap => m_tile;
        public Seed Seed => m_seed;

        public override string ToString()
        {
            return "Terrain".PadRight(10) +
                $"Seed: {m_seed.Pack()}, " +
                $"Tilesize: {Options.MAP_TILE_SIZE}, " +
                $"Size: {m_seed.Size}, " +
                $"Growth Rate: {m_seed.RegionGrowthRate}\n" +
                string.Join('\n', m_regions);
        }

        public int RngNext(int max) => m_rng.Next(max);
    }
}