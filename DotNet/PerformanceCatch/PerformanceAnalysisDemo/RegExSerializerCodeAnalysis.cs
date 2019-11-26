using System.Text.RegularExpressions;

namespace PerformanceAnalysisDemo
{
    public class RegExSerializerCodeAnalysis : ICodeAnalysis
    {
        string pattern = @"^[0-9]+(\.[0-9]{1,2})?$";

        public void BadWay()
        {
            var pattern = @"^[0-9]+(\.[0-9]{1,2})?$";
            var currencyRegex = new Regex(pattern);
            currencyRegex.IsMatch("10.10");
        }

        public void GoodWay()
        {
            //By default, the last 15 most recently used static regular expression patterns are cached. For applications that require a larger number of cached static regular expressions, the size of the cache can be adjusted by setting the Regex.CacheSize property.
            Regex.IsMatch("10.20", pattern);
        }
    }
}