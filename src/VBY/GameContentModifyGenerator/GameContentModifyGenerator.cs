using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace GameContentModifyGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class GameContentModifyGenerator : IIncrementalGenerator
{
    internal const string AttributeFullName = "VBY.GameContentModify.Config.MemberDataAttribute";
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterImplementationSourceOutput(context.SyntaxProvider.ForAttributeWithMetadataName(AttributeFullName, MetadataPredicate, MetadataTransform).Collect(), MetadataAction);
    }
    private static void MetadataAction(SourceProductionContext context, ImmutableArray<GeneratorAttributeSyntaxContext> sources)
    {
        var sb = new SpaceStringBuilder();
        sb.AppendUsing("System.ComponentModel");
        sb.AppendUsing("Newtonsoft.Json");
        foreach (var item in sources.GroupBy(x => x.TargetSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
        {
            var typeGroup = item.GroupBy(x => x.TargetSymbol.ContainingType, SymbolEqualityComparer.Default);
            var @namespace = item.Key.ToString();
            sb.AppendSpaceLine($"namespace {item.Key}")
              .AppendBrace();
            foreach (var type in typeGroup)
            {
                var fullTypeName = type.Key.ToString();
                var typeName = fullTypeName.Substring(@namespace.Length + 1);
                var typeNames = typeName.Split('.');
                var count = sb.SpaceCount;
                for (int i = 0; i < typeNames.Length; i++)
                {
                    sb.AppendSpaceLine($"partial class {typeNames[i]}")
                      .AppendBrace();
                }
                var list = new List<(string name, string value)>();
                foreach (var sa in type)
                {
                    var attributeData = sa.Attributes.First(x => x.AttributeClass.ToString() == AttributeFullName);
                    var arguments = attributeData.ConstructorArguments;
                    bool spawnPrivateField = false;
                    foreach(var namedArgument in attributeData.NamedArguments)
                    {
                        if(namedArgument.Key == "PrivateField" && (namedArgument.Value.Value?.Equals(true) ?? false))
                        {
                            spawnPrivateField = true;
                            break;
                        }
                    }
                    string defaultValue = null;
                    if(arguments.Length == 1 && sa.TargetNode is VariableDeclaratorSyntax { Initializer: { } } variableDeclaratorSyntax && variableDeclaratorSyntax.Initializer.Value.IsAnyKind(SyntaxKind.NumericLiteralExpression, SyntaxKind.TrueLiteralExpression, SyntaxKind.FalseLiteralExpression))
                    {
                        defaultValue = variableDeclaratorSyntax.Initializer.Value.ToString();
                    }
                    if (defaultValue is null && arguments.Length < 2)
                    {
                        continue;
                    }
                    var description = SymbolDisplay.FormatPrimitive(arguments[0].Value, false, false);
                    var pointIndex = description.IndexOf(':');
                    if (pointIndex == -1 && 
                        (sa.TargetNode.Parent is VariableDeclarationSyntax { Type: PredefinedTypeSyntax { Keyword.Text: "bool" } }) 
                     || (sa.TargetNode is PropertyDeclarationSyntax { Type: PredefinedTypeSyntax { Keyword.Text: "bool" } }))
                    {
                        sb.AppendSpaceLine($"[Description(\"{description}: {{0}}\")]");
                    }
                    else
                    {
                        sb.AppendSpaceLine($"[Description(\"{description}\")]");
                    }
                    if (pointIndex == -1)
                    {
                        sb.AppendSpaceLine($"[JsonProperty(\"{description}\")]");
                    }
                    else
                    {
                        sb.AppendSpaceLine($"[JsonProperty(\"{description.Substring(0, pointIndex)}\")]");
                    }
                    if (defaultValue is null)
                    {
                        if (arguments[1].Value is double d)
                        {
                            defaultValue = d.ToString("0.0");
                        }
                        else
                        {
                            defaultValue = arguments[1].ToCSharpString();
                        }
                    }
                    sb.AppendSpaceLine($"[DefaultValue({defaultValue})]");
                    var memberStaticName = sa.TargetSymbol.Name;
                    string memberType;
                    if (sa.TargetSymbol is IFieldSymbol fieldSymbol)
                    {
                        memberType = fieldSymbol.Type.ToString();
                    }
                    else
                    {
                        memberType = ((IPropertySymbol)sa.TargetSymbol).Type.ToString();
                    }
                    var memberName = memberStaticName.Substring("Static".Length);
                    list.Add((memberName, defaultValue));
                    sb.AppendBrace($"public {memberType} {memberName}");
                    sb.AppendSpaceLine($"get => {memberStaticName};");
                    sb.AppendSpaceLine($"set => {memberStaticName} = value;");
                    sb.ReduceBrace();
                    if (spawnPrivateField)
                    {
                        sb.AppendSpaceLine($"private static {memberType} _{memberStaticName} = {defaultValue};");
                    }
                }
                sb.AppendBrace("public void SetDefaults()");
                foreach(var (name, value) in list)
                {
                    sb.AppendSpaceLine($"{name} = {value};");
                }
                sb.ReduceBrace();
                while (count < sb.SpaceCount)
                {
                    sb.ReduceBrace();
                }
            }
            sb.ReduceBrace();
        };
        context.AddSource($"{nameof(GameContentModifyGenerator)}.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
    }
    private static GeneratorAttributeSyntaxContext MetadataTransform(GeneratorAttributeSyntaxContext context, CancellationToken token) => context;
    private static bool MetadataPredicate(SyntaxNode node, CancellationToken token)
    {
        if (node.IsKind(SyntaxKind.PropertyDeclaration))
        {
            return ((PropertyDeclarationSyntax)node).Identifier.ValueText.StartsWith("Static");
        }
        if (node.IsKind(SyntaxKind.VariableDeclarator) && node.Parent.IsKind(SyntaxKind.VariableDeclaration) && node.Parent!.Parent.IsKind(SyntaxKind.FieldDeclaration))
        {
            return ((VariableDeclaratorSyntax)node).Identifier.ValueText.StartsWith("Static");
        }
        return false;
    }
}
internal class SpaceStringBuilder
{
    int spaceCount = 0;
    public int SpaceCount { get => spaceCount; }
    private StringBuilder stringBuilder = new();
    public SpaceStringBuilder AppendLine(string value)
    {
        stringBuilder.AppendLine(value);
        return this;
    }
    public SpaceStringBuilder AppendSpaceLine(string value)
    {
        for (int i = 0; i < spaceCount; i++)
        {
            stringBuilder.Append("    ");
        }
        stringBuilder.AppendLine(value);
        return this;
    }
    public SpaceStringBuilder AppendBrace()
    {
        stringBuilder.AppendSpaceLine("{", spaceCount);
        spaceCount++;
        return this;
    }
    public SpaceStringBuilder AppendBrace(string value)
    {
        stringBuilder.AppendSpaceLine(value, spaceCount).AppendSpaceLine("{", spaceCount);
        spaceCount++;
        return this;
    }
    public SpaceStringBuilder ReduceBrace()
    {
        spaceCount--;
        stringBuilder.AppendSpaceLine("}", spaceCount);
        return this;
    }
    public SpaceStringBuilder AppendUsing(string @namespace) => AppendSpaceLine($"using {@namespace};");
    public override string ToString() => stringBuilder.ToString();
}
internal static class Extension
{
    public static StringBuilder AppendSpaceLine(this StringBuilder stringBuilder, string value, int spaceCount)
    {
        for (int i = 0; i < spaceCount; i++)
        {
            stringBuilder.Append("    ");
        }
        return stringBuilder.AppendLine(value);
    }
    public static bool IsAnyKind(this SyntaxNode node, params SyntaxKind[] kinds)
    {
        for (int i = 0; i < kinds.Length; i++)
        {
            if (node.IsKind(kinds[i]))
            {
                return true;
            }
        }
        return false;
    }
    public static string GetIdentifierValueText(this SyntaxNode node)
    {
        if (node.IsKind(SyntaxKind.PropertyDeclaration))
        {
            return ((PropertyDeclarationSyntax)node).Identifier.ValueText;
        }
        if (node.IsKind(SyntaxKind.VariableDeclarator) && node.Parent.IsKind(SyntaxKind.VariableDeclaration) && node.Parent!.Parent.IsKind(SyntaxKind.FieldDeclaration))
        {
            return ((VariableDeclaratorSyntax)node).Identifier.ValueText;
        }
        throw new InvalidOperationException($"{node.RawKind}");
    }
}