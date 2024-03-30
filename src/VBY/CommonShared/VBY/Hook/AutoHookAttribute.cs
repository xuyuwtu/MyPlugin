namespace VBY.Common.Hook;

[AttributeUsage(AttributeTargets.Method)]
public class AutoHookAttribute : Attribute
{
    public bool OnPostInit { get; set; }
    public bool Manual {  get; set; }
    public Type? Type { get; set; }
    public string? HookName { get; set; }
    public AutoHookAttribute() { }
    public AutoHookAttribute(bool onPostInit, bool manual = false) 
    {
        OnPostInit = onPostInit;
        Manual = manual;
    }
    public AutoHookAttribute(Type? type, string? hookName = null)
    {
        Type = type;
        HookName = hookName;
    }
}
