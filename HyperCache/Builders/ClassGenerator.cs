namespace HyperCache.Builders
{
    using HyperCache.Helper;
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
                var methods = GetMethods(typeSymbol);

                var classAsText = new StringBuilder();
                BuildClassHeader(nameSpace, className, typeSymbol, classAsText);
                MethodGenerator.BuildMethods(methods, classAsText);

                string emitLogging = "";
                var property = "HyperCache_AbsoluteExpiration";
                if (context.AnalyzerConfigOptions.GlobalOptions.TryGetValue($"build_property.{property}", out var emitLoggingSwitch))
                {
                    emitLogging = emitLoggingSwitch;//.Equals("true", System.StringComparison.OrdinalIgnoreCase);
                    classAsText.Append($@"
public void Another()
{{
var x = ""{emitLogging}"";
}}
");
                }


                BuildFooter(classAsText);
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
