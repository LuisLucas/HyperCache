﻿namespace HyperCacheGenerator
{
    using HyperCache.Builders;
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
                    CacheGenerator.AddHyperCache(context);
                    ClassGenerator.AddCacheToClass(cacheSyntaxReceiver.CacheCandidates, context);
                }

            }
            catch (Exception)
            {
                Debugger.Launch();
            }
        }
    }
}
