namespace VBY.ItemStrengthen;

public class ModifyInfo
{
    public ModifyTypes Type;
    public float Value;
    public void Apply(ref int value)
    {
        switch (Type)
        {
            case ModifyTypes.Add:
                value += (int)Value;
                break;
            case ModifyTypes.Change:
                value = (int)Value;
                break;
            case ModifyTypes.Increase:
                value = (int)(value * (1 + Value));
                break;
        }
    }
    public void Apply(ref int value, ref byte flag, byte addFlag)
    {
        Apply(ref value);
        flag += addFlag;
    }
    public void Apply(ref float value)
    {
        switch (Type)
        {
            case ModifyTypes.Add:
                value += Value;
                break;
            case ModifyTypes.Change:
                value = Value;
                break;
            case ModifyTypes.Increase:
                value = (value * (1 + Value));
                break;
        }
    }
    public void Apply(ref float value, ref byte flag, byte addFlag)
    {
        Apply(ref value);
        flag += addFlag;
    }
    public static ModifyInfo Parse(string? s)
    {
        if (string.IsNullOrEmpty(s))
        {
            throw new ArgumentException("null or empty string");
        }
        var result = new ModifyInfo();
        if (s.StartsWith('+') || s.StartsWith('-'))
        {
            if (s.EndsWith('%'))
            {
                result.Type = ModifyTypes.Increase;
                result.Value = float.Parse(s[1..^1]) / 100;
            }
            else
            {
                result.Type = ModifyTypes.Add;
                result.Value = float.Parse(s[1..]);
            }
        }
        else if (s.StartsWith("="))
        {
            result.Type = ModifyTypes.Change;
            result.Value = float.Parse(s[1..]);
        }
        else if (char.IsNumber(s[0]))
        {
            result.Type = ModifyTypes.Change;
            result.Value = float.Parse(s);
        }
        else
        {
            throw new ArgumentException($"invalid format '{s}'");
        }
        return result;
    }
    public override string ToString()
    {
        string format = Type switch
        {
            ModifyTypes.Add => "+#;-#;0",
            ModifyTypes.Increase => "+#%;-#%;0",
            _ => "G",
        };
        return Value.ToString(format);
    }
}
