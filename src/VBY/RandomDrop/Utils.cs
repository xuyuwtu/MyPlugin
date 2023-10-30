using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;

namespace VBY.RandomDrop;

public static partial class Utils
{
    internal static int[] NoUseItemID { get; } = new int[] { 3705, 3706, 3847, 3848, 3849, 3850, 3851, 3853, 3861, 3862, 3978, 4143, 4722, 5013 };
#pragma warning disable IDE0060 // 删除未使用的参数
    private static void Name(this bool boolField, string name) { }
#pragma warning restore IDE0060 // 删除未使用的参数
    private static void FieldNameInfo()
    {
        NPC.downedBoss1.Name("克苏鲁之眼");
        NPC.downedBoss2.Name("邪恶Boss");
        NPC.downedBoss3.Name("骷髅王");
        NPC.downedSlimeKing.Name("史莱姆王");
        NPC.downedQueenBee.Name("蜂王");
        NPC.downedDeerclops.Name("独眼巨鹿");
        Main.hardMode.Name("血肉墙");
        NPC.downedQueenSlime.Name("史莱姆皇后");
        NPC.downedMechBossAny.Name("任意机械BOSS");
        NPC.downedMechBoss1.Name("毁灭者");
        NPC.downedMechBoss2.Name("双子魔眼");
        NPC.downedMechBoss3.Name("机械骷髅王");
        NPC.downedPlantBoss.Name("世纪之花");
        NPC.downedGolemBoss.Name("石巨人");
        NPC.downedFishron.Name("猪龙鱼公爵");
        NPC.downedEmpressOfLight.Name("光之女皇");
        NPC.downedAncientCultist.Name("拜月教邪教徒");
        NPC.downedTowerSolar.Name("日耀柱");
        NPC.downedTowerNebula.Name("星云柱");
        NPC.downedTowerVortex.Name("星旋柱");
        NPC.downedTowerStardust.Name("星尘柱");
        NPC.downedMoonlord.Name("月亮领主");
        NPC.downedHalloweenTree.Name("哀木");
        NPC.downedHalloweenKing.Name("南瓜王");
        NPC.downedChristmasTree.Name("常绿尖叫怪");
        NPC.downedChristmasSantank.Name("圣诞坦克");
        NPC.downedChristmasIceQueen.Name("冰雪女王");
        NPC.downedGoblins.Name("哥布林军队");
        NPC.downedFrost.Name("雪人军团");
        NPC.downedPirates.Name("海盗入侵");
        NPC.downedMartians.Name("火星暴乱");
        NPC.savedGoblin.Name("拯救哥布林");
        NPC.savedMech.Name("拯救机械师");
        NPC.savedGolfer.Name("拯救高尔夫球手");
        NPC.savedAngler.Name("拯救渔夫");
        NPC.savedBartender.Name("拯救酒保");
        NPC.savedStylist.Name("拯救发型师");
        NPC.savedTaxCollector.Name("拯救税收管");
        NPC.savedWizard.Name("拯救巫师");
        Main.drunkWorld.Name("醉酒世界");
        Main.notTheBeesWorld.Name("蜂蜜世界");
        Main.getGoodWorld.Name("ftw");
        Main.tenthAnniversaryWorld.Name("十周年世界");
        Main.dontStarveWorld.Name("永恒领域");
        Main.noTrapsWorld.Name("陷阱世界");
        Main.remixWorld.Name("颠倒世界");
        Main.zenithWorld.Name("天顶世界");
        Main.expertMode.Name("专家模式");
        Main.masterMode.Name("大师模式");
    }
    internal static Dictionary<string, string> GetDefaultAliases()
    {
        var aliases = new Dictionary<string, string>();
        var readMethod = new Action(FieldNameInfo);
        var module = readMethod.Method.Module;
        byte[] ilBytes = readMethod.Method.GetMethodBody()!.GetILAsByteArray()!;
        for (int index = 0; index < ilBytes.Length; index++)
        {
            switch (ilBytes[index])
            {
                case OpcodeValue.ldsfld:
                    if (ilBytes[index + 4 + 1] == OpcodeValue.ldstr)
                    {
                        FieldInfo field = module.ResolveField(BitConverter.ToInt32(ilBytes, index + 1))!;
                        index += 4 + 1;
                        aliases.Add(module.ResolveString(BitConverter.ToInt32(ilBytes, index + 1)), $"{field.DeclaringType!.FullName}.{field.Name}");
                        index += 4;
                    }
                    else if (ilBytes[index + 4 + 1] == OpcodeValue.call)
                    {
                        MethodBase method = module.ResolveMethod(BitConverter.ToInt32(ilBytes, index + 1))!;
                        index += 4 + 1;
                        aliases.Add(module.ResolveString(BitConverter.ToInt32(ilBytes, index + 1)), $"{method.DeclaringType!.FullName}.{method.Name.Substring(4)}");
                        index += 4;
                    }
                    break;
            }
        }
        return aliases;
    }
    internal static Dictionary<string, IncludeIDInfo> GetEmptyItemIDs(Dictionary<string, string> aliases)
    {
        var dic = new Dictionary<string, IncludeIDInfo>
        {
            { "true", new(new(),new()) }
        };
        foreach (var alias in aliases.Keys)
        {
            dic.Add(alias, new());
        }
        return dic;
    }
    internal static void PostInitDictionary(this Dictionary<string, IncludeIDInfo> dict)
    {
        foreach (var value in dict.Values)
        {
            value.MakeIntsShort();
            value.SetEmptyListNull();
            value.SubItemIDs?.PostInitDictionary();
        }
    }
    internal static void GetTrueIndexAddToList(List<int> list, bool[] bools, int startIndex = 1)
    {
        list.Clear();
        for (int i = startIndex; i < bools.Length; i++)
        {
            if (!NoUseItemID.Contains(i) && bools[i])
            {
                list.Add(i);
            }
        }
    }
    internal static List<int> GetNPCCommonDropItemID(int npcNetId)
    {
        var list = new List<int>();
        var rules = Main.ItemDropSolver._database.GetRulesForNPCID(npcNetId, false);
        for(int i = 0; i < rules.Count; i++)
        {
            if (rules[i] is CommonDrop commonDrop)
            {
                list.Add(commonDrop.itemId);
            }
        }
        return list;
    }
    internal static void SetNPCCommonDropItemID(int npcNetId, bool[] bools)
    {
        var rules = Main.ItemDropSolver._database.GetRulesForNPCID(npcNetId, false);
        int i,j ,jTarget;
        int iTarget = rules.Count;
        for (i = 0; i < iTarget; i++)
        {
            var rule = rules[i]; 
            if (rule is ItemDropWithConditionRule conditionRule)
            {
                if (conditionRule.condition is Conditions.NotFromStatue or Conditions.NamedNPC)
                {
                    bools[conditionRule.itemId] = true;
                }
            }
            else if (rule is CommonDrop)
            {
                bools[((CommonDrop)rule).itemId] = true;
            }
            else if (rule is OneFromOptionsDropRule oneFromOptionsDropRule)
            {
                jTarget = oneFromOptionsDropRule.dropIds.Length;
                for(j = 0; j < jTarget; j++)
                {
                    bools[oneFromOptionsDropRule.dropIds[i]] = true;
                }
            }
            else if(rule is DropBasedOnExpertMode dropBasedOnExpertMode)
            {
                if(dropBasedOnExpertMode is { ruleForNormalMode: CommonDrop normalRule, ruleForExpertMode: CommonDrop expertRule })
                {
                    if(normalRule.itemId == expertRule.itemId)
                    {
                        bools[normalRule.itemId] = true;
                    }
                }
            }
        }
    }
    internal static List<int> GetMaterialType(int type, bool onlyOne = true)
    {
        var list = new List<int> { type };
        for (int index = 0; index < Main.recipe.Length; index++)
        {
            if (Main.recipe[index] is null) continue;
            var recipe = Main.recipe[index];
            if (onlyOne)
            {
                int count = 0;
                for (int j = 0; j < recipe.requiredItem.Length; j++)
                {
                    if (recipe.requiredItem[j].type > 0)
                    {
                        count++;
                        if (count > 1)
                        {
                            break;
                        }
                    }
                }
                if (count > 1)
                {
                    continue;
                }
                if (recipe.requiredItem[0].type == type)
                {
                    list.Add(recipe.createItem.type);
                }
            }
            else
            {
                if (recipe.requiredItem.Any(x => x.type == type))
                {
                    list.Add(type);
                }
            }
        }
        return list;
    }
    internal static List<int> GetMaterialsType(int type)
    {
        var list = new List<int> { type };
        for (int index = 0; index < Main.recipe.Length; index++)
        {
            if (Main.recipe[index] is null) continue;
            var recipe = Main.recipe[index];
            if (recipe.requiredItem.Any(x => x.type == type))
            {
                recipe.requiredItem.Where(x => x.type > 0, x => list.Add(x.type));
            }
        }
        return list;
    }
    internal static List<int> GetMaterialCreateItemType(int type)
    {
        var list = new List<int> { type };
        for (int index = 0; index < Main.recipe.Length; index++)
        {
            if (Main.recipe[index] is null) continue;
            var recipe = Main.recipe[index];
            if (recipe.requiredItem.Any(x => x.type == type))
            {
                list.Add(recipe.createItem.type);
            }
        }
        return list;
    }
    internal static List<int> GetPredicateItemID(Predicate<Item> predicate)
    {
        var list = new List<int>();
        var item = new Item();
        for (int i = 1; i < ItemID.Count; i++)
        {
            item.SetDefaults(i);
            if (predicate(item))
            {
                list.Add(i);
            }
        }
        return list;
    }
    internal static List<int> AddOreAndBar(this List<int> list, IList<int> ints)
    {
        if(ints.Count % 2 != 0)
        {
            throw new ArgumentException("ints.Length % 2 != 0");
        }
        for(int i = 0 ; i < ints.Count; i += 2)
        {
            list.AddOreAndBar(ints[i], ints[i + 1]);
        }
        return list;
    }
    internal static List<int> AddOreAndBar(this List<int> list, int oreType, int barType, bool onlyOne = true)
    {
        list.Add(oreType);
        list.AddRange(GetMaterialType(barType, onlyOne));
        return list;
    }
    internal static List<int> AddPredicateItemID(this List<int> list, Predicate<Item> predicate)
    {
        var item = new Item();
        for (int i = 1; i < ItemID.Count; i++)
        {
            item.SetDefaults(i);
            if (predicate(item))
            {
                list.Add(i);
            }
        }
        return list;
    }
    public static int GetCount(this Range range) => Math.Abs((range.Start.IsFromEnd ? -range.Start.Value : range.Start.Value) - (range.End.IsFromEnd ? -range.End.Value : range.End.Value));
    public static int[] ToInts(this Range range)
    {
        var min = Math.Min(range.Start.IsFromEnd ? -range.Start.Value : range.Start.Value, range.End.IsFromEnd ? -range.End.Value : range.End.Value);
        var max = Math.Max(range.Start.IsFromEnd ? -range.Start.Value : range.Start.Value, range.End.IsFromEnd ? -range.End.Value : range.End.Value);
        var count = max - min;
        var result = new int[count];
        for (int i = 0; i < count; i++)
        {
            result[i] = min + i;
        }
        return result;
    }
    public static int[] ToInts(params Range[] ranges)
    {
        var array = new int[ranges.Select(x => x.GetCount()).Sum()];
        int startIndex = 0;
        for(int i = 0; i < ranges.Length; i++)
        {
            var range = ranges[i];
            var min = Math.Min(range.Start.IsFromEnd ? -range.Start.Value : range.Start.Value, range.End.IsFromEnd ? -range.End.Value : range.End.Value);
            var max = Math.Max(range.Start.IsFromEnd ? -range.Start.Value : range.Start.Value, range.End.IsFromEnd ? -range.End.Value : range.End.Value);
            var count = max - min;
            for (int j = 0; j < count; j++)
            {
                array[startIndex + j] = min + j;
            }
            startIndex += count;
        }
        return array;
    }
    public static void LdcI4(this ILGenerator il, int i4)
    {
        switch (i4)
        {
            case 0:
                il.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                il.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                il.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                il.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                il.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                il.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if (i4 >= sbyte.MinValue && i4 <= sbyte.MaxValue)
                {
                    il.Emit(OpCodes.Ldc_I4_S, (sbyte)i4);
                }
                else
                {
                    il.Emit(OpCodes.Ldc_I4, i4);
                }
                break;
        }
    }
    public static int GetRequiredItemCount(this Recipe recipe)
    {
        var index = recipe.requiredItem.IndexOf(0, x => x.type == 0);
        if (index == -1)
        {
            return recipe.requiredItem.Length;
        }
        return index;
    }
    public static int GetRequiredItemIsNotCommonCount(this Recipe recipe, bool[] commonList)
    {
        var validRequiredItemCount = recipe.GetRequiredItemCount();
        var notCommonCount = 0;
        for (int i = 0; i < validRequiredItemCount; i++)
        {
            if (!commonList[recipe.requiredItem[i].type])
            {
                notCommonCount++;
            }
        }
        return notCommonCount;
    }
    public static int GetRequiredTileCount(this Recipe recipe)
    {
        var index = recipe.requiredTile.IndexOf(-1);
        if (index == -1)
        {
            return recipe.requiredTile.Length;
        }
        return index;
    }
    public static int GetRequiredTileIsNotCommonCount(this Recipe recipe, string?[] progressList)
    {
        var validRequiredTileCount = recipe.GetRequiredItemCount();
        var notCommonCount = 0;
        for (int i = 0; i < validRequiredTileCount; i++)
        {
            if (progressList[recipe.requiredTile[i]]?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false)
            {
                notCommonCount++;
            }
        }
        return notCommonCount;
    }
}
public static class OpcodeValue
{
    public const byte nop = 0x0000;
    public const byte stloc_0 = 0x000A;
    public const byte ldc_i4_0 = 0x0016;
    public const byte call = 0x0028;
    public const byte ret = 0x002A;
    public const byte ldstr = 0x0072;
    public const byte ldsfld = 0x007E;
}