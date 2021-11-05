namespace HyperCacheGenerator
{
    using HyperCache.Builders;
    using HyperCache.Configurations;
    using Microsoft.CodeAnalysis;
    using System;
    using System.Diagnostics;
    using System.Linq;

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
                if (sintaxReceiver is HyperCacheSyntaxReceiver cacheSyntaxReceiver && cacheSyntaxReceiver.CacheCandidates.Any())
                {
                    var config = HyperCacheConfiguration.GetConfigurations(context);
                    CacheGenerator.AddHyperCache(context);
                    ClassGenerator.AddCacheToClass(cacheSyntaxReceiver.CacheCandidates, context, config);
                }

            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }
    }
}
