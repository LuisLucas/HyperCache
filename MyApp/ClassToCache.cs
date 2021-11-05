namespace MyApp
{
    using HyperCacheGenerator;
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

        string IClassToCache.GetSomething(int anInt)
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
}
