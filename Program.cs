using System;
using System.Diagnostics;

namespace generate_terrain
{
    public class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new Benchmark();
            var stopwatch = new Stopwatch();
            var seed = new Seed(
                size: 1,
                region_count: 8,
                region_growth_rate: 5
            );

            Terrain terrain = new Terrain(seed);

            Console.WriteLine(terrain);

            benchmark.Run(() => terrain.DrawFaultLines(), "Generating fault lines");
            benchmark.Run(() => terrain.SaveBitmap("terrain"), "Writing bitmap");
        }
    }
}
