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
                region_count: 3,
                region_growth_rate: 6
            );

            Terrain terrain = new Terrain(seed);

            Console.WriteLine(terrain);

            benchmark.Run(() => terrain.DrawFaultLines(), "Generating fault lines");
            benchmark.Run(() => terrain.SaveBitmap("terrain"), "Writing bitmap");
        }
    }
}
