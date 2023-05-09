﻿namespace RSCG_TimeBombComment;
internal class GenerateCommentsAsError
{

    public static void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var s = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: (sn, _) => FindCorrectComment(sn),
            transform: (ctx, _) => GetDataForGeneration(ctx))
            .Where(it => it != null)
            .SelectMany((it, _) => it!)
            ;

        var c = context
            .CompilationProvider
            .Combine(s.Collect());

        context.RegisterSourceOutput(c,
           (spc, source) => Execute(source.Left, source.Right, spc));
    }
    private static Diagnostic GenerateForTB(SyntaxTrivia item)
    {
        string text = item.ToFullString();
        string message = "";

        DiagnosticSeverity severity = DiagnosticSeverity.Warning;
        text = text.Substring(commentStart.Length).Trim();
        message = text;
        if (text.Length >= 10)//yyyy.MM.dd
        {
            if (DateTime.TryParseExact(text.Substring(0, 10), "yyyy-MM-dd", null, DateTimeStyles.AssumeUniversal, out var date))
            {
                message = text.Substring(10);
                if (date <= DateTime.UtcNow.Date)
                {
                    severity = DiagnosticSeverity.Error;
                }
                else
                {
                    severity = DiagnosticSeverity.Hidden;
                }
            }
        }

        string desc = text;
        var dd = new DiagnosticDescriptor(DiagnosticId, Title, message, Category, severity, isEnabledByDefault: true, description: desc);
        var dg = Diagnostic.Create(dd, item.GetLocation());
        return dg;

    }
    private static void Execute(Compilation compilation, ImmutableArray<SyntaxTrivia> comments, SourceProductionContext spc)
    {
        if (comments.IsDefaultOrEmpty)
            return;
        var data = comments.Distinct();
        //int nr = 0;
        bool isOnRelease = (compilation.Options?.OptimizationLevel == OptimizationLevel.Release);
        foreach (var item in data)
        {
            var text = item.ToFullString();
            if (text.StartsWith(commentStart))
            {
                spc.ReportDiagnostic(GenerateForTB(item));
                continue;
            }
            if (isOnRelease)
            {
                if (text.StartsWith(commentDebug1) || text.StartsWith(commentDebug2))
                {
                    spc.ReportDiagnostic(GenerateForJFD(item));
                }
            }
        }
    }

    private static Diagnostic GenerateForJFD(SyntaxTrivia item)
    {
        var severity = DiagnosticSeverity.Error;
        var text = item.ToFullString();
        string message = text;
        string desc = text;
        var dd = new DiagnosticDescriptor(DiagnosticId, Title, message, Category, severity, isEnabledByDefault: true, description: desc);
        var dg = Diagnostic.Create(dd, item.GetLocation());
        return dg;

    }

    private static readonly string DiagnosticId = "TB";
    private static readonly string Title = "TB";
    private static readonly string Category = "TB";
    internal static string commentStart = "//TB:";
    internal static string commentDebug1 = "//JFD:";
    internal static string commentDebug2 = "//Just for debug:";
    internal static string obsoleteStart = "TB_";

    private static SyntaxTrivia[]? GetDataForGeneration(GeneratorSyntaxContext context)
    {
        List<SyntaxTrivia> ret = new();
        var atts = context.Node as BlockSyntax;
        if (atts == null) return null;
        var tr = atts.DescendantTrivia();
        foreach (var item in tr)
        {
            var text = item.ToFullString();
            if (text.Contains(commentStart))
            {
                ret.Add(item);
            }
            if (text.Contains(commentDebug1))
            {
                ret.Add(item);
            }
            if (text.Contains(commentDebug2))
            {
                ret.Add(item);
            }
        }
        return ret.ToArray();

    }
    private static bool FindCorrectComment(SyntaxNode node)
    {
        if (node is BlockSyntax block)
        {
            var str = node.ToFullString();
            if (str.Contains(commentStart))
            {
                return true;
            }
        }
        return false;

    }


}
