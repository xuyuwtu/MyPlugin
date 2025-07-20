using System.Reflection;

using TerrariaApi.Server;

using VBY.ProjectileAI;

namespace VBY.ProjectileTest;
[ApiVersion(2, 1)]
public class Plugin : TerrariaPlugin
{
    public override string Name => "VBY.ProjectileTest";
    public override string Author => "yu";
    public override Version Version => new(1, 0, 0, 0);
    private MethodInfo[] methods = typeof(ProjectileAIs).GetMethods(BindingFlags.Static | BindingFlags.Public);
    public Plugin(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        foreach (MethodInfo method in methods)
        {
            AIs.SetMethod(method);
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (MethodInfo method in methods)
            {
                AIs.RemoveMethod(method);
            }
        }
    }
}
