using System.Collections.Immutable;

using IDAnalyzer;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace TestConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var code =
                """
                namespace IDAnalyzer
                {
                    using System;
                    [AttributeUsage(AttributeTargets.Parameter)]
                    [System.Diagnostics.Conditional("ADD_IDTYPEATTRIBUTE")]
                    public class IDTypeAttribute : Attribute
                    {
                        public string Type { get; }
                        public IDTypeAttribute(string type)
                        {
                            Type = type;
                        }
                    }
                }
                namespace Terraria
                {
                    using IDAnalyzer;
                    using Terraria.ID;
                    public class NPC
                    {
                        public static bool AnyNPCs(int type) => true;
                        public static bool Test([IDType(nameof(ItemID))] int type) => true;
                    }
                    namespace ID
                    {
                        public class ItemID
                        {
                            public const ushort CobaltHat = 371;
                        }
                        public class NPCID
                        {
                            public const ushort DukeFishron = 370;
                        }
                    }
                }
                namespace ConsoleApp
                {
                    using System;
                    using IDAnalyzer;
                    using Terraria.ID;
                    public class Program()
                    {
                        public static void Main()
                        {
                            var a = new ItemInfo(370);
                        }
                        static void OutType(ItemInfo info)
                        {
                            Console.WriteLine(info.Type);
                        }
                    }
                    class ItemInfo
                    {
                        public int Type { get; set; }
                        public ItemInfo([IDType(nameof(ItemID))] int type) 
                        {
                            Type = type;
                        }
                    }
                }
                """;
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            var assemblyRoot = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
            MetadataReference[] references = [
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(assemblyRoot, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(assemblyRoot, "System.Console.dll"))
            ];
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            var csharpCompilation = CSharpCompilation.Create("Test", [tree], references, options);
            var semanticModel = csharpCompilation.GetSemanticModel(tree);
            var newCsharpCompilation = csharpCompilation.WithAnalyzers([new IDAnalyzer.IDAnalyzer()]);
            var diagnostics = await newCsharpCompilation.GetAnalyzerDiagnosticsAsync();
            var codefix = new IDAnalyzerCodeFixProvider();
            foreach (var diagnostic in diagnostics)
            {
                Console.WriteLine(diagnostic.GetMessage());
            }
            var ms = new MemoryStream();
            var result = csharpCompilation.Emit(ms);
            if (!result.Success)
            {
                foreach (var diagnostics2 in result.Diagnostics)
                {
                    Console.WriteLine(diagnostics2);
                }
                return;
            }
            var workspace = new AdhocWorkspace();
            var projectId = ProjectId.CreateNewId();
            var docId = DocumentId.CreateNewId(projectId);

            var solution = workspace.CurrentSolution
                .AddProject(projectId, "Test", "Test", LanguageNames.CSharp)
                .AddMetadataReference(projectId, references[0])
                .AddMetadataReference(projectId, references[1])
                .AddMetadataReference(projectId, references[2])
                .AddDocument(docId, "Test.cs", code);

            var document = solution.GetDocument(docId)!;

            var codeFix = new IDAnalyzerCodeFixProvider();
            var actions = new List<CodeAction>();
            await codefix.RegisterCodeFixesAsync(new Microsoft.CodeAnalysis.CodeFixes.CodeFixContext(document, diagnostics[0], (action, diagnostic) => actions.Add(action), default));
            var operations = await actions[0].GetOperationsAsync(default);
            var appliedOp = operations.OfType<ApplyChangesOperation>().Single();

            //var action = await IDAnalyzerCodeFixProvider.GetFixAsync(document, diagnostics)!;
            //var actions = new List<CodeAction>();
            //var operations = await action.GetOperationsAsync(CancellationToken.None);
            //var appliedOp = operations.OfType<ApplyChangesOperation>().Single()!;

            var newSolution = appliedOp.ChangedSolution;
            var newDoc = newSolution.GetDocument(docId)!;
            var newText = (await newDoc.GetTextAsync()).ToString();
            Console.WriteLine(newText);
        }
        private static async Task<ImmutableArray<Diagnostic>> GetDiagnosticsAsync(Document document, DiagnosticAnalyzer analyzer)
        {
            var compilation = await document.Project.GetCompilationAsync()!;
            var compilationWithAnalyzers = compilation!.WithAnalyzers([analyzer]);
            return await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();
        }
    }
}