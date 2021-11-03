namespace HyperCache.Helper
{
    using System.Collections.Generic;
    using System.Text;

    public static class IEnumerableExtensions
    {
        public static string AppendAll(this IEnumerable<string> collection, string seperator)
        {
            using var enumerator = collection.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return string.Empty;
            }

            var builder = new StringBuilder(enumerator.Current);
            while (enumerator.MoveNext())
            {
                builder.Append(seperator).Append(enumerator.Current);
            }
            return builder.ToString();
        }
    }
}
