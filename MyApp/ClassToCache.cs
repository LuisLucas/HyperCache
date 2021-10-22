namespace MyApp
{
    using HyperCacheGenerator;
    using System;
    
    public interface IClassToCache
    {
        string GetSomething(int anInt);
    }

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


    public partial class ClassToCache : IClassToCache
    {
        string IClassToCache.GetSomething(int anInt)
        {
            Console.WriteLine("This was called in IClassToCache");
            return this.GetSomething(anInt);
        }
    }



    /*public class ProxyCache : ICache
    {
        public ProxyCache()
        {

        }

        public string GetSomething(int anInt)
        {
            return string.Empty;
        }
    }

    public interface ICache
    {

    }*/
}
