using System.Collections.ObjectModel;
using System.Collections.Specialized;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;

using TerrariaApi.Server;

using VBY.Common.Hook;

namespace VBY.GameContentExtension;

public class GameContentExtension : TerrariaPlugin
{
    public override string Name => "VBY.GameContentExtension";
    private static readonly ObservableCollection<(Func<Projectile, bool> condition, Action<Projectile> action)> ProjectileKillActions;
    private static readonly ActionHook ProjectileKillHook;
    static GameContentExtension()
    {
        ProjectileKillHook = new ActionHook(static () => On.Terraria.Projectile.Kill += OnProjectile_Kill);
        ProjectileKillActions = new();
    }

    private static void OnActionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems is null)
        {
            ProjectileKillHook.Unregister();
            return;
        }
        if (e.NewItems.Count == 0)
        {
            ProjectileKillHook.Unregister();
        }
        else if (e.NewItems.Count > 0)
        {
            ProjectileKillHook.Register();
        }
    }

    public GameContentExtension(Main game) : base(game)
    {
    }
    public override void Initialize()
    {
        ProjectileKillActions.CollectionChanged += OnActionChanged;
        //On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
        ProjectileKillActions.Add((static self => (self is { type: ProjectileID.FallingStar, damage: 999 } && self.owner == Main.myPlayer) || (self.type == ProjectileID.Starfury && self.owner == Main.myPlayer && self.ai[2] == -1f), static self =>
        {
            Projectile.NewProjectile(null, self.position, Vector2.Zero, ProjectileID.TNTBarrel, 999, 0, Main.myPlayer);
        }
        ));
        ProjectileKillActions.Add((static self => self.type == ProjectileID.TNTBarrel && self.damage == 999 && self.owner == Main.myPlayer, static self =>
        {
            const int radius = 10;
            Vector2 explodeCenter = self.position;
            var center = explodeCenter.ToTileCoordinates();
            int minX = center.X - radius;
            int maxX = center.X + radius;
            int minY = center.Y - radius;
            int maxY = center.Y + radius;
            if (minX < 0)
            {
                minX = 0;
            }
            if (maxX > Main.maxTilesX)
            {
                maxX = Main.maxTilesX;
            }
            if (minY < 0)
            {
                minY = 0;
            }
            if (maxY > Main.maxTilesY)
            {
                maxY = Main.maxTilesY;
            }
            self.ExplodeTiles(explodeCenter, radius, minX, maxX, minY, maxY, self.ShouldWallExplode(explodeCenter, radius, minX, maxX, minY, maxY));
        }
        ));
        //On.Terraria.GameContent.ItemDropRules.CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0 += OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ProjectileKillActions.CollectionChanged -= OnActionChanged;
            //On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float -= OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
            ProjectileKillHook.Unregister();
            //On.Terraria.GameContent.ItemDropRules.CommonCode.DropItemLocalPerClientAndSetNPCMoneyTo0 -= OnCommonCode_DropItemLocalPerClientAndSetNPCMoneyTo0;
        }
    }
    private static int OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float(On.Terraria.Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, Terraria.DataStructures.IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2)
    {
        //var targetIndex = -1;
        if (Type == ProjectileID.FallingStar && Damage == 999 && (Owner == Main.myPlayer || Owner == -1))
        {
            Type = Main.rand.Next(ProjectileID.Meteor1, ProjectileID.Meteor3 + 1);
            //var minLength = -1f;
            //var projPosition = new Vector2(X, Y);
            //for (int i = 0; i < Main.maxPlayers; i++)
            //{
            //    if (!Main.player[i].active || Main.player[i].dead)
            //    {
            //        continue;
            //    }

            //    var length = (Main.player[i].position - projPosition).Length();
            //    if (minLength == -1f || length < minLength)
            //    {
            //        targetIndex = i;
            //        minLength = length;
            //    }
            //}
            //do
            //{
            //    if (targetIndex == -1)
            //    {
            //        break;
            //    }
            //    var targetPosition = Main.player[targetIndex].Center;
            //    if (Math.Abs(targetPosition.X - projPosition.X) > 16f * 420)
            //    {
            //        targetIndex = -1;
            //        break;
            //    }
            //    var velocity = (targetPosition - projPosition).ToRotation().ToRotationVector2() * 8;
            //    SpeedX = velocity.X;
            //    SpeedY = velocity.Y;
            //    Type = ProjectileID.Starfury;
            //    Damage = Main.player[targetIndex].statLifeMax / 9;
            //    ai1 = targetPosition.Y;
            //    ai2 = -1f;
            //} while (false);
        }
        //bool orgPet = false;
        //if(targetIndex != -1)
        //{
        //    orgPet = Main.projPet[Type];
        //    Main.projPet[Type] = true;
        //}
        var index = orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
        //if (targetIndex != -1)
        //{
        //    Main.projPet[Type] = orgPet;
        //}
        return index;
    }
    private static void OnProjectile_Kill(On.Terraria.Projectile.orig_Kill orig, Projectile self)
    {
        orig(self);
        if (self is null)
        {
            return;
        }
        foreach (var (condition, action) in ProjectileKillActions)
        {
            if (condition(self))
            {
                action(self);
            }
        }
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
