using System;
using System.Collections.Generic;
using System.Text;

namespace HyperCache.Writer
{
    public class ClassWriter
    {
        private StringBuilder classAsText;

        public ClassWriter()
        {
            classAsText = new StringBuilder();
        }

        internal void WriteHeader(string nameSpace, string className)
        {
            classAsText.Append($@"
namespace {nameSpace}
{{
    using System;
    using Microsoft.Extensions.Caching.Memory;

    public partial class {className} : 
{{
");
        }
    }
}
