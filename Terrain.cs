using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace generate_terrain
{
    public class Terrain
    {
        uint[] m_tile = new uint[Options.MAP_TILE_SIZE * Options.MAP_TILE_SIZE];
        uint[] m_bitmap;
        int m_size;
        int m_deadzone;

        public Terrain(int size)
        {
            if (size != 1)
            {
                throw new NotImplementedException("Tiling is not implemented");
            }

            m_size = size * Options.MAP_TILE_SIZE;
            m_bitmap = new uint[size * m_tile.Length];
            m_deadzone = m_bitmap.Length / 8;

            Array.Fill<uint>(m_tile, Options.SEA_FLOOR);
        }

        public void DrawFaultLines(Region[] regions, int rate)
        {
            var center = new Vector2(Options.MAP_TILE_SIZE / 2);
            var fault_map = new Dictionary<Region, Faultline>();

            while (regions.Any(region => region.IsGrowing))
            {
                foreach (var region in regions)
                {
                    region.Grow(this);
                }
            }

            // TODO: reflect faultines into bitmap
            Array.Copy(m_tile, m_bitmap, m_tile.Length);
        }

        public bool Contains(Vector2 point)
        {
            return point.X > 0 && point.X < m_size && point.Y > 0 && point.Y < m_size;
        }

        public int Size => m_size;
        public int Deadzone => m_deadzone;
        public uint[] Bitmap => m_bitmap;
        public uint[] Tilemap => m_tile;
    }
}