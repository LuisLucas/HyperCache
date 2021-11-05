namespace HyperCache.Builders
{
    using HyperCache.Configurations.Models;
    using HyperCache.Helper;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ClassGenerator
    {
        internal static void AddCacheToClass(List<ClassDeclarationSyntax> cacheCandidates, GeneratorExecutionContext context, HyperCacheConfig config)
        {
            foreach (var classToCache in cacheCandidates)
            {                
                var className = classToCache.GetClassName();
                var nameSpace = classToCache.GetNameSpace();             
                var typeSymbol = GetTypeSymbol(context, classToCache);
                var interfaces = typeSymbol.AllInterfaces.Select(x => x.Name);     
                var methods = GetMethods(typeSymbol);
               
                var classAsText = new StringBuilder();
                BuildClassHeader(nameSpace, className, typeSymbol, classAsText);
                BuildClassMethods(methods, config, classAsText);
                BuildClassFooter(classAsText);
                AddClassToSource(context, nameSpace, className, classAsText);
            }
        }

        private static void BuildClassHeader(string nameSpace, string className, INamedTypeSymbol classtoAddCache, StringBuilder classAsText)
        {
            var interfaces = classtoAddCache.AllInterfaces.Select(x => x.Name).AppendAll(", ");
            classAsText.Append($@"
namespace {nameSpace}
{{
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using HyperCache;

    public partial class {className} : {interfaces}
{{
");
        }

        public static void BuildClassMethods(IEnumerable<IMethodSymbol> methods, HyperCacheConfig config, StringBuilder classAsText)
        {
            foreach (var method in methods)
            {
                var interfaceName = method.ContainingType.Name;
                var methodName = method.Name;
                var returnType = method.ReturnType.ToString();
                var parameters = method.Parameters.Select(param => param.Type + " " + param.Name).AppendAll(",");
                var cacheKey = method.Name + method.Parameters.Select(param => param.Name).AppendAll("_");
                var paramsPassed = method.Parameters.Select(param => param.Name).AppendAll(", ");

                BuidMethodHeader(classAsText, interfaceName, methodName, returnType, parameters, cacheKey);
                BuildMethodCacheConfigurations(config, classAsText);
                BuildMethodFooter(classAsText, methodName, paramsPassed);
            }
        }

        private static void BuildMethodFooter(StringBuilder classAsText, string methodName, string paramsPassed)
        {
            classAsText.Append($@"
            var res = this.{methodName}({paramsPassed});
            return res;
        }});
    }}
");
        }

        private static void BuildMethodCacheConfigurations(HyperCacheConfig config, StringBuilder classAsText)
        {
            if (config.AbsoluteTimeExpiration.HasConfiguration)
            {
                classAsText.Append($@"        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes({config.AbsoluteTimeExpiration.Value});");
            }

            if (config.SlidingTimeExpiration.HasConfiguration)
            {
                classAsText.Append($@"        entry.SlidingExpiration = TimeSpan.FromMinutes({config.SlidingTimeExpiration.Value});");
            }
        }

        private static void BuidMethodHeader(StringBuilder classAsText, string interfaceName, string methodName, string returnType, string parameters, string cacheKey)
        {
            classAsText.Append($@" 
    {returnType} {interfaceName}.{methodName}({parameters})
    {{
        var cacheKey = ""{cacheKey}"";
        return HyperCacheGenerated.Cache.GetOrCreate(cacheKey, entry =>
        {{
    ");
        }

        private static void BuildClassFooter(StringBuilder classAsText)
        {
            classAsText.Append($@"
    }}
}}
");
        }

        private static void AddClassToSource(GeneratorExecutionContext context, string nameSpace, string className, StringBuilder classAsText)
        {
            context.AddSource($"{nameSpace}_{className}", SourceText.From(classAsText.ToString(), Encoding.UTF8));
        }

        private static IEnumerable<IMethodSymbol> GetMethods(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol
                .AllInterfaces
                .SelectMany(x => x.GetMembers())
                .Where(x => x.Kind is SymbolKind.Method && typeSymbol.GetMembers(x.ContainingNamespace.Name + "." + x.ContainingType.Name + "." + x.Name).IsEmpty)
                .Select(x => (IMethodSymbol)x)
                .Where(x => !x.ReturnsVoid);
        }

        private static INamedTypeSymbol GetTypeSymbol(GeneratorExecutionContext context, ClassDeclarationSyntax classToCache)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classToCache.SyntaxTree);
            var typeSymbol = (INamedTypeSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, classToCache)!;
            return typeSymbol;
        }
    }
}
