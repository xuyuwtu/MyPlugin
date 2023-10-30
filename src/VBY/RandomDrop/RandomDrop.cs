using MonoMod.RuntimeDetour.HookGen;
using Newtonsoft.Json;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
namespace VBY.RandomDrop;

[ApiVersion(2, 1)]
public class RandomDrop : TerrariaPlugin
{
    public override string Name => GetType().Namespace!;
    public override Version Version => GetType().Assembly.GetName().Version!;
    static List<int> CanDropItemIDs = new(ItemID.Count);
    static List<int> UpdateDropNPCIDs = new();
    static Func<bool[]> GetCanDropItemIDBoolArrayFunc = () => Array.Empty<bool>();
    static bool[] AddedItemIDs = ItemID.Sets.Factory.CreateBoolSet(false, Utils.NoUseItemID);
    static Common.Config.ConfigManager<MainConfig> Config { get; } = new(MainConfig.GetDefaultConfig)
    {
        SerializerSettings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore },
        PostLoadAction = static (instance, _) =>
        {
            if (!string.IsNullOrEmpty(instance.ImportAliases) && File.Exists(instance.ImportAliases))
            {
                foreach (var item in JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(instance.ImportAliases))!)
                {
                    if (!instance.Aliases.ContainsKey(item.Key))
                    {
                        instance.Aliases.Add(item.Key, item.Value);
                    }
                }
            }
        }
    };
    Command AddCommand;
    public RandomDrop(Main game) : base(game)
    {
        AddCommand = new(Cmd, "rd");
    }
    public override void Initialize()
    {
        Commands.ChatCommands.Add(AddCommand);
        Config.Load(TSPlayer.Server);
        InitializeDynamicMethod();
        On.Terraria.NPC.OnGameEventClearedForTheFirstTime += OnNPC_OnGameEventClearedForTheFirstTime;
        On.Terraria.NPC.DoDeathEvents += OnNPC_DoDeathEvents;
        On.Terraria.NPC.NPCLoot_DropItems += OnNPC_NPCLoot_DropItems;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            On.Terraria.NPC.OnGameEventClearedForTheFirstTime -= OnNPC_OnGameEventClearedForTheFirstTime;
            On.Terraria.NPC.DoDeathEvents -= OnNPC_DoDeathEvents;
            On.Terraria.NPC.NPCLoot_DropItems -= OnNPC_NPCLoot_DropItems;
            var owner = HookEndpointManager.GetOwner(OnNPC_DoDeathEvents);
            if (owner is not null)
            {
                ((System.Collections.IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!).Remove(owner);
            }
        }
    }
    private void OnNPC_OnGameEventClearedForTheFirstTime(On.Terraria.NPC.orig_OnGameEventClearedForTheFirstTime orig, int gameEventId)
    {
        orig(gameEventId);
        UpdateCanDropItemIDs();
    }
    private void OnNPC_NPCLoot_DropItems(On.Terraria.NPC.orig_NPCLoot_DropItems orig, NPC self, Player closestPlayer)
    {
        if (CanDropItemIDs.Count == 0)
        {
            Item.NewItem(self.GetItemSource_Loot(), self.Center, default, Main.rand.Next(1, ItemID.Count));
        }
        else
        {
            Item.NewItem(self.GetItemSource_Loot(), self.Center, default, CanDropItemIDs[Main.rand.Next(CanDropItemIDs.Count)]);
        }
    }
    private void OnNPC_DoDeathEvents(On.Terraria.NPC.orig_DoDeathEvents orig, NPC self, Player closestPlayer)
    {
        if (UpdateDropNPCIDs.Contains(self.type))
        {
            UpdateCanDropItemIDs();
        }
        orig(self, closestPlayer);
    }
    private void Cmd(CommandArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage(
                "/rd status\n" +
                "/rd update\n" +
                "/rd reload");
            return;
        }
        switch (args.Parameters[0])
        {
            case "status":
                args.Player.SendInfoMessage($"ID Count: {CanDropItemIDs.Count}");
                var sb = new StringBuilder();
                sb.Append("Not Add IDs:");
                int start = -1, end;
                bool first = false;
                for (int i = 1; i < AddedItemIDs.Length; i++)
                {
                    if (AddedItemIDs[i])
                    {
                        if (start == -1)
                        {
                            start = i;
                        }
                    }
                    else
                    {
                        if (start == -1)
                        {
                            continue;
                        }
                        end = i - 1;
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            sb.Append(", ");
                        }
                        if (start == end)
                        {
                            sb.Append(start);
                        }
                        else
                        {
                            sb.Append($"{start}-{end}");
                        }
                        start = -1;
                    }
                }
                args.Player.SendInfoMessage(sb.ToString());
                break;
            case "update":
                UpdateCanDropItemIDs();
                args.Player.SendInfoMessage("update success");
                break;
            case "reload":
                if (Config.Load(args.Player))
                {
                    AddedItemIDs = ItemID.Sets.Factory.CreateBoolSet(false, Utils.NoUseItemID);
                    InitializeDynamicMethod();
                    args.Player.SendInfoMessage("reload success");
                    goto case "update";
                }
                break;
            default:
                args.Player.SendInfoMessage($"未知命令:{args.Parameters[0]}");
                break;
        }
    }
    private static void InitializeDynamicMethod()
    {
        var updateMethod = new DynamicMethod("GetCanDropItemIDsDynamicMethod", typeof(bool[]), null);
        var il = updateMethod.GetILGenerator();
        il.DeclareLocal(typeof(bool[]));
        il.DeclareLocal(typeof(int));
        il.LdcI4(ItemID.Count);
        il.Emit(OpCodes.Newarr, typeof(bool));
        il.Emit(OpCodes.Stloc_0);
        foreach (var item in Config.Instance.ItemIDs)
        {
            EmitSet(il, item);
        }
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ret);
        GetCanDropItemIDBoolArrayFunc = updateMethod.CreateDelegate<Func<bool[]>>();
        UpdateCanDropItemIDs();
    }
    private static void EmitSet(ILGenerator il, KeyValuePair<string, IncludeIDInfo> item)
    {
        var typeName = item.Key;
        if (typeName.Equals("true", StringComparison.OrdinalIgnoreCase))
        {
            EmitSet(il, item.Value, Config.Instance.ItemIDGroups);
            return;
        }
        bool reversal = false;
        if (typeName[0] == '!')
        {
            reversal = true;
            typeName = typeName.Substring(1);
        }
        if (Config.Instance.Aliases.TryGetValue(typeName, out var realTypeName))
        {
            typeName = realTypeName;
        }
        var memberName = typeName.Substring(typeName.LastIndexOf('.') + 1);
        typeName = typeName.Substring(0, typeName.LastIndexOf('.'));
        var type = typeof(NPC).Assembly.GetType(typeName);
        if (type is null)
        {
            Console.WriteLine($"{typeName} is not find");
            return;
        }
        var member = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Static | BindingFlags.Public).FirstOrDefault();
        if (member is null)
        {
            Console.WriteLine($"{typeName}.{memberName} does not exist.");
            return;
        }
        switch (member.MemberType)
        {
            case MemberTypes.Field:
                var field = (FieldInfo)member;
                if (field.FieldType != typeof(bool))
                {
                    Console.WriteLine("FieldType is not bool");
                    return;
                }
                il.Emit(OpCodes.Ldsfld, field);
                break;
            case MemberTypes.Property:
                var property = (PropertyInfo)member;
                if (property.PropertyType != typeof(bool))
                {
                    Console.WriteLine("PropertyType is not bool");
                    return;
                }
                il.Emit(OpCodes.Call, property.GetGetMethod()!);
                break;
        }
        if (reversal)
        {
            EmitNotBool(il);
        }
        var endifLabel = il.DefineLabel();
        il.Emit(OpCodes.Brfalse, endifLabel);
        EmitSet(il, item.Value, Config.Instance.ItemIDGroups);
        il.MarkLabel(endifLabel);
    }
    private static void EmitSet(ILGenerator il, IncludeIDInfo info, Dictionary<string, IncludeIDInfo> itemGroup)
    {
        if (info.Ints is not null)
        {
            foreach (var id in info.Ints)
            {
                EmitSetID(il, id);
            }
        }
        if (info.Strings is not null)
        {
            foreach (var str in info.Strings)
            {
                if (Regex.IsMatch(str, @"^\d{1,4}$"))
                {
                    EmitSetID(il, int.Parse(str));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}-\d{1,4}$"))
                {
                    int num1 = int.Parse(str.AsSpan(0, str.IndexOf('-')));
                    int num2 = int.Parse(str.AsSpan(str.IndexOf('-') + 1));
                    EmitSetRangeID(il, Math.Min(num1, num2), Math.Max(num1, num2));
                }
                else if (Regex.IsMatch(str, @"^\d{1,4}\+\d{1,2}$"))
                {
                    int start = int.Parse(str.AsSpan(0, str.IndexOf('+')));
                    int count = int.Parse(str.AsSpan(str.IndexOf('+') + 1));
                    EmitSetRangeID(il, start, start + count - 1);
                }
                else if (Regex.IsMatch(str, @"^\d{1,3}\[\d{1,3}-\d{1,3}\]$"))
                {
                    var match = Regex.Match(str, @"^(\d{1,3})\[(\d{1,3})-(\d{1,3})\]$");
                    var baseStr = match.Groups[1].Value;
                    int num1 = int.Parse(baseStr + match.Groups[2].Value);
                    int num2 = int.Parse(baseStr + match.Groups[3].Value);
                    EmitSetRangeID(il, Math.Min(num1, num2), Math.Max(num1, num2));
                }
                else if (itemGroup.TryGetValue(str, out var groupInfo))
                {
                    EmitSet(il, groupInfo, itemGroup);
                }
            }
        }
        if (info.SubItemIDs is not null)
        {
            foreach (var item in info.SubItemIDs)
            {
                EmitSet(il, item);
            }
        }
    }
    private static void EmitSetID(ILGenerator il, int id)
    {
        AddedItemIDs[id] = true;
        il.Emit(OpCodes.Ldloc_0);
        il.LdcI4(id);
        il.Emit(OpCodes.Ldc_I4_1);
        il.Emit(OpCodes.Stelem_I1);
    }
    private static void EmitSetRangeID(ILGenerator il, int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            AddedItemIDs[i] = true;
        }
        var loopStart = il.DefineLabel();
        var loopHead = il.DefineLabel();
        var loopEnd = il.DefineLabel();
        // int i = start
        il.LdcI4(start);
        il.Emit(OpCodes.Stloc_1);

        il.Emit(OpCodes.Br, loopHead);
        il.MarkLabel(loopStart);

        // bools[i] = true
        il.Emit(OpCodes.Ldloc_0);
        il.Emit(OpCodes.Ldloc_1);
        il.Emit(OpCodes.Ldc_I4_1);
        il.Emit(OpCodes.Stelem_I1);

        // i++
        il.Emit(OpCodes.Ldloc_1);
        il.Emit(OpCodes.Ldc_I4_1);
        il.Emit(OpCodes.Add);
        il.Emit(OpCodes.Stloc_1);

        // i <= end
        il.MarkLabel(loopHead);
        il.Emit(OpCodes.Ldloc_1);
        il.LdcI4(end);
        il.Emit(OpCodes.Cgt);
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ceq);
        il.Emit(OpCodes.Brtrue, loopStart);
        il.MarkLabel(loopEnd);
    }
    private static void EmitNotBool(ILGenerator il)
    {
        il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ceq);
    }
    private static void UpdateCanDropItemIDs()
    {
        CanDropItemIDs.Clear();
        Utils.GetTrueIndexAddToList(CanDropItemIDs, GetCanDropItemIDBoolArrayFunc());
    }
}