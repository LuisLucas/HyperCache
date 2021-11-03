﻿namespace HyperCache.Builders
{
    using HyperCache.Helper;
    using Microsoft.CodeAnalysis;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text;

    public static class MethodGenerator
    {
        public static void BuildMethods(IEnumerable<IMethodSymbol> methods, string interfaceName, ImmutableArray<INamedTypeSymbol> interfaces, StringBuilder classBuilder)
        {
            foreach(var interfaceType in interfaces)
            {
                var m = interfaceType
                    .GetMembers()
                    .Where(x => x.Kind is SymbolKind.Method)
                    .Select(x => (IMethodSymbol)x);

                /*foreach (var method in m)
                {
                    var methodName = method.Name;
                    var returnType = method.ReturnType.ToString();
                    var parameters = method.Parameters.Select(param => param.Type + " " + param.Name).AppendAll(",");
                    var cacheKey = method.Parameters.Select(param => param.Name).AppendAll(" + ");
                    var paramsPassed = method.Parameters.Select(param => param.Name).AppendAll(", ");

                    BuildClassMethods(interfaceName, classBuilder, methodName, returnType, parameters, cacheKey, paramsPassed);
                }*/
            }

            foreach (var method in methods.Where(x => x.DeclaredAccessibility != Accessibility.Private))
            {
                if (method.MethodKind == MethodKind.Constructor)
                {
                    continue;
                }

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
    Console.WriteLine(""This was called in class to cache generated"");
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
    }
}