using System;
using System.Numerics;

namespace generate_terrain
{
    public class CircleRegionStepper : IRegionStepper
    {
        public void Step(Terrain terrain, RegionData region)
        {
            Vector2 point = new Vector2((int)region.Radius, 0);

            float re = 0f;
            while (point.Y <= point.X)
            {
                DrawSegments(terrain, region, point);

                // Setup the next move
                var next = point + Helpers.UP;
                re = RadiusError(next, region.Radius);

                var d = ((re + next.Y) * 2) + next.X - 1;
                point = d > 0 ? next + Helpers.LEFT : next;
            }
        }

        private void DrawSegments(Terrain terrain, RegionData region, Vector2 point)
        {
            terrain.DrawRegionPoint(region.Center + point, region.Color);
            terrain.DrawRegionPoint(region.Center + point * Helpers.FLIPY, region.Color);
            terrain.DrawRegionPoint(region.Center + point * Helpers.FLIPX, region.Color);
            terrain.DrawRegionPoint(region.Center + point * Helpers.FLIPX * Helpers.FLIPY, region.Color);

            var transpose = Helpers.Transpose(point);
            terrain.DrawRegionPoint(region.Center + transpose, region.Color);
            terrain.DrawRegionPoint(region.Center + transpose * Helpers.FLIPY, region.Color);
            terrain.DrawRegionPoint(region.Center + transpose * Helpers.FLIPX, region.Color);
            terrain.DrawRegionPoint(region.Center + transpose * Helpers.FLIPX * Helpers.FLIPY, region.Color);
        }

        private static float RadiusError(Vector2 point, float r) =>
            point.X * point.X + point.Y * point.Y - r * r;
    }
}