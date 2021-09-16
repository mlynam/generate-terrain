using System;
using System.Numerics;

namespace generate_terrain
{
    public partial class Region
    {
        public void Sweep()
        {
            var circle = MathF.PI * 2;
            var sweep = MathF.PI / Options.MAP_TILE_SIZE * m_radius;
            var angle = 0f;

            while (angle < circle)
            {
                var x = MathF.Cos(angle);
                var y = MathF.Sin(angle);
                var to = m_center + new Vector2(x, y) * m_radius;

                var r = to - m_center;
                var steps = (int)MathF.Ceiling(r.Length());

                for (int i = 0; i < steps; ++i)
                {
                    var pixel = m_center + Vector2.Normalize(r) * i;
                    if (m_terrain.Contains(pixel))
                    {
                        var index = ConvertPositionToIndex(pixel);
                        if (m_terrain.Tilemap[index] == Options.SEA_FLOOR)
                        {
                            m_terrain.Tilemap[index] = m_color;
                        }
                    }
                }

                angle += sweep;
            }
        }
    }
}