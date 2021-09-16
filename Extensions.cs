using System;
using System.Numerics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace generate_terrain
{
    public static class Extensions
    {
        public static void SaveBitmap(this Terrain terrain, string name)
        {
            Rgba32[] pixels = new Rgba32[terrain.Bitmap.Length];
            for (int i = 0; i < pixels.Length; i++)
            {
                uint rgba = terrain.Bitmap[i];
                pixels[i] = new Rgba32(
                    r: (byte)(rgba >> 24),
                    g: (byte)(rgba >> 16),
                    b: (byte)(rgba >> 8),
                    a: (byte)(rgba)
                );
            }

            var image = Image<Rgba32>.LoadPixelData(pixels, terrain.Size, terrain.Size);
            image.SaveAsBmp($"./{name}.bmp");
        }

        public static void Update(this Vector2 target, Vector2 value)
        {
            target.X = value.X;
            target.Y = value.Y;
        }

        public static int IntClampMax(this float value, int max)
        {
            return Math.Min(Math.Max((int)value, 0), max);
        }
    }
}