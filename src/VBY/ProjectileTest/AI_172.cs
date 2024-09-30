using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBY.ProjectileTest;

partial class ProjectileAIs
{
    public static int[][] hallowIndices = new int[Main.maxProjectiles + 1][];
    public static void AI_172_HallowBossRainbowPelletStorm(Projectile self)
    {
        if (self.localAI[1] <= 90f)
        {
            self.localAI[1] += 1f;
            self.scale = 0.5f;
            self.Opacity = 0.5f;
            float lerpValue = Terraria.Utils.GetLerpValue(0f, 90f, self.localAI[1]);
            self.scale = MathHelper.Lerp(5f, 1f, lerpValue);
            self.Opacity = 1f - (1f - lerpValue * lerpValue);
            return;
        }
        self.scale = 1f;
        self.Opacity = 1f;
        float num = 150f + 10f * self.AI_172_GetPelletStormsCount();
        self.localAI[0] += 1f;
        if (self.localAI[0] >= num)
        {
            self.Kill();
            return;
        }
        self.velocity = Vector2.Zero;
        self.rotation = 0f;
        int num2 = self.AI_172_GetPelletStormsCount();
        if (hallowIndices[self.whoAmI] is null)
        {
            hallowIndices[self.whoAmI] = new int[6 * 3];
            Array.Fill(hallowIndices[self.whoAmI], -1);
        }
        for (int i = 0; i < num2; i++)
        {
            var hallowBossPelletStormInfo = self.AI_172_GetPelletStormInfo(i);
            var array = hallowIndices[self.whoAmI];
            for (int j = 0; j < hallowBossPelletStormInfo.BulletsInStorm; j++)
            {
                var projIndex = i * 3 + j;
                if (hallowBossPelletStormInfo.IsValid(j))
                {
                    var center = hallowBossPelletStormInfo.GetBulletPosition(j, self.Center);
                    if (array[projIndex] == -1)
                    {
                        array[projIndex] = Projectile.NewProjectile(null, center, hallowBossPelletStormInfo.StartAngle.ToRotationVector2() * 10, 919, 1, 0);
                        Main.projectile[array[projIndex]].localAI[1] = 1f;
                        Main.projectile[array[projIndex]].localAI[2] = 255;
                    }
                    //if (Main.projectile.IndexInRange(array[projIndex]))
                    //{
                    //    Main.projectile[array[projIndex]].Center = center;
                    //    Main.projectile[array[projIndex]].netUpdate = true;
                    //}
                }
                else
                {
                    array[projIndex] = -1;
                }
            }
        }
    }

}
