using System;
using System.Numerics;

namespace generate_terrain
{
    public class Faultline
    {
        Vector2 m_position;

        public Faultline(Vector2 start)
        {
            m_position = start;
        }

        public void DrawTo(Vector2 to, Terrain terrain, uint color)
        {
            var r = to - m_position;
            var steps = (int)MathF.Ceiling(r.Length());

            for (int i = 0; i < steps; ++i)
            {
                var pixel = m_position + Vector2.Normalize(r) * i;
                if (terrain.Contains(pixel))
                {
                    var index = (int)pixel.Y * Options.MAP_TILE_SIZE + (int)pixel.X;
                    terrain.Tilemap[index] = color;
                }
            }
        }

        public Vector2 Position => m_position;
    }
}