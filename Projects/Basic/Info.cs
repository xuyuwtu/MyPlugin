using System.Reflection;

using TShockAPI;

using VBY.Basic.Command;

namespace VBY.Basic;
public static class Info
{
    public static Dictionary<string, string> TypeNames = new() { ["String"] = "str", ["Int32"] = "int", ["Byte"] = "byte", ["TSPlayer"] = "TSPly", [TypeOf.ListOfString.Name] = "str[]"};
}
public static class TypeOf
{
    public static readonly Dictionary<string, MethodInfo> Methods = new();
    public static readonly Type Byte = typeof(byte), Int16 = typeof(short), Int32 = typeof(int), Int64 = typeof(long);
    public static readonly Type[] ValueType = new Type[] { Byte, Int16, Int32, Int64 };
    public static readonly Type Void = typeof(void), String = typeof(string), Object = typeof(object), Char = typeof(char), Boolean = typeof(bool), DateTime = typeof(DateTime);
    public static readonly Type ListOfString = typeof(List<string>), Console = typeof(Console);
    public static readonly Type SubCmdArgs = typeof(SubCmdArgs);
    public static readonly Type TSPlayer = typeof(TSPlayer), CommandArgs = typeof(CommandArgs);
    public static readonly Type SubCmdNodeRun = typeof(SubCmdNodeRun), SubCmdD = typeof(SubCmdD);
}
