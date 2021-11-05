namespace HyperCacheGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Generic;

    public class HyperCacheSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CacheCandidates { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.HaveAttribute(HyperCacheAttribute.Name))
            {
                CacheCandidates.Add(classSyntax);
            }
        }
    }
}
