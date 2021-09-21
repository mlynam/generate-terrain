using System.Numerics;

namespace generate_terrain
{
    public static class Helpers
    {
        public static readonly Vector2 TILE_TOP_LEFT = new Vector2(0, 0);
        public static readonly Vector2 TILE_TOP_RIGHT = new Vector2(Terrain.MAP_TILE_SIZE, 0);
        public static readonly Vector2 TILE_BOTTOM_RIGHT = new Vector2(Terrain.MAP_TILE_SIZE, Terrain.MAP_TILE_SIZE);
        public static readonly Vector2 TILE_BOTTOM_LEFT = new Vector2(0, Terrain.MAP_TILE_SIZE);
        public static readonly Vector2 UP = new Vector2(0, 1);
        public static readonly Vector2 DOWN = new Vector2(0, -1);
        public static readonly Vector2 LEFT = new Vector2(-1, 0);
        public static readonly Vector2 RIGHT = new Vector2(1, 0);
        public static readonly Vector2 FLIPX = new Vector2(-1, 1);
        public static readonly Vector2 FLIPY = new Vector2(1, -1);

        public static Vector2 Transpose(Vector2 point) => new Vector2(point.Y, point.X);
    }
}