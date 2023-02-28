using System.Data;
using System.Reflection.Emit;

using Terraria;

using TShockAPI;
using TShockAPI.DB;

using VBY.Basic;
using VBY.Basic.Emit;

namespace VBY.Shop;

public static class Utils
{
    public static Func<Player, bool>[] ZoneFuncs;
    internal static Dictionary<Type, object> GetFuncs = new();
    internal static Dictionary<Type, string> TableHeaders = new();
    internal static Dictionary<string, Action<ILGenerator>> PrintEmit = new()
    {
        ["MoneyName"] = x =>
        {
            x.Emit(OpCodes.Call, typeof(Shop).GetProperty("MoneyName")!.GetGetMethod()!);
        }
    };
    static Utils()
    {
        ZoneFuncs = new Func<Player, bool>[]
        {
            _ => true,
            p => p.ZoneDungeon, //地牢
            p => p.ZoneCorrupt, //腐化
            p => p.ZoneHallow, //神圣
            p => p.ZoneMeteor, //流星
            p => p.ZoneJungle, //丛林
            p => p.ZoneSnow, //雪原
            p => p.ZoneCrimson, //猩红
            p => p.ZoneWaterCandle, //水蜡烛
            p => p.ZonePeaceCandle, //和平蜡烛
            p => p.ZoneTowerSolar, //日耀
            p => p.ZoneTowerVortex, //星漩
            p => p.ZoneTowerNebula, //星云
            p => p.ZoneTowerStardust, //星尘
            p => p.ZoneDesert, //沙漠
            p => p.ZoneGlowshroom, //蘑菇
            p => p.ZoneUndergroundDesert, //地下沙漠
            p => p.ZoneSkyHeight , //天空
            p => p.ZoneOverworldHeight , //地表
            p => p.ZoneDirtLayerHeight , //地下(泥土层)
            p => p.ZoneRockLayerHeight , //洞穴(岩石层)
            p => p.ZoneUnderworldHeight , //地狱
            p => p.ZoneBeach , //沙滩(海洋)
            p => p.ZoneRain , //雨
            p => p.ZoneSandstorm , //沙尘暴
            p => p.ZoneOldOneArmy , //旧日军团
            p => p.ZoneGranite , //花岗岩
            p => p.ZoneMarble , //大理石
            p => p.ZoneHive , //蜂巢
            p => p.ZoneGemCave , //宝石洞穴
            p => p.ZoneLihzhardTemple , //神庙
            p => p.ZoneGraveyard //墓地
        };
    }
    public static bool Check<TArgs>(string str, TArgs args, Func<string, TArgs, bool> func) where TArgs : class
    {
        if (string.IsNullOrEmpty(str) || args is null)
            return true;
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
                    result = !result;
            }
            if (!result)
                break;
        }
        return result;
    }
    public static bool ZoneCheck(Player player, string str) => Check(str, player, (s, p) => int.TryParse(s, out int index) && index >= 0 && index < ZoneFuncs.Length && ZoneFuncs[index](player));
    public static bool ZoneCheck(TSPlayer tsplayer, string str) => ZoneCheck(tsplayer.TPlayer, str);
    public static bool GroupCheck(TSPlayer player, string str) => Check(str, player, (s, p) => p.Group.Name == s);
    internal static Delegate GetReaderNewFunc(Type type)
    {
        var constructor = type.GetConstructors().First(x => x.GetParameters().Length > 0);
        var method = new DynamicMethod("Get" + type.Name, type, new Type[] { typeof(IDataReader) }, type.Module);
        var il = method.GetILGenerator();
        var getmethod = typeof(DbExt).GetMethod("Get", new Type[] { typeof(IDataReader), typeof(string) });
        var parameters = constructor.GetParameters();
        for (int i = 0; i < parameters.Length; i++)
        {
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldstr, parameters[i].Name!.FirstUpper());
            il.Emit(OpCodes.Call, getmethod!.MakeGenericMethod(parameters[i].ParameterType));
        }
        il.Emit(OpCodes.Newobj, constructor);
        il.Emit(OpCodes.Ret);
        return method.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(IDataReader), type));
    }
    public static Func<IDataReader, T> GetFunc<T>() where T : Shops => (Func<IDataReader, T>)GetFuncs[typeof(T)];
    public static T? SelectShop<T>(int buyId) where T : Shops
    {
        var type = typeof(T);
        using var reader = Shop.DB.QueryReader($"SELECT {TableHeaders[type]} FROM {type.Name} WHERE BuyId = @0", buyId);
        if (reader.Read())
            return GetFunc<T>()(reader.Reader);
        else
            return null;
    }
    internal static Func<Shops, object[]> GetPrintArgsFunc(Type type,string print)
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
                        throw new Exception("PrintEmit not find");
                }
            }
            action!(il);
            il.Emit(OpCodes.Stelem_Ref);
        }
        il.Emit(OpCodes.Ret);
        return method.CreateDelegate<Func<Shops, object[]>>();
    }
    public static void FormatReplace(Show show,string format)
    {
        Basic.Utils.FormatReplace(ref show.SystemFormat, format);
        Basic.Utils.FormatReplace(ref show.PlayerFormat, format);
    }
}