using System;

namespace generate_terrain
{
    public class Program
    {
        static void Main(string[] args)
        {
            int rng_seed = 3200;
            int size_seed = 1;
            int fault_line_seed = 5;
            int region_seed = 3;

            Terrain terrain = new Terrain(size_seed);
            Region[] regions = new Region[region_seed];

            int regionSize = (terrain.Tilemap.Length - terrain.Deadzone * region_seed) / region_seed;

            var rng = new Random(rng_seed);
            int offset = terrain.Deadzone / 2;

            for (int i = 0; i < regions.Length; i++)
            {
                var next = rng.Next(regionSize);
                var color = rng.Next(0xFFFFFF);
                var rate = rng.Next(fault_line_seed, region_seed + fault_line_seed);

                var region = new Region(
                    bitmap_index: offset + next,
                    color: (uint)(color | 0x000000FF),
                    rate: rate,
                    name: $"Region {i + 1}");

                terrain.Tilemap[region.Index] = 0xAAAAAAFF;
                regions[i] = region;
                offset += (regionSize + terrain.Deadzone);

                Console.WriteLine(region);
            }

            terrain.DrawFaultLines(regions, fault_line_seed);
            terrain.SaveBitmap("terrain");
        }
    }
}
