namespace HyperCacheGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.Text;
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    [Generator]
    public class HyperCacheGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new HyperCacheSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                var sintaxReceiver = context.SyntaxReceiver;
                if (sintaxReceiver is HyperCacheSyntaxReceiver cacheSyntaxReceiver && cacheSyntaxReceiver.CandidateProxies.Any())
                {
                    CacheGenerator.AddCache(context);

                    foreach (var classToCache in cacheSyntaxReceiver.CandidateProxies)
                    {

                    }
                }

            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }
    }

    public static class CacheGenerator
    {
        public static void AddCache(GeneratorExecutionContext context)
        {
            var sourceBuilder = new StringBuilder(@"
                    namespace MyApp
                    {
                        using Microsoft.Extensions.Caching.Memory;

                        public static class HyperCacheGenerated
                        {
                            public static MemoryCache Cache { get; private set; }

                            static HyperCacheGenerated()
                            {
                                Cache = new MemoryCache(new MemoryCacheOptions());
                            }
                        }
                    }");

            context.AddSource("HyperCacheGenerated", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }
    }
}
