using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TerrariaPacketSourceGenerator;

[Generator(LanguageNames.CSharp)]
public sealed class TerrariaPacketGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(context.CompilationProvider, CompilationAction);
    }
    internal static void CompilationAction(SourceProductionContext context, Compilation compilation)
    {
        var sb = new SpaceStringBuilder();
        var getFuncsInitMethods = new List<string>();
        var type = compilation.GetTypeByMetadataName("VBY.Shop.TableInfo");
        var members = type.GetTypeMembers();
        sb.AppendUsing("System.Data")
          .AppendUsing("TShockAPI.DB")
          .AppendLine("")
          .AppendLine("namespace VBY.Shop;")
          .AppendBrace("partial class TableInfo");
        foreach (var member in members)
        {
            if (member.IsType)
            {
                sb.AppendBrace($"partial class {member.Name}");
                if (!member.Constructors.Any(x => x.Parameters.Length == 0 && !x.IsStatic))
                {
                    sb.AppendSpaceLine($"public {member.Name}() {{ }}");
                }
                var index = member.Constructors.FindIndex(x => x.Parameters.Length > 0);
                if (index != -1)
                {
                    sb.AppendBrace($"public static {member.Name} Get{member.Name}(IDataReader reader)");
                    sb.AppendSpaceLine($"return new({string.Join(", ", member.Constructors[index].Parameters.Select(x => $"reader.Get<{x.Type}>(nameof({x.Name.FirstUpper()}))"))});");
                    sb.ReduceBrace();
                    getFuncsInitMethods.Add($"{member.Name}.Get{member.Name}");
                }
                sb.ReduceBrace();
            }
        }
        sb.ReduceBrace();

        sb.AppendBrace("partial class Utils")
          .AppendBrace("public static void GetFuncsInitialize()");
        foreach (var methodFullName in getFuncsInitMethods)
        {
            sb.AppendSpaceLine($"GetFuncs.Add(typeof(TableInfo.{methodFullName.Substring(0, methodFullName.IndexOf('.'))}), TableInfo.{methodFullName});");
        }
        sb.ReduceBrace()
          .ReduceBrace();
        context.AddSource(nameof(TerrariaPacketGenerator), sb.ToString());
    }
}
internal class SpaceStringBuilder
{
    int spaceCount = 0;
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
    public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (predicate(list[i]))
            {
                return i;
            }
        }
        return -1;
    }
    public static StringBuilder AppendSpaceLine(this StringBuilder stringBuilder, string value, int spaceCount)
    {
        for (int i = 0; i < spaceCount; i++)
        {
            stringBuilder.Append("    ");
        }
        return stringBuilder.AppendLine(value);
    }
    public static string FirstUpper(this string value)
    {
        var chars = value.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);
        return new(chars);
    }
}