using System;
using System.Diagnostics;

namespace generate_terrain
{
    public class Benchmark
    {
        Stopwatch m_stopwatch = new Stopwatch();
        object m_lock = new Object();

        public void Run(Action cb, string message)
        {
            lock (m_lock)
            {
                m_stopwatch.Reset();
                Console.Write(message + "...");
                m_stopwatch.Start();
                cb();
                m_stopwatch.Stop();
                Console.WriteLine($"{m_stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}