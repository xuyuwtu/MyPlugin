using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

using Terraria;
using Microsoft.CodeAnalysis.Diagnostics;
using IDAnalyzer;
using Microsoft.CodeAnalysis.CodeFixes;

namespace TestConsoleApp;

internal class Program
{
    static async Task Main(string[] args)
    {
        var code =
            """
            namespace Terraria
            {
                public class NPC
                {
                    public static bool AnyNPCs(int type) => true;
                }
            }
            namespace ConsoleApp
            {
                public class Program()
                {
                    public static void Main()
                    {
                        System.Console.WriteLine(Terraria.NPC.AnyNPCs(370));
                    }
                }
            }
            """;
        SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
        SyntaxNode root = tree.GetRoot();
        var csharpCompilation = CSharpCompilation.Create("MyCompilation")
                                                 .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                                                 .AddSyntaxTrees(tree);
        var semanticModel = csharpCompilation.GetSemanticModel(tree);
        var newCsharpCompilation = csharpCompilation.WithAnalyzers([new IDAnalyzer.IDAnalyzer()]);
        var diagnostics = await newCsharpCompilation.GetAnalyzerDiagnosticsAsync();
        var codefix = new IDAnalyzerCodeFixProvider();
        
        foreach (var diagnostic in diagnostics)
        {
            Console.WriteLine(diagnostic.GetMessage());
        }
    }
}