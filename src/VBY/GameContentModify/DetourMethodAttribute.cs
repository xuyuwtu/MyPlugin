namespace VBY.GameContentModify;

[AttributeUsage(AttributeTargets.Method)]
public class DetourMethodAttribute(bool useParam = false) : Attribute
{
    public bool UseParam { get; set; } = useParam;
}