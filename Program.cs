using System;
using System.Diagnostics;

namespace generate_terrain
{
    public class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            var seed = new Seed(
                size: 1,
                region_count: 8,
                region_growth_rate: 5
            );

            Terrain terrain = new Terrain(seed);

            Console.WriteLine(terrain);

            Console.Write("\nGenerating fault lines...");
            stopwatch.Start();
            terrain.DrawFaultLines();
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");

            stopwatch.Reset();
            stopwatch.Start();
            Console.Write("Writing bitmap...");
            terrain.SaveBitmap("terrain");
            stopwatch.Stop();
            Console.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
