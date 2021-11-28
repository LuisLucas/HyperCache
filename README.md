# HyperCache

Lib that leverages [source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) to add a simple memory cache to classes marked with a HyperCache attribute.

Aims to add a simple memory cache to classes withouth need to write code for it. 

## How it works

HyperCache lib starts by adding a static HyperCacheGenerated class that creates a new MemoryCache instance. 
This static class is only added if attribute HyperCache are in use.
This attribute tells that this classes wants to make use of the memoryCache. 
Class must be a partial class and needs to implement an interface for now.

## How To

```
  public interface ISimpleClass
  {
      int IntReturnMethod(int param1, string param2);
  }
  
  [HyperCache]
  partial class SimpleClass : ISimpleClass
  {
      private readonly Random random;

      public SimpleClass()
      {
          this.random = new Random();
      }
  
      public int IntReturnMethod(int param1, string param2)
      {
          return this.random.Next() * param1 * param2.Length;
      }
  }
```

This will generate 

```
 partial class SimpleClass : ISimpleClass
 {
    int ISimpleClass.IntReturnMethod(int param1, string param2)
    {
        var cacheKey = "IntReturnMethod_" + param1 + param2;
        return HyperCacheGenerated.Cache.GetOrCreate(cacheKey, entry =>   
        {        
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            var res =  this.IntReturnMethod(param1, param2);
            return res;
        });
    }
 }
```
So a call to 
```
ISimpleClass simpleClass = new SimpleClass();
var returnValue = simpleClass.IntReturnMethod(1, "something");
```
will hit the generated class and cache the result.
