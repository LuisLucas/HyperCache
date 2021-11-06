namespace ProjectExample.SimpleTest
{
    using HyperCacheGenerator;
    using System;
    using System.Text;

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
