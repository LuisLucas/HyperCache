namespace HyperCache.Helper
{
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    public static class ClassDeclarationExtensions
    {
        public static string GetClassName(this ClassDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Identifier.ValueText;
        }

        public static string GetNameSpace(this ClassDeclarationSyntax typeDeclaration)
        {
            var nameSpaceDeclaration = (NamespaceDeclarationSyntax)typeDeclaration.Parent;
            return nameSpaceDeclaration.Name.ToString();
        }
    }
}
