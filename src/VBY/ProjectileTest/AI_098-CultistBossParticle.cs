using VBY.ProjectileAI;

namespace VBY.ProjectileTest;

public static partial class ProjectileAIs
{
    public static void AI_098(Projectile self)
    {
        Vector2 destination = new(self.ai[0], self.ai[1]);
        Vector2 distance = destination - self.Center;
        if (distance.Length() < self.velocity.Length())
        {
            if (self.ai[2] == -1)
            {
                ref var info = ref ProjectileAI.Plugin.NewProjectileInfos[self.whoAmI];
                Projectile.NewProjectile(self.GetProjectileSource_FromThis(), self.Center, new Vector2(info.SpeedX, info.SpeedY), info.Type, info.Damage, info.KnockBack, -1, info.AI0, info.AI1, info.AI2);
            }
            self.Kill();
            return;
        }
        distance.Normalize();
        distance *= 15f;
        self.velocity = Vector2.Lerp(self.velocity, distance, 0.1f);
        //for (int num818 = 0; num818 < 2; num818++)
        //{
        //    int num819 = Dust.NewDust(self.Center, 0, 0, 228, 0f, 0f, 100);
        //    Main.dust[num819].noGravity = true;
        //    Dust dust2 = Main.dust[num819];
        //    dust2.position += new Vector2(4f);
        //    dust2 = Main.dust[num819];
        //    dust2.scale += Main.rand.NextFloat() * 1f;
        //}
    }
}
