using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IDAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IDAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "TR0000";
    internal static readonly LocalizableString Title = "Change magic numbers into appropriate ID values";
    internal static readonly LocalizableString MessageFormat = "The number {0} should be changed to {1} for readability";
    internal static readonly LocalizableString Description = "Changes magic numbers into appropriate ID values.";
    internal const string Category = "Design";

    internal static DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Info, true, Description);
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        //context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);

        context.RegisterSyntaxNodeAction(EqualsExpressionAction, SyntaxKind.EqualsExpression, SyntaxKind.NotEqualsExpression);
        context.RegisterSyntaxNodeAction(SimpleAssignmentExpressionAction, SyntaxKind.SimpleAssignmentExpression);
        context.RegisterSyntaxNodeAction(InvocationExpressionAction, SyntaxKind.InvocationExpression);
        context.RegisterSyntaxNodeAction(CaseSwitchLabelAction, SyntaxKind.CaseSwitchLabel);
        context.RegisterSyntaxNodeAction(ElementAccessExpressionAction, SyntaxKind.ElementAccessExpression);
    }

    private void ElementAccessExpressionAction(SyntaxNodeAnalysisContext context)
    {
        if (context.Node is ElementAccessExpressionSyntax
            {
                Expression: MemberAccessExpressionSyntax memberAccess,
                ArgumentList.Arguments: [
                {
                    Expression: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression } literalExpression
                }]
            })
        {
            var fullName = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type!.ToString();
            if (ElementAccessExpressionReportFilter.TryGetValue(fullName, out var idFilterInfos))
            {
                for (int i = 0; i < idFilterInfos.Length; i++)
                {
                    var idFilterInfo = idFilterInfos[i];
                    if (memberAccess.Name.Identifier.ValueText.OrdinalEquals(idFilterInfo.MemberName) && int.TryParse(literalExpression.Token.Text, out var id) && idFilterInfo.IdToNameDict.TryGetValue(id, out var name))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), idFilterInfo.Properties, id, $"{idFilterInfo.IdName}.{name}"));
                    }
                }
            }
        }
    }

    public static void EqualsExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (BinaryExpressionSyntax)context.Node;
        ExpressionAction(context, node, node.Left, node.Right);
    }
    public static void SimpleAssignmentExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (AssignmentExpressionSyntax)context.Node;
        ExpressionAction(context, node, node.Left, node.Right);
    }
    public static void CaseSwitchLabelAction(SyntaxNodeAnalysisContext context)
    {
        var node = (CaseSwitchLabelSyntax)context.Node;
        if (node.Value is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression } literalExpression)
        {
            if (node.Parent?.Parent is SwitchStatementSyntax switchStatement)
            {
                ExpressionAction(context, literalExpression, switchStatement.Expression, literalExpression);
            }
        }
    }
    public static void InvocationExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (InvocationExpressionSyntax)context.Node;
        if (node.Expression is not MemberAccessExpressionSyntax memberAccess)
        {
            return;
        }
        var fullName = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type!.ToString();
        if (InvocationExpressionReportFilter.TryGetValue(fullName, out var methodFilterInfos))
        {
            IMethodSymbol? methodSymbol = null;
            var methodName = memberAccess.Name.Identifier.ValueText;
            for (int i = 0; i < methodFilterInfos.Length; i++)
            {
                var methodFilterInfo = methodFilterInfos[i];
                if (!methodName.OrdinalEquals(methodFilterInfo.MethodName))
                {
                    continue;
                }
                if (methodFilterInfo.ArgumentCount != -1)
                {
                    methodSymbol ??= context.SemanticModel.GetSymbolInfo(memberAccess, context.CancellationToken).Symbol as IMethodSymbol;
                    if (methodSymbol is null)
                    {
                        continue;
                    }
                    if (methodSymbol.Parameters.Length != methodFilterInfo.ArgumentCount)
                    {
                        continue;
                    }
                }
                if (node.ArgumentList.Arguments.Count > methodFilterInfo.CheckIndex
                    && node.ArgumentList.Arguments[methodFilterInfo.CheckIndex] is { Expression: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression } literalExpression }
                    && int.TryParse(literalExpression.Token.Text, out var id) && methodFilterInfo.IdToNameDict.TryGetValue(id, out var name))
                {
                    context.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), methodFilterInfo.Properties, id, $"{methodFilterInfo.IdName}.{name}"));
                }
            }
        }
    }
    internal static void ExpressionAction(SyntaxNodeAnalysisContext context, SyntaxNode reportNode, ExpressionSyntax left, ExpressionSyntax right)
    {
        // example: npc.type == 10
        if (left is MemberAccessExpressionSyntax && right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression })
        {
            var memberemberAccess = (MemberAccessExpressionSyntax)left;
            var literalExpression = (LiteralExpressionSyntax)right;
            var fullName = context.SemanticModel.GetTypeInfo(memberemberAccess.Expression, context.CancellationToken).Type!.ToString();
            if (MemberAccessExpressionReportFilter.TryGetValue(fullName, out var idFilterInfos))
            {
                for (int i = 0; i < idFilterInfos.Length; i++)
                {
                    var idFilterInfo = idFilterInfos[i];
                    if (memberemberAccess.Name.Identifier.ValueText.OrdinalEquals(idFilterInfo.MemberName) && int.TryParse(literalExpression.Token.Text, out var id) && idFilterInfo.IdToNameDict.TryGetValue(id, out var name))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), idFilterInfo.Properties, id, $"{idFilterInfo.IdName}.{name}"));
                    }
                }
            }
        }
        else if (left is ElementAccessExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccess } elementAccess && right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression })
        {
            var fullName = context.SemanticModel.GetTypeInfo(memberAccess.Expression, context.CancellationToken).Type!.ToString();
            if (RightElementAccessExpressionReportFilter.TryGetValue(fullName, out var idFilterInfos))
            {
                var literalExpression = (LiteralExpressionSyntax)right;
                for (int i = 0; i < idFilterInfos.Length; i++)
                {
                    var idFilterInfo = idFilterInfos[i];
                    if (memberAccess.Name.Identifier.ValueText.OrdinalEquals(idFilterInfo.MemberName) && int.TryParse(literalExpression.Token.Text, out var id) && idFilterInfo.IdToNameDict.TryGetValue(id, out var name))
                    {
                        context.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), idFilterInfo.Properties, id, $"{idFilterInfo.IdName}.{name}"));
                    }
                }
            }
        }
    }

    internal static FrozenDictionary<string, IdFilterInfo[]> MemberAccessExpressionReportFilter;
    internal static FrozenDictionary<string, IdFilterInfo[]> ElementAccessExpressionReportFilter;
    internal static FrozenDictionary<string, IdFilterInfo[]> RightElementAccessExpressionReportFilter;
    internal static FrozenDictionary<string, MethodFilterInfo[]> InvocationExpressionReportFilter;

    internal static FrozenDictionary<string, FrozenDictionary<int, string>> IDsDict;
    static IDAnalyzer()
    {
        MemberAccessExpressionReportFilter = new Dictionary<string, IdFilterInfo[]>()
        {
            { "Terraria.NPC", [GetIdFilterInfo("type", nameof(IDs.NPCID))] },
            { "Terraria.Item", [GetIdFilterInfo("type", nameof(IDs.ItemID))] },
            { "Terraria.ITile", [GetIdFilterInfo("type", nameof(IDs.TileID)), GetIdFilterInfo("wall", nameof(IDs.WallID))] },
            { "Terraria.Projectile", [GetIdFilterInfo("type", nameof(IDs.ProjectileID))] },
            { "Terraria.Main", [GetIdFilterInfo("invasionType", nameof(IDs.InvasionID))] },
            { "Terraria.Player", [GetIdFilterInfo("difficulty", nameof(IDs.PlayerDifficultyID))] },
        }.ToFrozenDictionary(StringComparer.Ordinal);
        ElementAccessExpressionReportFilter = new Dictionary<string, IdFilterInfo[]>()
        {
            { "Terraria.Main", [GetIdFilterInfo("townNPCCanSpawn", nameof(IDs.NPCID))] },
            { "Terraria.NPC", [
                GetIdFilterInfo("buffImmune", nameof(IDs.BuffID))
            ] },
            { "Terraria.Player", [
                GetIdFilterInfo("buffImmune", nameof(IDs.BuffID))
            ] }
        }.ToFrozenDictionary(StringComparer.Ordinal);
        RightElementAccessExpressionReportFilter = new Dictionary<string, IdFilterInfo[]>()
        {
            { "Terraria.NPC", [GetIdFilterInfo("buffType", nameof(IDs.BuffID))] },
            { "Terraria.Player", [GetIdFilterInfo("buffType", nameof(IDs.BuffID))] }
        }.ToFrozenDictionary(StringComparer.Ordinal);

        InvocationExpressionReportFilter = new Dictionary<string, MethodFilterInfo[]>()
        {
            { "Terraria.NPC", [
                GetMethodFilterInfo("AnyNPCs", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("CountNPCS", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("FindFirstNPC", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("SpawnOnPlayer", nameof(IDs.NPCID), 1),
                GetMethodFilterInfo("SpawnBoss", nameof(IDs.NPCID), 2),
                GetMethodFilterInfo("MechSpawn", nameof(IDs.NPCID), 2),
                GetMethodFilterInfo("NewNPC", nameof(IDs.NPCID), 3..),
                GetMethodFilterInfo("SetDefaults", nameof(IDs.NPCID), 0..),
            ] },
            { "Terraria.Item", [
                GetMethodFilterInfo("NewItem", nameof(IDs.ItemID), 3, 9),
                GetMethodFilterInfo("NewItem", nameof(IDs.ItemID), 5, 11),
                GetMethodFilterInfo("SetDefaults", nameof(IDs.ItemID), 0..),
            ] },
            { "Terraria.Projectile", [
                GetMethodFilterInfo("NewProjectile", nameof(IDs.ProjectileID), 3, 10),
                GetMethodFilterInfo("NewProjectile", nameof(IDs.ProjectileID), 5, 12),
            ] },
            { "Terraria.NetMessage", [
                GetMethodFilterInfo("SendData", nameof(IDs.MessageID), 0..),
                GetMethodFilterInfo("TrySendData", nameof(IDs.MessageID), 0..),
            ] },
            { "Terraria.Audio.SoundEngine", [
                GetMethodFilterInfo("PlaySound", nameof(IDs.SoundID), 0..),
            ] },
            { "Terraria.TileObject", [
                GetMethodFilterInfo("CanPlace", nameof(IDs.TileID), 2..)
            ] },
            { "Terraria.Player", [
                GetMethodFilterInfo("IsTileTypeInInteractionRange", nameof(IDs.TileID), 0..),
                GetMethodFilterInfo("isNearNPC", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("AddBuff", nameof(IDs.BuffID), 0)
            ] }
        }.ToFrozenDictionary(StringComparer.Ordinal);

        IDsDict = new IDsDictBuilder()
            .Add(nameof(IDs.NPCID))
            .Add(nameof(IDs.TileID))
            .Add(nameof(IDs.WallID))
            .Add(nameof(IDs.MessageID))
            .Add(nameof(IDs.InvasionID))
            .Add(nameof(IDs.ProjectileID))
            .Add(nameof(IDs.PlayerDifficultyID))
            .Add(nameof(IDs.SoundID))
            .Add(nameof(IDs.BuffID))
            .Build();
    }
    private static IdFilterInfo GetIdFilterInfo(string memberName, string idName) => new(memberName, IDs.GetInt32ID(idName), AddType(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, int checkIndex, int argumentCount) => new(methodName, checkIndex, argumentCount, IDs.GetInt32ID(idName), AddType(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, int checkIndex) => new(methodName, checkIndex, checkIndex + 1, IDs.GetInt32ID(idName), AddType(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, Range range) => new(methodName, range.Start.Value, -1, IDs.GetInt32ID(idName), AddType(idName), idName);
    private static ImmutableDictionary<string, string?> AddType(string type)
    {
        const string key = "type";
        var builder = ImmutableDictionary.CreateBuilder<string, string?>(StringComparer.Ordinal);
        builder[key] = type;
        return builder.ToImmutable();
    }
}
struct IDsDictBuilder
{
    private Dictionary<string, FrozenDictionary<int, string>> _dict;
    public IDsDictBuilder()
    {
        _dict = new();
    }
    public IDsDictBuilder Add(string idName)
    {
        _dict.Add(idName, IDs.GetInt32ID(idName));
        return this;
    }
    public FrozenDictionary<string, FrozenDictionary<int, string>> Build() => _dict.ToFrozenDictionary(StringComparer.Ordinal);
}
internal sealed class IdFilterInfo(string memberName, FrozenDictionary<int, string> idToNameDict, ImmutableDictionary<string, string?> properties, string idName)
{
    public string MemberName = memberName;
    public FrozenDictionary<int, string> IdToNameDict = idToNameDict;
    public ImmutableDictionary<string, string?> Properties = properties;
    public string IdName = idName;
}

public sealed class MethodFilterInfo
{
    public string MethodName;
    public int ArgumentCount;
    public int CheckIndex;
    public FrozenDictionary<int, string> IdToNameDict;
    public ImmutableDictionary<string, string?> Properties;
    public string IdName;

    public MethodFilterInfo(string methodName, int checkIndex, int argumentCount, FrozenDictionary<int, string> idToNameDict, ImmutableDictionary<string, string?> properties, string idName)
    {
        MethodName = methodName;
        if (argumentCount < 0)
        {
            argumentCount = -1;
        }
        if (argumentCount != -1 && checkIndex >= argumentCount)
        {
            throw new ArgumentException("checkIndex >= argumentCount");
        }
        ArgumentCount = argumentCount;
        CheckIndex = checkIndex;
        IdToNameDict = idToNameDict;
        Properties = properties;
        IdName = idName;
    }
}

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(IDAnalyzerCodeFixProvider)), Shared]
public sealed class IDAnalyzerCodeFixProvider : CodeFixProvider
{
    private const string Title = "Change magic number into appropriate ID value";
    public sealed override ImmutableArray<string> FixableDiagnosticIds => [IDAnalyzer.DiagnosticId];
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        if(root is null)
        {
            return;
        }
        var diagnostic = context.Diagnostics.First();
        var type = diagnostic.Properties["type"]!;
        var diagnosticSpan = diagnostic.Location.SourceSpan;
        if (diagnostic.Id == IDAnalyzer.DiagnosticId)
        {
            foreach (var declaration in root.FindToken(diagnosticSpan.Start).Parent!.AncestorsAndSelf())
            {
                if (declaration is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression })
                {
                    context.RegisterCodeFix(CodeAction.Create(Title, c => ReplaceNodeAsync(type, context.Document, (LiteralExpressionSyntax)declaration, c), Title), diagnostic);
                }
                else if(declaration is PrefixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.UnaryMinusExpression })
                {
                    context.RegisterCodeFix(CodeAction.Create(Title, c => ReplaceNodeAsync(type, context.Document, (PrefixUnaryExpressionSyntax)declaration, c), Title), diagnostic);
                }
            }
        }
    }
    private static Task<Document> ReplaceNodeAsync(string type, Document document, LiteralExpressionSyntax literalExpression, CancellationToken cancellationToken)
    {
        var root = document.GetSyntaxRootAsync(cancellationToken).Result!;
        SyntaxNode newRoot = root.ReplaceNode(literalExpression, SyntaxFactory.IdentifierName(SyntaxFactory.Identifier($"{type}.{IDAnalyzer.IDsDict[type][int.Parse(literalExpression.Token.Text)]}")))!;
        return Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
    private static Task<Document> ReplaceNodeAsync(string type, Document document, PrefixUnaryExpressionSyntax prefixUnaryExpression, CancellationToken cancellationToken)
    {
        var root = document.GetSyntaxRootAsync(cancellationToken).Result!;
        SyntaxNode newRoot = root.ReplaceNode(prefixUnaryExpression, SyntaxFactory.IdentifierName(SyntaxFactory.Identifier($"{type}.{IDAnalyzer.IDsDict[type][int.Parse(prefixUnaryExpression.Parent!.GetText().ToString())]}")))!;
        return Task.FromResult(document.WithSyntaxRoot(newRoot));
    }
}