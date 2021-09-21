using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace generate_terrain
{
    public class Terrain
    {
        public const int MAP_TILE_SIZE = 512;
        public const uint SEA_FLOOR = 0x0A1435FF;

        List<Region<IRegionStepper>> m_regions = new List<Region<IRegionStepper>>();
        uint[] m_tile = new uint[MAP_TILE_SIZE * MAP_TILE_SIZE];
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
            m_size = size * MAP_TILE_SIZE;
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

            Array.Fill<uint>(m_tile, SEA_FLOOR);
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

            for (int i = 0; i < m_tile.Length; i++)
            {
                if (m_tile[i] == SEA_FLOOR)
                {
                    m_tile[i] = 0xFF;
                }
                else
                {
                    m_tile[i] = SEA_FLOOR;
                }
            }

            Array.Copy(m_tile, m_bitmap, m_tile.Length);
        }

        public void DrawRegionPoint(Vector2 point, uint color)
        {
            var i = ConvertPositionToTileIndex(point);
            if (Contains(point) && m_tile[i] == SEA_FLOOR)
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
                    var j = ConvertPositionToTileIndex(position);
                    if (Contains(position) && m_tile[j] != SEA_FLOOR && m_tile[j] != color)
                    {
                        // Ineligible point
                        return;
                    }
                }

                if (m_tile[i] == SEA_FLOOR)
                {
                    m_tile[i] = color;
                }
            }
        }

        public Vector2 ConvertTileIndexToPosition(int index) => new Vector2(
            x: index % MAP_TILE_SIZE,
            y: index / MAP_TILE_SIZE
        );

        public int ConvertPositionToTileIndex(Vector2 position) =>
            (int)position.Y * MAP_TILE_SIZE +
            (int)position.X;

        public bool Contains(Vector2 point) =>
            point.X >= Helpers.TILE_TOP_LEFT.X &&
            point.Y >= Helpers.TILE_TOP_LEFT.Y &&
            point.X < Helpers.TILE_BOTTOM_RIGHT.X &&
            point.Y < Helpers.TILE_BOTTOM_RIGHT.Y;

        private void PaintFaults(int stride, ArraySegment<uint> segment)
        {
            if (stride == 2)
            {
                for (int i = 0; i < segment.Count; i += stride)
                {
                    if (segment[i] != segment[i + 1])
                    {
                        segment[i] = 0x000000FF;
                    }
                }

                return;
            }

            for (int i = 0; i < segment.Count - stride; i += stride)
            {
                var next = segment[i..(i + stride)];
                if (next[0] != next[^1])
                {
                    PaintFaults(stride / 2, next);
                }
            }
        }

        public int Size => m_size;
        public int Deadzone => m_deadzone;
        public uint[] Bitmap => m_bitmap;
        public uint[] Tilemap => m_tile;
        public Seed Seed => m_seed;

        public override string ToString()
        {
            return "Terrain".PadRight(10) +
                $"Seed: {m_seed.Pack()}, " +
                $"Tilesize: {MAP_TILE_SIZE}, " +
                $"Size: {m_seed.Size}, " +
                $"Growth Rate: {m_seed.RegionGrowthRate}\n" +
                string.Join('\n', m_regions);
        }

        public int RngNext(int max, int min = 0) => m_rng.Next(min, max);
    }
}