using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Emit;

using Terraria;

using TShockAPI;
using TShockAPI.DB;

using VBY.Common;
using VBY.Common.Command;
using VBY.Common.Emit;

namespace VBY.Shop;

public static partial class Utils
{
    public static Func<Player, bool>[] ZoneFuncs;
    internal static Dictionary<Type, Shops> ShopInstances = new();
    internal static Dictionary<Type, Delegate> GetFuncs = new();
    internal static Dictionary<Type, Action<SubCmdArgs>> AddActions = new();
    internal static Dictionary<Type, string> TableHeaders = new();
    internal static Dictionary<string, Action<ILGenerator>> PrintEmit = new()
    {
        ["MoneyName"] = x =>
        {
            x.Emit(OpCodes.Call, typeof(ShopPlugin).GetProperty("MoneyName")!.GetGetMethod()!);
        }
    };
    static Utils()
    {
        ZoneFuncs = new Func<Player, bool>[]
        {
            static _ => true,
            //zone1
            static p => p.ZoneDungeon, //地牢
            static p => p.ZoneCorrupt, //腐化
            static p => p.ZoneHallow, //神圣
            static p => p.ZoneMeteor, //流星
            static p => p.ZoneJungle, //丛林
            static p => p.ZoneSnow, //雪原
            static p => p.ZoneCrimson, //猩红
            static p => p.ZoneWaterCandle, //水蜡烛
            //zone2
            static p => p.ZonePeaceCandle, //和平蜡烛
            static p => p.ZoneTowerSolar, //日耀
            static p => p.ZoneTowerVortex, //星漩
            static p => p.ZoneTowerNebula, //星云
            static p => p.ZoneTowerStardust, //星尘
            static p => p.ZoneDesert, //沙漠
            static p => p.ZoneGlowshroom, //蘑菇
            static p => p.ZoneUndergroundDesert, //地下沙漠
            //zone3
            static p => p.ZoneSkyHeight, //天空
            static p => p.ZoneOverworldHeight, //地表
            static p => p.ZoneDirtLayerHeight, //地下(泥土层)
            static p => p.ZoneRockLayerHeight, //洞穴(岩石层)
            static p => p.ZoneUnderworldHeight, //地狱
            static p => p.ZoneBeach, //沙滩(海洋)
            static p => p.ZoneRain, //雨
            static p => p.ZoneSandstorm, //沙尘暴
            //zone4
            static p => p.ZoneOldOneArmy, //旧日军团
            static p => p.ZoneGranite, //花岗岩
            static p => p.ZoneMarble, //大理石
            static p => p.ZoneHive, //蜂巢
            static p => p.ZoneGemCave, //宝石洞穴
            static p => p.ZoneLihzhardTemple, //神庙
            static p => p.ZoneGraveyard, //墓地
            //end 1436
            static p => p.ZoneShadowCandle, //暗影蜡烛
            //zone5
            static p => p.ZoneShimmer //微光
        };
    }
    public static bool IsValidExpression(string expression, out string error)
    {
        error = "";
        if (!expression.All(x => x is '(' or ')' or ',' or '|' or '!' || char.IsNumber(x)))
        {
            error = "有无效字符";
            return false;
        }
        int bracketLeftCount = 0;
        for (int i = 0; i < expression.Length; i++)
        {
            switch (expression[i])
            {
                case '(':
                    bracketLeftCount++;
                    break;
                case ')':
                    bracketLeftCount--;
                    break;
                case '!':
                    // '...!'  || ('num!' || '!,' || '!|')
                    if (i + 1 == expression.Length || ((i != 0) && (char.IsNumber(expression[i - 1]) || expression[i + 1] is ',' or '|')))
                    {
                        error = "错误感叹号位置";
                        return false;
                    }
                    break;
            }
        }
        if (bracketLeftCount != 0)
        {
            error = "括号不匹配";
            return false;
        }
        return true;
    }
    public static bool EvaluateExpression(ReadOnlySpan<char> span, Func<bool>[] checkFuncs)
    {
        if (span.Length == 0)
        {
            return true;
        }
        while (span[0] == '(' && span[span.Length - 2] == ')')
        {
            span = span.Slice(1, span.Length - 2);
            if (span.Length == 0)
            {
                return true;
            }
        }
        int startIndex = 0;
        bool negation = false;
        for (int i = 0; i < span.Length; i++)
        {
            switch (span[i])
            {
                case ',':
                    {
                        var result = EvaluateExpression(span.Slice(startIndex, i - startIndex), checkFuncs);
                        if (negation)
                        {
                            result = !result;
                            negation = false;
                        }
                        if (!result)
                        {
                            return false;
                        }
                        startIndex = i + 1;
                        break;
                    }
                case '|':
                    {
                        var result = EvaluateExpression(span.Slice(startIndex, i - startIndex), checkFuncs);
                        if (negation)
                        {
                            result = !result;
                            negation = false;
                        }
                        if (result)
                        {
                            return true;
                        }
                        startIndex = i + 1;
                        break;
                    }
                case '!':
                    negation = true;
                    break;
                case '(':
                    {
                        startIndex = i;
                        int curBracketCount = 1;
                        for (int j = i + 1; j < span.Length; j++)
                        {
                            if (span[j] == '(')
                            {
                                curBracketCount++;
                            }
                            else if (span[j] == ')')
                            {
                                curBracketCount--;
                                if (curBracketCount == 0)
                                {
                                    i = j;
                                    break;
                                }
                            }
                        }
                        if (i + 1 == span.Length)
                        {
                            return EvaluateExpression(span.Slice(startIndex), checkFuncs);
                        }
                        break;
                    }
            }
        }
        return checkFuncs[int.Parse(span.Slice(startIndex))]();
    }
    public static bool Check<TArgs>(string str, TArgs args, Func<string, TArgs, bool> func) where TArgs : class
    {
        if (string.IsNullOrEmpty(str) || args is null)
        {
            return true;
        }

        var result = false;
        foreach (var item in str.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            if (item.Contains('|'))
            {
                foreach (var item2 in item.Split('|', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (Check(item2, args, func))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                var flag = item[0] == '-';
                result = func(flag ? item.Remove(0, 1) : item, args);
                if (flag)
                {
                    result = !result;
                }
            }
            if (!result)
            {
                break;
            }
        }
        return result;
    }
    public static bool ZoneCheck(Player player, string str) => Check(str, player, (s, p) => int.TryParse(s, out int index) && index >= 0 && index < ZoneFuncs.Length && ZoneFuncs[index](player));
    public static bool ZoneCheck(TSPlayer tsplayer, string str) => ZoneCheck(tsplayer.TPlayer, str);
    public static bool GroupCheck(TSPlayer player, string str) => Check(str, player, (s, p) => p.Group.Name == s);
    //internal static Delegate GetReaderNewFunc(Type type)
    //{
    //    var constructor = type.GetConstructors().First(x => x.GetParameters().Length > 0);
    //    var method = new DynamicMethod("Get" + type.Name, type, new Type[] { typeof(IDataReader) }, type.Module);
    //    var il = method.GetILGenerator();
    //    var getmethod = typeof(DbExt).GetMethod("Get", new Type[] { typeof(IDataReader), typeof(string) });
    //    var parameters = constructor.GetParameters();
    //    for (int i = 0; i < parameters.Length; i++)
    //    {
    //        il.Emit(OpCodes.Ldarg_0);
    //        il.Emit(OpCodes.Ldstr, parameters[i].Name!.FirstUpper());
    //        il.Emit(OpCodes.Call, getmethod!.MakeGenericMethod(parameters[i].ParameterType));
    //    }
    //    il.Emit(OpCodes.Newobj, constructor);
    //    il.Emit(OpCodes.Ret);
    //    return method.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(IDataReader), type));
    //}
    public static Func<IDataReader, T> GetFunc<T>() => (Func<IDataReader, T>)GetFuncs[typeof(T)];
    public static T? SelectShop<T>(int buyId) where T : Shops
    {
        var type = typeof(T);
        using var reader = ShopPlugin.DB.QueryReader($"SELECT {TableHeaders[type]} FROM {type.Name} WHERE BuyId = @0", buyId);
        if (reader.Read())
        {
            return GetFunc<T>()(reader.Reader);
        }
        else
        {
            return null;
        }
    }
    public static ShopPlayer SelectShopPlayer(TSPlayer player)
    {
        using var reader = ShopPlugin.DB.QueryReader($"SELECT {TableHeaders[typeof(TableInfo.PlayerInfo)]} FROM {nameof(TableInfo.PlayerInfo)} WHERE Name = @0", player.Name);
        if (reader.Read())
        {
            return GetFunc<TableInfo.PlayerInfo>()(reader.Reader).GetShopPlayer(player);
        }
        else
        {
            return new ShopPlayer(player);
        }
    }
    public static bool TryGetOnlineShopPlayer(string name, [MaybeNullWhen(false)] out ShopPlayer shopPlayer)
    {
        shopPlayer = ShopPlugin.Players.FirstOrDefault(x => x.TSPlayer.Name == name);
        return shopPlayer is not null;
    }
    internal static Func<Shops, object[]> GetPrintArgsFunc(Type type, string print)
    {
        var prints = print.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var method = new DynamicMethod("GetPrint" + type.Name, typeof(object[]), new Type[] { typeof(Shops) }, type.Module);
        var arrCount = prints.Length;
        var il = method.GetILGenerator();
        il.EmitNewarr(TypeOf.Object, arrCount);
        for (int i = 0; i < arrCount; i++)
        {
            il.Emit(OpCodes.Dup);
            EmitHelper.Ldc_I4(il, i);
            var curType = type;
            var getstr = prints[i];
            Action<ILGenerator>? action = null;
            while (action is null)
            {
                if (!PrintEmit.TryGetValue(getstr, out action))
                {
                    getstr = $"{curType.Name}.{prints[i]}";
                    curType = curType.BaseType!;
                    if (curType is null)
                    {
                        throw new Exception("PrintEmit not find");
                    }
                }
            }
            action!(il);
            il.Emit(OpCodes.Stelem_Ref);
        }
        il.Emit(OpCodes.Ret);
        return method.CreateDelegate<Func<Shops, object[]>>();
    }
}