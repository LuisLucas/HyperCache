namespace HyperCacheGenerator
{
    using System;

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class HyperCacheAttribute : Attribute
    {
        public static string Name = nameof(HyperCacheAttribute).Replace("Attribute", string.Empty);

        public HyperCacheAttribute()
        {
        }
    }
}
