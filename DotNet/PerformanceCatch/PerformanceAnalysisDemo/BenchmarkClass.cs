using BenchmarkDotNet.Attributes;

namespace PerformanceAnalysisDemo
{
    [RPlotExporter, RankColumn]
    public class BenchmarkClass
    {
        private readonly ICodeAnalysis jsonCodeAnalysis = new JsonSerializerCodeAnalysis();
        private readonly ICodeAnalysis regexCodeAnalysis = new RegExSerializerCodeAnalysis();

        [Benchmark]
        [BenchmarkCategory("JsonCodeAnalysis", "1")]
        public void JsonCodeAnalysisBadWay() => jsonCodeAnalysis.BadWay();

        [Benchmark]
        [BenchmarkCategory("JsonCodeAnalysis", "2")]
        public void JsonCodeAnalysisGoodWay() => jsonCodeAnalysis.GoodWay();

        [Benchmark]
        [BenchmarkCategory("RegexCodeAnalysis", "1")]
        public void RegexCodeAnalysisBadWay() => regexCodeAnalysis.BadWay();

        [Benchmark]
        [BenchmarkCategory("RegexCodeAnalysis", "2")]
        public void RegexCodeAnalysisGoodWay() => regexCodeAnalysis.GoodWay();
    }
}