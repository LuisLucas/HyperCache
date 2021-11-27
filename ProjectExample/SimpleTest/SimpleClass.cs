namespace ProjectExample.SimpleTest
{
    using HyperCacheGenerator;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Threading.Tasks;

    [HyperCache]
    partial class SimpleClass : ISimpleClass
    {
        private readonly Random random;

        public SimpleClass()
        {
            this.random = new Random();
        }

        /// <summary>
        /// This should not be cached
        /// </summary>
        public void VoidMethod()
        {
            /* doing things */
        }

        public int IntReturnMethod()
        {
            return this.random.Next();
        }

        public int IntReturnMethod(int param1)
        {
            return this.random.Next() * param1;
        }

        public async Task<string> StringReturnMethod(int param1)
        {
            return await Test(param1);
        }

        private async Task<string> Test(int param1)
        {
            return await Task.Run(() => (this.random.Next() * param1).ToString());
        }

        /*async Task<string> ISimpleClass.StringReturnMethod(int param1)
        {
            var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new Microsoft.Extensions.Caching.Memory.MemoryCacheOptions());
            var x = cache.GetOrCreateAsync("", async entry => await this.StringReturnMethod(param1));
            return (this.random.Next() * param1).ToString();
        }*/

        public int IntReturnMethod(int param1, string param2)
        {
            return this.random.Next() * param1 * param2.Length;
        }

        public int IntReturnMethod(int? param1, string param2)
        {
            return this.random.Next() * param1 ?? 1 * param2.Length;
        }

        public int IntReturnMethod(string param1, int? param2)
        {
            return this.random.Next() * param1.Length * param2 ?? 1;
        }
    }
}
