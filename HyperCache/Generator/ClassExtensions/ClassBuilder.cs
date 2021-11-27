namespace HyperCache.Generator.ClassExtensions
{
    using System.Text;

    public static class ClassBuilder
    {
        public static StringBuilder OpenBrackets(this StringBuilder classAsText)
        {
            classAsText.Append($@"
{{");
            return classAsText;
        }

        public static StringBuilder CloseBrackets(this StringBuilder classAsText)
        {
            classAsText.Append($@"
}}");
            return classAsText;
        }

        public static StringBuilder NameSpace(this StringBuilder classAsText, string nameSpace)
        {
            classAsText.Append($@"
namespace {nameSpace}");
            return classAsText;
        }

        public static StringBuilder Usings(this StringBuilder classAsText, bool asyncMethods = false)
        {
            classAsText.Append($@"
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using HyperCache;
");

            if (asyncMethods)
            {
                classAsText.Append($@"
    using System.Threading.Tasks;
");
            }

            return classAsText;
        }

        public static StringBuilder ClassDeclaration(this StringBuilder classAsText, string className, string interfaces)
        {
            classAsText.Append($@"

    public partial class {className} : {interfaces}
");
            return classAsText;
        }

        public static StringBuilder MethodDeclaration(this StringBuilder classAsText, string returnType, string interfaceName, string methodName, string parameters, bool async = false)
        {
            var methodDeclaration = $" {returnType} {interfaceName}.{methodName} ({ parameters})";

            if (async)
            {
                methodDeclaration = async + methodDeclaration;
            }

            classAsText.Append($@" 
    {methodDeclaration}
");

            return classAsText;
        }

        public static StringBuilder Cache(this StringBuilder classAsText, string methodName, string cacheKey, bool async = false)
        {
            var cache = $@"        var cacheKey = ""{methodName}"";";

            if (!string.IsNullOrEmpty(cacheKey))
            {
                cache = $@"        var cacheKey = ""{methodName}_"" + {cacheKey};";
            }

            classAsText.Append(cache);

            if (async)
            {
                ;
                classAsText.Append($@"
        return HyperCacheGenerated.Cache.GetOrCreateAsync(cacheKey, async entry =>
        ");
            }
            else
            {
                classAsText.Append($@"
        return HyperCacheGenerated.Cache.GetOrCreate(cacheKey, entry =>
        ");
            }

            return classAsText;
        }

        public static StringBuilder CacheEntryConfiguration(this StringBuilder classAsText, string configName, int timeInMinutes)
        {
            classAsText.Append($@"        
            entry.{configName} = TimeSpan.FromMinutes({timeInMinutes});");
            return classAsText;
        }

        public static StringBuilder OriginalMethodCall(this StringBuilder classAsText, bool isAsync, string methodName, string paramsPassed)
        {
            var methodCall = string.Format("var res = {0} this.{1}({2});", isAsync ? "await" : string.Empty, methodName, paramsPassed);
            classAsText.Append($@"
            {methodCall}
            return res;
        }});");

            return classAsText;
        }
    }
}
