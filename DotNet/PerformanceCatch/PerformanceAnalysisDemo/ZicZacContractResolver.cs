using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PerformanceAnalysisDemo
{
    public class ZicZacContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var result = base.CreateProperty(member, memberSerialization);

            var property = member as PropertyInfo;

            if (property.PropertyType == typeof(string))
            {
                result.ValueProvider = new StringValueProvider(property);
            }
            
            return result;
        }
        
    }
}