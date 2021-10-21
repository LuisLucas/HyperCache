namespace HyperCacheGenerator
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Linq;

    public static class RoslynExtensions
    {
        public static bool HaveAttribute(this ClassDeclarationSyntax classSyntax, string attributeName)
        {
            return classSyntax.AttributeLists.Count > 0 &&
                   classSyntax.AttributeLists.SelectMany(al => al.Attributes
                           .Where(a => (a.Name as IdentifierNameSyntax).Identifier.Text == attributeName))
                       .Any();
        }
    }
}
