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
            //var result = "";
            var result = classTocache.GetSomething(randomInt);
            Console.WriteLine("First call: " + result);

            result = classTocache.GetSomething(randomInt);
            Console.WriteLine("Second call: " + result);

            IClassToCache test = new ClassToCache();
            result = test.GetSomething(randomInt);
            Console.WriteLine("Third call: " + result);

            result = test.GetSomething(randomInt);
            Console.WriteLine("Fourth call cached: " + result);

            result = test.GetSomethingWithMoreParams(randomInt, "yikes");
            Console.WriteLine("Five call: " + result);

            result = test.GetSomethingWithMoreParams(randomInt, "another One");
            Console.WriteLine("Six call cached: " + result);

            result = test.GetSomethingWithMoreParams(randomInt, "yikes");
            Console.WriteLine("Seven call cached: " + result);

            result = test.GetSomethingWithMoreParams(randomInt, "another One");
            Console.WriteLine("Eight call cached: " + result);

            var resultBool = test.GetSomethingWitEvenMoreParams(randomInt, "yikes", true);
            Console.WriteLine("nine call cached: " + resultBool);

            resultBool = test.GetSomethingWitEvenMoreParams(randomInt, "another One", true);
            Console.WriteLine("ten call cached: " + resultBool);

            resultBool = test.GetSomethingWitEvenMoreParams(randomInt, "yikes", true);
            Console.WriteLine("eleven call cached: " + resultBool);
        }
    }
}
