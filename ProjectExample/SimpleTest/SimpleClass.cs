﻿namespace ProjectExample.SimpleTest
{
    using HyperCacheGenerator;
    using System;

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
    }
}
