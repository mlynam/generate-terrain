/**
 * Implementation of https://en.wikipedia.org/wiki/Midpoint_circle_algorithm.
 * This implementation uses floating point operations to fill in any voids
 * as a result of integral math.
 */

using System.Numerics;

namespace generate_terrain
{
    public class CirclePlateStepper : IPlateStepper
    {
        public void Step(IPlate plate)
        {
            Vector2 point = new Vector2((int)plate.Radius, 0);

            float re = 0f;
            while (point.Y <= point.X)
            {
                DrawSegments(plate, point);

                // Setup the next move
                var next = point + Helpers.UP;
                re = RadiusError(next, plate.Radius);

                var d = ((re + next.Y) * 2) + next.X - 1;
                point = d > 0 ? next + Helpers.LEFT : next;
            }
        }

        private void DrawSegments(IPlate plate, Vector2 point)
        {
            plate.DrawRegionPoint(plate.Center + point);
            plate.DrawRegionPoint(plate.Center + point * Helpers.FLIPY);
            plate.DrawRegionPoint(plate.Center + point * Helpers.FLIPX);
            plate.DrawRegionPoint(plate.Center + point * Helpers.FLIPX * Helpers.FLIPY);

            var transpose = Helpers.Transpose(point);
            plate.DrawRegionPoint(plate.Center + transpose);
            plate.DrawRegionPoint(plate.Center + transpose * Helpers.FLIPY);
            plate.DrawRegionPoint(plate.Center + transpose * Helpers.FLIPX);
            plate.DrawRegionPoint(plate.Center + transpose * Helpers.FLIPX * Helpers.FLIPY);
        }

        private static float RadiusError(Vector2 point, float r) =>
            point.X * point.X + point.Y * point.Y - r * r;
    }
}