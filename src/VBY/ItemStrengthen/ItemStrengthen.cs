using System.ComponentModel;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using Microsoft.Xna.Framework;

using Terraria;

using TerrariaApi.Server;

using TShockAPI;

using VBY.Common.CommandV2;
using VBY.Common.Config;

namespace VBY.ItemStrengthen;

[ApiVersion(2, 1)]
public partial class ItemStrengthen : TerrariaPlugin
{
    public override string Name { get; }
    public override string Description => "武器强化";
    public override string Author => "yu";
    public override Version Version => GetType().Assembly.GetName().Version!;
    private ConfigManager<Configuration> Config = new("Config", "VBY.ItemStrengthen.json", static () => new()
    {
        IDs = new()
        {
            { "史莱姆王", 1 },
            { "克眼", 2 },
            { "世吞克脑", 3 },
            { "蜂王", 4},
            { "骷髅王", 5},
            { "鹿角怪", 6},
            { "困难模式", 7},
            { "史莱姆皇后", 8 },
            { "任意机械BOSS", 9 },
            { "毁灭者", 10 },
            { "双子魔眼", 11 },
            { "机械骷髅王", 12 },
            { "世纪之花", 13 },
            { "石巨人", 14 },
            { "猪鲨", 15 },
            { "光女", 16 },
            { "教徒", 17 },
            { "日耀柱", 18 },
            { "星云柱", 19 },
            { "星璇柱", 20 },
            { "星尘柱", 21 },
            { "月总", 22 },
            { "衰木", 23 },
            { "南瓜王", 24 },
            { "常绿尖叫怪", 25 },
            { "圣诞坦克", 26 },
            { "冰雪女王", 27 },
            { "四柱", 28 },
            { "血月小丑", 29 },
            { "哥布林入侵", 30 },
            { "海盗入侵", 31 },
            { "火星暴乱", 32 },
        }
    });

    private static void OnPreSave(Configuration configuration)
    {
        throw new NotImplementedException();
    }

    private readonly ItemModifyInfo itemInfo;
    private readonly Item defaultItemInfo;
    private static readonly string[] CanSetFieldNames = { 
        nameof(Item.prefix),
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
    private static readonly string[] SetStrings = CanSetFieldNames.Select(static x => x.ToLowerInvariant()).ToArray();
    private static readonly Dictionary<string, int> SetShortStrings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Action<ItemModifyInfo, string>[] SetActions = CanSetFieldNames.Select(static x => GetParseSet<ItemModifyInfo>(x)).ToArray();
    private SubCmdRoot CmdCommand, CtlCommand;
    private Command[] AddCommands;
    static ItemStrengthen()
    {
        SetShortStrings.Add("p", SetStrings.IndexOf(nameof(ItemModifyInfo.prefix), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("sc", SetStrings.IndexOf(nameof(ItemModifyInfo.scale), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("w", SetStrings.IndexOf(nameof(ItemModifyInfo.width), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("h", SetStrings.IndexOf(nameof(ItemModifyInfo.height), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("d", SetStrings.IndexOf(nameof(ItemModifyInfo.damage), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("ut", SetStrings.IndexOf(nameof(ItemModifyInfo.useTime), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("kb", SetStrings.IndexOf(nameof(ItemModifyInfo.knockBack), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("sh", SetStrings.IndexOf(nameof(ItemModifyInfo.shoot), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("sp", SetStrings.IndexOf(nameof(ItemModifyInfo.shootSpeed), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("uan", SetStrings.IndexOf(nameof(ItemModifyInfo.useAnimation), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("a", SetStrings.IndexOf(nameof(ItemModifyInfo.ammo), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("c", SetStrings.IndexOf(nameof(ItemModifyInfo.color), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("uam", SetStrings.IndexOf(nameof(ItemModifyInfo.useAmmo), StringComparer.OrdinalIgnoreCase));
        SetShortStrings.Add("na", SetStrings.IndexOf(nameof(ItemModifyInfo.notAmmo), StringComparer.OrdinalIgnoreCase));
    }
    public ItemStrengthen(Main game) : base(game)
    {
        Name = GetType().Namespace ?? nameof(ItemStrengthen);
        defaultItemInfo = new();
        itemInfo = new();
        itemInfo.PrefixChanged += OnItemInfo_PrefixChanged;
        CmdCommand = new("ItemStrengthen")
        {
            new SubCmdRun("Flush", "刷新", CmdFlush, "f", "flush"),
            new SubCmdRun("List", "列表", CmdList, "l", "list")
        };
        CtlCommand = new("ItemStrengthenCtl");
        CtlCommand.Adds(2, CtlDefault, CtlGive, CtlPrint, CtlSet);
        AddCommands = new Command[] { CmdCommand.GetCommand("vby.itemstrengthen.use", new string[] { "is" }), CtlCommand.GetCommand("isc", new string[] { "isc" }) };
    }

    private void OnItemInfo_PrefixChanged(int value)
    {
        if(defaultItemInfo.type > 0)
        {
            defaultItemInfo.Prefix(value);
        }
    }

    public override void Initialize()
    {
        Config.Load(TSPlayer.Server);
        Commands.ChatCommands.AddRange(AddCommands);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            foreach (var cmd in AddCommands)
            {
                Commands.ChatCommands.Remove(cmd);
            }
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
            var prefix = args.Parameters.Count > 1 ? int.Parse(args.Parameters[1]) : 0;
            itemInfo.ResetStats(type);
            defaultItemInfo.SetDefaults(type);
            if (prefix != 0)
            {
                defaultItemInfo.Prefix(prefix);
                args.Player.SendSuccessMessage("set default type({0}) prefix({1}) success", type, defaultItemInfo.prefix);
                itemInfo.prefix = defaultItemInfo.prefix;
            }
            else
            {
                args.Player.SendSuccessMessage("set default type({0}) success", type);
            }
        }
        catch(FormatException ex)
        {
            args.Player.SendErrorMessage($"格式错误 {ex.Message}");
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
        var itemIndex = Item.NewItem(null, TShock.Players[playerIndex].TPlayer.Center, Vector2.Zero, itemInfo.type, 1, false, itemInfo.prefix == -1 ? 0 : itemInfo.prefix);
        var item = Main.item[itemIndex];
        item.playerIndexTheItemIsReservedFor = playerIndex;
        var (flag1, flag2) = itemInfo.Apply(item);
        TSPlayer.All.SendData(PacketTypes.UpdateItemDrop, "", itemIndex);
        TSPlayer.All.SendData(PacketTypes.ItemOwner, "", itemIndex);
        TSPlayer.All.SendData(PacketTypes.TweakItem, "", itemIndex, flag1, flag2);
    }
    [Description("Print")]
    public void CtlPrint(SubCmdArgs args)
    {
        args.Player.SendInfoMessage(new StringBuilder()
                    .AppendFormat("type={0}\n", itemInfo.type)
                    .AppendFormat("name={0}\n", defaultItemInfo.Name)
                    .AppendFormat("prefix={0}\n", itemInfo.prefix)
                    .AppendFormat("scale={0}\n", itemInfo.scale.GetApplyText(defaultItemInfo.scale))
                    .AppendFormat("width={0}\n", itemInfo.width.GetApplyText(defaultItemInfo.width))
                    .AppendFormat("height={0}\n", itemInfo.height.GetApplyText(defaultItemInfo.height))
                    .AppendFormat("damage={0}\n", itemInfo.damage.GetApplyText(defaultItemInfo.damage))
                    .AppendFormat("useTime={0}\n", itemInfo.useTime.GetApplyText(defaultItemInfo.useTime))
                    .AppendFormat("knockBack={0}\n", itemInfo.knockBack.GetApplyText(defaultItemInfo.knockBack))
                    .AppendFormat("shootSpeed={0}\n", itemInfo.shootSpeed.GetApplyText(defaultItemInfo.shootSpeed))
                    .AppendFormat("useAnimation={0}\n", itemInfo.useAnimation.GetApplyText(defaultItemInfo.useAnimation))
                    .AppendFormat("ammo={0}\n", itemInfo.ammo.GetApplyText(defaultItemInfo.ammo))
                    .AppendFormat("shoot={0}\n", itemInfo.shoot.GetApplyText(defaultItemInfo.shoot))
                    .AppendFormat("color={0}\n", itemInfo.color.GetApplyText(defaultItemInfo.color))
                    .AppendFormat("useAmmo={0}\n", itemInfo.useAmmo.GetApplyText(defaultItemInfo.useAmmo))
                    .AppendFormat("notAmmo={0}", itemInfo.notAmmo.GetApplyText(defaultItemInfo.notAmmo))
                    .ToString());
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
            try
            {
                SetActions[index].Invoke(itemInfo, args.Parameters[i + 1]);
                args.Player.SendInfoMessage("{0} => {1}", SetStrings[index], args.Parameters[i + 1]);
            }
            catch(Exception ex)
            {
                args.Player.SendErrorMessage(ex.Message);
            }
        }
    }
    private static Action<T, string> GetParseSet<T>(string memberName)
    {
        var type = typeof(T);
        var method = new DynamicMethod($"DynamicSet{memberName}", null, new Type[] { typeof(ItemModifyInfo), typeof(string) });
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldarg_1);
        var members = type.GetMember(memberName);
        if(members.Length == 0)
        {
            throw new Exception($"{type.FullName} member '{memberName}' not find");
        }
        var member = members[0];
        var isField = member.MemberType == MemberTypes.Field;
        var memberType = isField ? ((FieldInfo)member).FieldType : ((PropertyInfo)member).PropertyType;
        if (memberType.IsValueType)
        {
            Type parseType;
            var isNullable = memberType.IsGenericType;
            Type[] genericTypes = null!;
            if (isNullable)
            {
                genericTypes = memberType.GetGenericArguments();
                parseType = genericTypes[0];
            }
            else
            {
                parseType = memberType;
            }
            if (parseType == typeof(Color))
            {
                il.Emit(OpCodes.Call, ((Func<string, Color>)NewColor).Method);
            }
            else
            {
                il.Emit(OpCodes.Call, parseType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(string) })!);
            }
            if (isNullable)
            {
                il.Emit(OpCodes.Newobj, memberType.GetConstructor(genericTypes)!);
            }
        }
        else
        {
            il.Emit(OpCodes.Call, memberType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(string) })!);
        }
        if (isField)
        {
            il.Emit(OpCodes.Stfld, (FieldInfo)member);
        }
        else
        {
            il.Emit(OpCodes.Call, ((PropertyInfo)member).SetMethod!);
        }
        il.Emit(OpCodes.Ret);
        return method.CreateDelegate<Action<T, string>>();
    }
    private static Color NewColor(string color) => new(uint.Parse(color));
}