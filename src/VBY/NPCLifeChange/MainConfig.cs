using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Terraria.ID;

namespace VBY.NpcLifeChange;

public class MainConfig
{
    public string Life = "";
    public string Strength = "";
    public object[] SkipNPCIDs = new object[] { NPCID.CorruptSlime };
    public string[] SkipNPCProperty = new string[] { nameof(Terraria.NPC.townNPC) };
}
public class ChangeInfo
{
    public float Value;
    public ChangeType Type;
    public float GetValue(float value)
    {
        switch (Type)
        {
            case ChangeType.Set:
                return Value;
            case ChangeType.Add:
                return value + Value;
            case ChangeType.Sub:
                return value - Value;
            case ChangeType.Mul:
                return value * Value;
            case ChangeType.Div:
                return value / Value;
            default:
                return value;
        }
    }
    public static bool TryParse(string s, [MaybeNullWhen(false)] out ChangeInfo changeInfo)
    {
        if(string.IsNullOrEmpty(s))
        {
            changeInfo = new() { Value = 0, Type = ChangeType.None };
            return true;
        }
        changeInfo = null;
        if (!Regex.IsMatch(s, @"[+\-*/]\d+\.\d+") && !Regex.IsMatch(s, @"[+\-*/]\d+"))
        {
            return false;
        }
        if (!float.TryParse(s.AsSpan()[1..], out var value))
        {
            return false;
        }
        changeInfo = new ChangeInfo
        {
            Value = value
        };
        switch (s[0])
        {
            case '=':
                changeInfo.Type = ChangeType.Set;
                break;
            case '+':
                changeInfo.Type = ChangeType.Add;
                break;
            case '-':
                changeInfo.Type = ChangeType.Sub;
                break;
            case '*':
                changeInfo.Type = ChangeType.Mul;
                break;
            case '/':
                changeInfo.Type = ChangeType.Div;
                break;
        }
        return true;
    }
}
public enum ChangeType
{
    None, Set, Add, Sub, Mul, Div
}