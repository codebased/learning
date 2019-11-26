using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;

namespace PerformanceAnalysisDemo
{
    public class JsonSerializerCodeAnalysis : ICodeAnalysis
    {
        private static readonly Person person = new Person {Name = "Me"};
        private static readonly ZicZacContractResolver zicZacContractResolver = new ZicZacContractResolver();

        public void BadWay()
        {
            JsonConvert.SerializeObject(person, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new ZicZacContractResolver()
            });
        }

        public void GoodWay()
        {
            JsonConvert.SerializeObject(person, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = zicZacContractResolver
            });
        }
    }
}