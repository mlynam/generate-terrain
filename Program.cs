using System;

namespace generate_terrain
{
    public class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new Benchmark();
            var seed = new Seed(
                size: 1,
                region_count: 4,
                region_growth_rate: 3
            );

            Terrain terrain = new Terrain(seed);

            Console.WriteLine(terrain);

            benchmark.Run(() => terrain.DrawFaultLines(), "Generating fault lines");
            benchmark.Run(() => terrain.SaveBitmap("terrain"), "Writing bitmap");
        }
    }
}
