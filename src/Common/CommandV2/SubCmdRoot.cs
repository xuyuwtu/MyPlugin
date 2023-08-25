namespace VBY.Common.CommandV2;

public class SubCmdRoot : SubCmdList
{
    public override SubCmdList? Parent 
    { 
        get => throw new Exception("SubCmdRoot don't have Parent"); 
        internal set => throw new Exception("SubCmdRoot don't need Parent"); 
    }
    public SubCmdRoot(string cmdName) : base(cmdName, "", cmdName.ToLower()) { }
    public SubCmdRoot(string cmdName, string permission) : this(cmdName)
    {
        AllowInfo.Permission = permission;
    }
}