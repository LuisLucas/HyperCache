namespace HyperCache.Builders
{
    using HyperCache.Helper;
    using HyperCache.Writer;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ClassGenerator
    {
        internal static void AddCacheToClass(List<ClassDeclarationSyntax> cacheCandidates, GeneratorExecutionContext context)
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
                BuildClassMethods(methods, classAsText);
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

    public partial class {className} : {interfaces}
{{
");
        }

        private static void AddClassToSource(GeneratorExecutionContext context, string nameSpace, string className, StringBuilder classBuilder)
        {
            context.AddSource($"{nameSpace}_{className}", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        public static void BuildClassMethods(IEnumerable<IMethodSymbol> methods, StringBuilder classBuilder)
        {
            foreach (var method in methods.Where(x => x.DeclaredAccessibility != Accessibility.Private))
            {
                var interfaceName = method.ContainingType.Name;
                var methodName = method.Name;
                var returnType = method.ReturnType.ToString();
                var parameters = method.Parameters.Select(param => param.Type + " " + param.Name).AppendAll(",");
                var cacheKey = method.Parameters.Select(param => param.Name).AppendAll(" + ");
                var paramsPassed = method.Parameters.Select(param => param.Name).AppendAll(", ");

                BuildClassMethods(interfaceName, classBuilder, methodName, returnType, parameters, cacheKey, paramsPassed);
            }
        }

        private static void BuildClassMethods(string interfaceName, StringBuilder classBuilder, string methodName, string returnType, string parameters, string cacheKey, string paramsPassed)
        {
            classBuilder.Append($@" 
{returnType} {interfaceName}.{methodName}({parameters})
{{
    var cacheKey = ""{methodName}"" + ""_"" + {cacheKey};
    HyperCacheGenerated.Cache.TryGetValue(cacheKey, out var result);
    if(result is null)
    {{
        var res = this.{methodName}({paramsPassed});
        HyperCacheGenerated.Cache.Set(cacheKey, res);
        return res;
    }}
    return ({returnType})result;
}}
");
        }

        private static void BuildClassFooter(StringBuilder classBuilder)
        {
            classBuilder.Append($@"
    }}
}}
");
        }

        private static IEnumerable<IMethodSymbol> GetMethods(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol
                .AllInterfaces
                .SelectMany(x => x.GetMembers())
                .Where(x => x.Kind is SymbolKind.Method && typeSymbol.GetMembers(x.ContainingNamespace.Name + "." + x.ContainingType.Name + "." + x.Name).IsEmpty)
                .Select(x => (IMethodSymbol)x);
        }

        private static INamedTypeSymbol GetTypeSymbol(GeneratorExecutionContext context, ClassDeclarationSyntax classToCache)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classToCache.SyntaxTree);
            var typeSymbol = (INamedTypeSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, classToCache)!;
            return typeSymbol;
        }
    }
}
