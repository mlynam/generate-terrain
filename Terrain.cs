using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace generate_terrain
{
    public class Terrain
    {
        List<Region<IRegionStepper>> m_regions = new List<Region<IRegionStepper>>();
        uint[] m_tile = new uint[Helpers.MAP_TILE_SIZE * Helpers.MAP_TILE_SIZE];
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
            m_size = size * Helpers.MAP_TILE_SIZE;
            m_rng = new Random(seed.Pack());
            m_bitmap = new uint[size * m_tile.Length];
            m_deadzone = m_tile.Length / (seed.RegionCount + 1) / seed.RegionCount;

            int region_size = (m_tile.Length - m_deadzone * (seed.RegionCount + 1)) / seed.RegionCount;
            int offset = m_deadzone / 2;

            for (int i = 0; i < seed.RegionCount; i++)
            {
                var next = m_rng.Next(region_size);
                var region = new Region<IRegionStepper>(
                    bitmap_index: offset + next,
                    terrain: this,
                    stepper: new CircleRegionStepper()
                );

                m_tile[region.Index] = region.Color;
                offset += (region_size + m_deadzone);

                m_regions.Add(region);
            }

            Array.Fill<uint>(m_tile, Helpers.SEA_FLOOR);
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

            // TODO: paint edges black
            // TODO: paint regions as sea floor

            // TODO: reflect faultines into bitmap
            Array.Copy(m_tile, m_bitmap, m_tile.Length);
        }

        public void DrawPoint(Vector<int> point, uint color)
        {
            if (Contains(point))
            {
                var index = ConvertPositionToTileIndex(point);
                if (m_tile[index] == Helpers.SEA_FLOOR)
                {
                    m_tile[index] = color;
                }
            }
        }

        public Vector<int> ConvertTileIndexToPosition(int index)
        {
            var data = new int[4]
            {
                index % Helpers.MAP_TILE_SIZE,
                index / Helpers.MAP_TILE_SIZE,
                0,
                0,
            };

            return new Vector<int>(data);
        }

        public int ConvertPositionToTileIndex(Vector<int> position) => position[1] * Helpers.MAP_TILE_SIZE + position[0];

        public bool Contains(Vector<int> point) =>
            Vector.GreaterThanOrEqualAll(point, Helpers.TILE_TOP_LEFT) &&
            Vector.GreaterThanAll(Helpers.TILE_BOTTOM_RIGHT, point);

        public int Size => m_size;
        public int Deadzone => m_deadzone;
        public uint[] Bitmap => m_bitmap;
        public uint[] Tilemap => m_tile;
        public Seed Seed => m_seed;

        public override string ToString()
        {
            return "Terrain".PadRight(10) +
                $"Seed: {m_seed.Pack()}, " +
                $"Tilesize: {Helpers.MAP_TILE_SIZE}, " +
                $"Size: {m_seed.Size}, " +
                $"Growth Rate: {m_seed.RegionGrowthRate}\n" +
                string.Join('\n', m_regions);
        }

        public int RngNext(int max, int min = 0) => m_rng.Next(min, max);
    }
}