using System.Numerics;

namespace generate_terrain
{
    public static class Options
    {
        public const int MAP_TILE_SIZE = 256;
        public const uint SEA_FLOOR = 0x0A1435FF;
        public static readonly Vector<int> TILE_TOP_LEFT = new Vector<int>(new int[4] { 0, 0, 0, 0 });
        public static readonly Vector<int> TILE_TOP_RIGHT = new Vector<int>(new int[4] { MAP_TILE_SIZE, 0, 0, 0 });
        public static readonly Vector<int> TILE_BOTTOM_RIGHT = new Vector<int>(new int[4] { MAP_TILE_SIZE, MAP_TILE_SIZE, 1, 1 });
        public static readonly Vector<int> TILE_BOTTOM_LEFT = new Vector<int>(new int[4] { 0, MAP_TILE_SIZE, 0, 0 });
    }
}