using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using TerrariaApi.Server;

namespace VBY.GameContentExtension;

public class GameContentExtension : TerrariaPlugin
{
    public GameContentExtension(Main game) : base(game)
    {
    }
    public override void Initialize()
    {
        On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
        On.Terraria.Projectile.Kill += OnProjectile_Kill;
        //On.Terraria.GameContent.ItemDropRules.CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0 += OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float -= OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
            On.Terraria.Projectile.Kill -= OnProjectile_Kill;
            //On.Terraria.GameContent.ItemDropRules.CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0 -= OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0;
        }
    }
    private static int OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float(On.Terraria.Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, Terraria.DataStructures.IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2)
    {
        var targetIndex = -1;
        if (Type == ProjectileID.FallingStar && Damage == 999)
        {
            var minLength = -1f;
            var projPosition = new Vector2(X, Y);
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (!Main.player[i].active || Main.player[i].dead)
                {
                    continue;
                }

                var length = (Main.player[i].position - projPosition).Length();
                if (minLength == -1f || length < minLength)
                {
                    targetIndex = i;
                    minLength = length;
                }
            }
            do
            {
                if (targetIndex == -1)
                {
                    continue;
                }
                var targetPosition = Main.player[targetIndex].Center;
                if (Math.Abs(targetPosition.X - projPosition.X) > 16f * 420)
                {
                    targetIndex = -1;
                    continue;
                }
                //var velocity = (targetPosition - projPosition).ToRotation().ToRotationVector2() * 10;
                var velocity = (targetPosition - projPosition).ToRotation().ToRotationVector2() * 8;
                SpeedX = velocity.X;
                SpeedY = velocity.Y;
                //Type = ProjectileID.Starfury;
                //Damage = 10;
                //ai1 = targetPosition.Y;
            } while (false);
        }
        if(targetIndex != -1)
        {
            Main.projPet[Type] = true;
        }
        var index = orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
        if (targetIndex != -1)
        {
            Main.projPet[Type] = false;
        }
        return index;
    }
    private static void OnProjectile_Kill(On.Terraria.Projectile.orig_Kill orig, Projectile self)
    {
        if(self is null)
        {
            return;
        }
        if ((self.type == ProjectileID.FallingStar && self.damage is 999 or 1000) || (self.type == ProjectileID.Starfury && self.owner == Main.myPlayer && self.damage == 10))
        {
            Projectile.NewProjectile(null, self.position, Vector2.Zero, ProjectileID.TNTBarrel, self.damage, 0);
        }
        orig(self);
    }
    private static void OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0(On.Terraria.GameContent.ItemDropRules.CommonCode.orig_DropItemLocalPerClientAndSetNPCMoneyTo0 orig, NPC npc, int itemId, int stack, bool interactionRequired)
    {
        if (ItemID.Sets.BossBag[itemId])
        {
            return;
        }
        orig(npc, itemId, stack, interactionRequired);
    }
}
