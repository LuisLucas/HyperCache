namespace HyperCacheGenerator
{
    using HyperCache.Builders;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
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
                if (sintaxReceiver is HyperCacheSyntaxReceiver cacheSyntaxReceiver && cacheSyntaxReceiver.CacheCandidates.Any())
                {
                    CacheGenerator.AddHyperCache(context);
                    ClassGenerator.AddCacheClasses(cacheSyntaxReceiver.CacheCandidates, context);
                }

            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }
    }
}
