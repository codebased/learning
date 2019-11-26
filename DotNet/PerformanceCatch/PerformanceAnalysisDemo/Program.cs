using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PerformanceAnalysisDemo
{
    [MemoryDiagnoser]
    static class Program
    {
        static void Main() => BenchmarkRunner.Run(typeof(BenchmarkClass));
    }
}