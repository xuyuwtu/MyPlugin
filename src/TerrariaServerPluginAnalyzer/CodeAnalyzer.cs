using System;
using System.Collections.Immutable;
using System.Composition;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace TerrariaServerPluginAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class CodeAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "TR0002";
    internal static readonly LocalizableString Title = "InvalidCode";
    internal static readonly LocalizableString MessageFormat = "The call '{0}' is no effect in server";
    internal static readonly LocalizableString Description = "should remove code.";
    internal const string Category = "Design";
    internal static DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, true, Description);
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(InvocationExpressionAction, SyntaxKind.InvocationExpression);
    }
    public static void InvocationExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (InvocationExpressionSyntax)context.Node;
        if (node.Parent is not ExpressionStatementSyntax || node.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }
        var fullName = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type!.ToString();
        if ("Terraria.Audio.SoundEngine".OrdinalEquals(fullName))
        {
            var methodName = memberAccess.Name.Identifier.ValueText;
            if (methodName is "PlaySound")
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), "Terraria.Audio.SoundEngine.PlaySound()"));
            }
        }
        else if ("Terraria.Dust".OrdinalEquals(fullName))
        {
            var methodName = memberAccess.Name.Identifier.ValueText;
            if (methodName is "NewDust")
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), "Terraria.Dust.NewDust()"));
            }
        }
        else if ("Terraria.Gore".OrdinalEquals(fullName))
        {
            var methodName = memberAccess.Name.Identifier.ValueText;
            if (methodName is "NewGore")
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), "Terraria.Gore.NewGore()"));
            }
        }
        else if ("Terraria.Lighting".OrdinalEquals(fullName))
        {
            var methodName = memberAccess.Name.Identifier.ValueText;
            if (methodName is "AddLight")
            {
                context.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), "Terraria.Lighting.AddLight()"));
            }
        }
    }
}
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(CodeAnalyzerCodeFixProvider)), Shared]
public sealed class CodeAnalyzerCodeFixProvider : CodeFixProvider
{
    private const string Title = "should remove code";
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [CodeAnalyzer.DiagnosticId];
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root is null)
        {
            return;
        }
        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        if (root.FindToken(diagnosticSpan.Start).Parent is IdentifierNameSyntax { Parent: MemberAccessExpressionSyntax { Parent: InvocationExpressionSyntax { Parent: ExpressionStatementSyntax expressionStatement }} })
        {
            context.RegisterCodeFix(CodeAction.Create(Title, c => 
            {
                var newRoot = root.RemoveNode(expressionStatement, SyntaxRemoveOptions.KeepNoTrivia)!;
                return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
            }
            , Title), diagnostic);
        }
    }
}
