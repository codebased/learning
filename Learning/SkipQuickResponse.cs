using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace Learning
{
    public class SkipQuickDependencies : ITelemetryProcessor
    {
        public int Threshold { get; }
        public string DependencyType { get; }
        private ITelemetryProcessor Next { get; }

        public SkipQuickDependencies(int threshold, string dependencyType, ITelemetryProcessor next)
        {
            Threshold = threshold;
            DependencyType = dependencyType;
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (ShouldSkipDependency(item)) { return; }
            this.Next.Process(item);
        }

        private bool ShouldSkipDependency(ITelemetry item)
        {
            var dependency = item as DependencyTelemetry;
            if (dependency?.Type == DependencyType)
            {
                if (dependency.Duration.TotalMilliseconds < Threshold && dependency.Success == true)
                    return true;
            }
            return false;
        }
    }
}
