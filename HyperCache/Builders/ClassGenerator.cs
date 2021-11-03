namespace HyperCache.Builders
{
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
                var nameSpace = GetNamespace(classToCache);
                var className = GetClassName(classToCache);
                var typeSymbol = GetTypeSymbol(context, classToCache);

                var interfaceName = "I" + typeSymbol.Name;
                var interfaces = typeSymbol.AllInterfaces;
                var methods = GetMethods(typeSymbol);

                var classAsText = new StringBuilder();
                BuildClassHeader(nameSpace, className, interfaceName, classAsText);
                MethodGenerator.BuildMethods(methods, interfaceName, interfaces, classAsText);
                BuildFooter(classAsText);
                AddClassToSource(context, nameSpace, className, classAsText);
            }
        }

        private static void BuildClassHeader(string nameSpace, string className, string interfaceName, StringBuilder classAsText)
        {
             classAsText.Append($@"
namespace {nameSpace}
{{
    using System;
    using Microsoft.Extensions.Caching.Memory;

    public partial class {className} : {interfaceName}
    {{

");
        }

        private static void AddClassToSource(GeneratorExecutionContext context, string nameSpace, string className, StringBuilder classBuilder)
        {
            context.AddSource($"{nameSpace}_{className}", SourceText.From(classBuilder.ToString(), Encoding.UTF8));
        }

        private static void BuildFooter(StringBuilder classBuilder)
        {
            classBuilder.Append($@"
    }}
}}
");
        }

        private static IEnumerable<IMethodSymbol> GetMethods(INamedTypeSymbol typeSymbol)
        {
            return typeSymbol
                .GetMembers()
                .Where(x => x.Kind is SymbolKind.Method)
                .Select(x => (IMethodSymbol)x);
        }

        private static INamedTypeSymbol GetTypeSymbol(GeneratorExecutionContext context, ClassDeclarationSyntax classToCache)
        {
            var semanticModel = context.Compilation.GetSemanticModel(classToCache.SyntaxTree);
            var typeSymbol = (INamedTypeSymbol)ModelExtensions.GetDeclaredSymbol(semanticModel, classToCache)!;
            return typeSymbol;
        }

        private static string GetClassName(ClassDeclarationSyntax classToCache)
        {
            return classToCache.Identifier.ValueText;
        }

        private static string GetNamespace(ClassDeclarationSyntax classToCache)
        {
            var nameSpaceDeclaration = (NamespaceDeclarationSyntax)classToCache.Parent;
            return nameSpaceDeclaration.Name.ToString();
        }
    }
}
