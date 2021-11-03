namespace MyApp
{
    using HyperCacheGenerator;
    //using Microsoft.Extensions.Caching.Memory;
    using System;

    [HyperCache]
    public partial class ClassToCache : IClassToCache
    {
        private readonly Random random;
        public ClassToCache()
        {
            random = new Random();
        }

        public string GetSomething(int anInt)
        { 
            Console.WriteLine("This was called in ClassToCache");
            return Convert.ToString((anInt * random.Next()));
        }

        public string GetSomethingWithMoreParams(int anInt, string aString)
        {
            Console.WriteLine("This was called in ClassToCache");
            return aString;
        }

        public bool GetSomethingWitEvenMoreParams(int anInt, string aString, bool itsABool)
        {
            Console.WriteLine("This was called in ClassToCache");
            return anInt > 0 ? itsABool : !itsABool;
        }

        private string Test()
        {
            return "something";
        }
    }

    /*public partial class ClassToCache : IClassToCache
    {
        string IClassToCache.GetSomething(int anInt)
        {
            Console.WriteLine("This was called in IClassToCache");
            HyperCacheGenerated.Cache.TryGetValue("", out var result);
            if(result is null)
            {
                var res = this.GetSomething(anInt);
                HyperCacheGenerated.Cache.Set("", res);
                return res;
            }
            return (string)result;
        }
    }*/
}
