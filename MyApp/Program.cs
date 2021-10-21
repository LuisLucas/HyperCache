using System;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var randomInt = new Random().Next();
            var classTocache = new ClassToCache();
            var result = classTocache.GetSomething(randomInt);
            Console.WriteLine("First call: " + result);

            result = classTocache.GetSomething(randomInt);
            Console.WriteLine("Second call: " + result);

            IClassToCache test = new ClassToCache();
            result = test.GetSomething(randomInt);
            Console.WriteLine("Third call: " + result);
        }
    }
}
