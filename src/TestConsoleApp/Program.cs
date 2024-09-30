namespace TestConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ids = IDAnalyzer.IDs.GetInt16ID("InvasionID");
            foreach(var keyValue in ids)
            {
                Console.WriteLine($"{keyValue.Key} {keyValue.Value}");
            }
        }
    }
}
