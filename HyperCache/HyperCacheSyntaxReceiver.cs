namespace HyperCacheGenerator
{
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System.Collections.Generic;

    public class HyperCacheSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> CandidateProxies { get; } = new List<ClassDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax && classSyntax.HaveAttribute(HyperCacheAttribute.Name))
            {
                //var className = classSyntax.Identifier.ValueText;
                //var interfaceName = classSyntax.BaseList.Types.FirstOrDefault();
                CandidateProxies.Add(classSyntax);
            }
        }
    }

    public class HyperCacheCandidates
    {
        public string ClassName { get; set; }

        public ClassDeclarationSyntax ClassDeclarationSyntax { get; set; }
    }
}
