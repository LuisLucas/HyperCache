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
