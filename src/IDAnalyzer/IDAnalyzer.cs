using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace IDAnalyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class IDAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "IDA0000";
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

        context.RegisterSyntaxNodeAction(MethodParameterAction, SyntaxKind.InvocationExpression, SyntaxKind.ObjectCreationExpression, SyntaxKind.ImplicitObjectCreationExpression);
    }
    private static void MethodParameterAction(SyntaxNodeAnalysisContext ctx)
    {
        IMethodSymbol? methodSymbol;
        ArgumentListSyntax? argList;
        var semanticModel = ctx.SemanticModel;
        if (ctx.Node is InvocationExpressionSyntax inv)
        {
            methodSymbol = semanticModel.GetSymbolInfo(inv).Symbol as IMethodSymbol;
            argList = inv.ArgumentList;
        }
        else if (ctx.Node is ObjectCreationExpressionSyntax objCre)
        {
            methodSymbol = semanticModel.GetSymbolInfo(objCre).Symbol as IMethodSymbol;
            argList = objCre.ArgumentList;
        }
        else if (ctx.Node is ImplicitObjectCreationExpressionSyntax impObjCre)
        {
            methodSymbol = semanticModel.GetSymbolInfo(impObjCre).Symbol as IMethodSymbol;
            argList = impObjCre.ArgumentList;
        }
        else
        {
            return;
        }
        if (methodSymbol is null || argList is null)
        {
            return;
        }
        var args = argList.Arguments;
        for (int i = 0; i < args.Count; i++)
        {
            if (args[i] is not { Expression: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression } literalExpression })
            {
                continue;
            }
            var paramSymbol = methodSymbol.Parameters[i];

            AttributeData? idTypeAttr = null;
            foreach (var attr in paramSymbol.GetAttributes())
            {
                if (attr.AttributeClass is { Name: nameof(IDTypeAttribute) } &&
                    attr.AttributeClass.ContainingNamespace.ToDisplayString() == "IDAnalyzer")
                {
                    idTypeAttr = attr;
                    break;
                }
            }
            if (idTypeAttr is not { ConstructorArguments.Length: 1 })
            {
                continue;
            }
            if (idTypeAttr.ConstructorArguments.Length == 0)
            {
                continue;
            }
            var constArg = idTypeAttr.ConstructorArguments[0];
            if (constArg.Kind != TypedConstantKind.Primitive ||
                constArg.Value is not string)
            {
                continue;
            }
            var idType = (string)constArg.Value!;
            if (IDsDict.TryGetValue(idType, out var dict) && int.TryParse(literalExpression.Token.Text, out var id) && dict.TryGetValue(id, out var name))
            {
                ctx.ReportDiagnostic(Diagnostic.Create(Rule, literalExpression.GetLocation(), GetTypeDictionary(idType), id, $"{idType}.{name}"));
            }
        }
    }
    private static void ElementAccessExpressionAction(SyntaxNodeAnalysisContext context)
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
    private static void EqualsExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (BinaryExpressionSyntax)context.Node;
        ExpressionAction(context, node, node.Left, node.Right);
    }
    private static void SimpleAssignmentExpressionAction(SyntaxNodeAnalysisContext context)
    {
        var node = (AssignmentExpressionSyntax)context.Node;
        ExpressionAction(context, node, node.Left, node.Right);
    }
    private static void CaseSwitchLabelAction(SyntaxNodeAnalysisContext context)
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
    private static void InvocationExpressionAction(SyntaxNodeAnalysisContext context)
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
        else if (left is ElementAccessExpressionSyntax { Expression: MemberAccessExpressionSyntax memberAccess } && right is LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NumericLiteralExpression })
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
                GetIdFilterInfo("buffImmune", nameof(IDs.BuffID)),
                GetIdFilterInfo("npcsFoundForCheckActive", nameof(IDs.NPCID))
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
            { "Terraria.Main", [
                GetMethodFilterInfo("StartInvasion", nameof(IDs.InvasionID), 0)
            ] },
            { "Terraria.NPC", [
                GetMethodFilterInfo("AnyNPCs", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("CountNPCS", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("FindFirstNPC", nameof(IDs.NPCID), 0),
                GetMethodFilterInfo("SpawnOnPlayer", nameof(IDs.NPCID), 1),
                GetMethodFilterInfo("SpawnBoss", nameof(IDs.NPCID), 2),
                GetMethodFilterInfo("MechSpawn", nameof(IDs.NPCID), 2),
                GetMethodFilterInfo("NewNPC", nameof(IDs.NPCID), 3..),
                GetMethodFilterInfo("SetDefaults", nameof(IDs.NPCID), 0..),
                GetMethodFilterInfo("AddBuff", nameof(IDs.BuffID), 0..),
                GetMethodFilterInfo("SetEventFlagCleared", nameof(IDs.GameEventClearedID), 1),
                GetMethodFilterInfo("UnlockOrExchangePet", nameof(IDs.NPCID), 1, 4),
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
                GetMethodFilterInfo("isNearNPC", nameof(IDs.NPCID), 0..),
                GetMethodFilterInfo("AddBuff", nameof(IDs.BuffID), 0..),
                GetMethodFilterInfo("IsTileTypeInInteractionRange", nameof(IDs.TileID), 0..),
            ] },
            { "Terraria.WorldGen", [
                GetMethodFilterInfo("PlaceChest", nameof(IDs.TileID), 2..)
            ] }
        }.ToFrozenDictionary(StringComparer.Ordinal);

        IDsDict = IDs.AllID.ToFrozenDictionary();
    }
    private static IdFilterInfo GetIdFilterInfo(string memberName, string idName) => new(memberName, IDs.GetInt32ID(idName), GetTypeDictionary(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, int checkIndex, int argumentCount) => new(methodName, checkIndex, argumentCount, IDs.GetInt32ID(idName), GetTypeDictionary(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, int checkIndex) => new(methodName, checkIndex, checkIndex + 1, IDs.GetInt32ID(idName), GetTypeDictionary(idName), idName);
    private static MethodFilterInfo GetMethodFilterInfo(string methodName, string idName, Range range) => new(methodName, range.Start.Value, -1, IDs.GetInt32ID(idName), GetTypeDictionary(idName), idName);

    private static ImmutableDictionary<string, string?> GetTypeDictionary(string type)
    {
        const string key = "type";
        var builder = ImmutableDictionary.CreateBuilder<string, string?>(StringComparer.Ordinal);
        builder[key] = type;
        return builder.ToImmutable();
    }
}
