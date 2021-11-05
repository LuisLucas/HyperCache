namespace HyperCache.Configurations
{
    using HyperCache.Configurations.Models;
    using Microsoft.CodeAnalysis;

    public static class HyperCacheConfiguration
    {
        public static HyperCacheConfig GetConfigurations(GeneratorExecutionContext context)
        {
            var absoluteExpiration = TryGetConfiguration(context, "HyperCache_AbsoluteExpiration");
            var slidingExpiration = TryGetConfiguration(context, "HyperCache_SlidingExpiration");

            var configuration = new HyperCacheConfig(absoluteExpiration, slidingExpiration);
            return configuration;
        }

        private static ConfigurationValue TryGetConfiguration(GeneratorExecutionContext context, string property)
        {
            if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.{property}", out var configuration))
            {
                if (int.TryParse(configuration, out int result))
                {
                    return new ConfigurationValue(result > 0, result > 0 ? result : -1);
                }
            }

            return new ConfigurationValue(false, -1);
        }
    }
}
