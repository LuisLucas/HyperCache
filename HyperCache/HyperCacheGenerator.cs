namespace HyperCacheGenerator
{
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
                if (sintaxReceiver is HyperCacheSyntaxReceiver cacheSyntaxReceiver && cacheSyntaxReceiver.CandidateProxies.Any())
                {
                    CacheGenerator.AddCache(context);

                    foreach (var classToCache in cacheSyntaxReceiver.CandidateProxies)
                    {
                        var nameSpaceDeclaration = (NamespaceDeclarationSyntax)classToCache.Parent;
                        var nameSpace = nameSpaceDeclaration.Name; // + ".HyperCache";
                        var className = classToCache.Identifier.ValueText;

                        var semanticModel = context.Compilation.GetSemanticModel(classToCache.SyntaxTree);
                        var typeSymbol = (INamedTypeSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, classToCache)!;
                        //var symbol = semanticModel.GetDeclaredSymbol(classToCache);
                        var interfaceName = "I" + typeSymbol.Name;

                        var method = typeSymbol.GetMembers().Where(x => x is IMethodSymbol).Select(x => (IMethodSymbol)x);
                        var methodName = "";
                        var returnType = "";
                        var parameters = "";
                        var cacheKey = "";
                        foreach (var x in method)
                        {
                            methodName = x.Name;
                            returnType = x.ReturnType.ToString();
                            foreach(var param in x.Parameters)
                            {
                                parameters = param.Type + " " + param.Name;
                                cacheKey = param.Name + " ";
                            }
                        }
                        var classBuilder = new StringBuilder($@"
namespace {nameSpace}
{{
    using System;
    using Microsoft.Extensions.Caching.Memory;

    public partial class {className} : {interfaceName}
    {{
        {returnType} {interfaceName}.{methodName}({parameters})
        {{
            Console.WriteLine(""This was called in IClassToCache"");
            var cacheKey = ""{methodName}"" + {cacheKey};
            HyperCacheGenerated.Cache.TryGetValue(cacheKey, out var result);
            if(result is null)
            {{
                var res = this.GetSomething(anInt);
                HyperCacheGenerated.Cache.Set(cacheKey, res);
                return res;
            }}
            return (string)result;

        }}
    }}
}}
");
                        context.AddSource("FirstClass", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
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
                            public static IMemoryCache Cache { get; private set; }

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
