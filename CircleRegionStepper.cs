using System.Numerics;

namespace generate_terrain
{
    public class CircleRegionStepper : IRegionStepper
    {
        public void Step(Terrain terrain, RegionData region)
        {
            Vector<int> point = new Vector<int>(new int[4] {
                (int)region.Radius,
                0,
                0,
                0
            });

            float re = 0f;
            while (point[1] <= point[0])
            {
                DrawSegments(terrain, region, point);

                // Setup the next move
                var next = point + Helpers.UP;
                re = RadiusError(next, region.Radius);

                var d = ((re + next[1]) * 2) + next[0] - 1;
                point = d > 0 ? next + Helpers.LEFT : next;
            }
        }

        private void DrawSegments(Terrain terrain, RegionData region, Vector<int> point)
        {
            terrain.DrawPoint(region.Center + point, region.Color);
            terrain.DrawPoint(region.Center + point * Helpers.FLIPY, region.Color);
            terrain.DrawPoint(region.Center + point * Helpers.FLIPX, region.Color);
            terrain.DrawPoint(region.Center + point * Helpers.FLIPX * Helpers.FLIPY, region.Color);

            var transpose = Helpers.Transpose(point);
            terrain.DrawPoint(region.Center + transpose, region.Color);
            terrain.DrawPoint(region.Center + transpose * Helpers.FLIPY, region.Color);
            terrain.DrawPoint(region.Center + transpose * Helpers.FLIPX, region.Color);
            terrain.DrawPoint(region.Center + transpose * Helpers.FLIPX * Helpers.FLIPY, region.Color);
        }

        private static float RadiusError(Vector<int> point, float r) =>
            point[0] * point[0] + point[1] * point[1] - r * r;
    }
}