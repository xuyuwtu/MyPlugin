using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IDAnalyzer;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IDAnalyzerCodeFixProvider)), Shared]
public sealed class IDAnalyzerCodeFixProvider : CodeFixProvider
{
    internal const string Title = "Change magic number into appropriate ID value";
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [IDAnalyzer.DiagnosticId];
    private static FixAllProvider MyFixAllProvider { get; } = FixAllProvider.Create(FixAllAsync, [FixAllScope.Document]);
    public sealed override FixAllProvider GetFixAllProvider() => MyFixAllProvider;
    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if(root is null)
        {
            return;
        }
        foreach (var diagnostic in context.Diagnostics)
        {
            if (TryGetReplaceNode(diagnostic, root, context.Document, out var oldNode, out var newNode))
            {
                context.RegisterCodeFix(CodeAction.Create(Title, ct => ReplaceNodeAsync(root, context.Document, oldNode, newNode, ct), Title), diagnostic);
            }
        }
    }
    internal static bool TryGetReplaceNode(Diagnostic diagnostic, SyntaxNode root, Document document, [MaybeNullWhen(false)] out SyntaxNode oldNode, [MaybeNullWhen(false)] out SyntaxNode newNode)
    {
        oldNode = null;
        newNode = null;
        if (diagnostic.Id != IDAnalyzer.DiagnosticId)
        {
            return false;
        }
        var type = diagnostic.Properties["type"];
        if(type is null)
        {
            return false;
        }
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        foreach (var declaration in root.FindToken(diagnosticSpan.Start).Parent!.AncestorsAndSelf())
        {
            if (declaration is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression } literalExpression)
            {
                oldNode = literalExpression;
                newNode = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier($"{type}.{IDAnalyzer.IDsDict[type][int.Parse(literalExpression.Token.Text)]}"));
                return true;
            }
            else if (declaration is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.UnaryMinusExpression } prefixUnaryExpression)
            {
                oldNode = prefixUnaryExpression;
                newNode = SyntaxFactory.IdentifierName(SyntaxFactory.Identifier($"{type}.{IDAnalyzer.IDsDict[type][int.Parse(prefixUnaryExpression.Parent!.GetText().ToString())]}"));
                return true;
            }
        }
        return false;
    }
    internal static Task<Document> ReplaceNodeAsync(SyntaxNode root, Document document, SyntaxNode replaceNode, SyntaxNode targetNode, CancellationToken token)
    {
        var newRoot = root.ReplaceNode(replaceNode, targetNode)!;
        return Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
    private static async Task<Document?> FixAllAsync(FixAllContext fixAllContext, Document document, ImmutableArray<Diagnostic> diagnostics)
    {
        if (fixAllContext.Scope != FixAllScope.Document)
        {
            return null;
        }
        if (fixAllContext.Document is null)
        {
            return null;
        }
        var root = await fixAllContext.Document.GetSyntaxRootAsync();
        if (root is null)
        {
            return null;
        }
        var replaceNodes = new List<(SyntaxNode oldNode, SyntaxNode newNode)>();
        foreach (var diagnostic in diagnostics)
        {
            if (TryGetReplaceNode(diagnostic, root, fixAllContext.Document, out var oldNode, out var newNode))
            {
                replaceNodes.Add((oldNode, newNode));
            }
        }
        var newRoot = root.ReplaceNodes(replaceNodes.Select(x => x.oldNode), (original, second) =>
        {
            if (fixAllContext.CancellationToken.IsCancellationRequested)
            {
                return original;
            }
            foreach (var item in replaceNodes)
            {
                if (original == item.oldNode)
                {
                    return item.newNode;
                }
            }
            return original;
        })!;
        return fixAllContext.Document.WithSyntaxRoot(newRoot);
    }
    public static async Task<CodeAction?> GetFixAsync(Document document, IEnumerable<Diagnostic> diagnostics)
    {
        var root = await document.GetSyntaxRootAsync();
        if (root is null)
        {
            return null;
        }
        var replaceNodes = new List<(SyntaxNode oldNode, SyntaxNode newNode)>();
        foreach (var diagnostic in diagnostics)
        {
            if (TryGetReplaceNode(diagnostic, root, document, out var oldNode, out var newNode))
            {
                replaceNodes.Add((oldNode, newNode));
            }
        }
        return CodeAction.Create(Title, (token) =>
        {
            var newRoot = root.ReplaceNodes(replaceNodes.Select(x => x.oldNode), (original, second) =>
            {
                if (token.IsCancellationRequested)
                {
                    return original;
                }
                foreach (var item in replaceNodes)
                {
                    if (original == item.oldNode)
                    {
                        return item.newNode;
                    }
                }
                return original;
            })!;
            return Task.FromResult(document.WithSyntaxRoot(newRoot));
        });
    }
}