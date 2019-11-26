using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace PerformanceAnalysisDemo
{
    public class StringValueProvider : IValueProvider
    {
        private readonly PropertyInfo _targetProperty;

        public StringValueProvider(PropertyInfo targetProperty)
        {
            _targetProperty = targetProperty;
        }

        // SetValue gets called by Json.Net during deserialization.
        // The value parameter has the original value read from the JSON;
        // target is the object on which to set the value.
        public void SetValue(object target, object value)
        {
            _targetProperty.SetValue(target, value);
        }

        // GetValue is called by Json.Net during serialization.
        // The target parameter has the object from which to read the value;
        // the return value is what gets written to the JSON
        public object GetValue(object target)
        {
            var value = _targetProperty.GetValue(target);
            var stringValue = (value ??"").ToString();
            return new string(stringValue.ToLower().AsEnumerable().Select((c, i) => i % 2 == 0 ? c : char.ToUpper(c)).ToArray());
        }
    }
}