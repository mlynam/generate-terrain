using System;
using System.Numerics;

namespace generate_terrain
{
    public interface IPlate
    {
        void Grow();
        void DrawRegionPoint(Vector2 point);
        int Index { get; }
        Vector2 Center { get; }
        float Radius { get; }
        bool IsGrowing { get; }
        uint Color { get; }
        int DensityFactor { get; }
    }
}