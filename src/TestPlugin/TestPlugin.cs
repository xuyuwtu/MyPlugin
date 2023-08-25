using System.Diagnostics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;

namespace TestPlugin;
[ApiVersion(2, 1)]
public class TestPlugin : TerrariaPlugin
{
    public int count;
    public TestPlugin(Main game) : base(game)
    {
        Commands.ChatCommands.Add(new(args => args.Player.SendInfoMessage(Main.myPlayer.ToString()), "myply"));
    }

    public override void Initialize()
    {
        //On.Terraria.Sign.ReadSign += OnSign_ReadSign;
        //GetDataHandlers.TileEdit.Register(OnTileEdit);
        //GetDataHandlers.PlaceObject.Register(OnPlaceObject);
        //GetDataHandlers.PlayerUpdate.Register(OnPlayerUpdate);
        //On.Terraria.Player.Teleport += OnPlayer_Teleport;
        //On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool;
        On.Terraria.NPC.NewNPC += NPC_NewNPC;
        //On.Terraria.Projectile.NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float += OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float;
    }

    private int OnProjectile_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float(On.Terraria.Projectile.orig_NewProjectile_IEntitySource_float_float_float_float_int_int_float_int_float_float_float orig, Terraria.DataStructures.IEntitySource spawnSource, float X, float Y, float SpeedX, float SpeedY, int Type, int Damage, float KnockBack, int Owner, float ai0, float ai1, float ai2)
    {
        if(Type == 1007)
        {
            Console.WriteLine(new StackTrace().ToString());
        }
        return orig(spawnSource, X, Y, SpeedX, SpeedY, Type, Damage, KnockBack, Owner, ai0, ai1, ai2);
    }
    private void OnPlayer_Teleport(On.Terraria.Player.orig_Teleport orig, Player self, Microsoft.Xna.Framework.Vector2 newPos, int Style, int extraInfo)
    {
        
    }

    private int NPC_NewNPC(On.Terraria.NPC.orig_NewNPC orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Type, int Start, float ai0, float ai1, float ai2, float ai3, int Target)
    {

        if (Type is 222)
        {
            Console.WriteLine(new StackTrace().ToString());
        }
        return orig(source,X, Y, Type, Start, ai0, ai1, ai2, ai3, Target);
    }

    private int OnSign_ReadSign(On.Terraria.Sign.orig_ReadSign orig, int i, int j, bool CreateIfMissing)
    {
        var num = orig(i, j, CreateIfMissing);
        return num;
    }

    private int Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool(On.Terraria.Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
    {
        Console.WriteLine("on 1");
        Console.WriteLine("pre on 1");
        var num = orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
        Console.WriteLine("post on 1");
        if (Type is 473 or 474 or 475)
        {
            Console.WriteLine(new StackTrace().ToString());
            Console.WriteLine($"source:{source}\n" +
                $"X:{X}\n" +
                $"Y:{Y}\n" +
                $"Width:{Width}\n" +
                $"Height:{Height}\n" +
                $"Type:{Type}\n" +
                $"Stack:{Stack}\n" +
                $"noBroadcast:{noBroadcast}\n" +
                $"pfix:{pfix}\n" +
                $"noGrabDlay:{noGrabDelay}\n" +
                $"reverseLookup:{reverseLookup}");
        }
        return num;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            //GetDataHandlers.TileEdit.UnRegister(OnTileEdit);
            //GetDataHandlers.PlaceObject.UnRegister(OnPlaceObject);
            //On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool -= Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool;
        }
        base.Dispose(disposing);
    }
    private void OnTileEdit(object? args, GetDataHandlers.TileEditEventArgs e)
    {
        switch (e.Action)
        {
            case GetDataHandlers.EditAction.KillTile:
                if (Main.tile[e.X, e.Y].type == TileID.DemonAltar && e.Player.SelectedItem.type == ItemID.WoodenHammer)
                {
                    e.Player.Heal(50);
                    count++;
                    if (count == 5)
                    {
                        WorldGen.KillTile(e.X, e.Y);
                        foreach (var type in new int[] { 
                            ItemID.Meowmere, ItemID.StarWrath, 
                            ItemID.NebulaBlaze, ItemID.NebulaArcanum,
                            ItemID.StardustCellStaff, ItemID.StardustDragonStaff,
                            ItemID.VortexBeater, ItemID.Phantasm,
                            ItemID.SolarEruption, ItemID.DayBreak
                        })
                        {
                            Item.NewItem(null, e.X * 16, e.Y * 16, 16, 16, type);
                            Item.NewItem(null, e.X * 16, e.Y * 16, 16, 16, type);
                        }
                        NetMessage.SendTileSquare(-1, e.X, e.Y);
                        count = 0;
                    }
                }
                break;
        }
    }
    private void OnPlaceObject(object? args, GetDataHandlers.PlaceObjectEventArgs e)
    {
        Console.WriteLine(
            $"""
            X:{e.X}
            Y:{e.Y}
            Type:{e.Type}
            Style:{e.Style}
            Alternate:{e.Alternate}
            Direction:{e.Direction}
            """);
        if (e.Type == 14)
        {
            Utils.PlaceAndSendTileSquare3x2(e.X, e.Y, TileID.DemonAltar);
            e.Handled = true;
        }
    }
    private void OnPlayerUpdate(object? args, GetDataHandlers.PlayerUpdateEventArgs e)
    {
        if (e.Control.IsUsingItem && e.Player.SelectedItem.type == ItemID.LifeCrystal || e.Player.SelectedItem.type == ItemID.LifeFruit && e.Player.TPlayer.statLifeMax >= 400)
        {
            e.Player.SelectedItem.stack -= 1;
            TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", e.Player.Index, e.Player.TPlayer.selectedItem);
            e.Player.TPlayer.statLifeMax += 20;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, "", e.Player.Index);
            e.Player.Heal(20);
        }
    }
}
public static class Utils
{
    public static void PlaceAndSendTileSquare3x2(int x, int y, ushort type, int style = 0)
    {
        WorldGen.Place3x2(x, y, type, style);
        NetMessage.SendTileSquare(-1, x - 1, y - 1, 3, 2);
    }
}