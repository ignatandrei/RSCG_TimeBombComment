using Microsoft.CodeAnalysis;
using System;
using System.Globalization;

namespace RSCG_TimeBombComment
{
    [Generator]
    public class GenerateFromComments : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            DiagnosticDescriptor dd=null;
            var rec = context.SyntaxReceiver as ReceiveComments;
            if (rec == null)
                return;
            foreach(var item in rec.candidates)
            {
                
                var text = item.ToFullString().Replace(ReceiveComments.commentStart, "").Trim();
                bool Error = false;
                string message = text;
                string desc = text;
                if (text.Length >= 10)
                {
                    if (DateTime.TryParseExact(text.Substring(0,10), "yyyy-MM-dd", null,DateTimeStyles.AssumeUniversal,out var date))
                    {
                        message = text.Substring(10);
                        if (date <= DateTime.UtcNow.Date)
                        {
                            Error = true;
                        }
                    }
                }
                //dd = new DiagnosticDescriptor(text, text, text, "TB", (Error ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning),true);         
                dd = new DiagnosticDescriptor(DiagnosticId, Title, message, Category, (Error ? DiagnosticSeverity.Error : DiagnosticSeverity.Warning), isEnabledByDefault: true, description: desc);
                var dg=Diagnostic.Create(dd, item.GetLocation());
                context.ReportDiagnostic(dg);
            }
        }
        private static readonly string DiagnosticId = "TB";
        private static readonly string Title = "TB";
        private static readonly string Category = "TB";
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ReceiveComments());
        }
    }
}
