namespace VBY.ProjectileAI;

public static class TerrariaExtension
{
    public static void AIOutput(this Projectile npc)
    {
        Console.WriteLine(string.Join(",", npc.ai));
    }
}
