using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.Xna.Framework;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;

using VBY.Common;
using VBY.Common.CommandV2;

namespace VBY.ItemStrengthen;

[ApiVersion(2, 1)]
public partial class ItemStrengthen : TerrariaPlugin
{
    public override string Name => GetType().Name;
    public override string Description => "武器强化";
    public override string Author => "yu";
    public override Version Version => GetType().Assembly.GetName().Version!;
    private Config Config = new(TShock.SavePath);
    private readonly Item itemInfo = new();
    private readonly Item defaultItemInfo = new();
    private static readonly string[] CanSetFieldNames = { 
        nameof(Item.type), 
        nameof(Item.scale), 
        nameof(Item.width), 
        nameof(Item.height), 
        nameof(Item.damage), 
        nameof(Item.useTime), 
        nameof(Item.knockBack), 
        nameof(Item.shoot), 
        nameof(Item.shootSpeed), 
        nameof(Item.useAnimation), 
        nameof(Item.ammo),
        nameof(Item.color), 
        nameof(Item.useAmmo), 
        nameof(Item.notAmmo) 
    };
    private static readonly string[] SetStrings = CanSetFieldNames.Select(x => x.ToLower()).ToArray();
    private static readonly Dictionary<string, int> SetShortStrings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Action<Item, string>[] SetActions = CanSetFieldNames.Select(x => GetParseSet<Item>(x)).ToArray();
    private SubCmdRoot CmdCommand, CtlCommand;
    private Command[] AddCommands;
    static ItemStrengthen()
    {
        SetShortStrings.Add("t", SetStrings.IndexOf(nameof(Item.type)));
        SetShortStrings.Add("sc", SetStrings.IndexOf(nameof(Item.scale)));
        SetShortStrings.Add("w", SetStrings.IndexOf(nameof(Item.width)));
        SetShortStrings.Add("h", SetStrings.IndexOf(nameof(Item.height)));
        SetShortStrings.Add("d", SetStrings.IndexOf(nameof(Item.damage)));
        SetShortStrings.Add("ut", SetStrings.IndexOf(nameof(Item.useTime)));
        SetShortStrings.Add("kb", SetStrings.IndexOf(nameof(Item.knockBack)));
        SetShortStrings.Add("sh", SetStrings.IndexOf(nameof(Item.shoot)));
        SetShortStrings.Add("sp", SetStrings.IndexOf(nameof(Item.shootSpeed)));
        SetShortStrings.Add("uan", SetStrings.IndexOf(nameof(Item.useAnimation)));
        SetShortStrings.Add("a", SetStrings.IndexOf(nameof(Item.ammo)));
        SetShortStrings.Add("c", SetStrings.IndexOf(nameof(Item.color)));
        SetShortStrings.Add("uam", SetStrings.IndexOf(nameof(Item.useAmmo)));
        SetShortStrings.Add("na", SetStrings.IndexOf(nameof(Item.notAmmo)));
    }
    public ItemStrengthen(Main game) : base(game)
    {
        CmdCommand = new("ItemStrengthenCtl")
        {
            new SubCmdRun("Flush", "刷新", CmdFlush, "f", "flush"),
            new SubCmdRun("List", "列表", CmdList, "l", "list")
        };
        CtlCommand = new("ItemStrengthenCtl");
        CtlCommand.Adds(2, CtlDefault, CtlGive, CtlPrint, CtlSet);
        AddCommands = new Command[] { CmdCommand.GetCommand("vby.itemstrengthen.use", new string[] { "is" }), CtlCommand.GetCommand("isc", new string[] { "isc" }) };
    }

    public override void Initialize()
    {
        Config.Read(true);
        Commands.ChatCommands.AddRange(AddCommands);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var cmd in AddCommands)
                Commands.ChatCommands.Remove(cmd);
        }
        base.Dispose(disposing);
    }
    public void CmdFlush(SubCmdArgs args)
    {
    }
    public void CmdList(SubCmdArgs args)
    {

    }
    [Description("Default")]
    public void CtlDefault(SubCmdArgs args)
    {
        try
        {
            var type = args.Parameters.Count > 0 ? int.Parse(args.Parameters[0]) : itemInfo.type;
            itemInfo.SetDefaults(type);
            defaultItemInfo.SetDefaults(type);
            args.Player.SendSuccessMessage("set default type({0}) success", type);
        }
        catch(FormatException)
        {
            args.Player.SendErrorMessage($"格式错误 {args.Parameters[0]}");
        }
    }
    [Description("Give")]
    public void CtlGive(SubCmdArgs args)
    {
        var playerIndex = int.Parse(args.Parameters.ElementAtOrDefault(0, args.Player.Index.ToString()));
        if(playerIndex == -1)
        {
            args.Player.SendErrorMessage("not real player can't give");
            return;
        }
        var itemIndex = Item.NewItem(null, TShock.Players[playerIndex].TPlayer.Center, Vector2.Zero, itemInfo.type);
        var item = Main.item[itemIndex];
        item.playerIndexTheItemIsReservedFor = playerIndex;
        item.scale = itemInfo.scale;
        item.width = itemInfo.width;
        item.height = itemInfo.height;
        item.damage = itemInfo.damage;
        item.useTime = itemInfo.useTime;
        item.knockBack = itemInfo.knockBack;
        item.shootSpeed = itemInfo.shootSpeed;
        item.useAnimation = itemInfo.useAnimation;
        item.ammo = itemInfo.ammo;
        item.shoot = itemInfo.shoot;
        item.color = itemInfo.color;
        item.useAmmo = itemInfo.useAmmo;
        item.notAmmo = itemInfo.notAmmo;
        TSPlayer.All.SendData(PacketTypes.UpdateItemDrop, "", itemIndex);
        TSPlayer.All.SendData(PacketTypes.ItemOwner, "", itemIndex);
        TSPlayer.All.SendData(PacketTypes.TweakItem, "", itemIndex, 255, 63);
    }
    [Description("Print")]
    public void CtlPrint(SubCmdArgs args)
    {
        args.Player.SendInfoMessage(
            $"type={itemInfo.type}\n" +
            $"name={itemInfo.Name}\n" +
            $"scale={itemInfo.scale}{DefaultStr(itemInfo.scale == defaultItemInfo.scale)}\n" +
            $"width={itemInfo.width}{DefaultStr(itemInfo.width == defaultItemInfo.width)}\n" +
            $"height={itemInfo.height}{DefaultStr(itemInfo.height == defaultItemInfo.height)}\n" +
            $"damage={itemInfo.damage}{DefaultStr(itemInfo.damage == defaultItemInfo.damage)}\n" +
            $"useTime={itemInfo.useTime}{DefaultStr(itemInfo.useTime == defaultItemInfo.useTime)}\n" +
            $"knockBack={itemInfo.knockBack}{DefaultStr(itemInfo.knockBack == defaultItemInfo.knockBack)}\n" +
            $"shootSpeed={itemInfo.shootSpeed}{DefaultStr(itemInfo.shootSpeed == defaultItemInfo.shootSpeed)}\n" +
            $"useAnimation={itemInfo.useAnimation}{DefaultStr(itemInfo.useAnimation == defaultItemInfo.useAnimation)}\n" +
            $"ammo={itemInfo.ammo}{DefaultStr(itemInfo.ammo == defaultItemInfo.ammo)}\n" +
            $"shoot={itemInfo.shoot}{DefaultStr(itemInfo.shoot == defaultItemInfo.shoot)}\n" +
            $"color={itemInfo.color}{DefaultStr(itemInfo.color == defaultItemInfo.color)}\n" +
            $"useAmmo={itemInfo.useAmmo}{DefaultStr(itemInfo.useAmmo == defaultItemInfo.useAmmo)}\n" +
            $"notAmmo={itemInfo.notAmmo}{DefaultStr(itemInfo.notAmmo == defaultItemInfo.notAmmo)}\n"
            );
    }
    [Description("Set")]
    public void CtlSet(SubCmdArgs args)
    {
        if (args.Parameters.Count < 2)
        {
            args.Player.SendErrorMessage("Less than two parameters");
            return;
        }
        if(args.Parameters.Count % 2 != 0)
        {
            args.Player.SendErrorMessage("The number of parameters must be a multiple of 2");
            return;
        }
        for (int i = 0; i < args.Parameters.Count; i += 2) 
        {
            if(!SetShortStrings.TryGetValue(args.Parameters[i], out var index))
            {
                var where = SetStrings.Where(x => x.StartsWith(args.Parameters[i]));
                if (!where.Any())
                {
                    args.Player.SendErrorMessage("No matching parameters of '{0}'", args.Parameters[i]);
                    continue;
                }
                index = SetStrings.IndexOf(0, x => x == where.First());
            }
            args.Player.SendInfoMessage("{0} => {1}", SetStrings[index], args.Parameters[i + 1]);
            SetActions[index].Invoke(itemInfo, args.Parameters[i + 1]);
        }
        args.Player.SendInfoMessage("Set success");
    }
    private static Action<T, string> GetParseSet<T>(string fieldname)
    {
        var type = typeof(T);
        var method = new DynamicMethod("", null, new Type[] { typeof(Item), typeof(string) });
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        var field = type.GetField(fieldname) ?? throw new Exception($"{type.FullName} field '{fieldname}' not find");
        if (field.FieldType == typeof(Color))
            il.Emit(OpCodes.Call, ((Func<string, Color>)NewColor).Method);
        else
            il.Emit(OpCodes.Call, field.FieldType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, new Type[] { TypeOf.String })!);
        il.Emit(OpCodes.Stfld, field);
        il.Emit(OpCodes.Ret);
        return method.CreateDelegate<Action<T, string>>();
    }
    private static string DefaultStr(bool ceq) => ceq ? "(default)" : "";
    private static Color NewColor(string color) => new(uint.Parse(color));
}