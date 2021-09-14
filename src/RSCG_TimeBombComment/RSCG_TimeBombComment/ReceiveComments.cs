using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace RSCG_TimeBombComment
{
    internal class ReceiveComments : ISyntaxReceiver
    {
        internal List<SyntaxTrivia> candidates = new List<SyntaxTrivia>();

        internal static string commentStart = "//TB:";
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {

            if (syntaxNode is CompilationUnitSyntax)
            {
                var cu = syntaxNode as CompilationUnitSyntax;
                string s = syntaxNode.ToFullString();

                if (s.Contains(commentStart))
                {
                    var trivia = cu.DescendantTrivia().Where(it => it.IsKind(SyntaxKind.SingleLineCommentTrivia)).ToArray();
                    foreach (var item in trivia)
                    {
                        if (item.ToFullString().Contains(commentStart))
                        {
                            candidates.Add(item);
                        }
                    }
                }
            }
        }
    }
}