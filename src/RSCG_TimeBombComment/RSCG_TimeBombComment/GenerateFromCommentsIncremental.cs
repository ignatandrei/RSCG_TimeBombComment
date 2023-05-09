using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RSCG_TimeBombComment;
[Generator]
public class GenerateFromCommentsIncremental : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var s=context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (sn, _) => FindCorrectComment(sn),
            transform: (ctx, _) => GetDataForGeneration(ctx)
            )
            .Where(it => it.Item1 != null && it.Item2 != null);
        var c = context
            .CompilationProvider
            .Combine(s.Collect());

        context.RegisterSourceOutput(c,
           (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private void Execute(Compilation compilation, ImmutableArray<(AttributeSyntax?,ClassDeclarationSyntax?)> obsoleteAttr, SourceProductionContext spc)
    {
        if (obsoleteAttr.IsDefaultOrEmpty)
            return;
        var data = obsoleteAttr.Distinct();
        int nr=0;
        foreach(var item in data)
        {
            if (item.Item1 == null || item.Item2 == null) continue;
            var str = MakeObsoleteVariables(compilation, item.Item1, item.Item2, spc);
            if(str != null)
            {
                string id = item.Item1.Name.ToString() + "_" + (++nr);
                spc.AddSource(id, str);
            }

        }
    }
    private static readonly string DiagnosticId = "TB";
    private static readonly string Title = "TB";
    private static readonly string Category = "TB";
    private static readonly DiagnosticSeverity severity = DiagnosticSeverity.Warning;


    private string? MakeObsoleteVariables(Compilation compilation, AttributeSyntax? syntaxNode, ClassDeclarationSyntax? classForAttribute, SourceProductionContext context)
    {
        if(syntaxNode == null) return null;
        if(classForAttribute == null) return null;
        var id = syntaxNode.Name as IdentifierNameSyntax;
        if (id == null)
            return null;

        var args = syntaxNode.ArgumentList?.Arguments.ToArray();
        if (args?.Length != 2) //message and TB_ 
            return null;
        

        var TB_Date = args
                    .Select(it => it.Expression as IdentifierNameSyntax)
                    .Where(it => it != null)
                    .Select(it => it!.Identifier.Text)
                    .FirstOrDefault(it => it.StartsWith(ReceiveCommentsAndObsolete.obsoleteStart));
        if (TB_Date == null)
            return null;
        var dateLimitString = TB_Date.Substring(ReceiveCommentsAndObsolete.obsoleteStart.Length);
        if (!DateTime.TryParseExact(dateLimitString, "yyyyMMdd", null, DateTimeStyles.AssumeUniversal, out var dateLimit))
        {
            var dd = new DiagnosticDescriptor(DiagnosticId, Title, $"cannot parse {dateLimitString}", Category, severity, isEnabledByDefault: true, description: $"cannot parse {dateLimitString}");
            var dg = Diagnostic.Create(dd, syntaxNode.GetLocation());
            context.ReportDiagnostic(dg);
            return null;
        }
        var Error = (dateLimit <= DateTime.UtcNow.Date).ToString().ToLower();
        var namespaceClass = classForAttribute.Parent as NamespaceDeclarationSyntax;
        if (namespaceClass is null)
        {
            var dd = new DiagnosticDescriptor(DiagnosticId, Title, $"cannot find namespace ", Category, severity, isEnabledByDefault: true, description: $"cannot parse {dateLimitString}");
            var dg = Diagnostic.Create(dd, syntaxNode.GetLocation());
            context.ReportDiagnostic(dg);
            return null;
        }

        var varTb = $@"
namespace {namespaceClass.Name.GetText()} {{
    partial class {classForAttribute!.Identifier.Text} {{ 
        const bool {TB_Date} = {Error};
    }}
}}

                ";
        return varTb;

    }

    private (AttributeSyntax?,ClassDeclarationSyntax?) GetDataForGeneration(GeneratorSyntaxContext context)
    {
        var atts=context.Node as AttributeSyntax;
        if (atts is null) return (null, null);
        return (atts, FindClassParent(atts));
    }
    ClassDeclarationSyntax? FindClassParent(AttributeSyntax att)
    {
        var p = att.Parent;
        while (p != null && ((p as ClassDeclarationSyntax) == null))
        {
            p = p.Parent;
        }
        if (p is null) return null;
        if (p is ClassDeclarationSyntax a) return a;
        return null;
    }
    private bool FindCorrectComment(SyntaxNode node)
    {
        if (node is not AttributeSyntax) return false;
            
        var s = node.ToFullString();            
        return s.StartsWith("Obsolete");
        
        
    }
}
