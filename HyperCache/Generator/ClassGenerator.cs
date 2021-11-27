namespace HyperCache.Generator
{
    using HyperCache.Configurations.Models;
    using HyperCache.Generator.ClassExtensions;
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
                var asyncMethods = methods.Any(x => x.IsAsync);

                var cachedClass = new StringBuilder()
                    .NameSpace(nameSpace)
                    .OpenBrackets()
                    .Usings(asyncMethods)
                    .ClassDeclaration(className, interfaces.AppendAll(", "))
                    .OpenBrackets();

                BuildClassMethods(methods, config, cachedClass);

                cachedClass
                    .CloseBrackets() // class brackets
                    .CloseBrackets(); // namespace brackets

                AddClassToSource(context, nameSpace, className, cachedClass);
            }
        }

        private static void BuildClassMethods(IEnumerable<IMethodSymbol> methods, HyperCacheConfig config, StringBuilder classAsText)
        {
            foreach (var method in methods)
            {
                var interfaceName = method.ContainingType.Name;
                var methodName = method.Name;
                var isAsync = method.IsAsync;
                var returnType = method.ReturnType.ToString();
                var parameters = method.Parameters.Select(param => param.Type + " " + param.Name).AppendAll(",");
                var cacheKey = method.Parameters.Select(param => param.Name).AppendAll(" + ");
                var paramsPassed = method.Parameters.Select(param => param.Name).AppendAll(", ");

                classAsText
                    .MethodDeclaration(returnType, interfaceName, methodName, parameters, isAsync)
                    .OpenBrackets()
                    .Cache(methodName, cacheKey)
                    .OpenBrackets();

                if (config.AbsoluteTimeExpiration.HasConfiguration)
                {
                    classAsText.CacheEntryConfiguration("AbsoluteExpirationRelativeToNow", config.AbsoluteTimeExpiration.Value);
                }

                if (config.SlidingTimeExpiration.HasConfiguration)
                {
                    classAsText.CacheEntryConfiguration("SlidingExpiration", config.SlidingTimeExpiration.Value);
                }

                classAsText
                    .OriginalMethodCall(isAsync, methodName, paramsPassed)
                    .CloseBrackets(); // method brackets
            }
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
