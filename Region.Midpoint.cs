using System.Numerics;

namespace generate_terrain
{
    public partial class Region
    {
        public void Midpoint()
        {
            Vector<int> point = new Vector<int>(new int[4] {
                m_radius,
                0,
                0,
                0
            });

            int re = 0;
            while (point[1] <= point[0])
            {
                DrawSegments(point);

                // Setup the next move
                var next = point + UP;
                re = RadiusError(next, m_radius);

                var d = ((re + next[1]) << 1) + next[0] - 1;
                point = d > 0 ? next + LEFT : next;
            }
        }

        private void DrawSegments(Vector<int> point)
        {
            DrawPoint(point);
            DrawPoint(point * FLIPY);
            DrawPoint(point * FLIPX);
            DrawPoint(point * FLIPX * FLIPY);

            var transpose = Transpose(point);
            DrawPoint(transpose);
            DrawPoint(transpose * FLIPY);
            DrawPoint(transpose * FLIPX);
            DrawPoint(transpose * FLIPX * FLIPY);
        }

        private static int RadiusError(Vector<int> point, int r) =>
            point[0] * point[0] + point[1] * point[1] - r * r;

        private static Vector<int> Transpose(Vector<int> point) => new Vector<int>(new int[4] {
            point[1],
            point[0],
            0,
            0,
        });

        private static Vector<int> UP => new Vector<int>(new int[4] { 0, 1, 0, 0 });
        private static Vector<int> DOWN => new Vector<int>(new int[4] { 0, -1, 0, 0 });
        private static Vector<int> LEFT => new Vector<int>(new int[4] { -1, 0, 0, 0 });
        private static Vector<int> RIGHT => new Vector<int>(new int[4] { 1, 0, 0, 0 });
        private static Vector<int> FLIPX => new Vector<int>(new int[4] { -1, 1, 0, 0 });
        private static Vector<int> FLIPY => new Vector<int>(new int[4] { 1, -1, 0, 0 });
    }
}