# ⚡ HyperCache

A lightweight library that uses **C# Source Generators** to automatically inject in-memory caching into classes — **no manual cache code needed**.

> Just add an attribute, implement an interface, and get caching for free.

---

## ✨ What It Does

HyperCache adds a simple **MemoryCache** to classes marked with the `[HyperCache]` attribute. All generated caching logic is handled behind the scenes using source generators.

---

## ⚙️ How It Works

1. When a class is marked with `[HyperCache]`, the generator:
   - Adds a static `HyperCacheGenerated` class with a shared `MemoryCache` instance.
   - Creates a wrapper implementation for the class methods defined in the interface.
2. The class must:
   - Be declared as `partial`.
   - Implement an interface to define cacheable methods.

---

## 🧪 Example

### Interface
```csharp
public interface ISimpleClass
{
    int IntReturnMethod(int param1, string param2);
}
```

### Your Class
```csharp
[HyperCache]
partial class SimpleClass : ISimpleClass
{
    private readonly Random random = new();

    public int IntReturnMethod(int param1, string param2)
    {
        return this.random.Next() * param1 * param2.Length;
    }
}
```

### What Gets Generated
```csharp
partial class SimpleClass : ISimpleClass
{
    int ISimpleClass.IntReturnMethod(int param1, string param2)
    {
        var cacheKey = "IntReturnMethod_" + param1 + param2;
        return HyperCacheGenerated.Cache.GetOrCreate(cacheKey, entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            return this.IntReturnMethod(param1, param2);
        });
    }
}
```

Now any call like:
```csharp
ISimpleClass simple = new SimpleClass();
var value = simple.IntReturnMethod(1, "something");
```
...will automatically go through the cache layer.

---

## 📦 In Progress

- [ ] Allow method-level customization (cache duration, keys, etc.)
- [ ] Support async methods
- [ ] Avoid requiring interfaces (via method inspection)
- [ ] Add diagnostics for misuse or missing `partial` keyword

---

## 🧰 Requirements

- .NET 6 or newer
- Roslyn Source Generator support
- Your class must be:
  - `partial`
  - implementing an interface

---

## 📬 Feedback & Contributions

This is a WIP project built for exploration and productivity — feel free to fork, suggest improvements, or contribute via PR!

---

Made with ❤️
