using System;
using System.Collections.Generic;

namespace generate_terrain
{
    public partial class Region
    {
        HashSet<int> m_edge;

        private void InitializeNeighbor(int bitmap_index)
        {
            m_edge = new HashSet<int>
            {
                bitmap_index
            };
        }

        private void PaintEdges(Terrain terrain)
        {
            foreach (var index in m_edge)
            {
                terrain.Tilemap[index] = m_color;
            }
        }

        private int NeighborPixelTop(int from) => (from - Options.MAP_TILE_SIZE).StayPositive(from);
        private int NeighborPixelRight(int from, int max) => (from + 1).IntClamp(from, max);
        private int NeighborPixelBottom(int from, int max) => (from + Options.MAP_TILE_SIZE).IntClamp(from, max);
        private int NeighborPixelLeft(int from) => (from - 1).StayPositive(from);
        private bool IsNeighborRegion(int index, Terrain terrain) =>
            terrain.Tilemap[index] != Options.SEA_FLOOR && terrain.Tilemap[index] != m_color;

        private bool IsFaultLine(int index, Terrain terrain)
        {
            var top = NeighborPixelTop(index);
            if (IsNeighborRegion(top, terrain))
            {
                return true;
            }

            var right = NeighborPixelRight(index, terrain.Tilemap.Length - 1);
            if (IsNeighborRegion(right, terrain))
            {
                return true;
            }

            var bottom = NeighborPixelBottom(index, terrain.Tilemap.Length - 1);
            if (IsNeighborRegion(bottom, terrain))
            {
                return true;
            }

            var left = NeighborPixelLeft(index);
            if (IsNeighborRegion(left, terrain))
            {
                return true;
            }

            return false;
        }

        public void Neighbor(int rate = -1)
        {
            if (rate < 0)
            {   // Default invocation begins recursion
                Neighbor(m_rate);
            }
            else if (rate == 0)
            {   // When we exhaust the rate, we unwind
                return;
            }
            else
            {   // Expand the edge, paint and recurse
                var next = new HashSet<int>();
                foreach (var index in m_edge)
                {
                    // Top
                    var top = NeighborPixelTop(index);
                    if (index > 0 && !IsFaultLine(top, m_terrain) && m_terrain.Tilemap[top] != m_color)
                    {
                        next.Add(top);
                    }

                    // Right
                    var right = NeighborPixelRight(index, m_terrain.Tilemap.Length - 1);
                    if (right % Options.MAP_TILE_SIZE != 0 && !IsFaultLine(right, m_terrain) && m_terrain.Tilemap[right] != m_color)
                    {
                        next.Add(right);
                    }

                    // Bottom
                    var bottom = NeighborPixelBottom(index, m_terrain.Tilemap.Length - 1);
                    if (bottom < m_terrain.Tilemap.Length && !IsFaultLine(bottom, m_terrain) && m_terrain.Tilemap[bottom] != m_color)
                    {
                        next.Add(bottom);
                    }

                    // Left
                    var left = NeighborPixelLeft(index);
                    if (left % (Options.MAP_TILE_SIZE - 1) != 0 && !IsFaultLine(left, m_terrain) && m_terrain.Tilemap[left] != m_color)
                    {
                        next.Add(left);
                    }
                }

                next.ExceptWith(m_edge);

                foreach (var index in next)
                {
                    m_terrain.Tilemap[index] = m_color;
                }

                m_edge = next;
                Neighbor(rate - 1);
            }
        }
    }
}