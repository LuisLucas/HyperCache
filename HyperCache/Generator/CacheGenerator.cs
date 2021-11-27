namespace HyperCache.Generator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using System.Text;

    public static class CacheGenerator
    {
        internal static void AddHyperCache(GeneratorExecutionContext context)
        {
            var sourceBuilder = new StringBuilder(@"
                    namespace HyperCache
                    {
                        using Microsoft.Extensions.Caching.Memory;

                        public static class HyperCacheGenerated
                        {
                            public static IMemoryCache Cache { get; private set; }

                            static HyperCacheGenerated()
                            {
                                Cache = new MemoryCache(new MemoryCacheOptions());
                            }
                        }
                    }");

            context.AddSource("HyperCache", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
