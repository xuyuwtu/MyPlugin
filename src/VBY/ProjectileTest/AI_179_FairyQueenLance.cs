namespace VBY.ProjectileTest;

partial class ProjectileAIs
{
    public static void AI_179_FairyQueenLance(Projectile self)
    {
        self.localAI[0] += 1f;
        if (self.localAI[0] >= 60f)
        {
            self.velocity = self.ai[0].ToRotationVector2() * 40f;
        }
        else
        {
            if (self.localAI[1] is 1f or 2f)
            {
                var targetIndex = (int)self.localAI[2];
                var selfCenter = self.Center;
                if (targetIndex == 255 || Main.player[targetIndex] is { active: false } or { dead: true })
                {
                    var distance = float.MaxValue;
                    for (int i = 0; i < Main.maxPlayers; i++)
                    {
                        if (Main.player[i] is { active: false} or { dead: true })
                        {
                            continue;
                        }
                        var targetDistance = (Main.player[i].Center - selfCenter).Length();
                        if (targetDistance < distance)
                        {
                            distance = targetDistance;
                            targetIndex = i;
                        }
                    }
                }
                if ((int)self.localAI[0] % 6 == 0 && targetIndex != 255 && Main.player[targetIndex] is { active: true, dead: false } targetPlayer)
                {
                    var toTargetVector = targetPlayer.Center - self.Center;
                    if (toTargetVector.Length() < 1600f)
                    {
                        if (self.localAI[1] == 1f)
                        {
                            self.ai[0] = toTargetVector.ToRotation();
                            self.netUpdate = true;
                        }
                        else if (self.localAI[1] == 2f)
                        {
                            self.ai[0] = (targetPlayer.Center - self.Center + targetPlayer.velocity * (60 - self.localAI[0])).ToRotation();
                            self.netUpdate = true;
                        }
                    }
                }
            }
        }
        if (self.localAI[0] >= 360f)
        {
            self.Kill();
            return;
        }
        self.alpha = (int)MathHelper.Lerp(255f, 0f, Terraria.Utils.GetLerpValue(0f, 20f, self.localAI[0], clamped: true));
        self.rotation = self.ai[0];
    }
}
