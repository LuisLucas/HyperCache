# HyperCache

Lib that leverages [source generators](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) to add a simple memory cache to classes marked with a HyperCache attribute.

Aims to add a simple memory cache to classes withouth need to write code for it. 

## How it works

HyperCache lib starts by adding a static HyperCacheGenerated class that creates a new MemoryCache instance. 
This static class is only added if attribute HyperCache are in use.
This attribute tells that this classes wants to make use of the memoryCache. 
Class must be a partial class and needs to implement an interface for now.

For example:

In construction... a bunch of things missing
