using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

using MonoMod.RuntimeDetour;

using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Utilities;

using TShockAPI;

namespace VBY.GameContentModify;

internal static class Utils
{
    public static bool NextIsZero(this UnifiedRandom random, int maxValue)
    {
        if (maxValue < 1)
        {
            return false;
        }
        return random.Next(maxValue) == 0;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsTrueRet(this bool value, bool retValue) => !value || retValue;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFalseRet(this bool value, bool retValue) => !value && retValue;
    public static T SelectRandom<T>(UnifiedRandom random, params T[] choices)
    {
        return choices[random.Next(choices.Length)];
    }
    public static void SetEmpty(this Chest chest)
    {
        for (int i = 0; i < 40; i++)
        {
            chest.item[i] = new Item();
        }
    }
    public static Hook GetNameHook(Type targetMethodDeclaringType, Delegate replaceMethod, bool manualApply = true)
    {
        return new Hook(targetMethodDeclaringType.GetMethod(replaceMethod.Method.Name), replaceMethod.Method, new HookConfig() { ManualApply = manualApply });
    }

    public static Hook GetNameHook(Type targetMethodDeclaringType, MethodInfo replaceMethod, bool manualApply = true)
    {
        return new Hook(targetMethodDeclaringType.GetMethod(replaceMethod.Name), replaceMethod, new HookConfig() { ManualApply = manualApply });
    }

    public static Hook GetHook(Delegate method, bool manualApply = true)
    {
        return GetNameHook(method.Method.DeclaringType!.GetCustomAttribute<ReplaceTypeAttribute>()!.Type, method, manualApply);
    }
    public static Hook GetParamHook(Delegate method, bool manualApply = true)
    {
        var methodType = method.Method.DeclaringType!.GetCustomAttribute<ReplaceTypeAttribute>()!.Type;
        return new Hook(methodType.GetMethod(method.Method.Name, method.Method.GetParameters().Select(x => x.ParameterType).Skip(1).ToArray()), method.Method, new HookConfig() { ManualApply = manualApply });
    }
    public static Hook GetParamHook(Type targetMethodDeclaringType, MethodInfo method, bool manualApply = true)
    {
        return new Hook(targetMethodDeclaringType.GetMethod(method.Name, method.GetParameters().Select(x => x.ParameterType).Skip(1).ToArray()), method, new HookConfig() { ManualApply = manualApply });
    }
    public static void Deconstruct(this Chest chest, out int x, out int y)
    {
        x = chest.x;
        y = chest.y;
    }
    public static bool SetMemberValue(TSPlayer player, object target, string propertyName, string realPropertyName, string value)
    {
        if (propertyName.Contains('.'))
        {
            var checkType = target.GetType();
            var propertyNames = propertyName.Split('.');
            for (int i = 0; i < propertyNames.Length - 1; i++)
            {
                if (!TryGetFieldOrPropertyMember(checkType, propertyNames[i], target, out checkType, out object? newTarget))
                {
                    player.SendInfoMessage($"没有找到属性 {string.Join('.', propertyNames, 0, i + 1)}");
                    return false;
                }
                target = newTarget;
            }
            return SetMemberValue(player, target, propertyNames[^1], propertyName, value);
        }
        var type = target.GetType();
        var members = type.GetMember(propertyName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
        if (members.Length == 0)
        {
            player.SendInfoMessage($"没找到属性 {propertyName}");
        }
        else
        {
            var member = members[0];
            Func<object?, object?> getFunc;
            Action<object?, object> setFunc;
            if (member.MemberType == MemberTypes.Field)
            {
                var field = (FieldInfo)member;
                getFunc = field.GetValue;
                setFunc = field.SetValue;
            }
            else
            {
                var property = (PropertyInfo)member;
                getFunc = property.GetValue;
                setFunc = property.SetValue;
            }
            var propertyValue = getFunc(target);
            switch (propertyValue)
            {
                case int:
                    if (int.TryParse(value, out var intValue))
                    {
                        setFunc(target, intValue);
                        player.SendInfoMessage($"{realPropertyName} = {intValue}");
                        return true;
                    }
                    player.SendInfoMessage($"{value} 转换为 int 失败");
                    break;
                case bool:
                    if (bool.TryParse(value, out var boolValue))
                    {
                        setFunc(target, boolValue);
                        player.SendInfoMessage($"{realPropertyName} = {boolValue}");
                        return true;
                    }
                    player.SendInfoMessage($"{value} 转换为 bool 失败");
                    break;
                case int[]:
                case string[]:
                    player.SendInfoMessage("数组不受支持");
                    break;
                default:
                    player.SendInfoMessage($"类型 {type.FullName} 不受支持");
                    break;
            }
        }
        return false;
    }
    public static bool TryGetFieldOrPropertyMember(Type type, string memberName, object target, [MaybeNullWhen(false)] out Type memberType, [MaybeNullWhen(false)] out object newTarget)
    {
        var members = type.GetMember(memberName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public);
        if (members.Length == 0)
        {
            memberType = null;
            newTarget = null;
            return false;
        }
        var findMember = members[0];
        if (findMember.MemberType == MemberTypes.Field)
        {
            var field = (FieldInfo)findMember;
            memberType = field.FieldType;
            newTarget = field.GetValue(target)!;
        }
        else
        {
            var property = (PropertyInfo)findMember;
            memberType = property.PropertyType;
            newTarget = property.GetValue(target)!;
        }
        return true;
    }
    public static Type GetFieldOrPropertyType(MemberInfo memberInfo)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            return ((FieldInfo)memberInfo).FieldType;
        }
        return ((PropertyInfo)memberInfo).PropertyType;
    }
    public static object? GetFieldOrPropertyValue(MemberInfo memberInfo, object? target = null)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            return ((FieldInfo)memberInfo).GetValue(target);
        }
        return ((PropertyInfo)memberInfo).GetValue(target);
    }
    public static void SetFieldOrPropertyValue(MemberInfo memberInfo, object? target, object? value)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            ((FieldInfo)memberInfo).SetValue(target, value);
            return;
        }
        ((PropertyInfo)memberInfo).SetValue(target, value);
    }
    public static Type GetFieldOrPropertyType(MemberInfo memberInfo, out Func<object?, object?> getFunc, out Action<object?, object?> setFunc)
    {
        if (memberInfo.MemberType == MemberTypes.Field)
        {
            var field = (FieldInfo)memberInfo;
            getFunc = field.GetValue;
            setFunc = field.SetValue;
            return field.FieldType;
        }
        var property = ((PropertyInfo)memberInfo);
        getFunc = property.GetValue;
        setFunc = property.SetValue;
        return property.PropertyType;
    }
    public static void HandleNamedDetour(ref bool field, bool value, params string[] detourNames)
    {
        if (field != value)
        {
            if (value)
            {
                foreach (string detourName in detourNames)
                {
                    GameContentModify.NamedHooks[detourName].Apply();
                }
            }
            else
            {
                foreach (string detourName in detourNames)
                {
                    GameContentModify.NamedHooks[detourName].Undo();
                }
            }
            field = value;
        }
    }
    public static void HandleNamedHook(bool value, params string[] detourNames)
    {
        foreach (var detourName in detourNames)
        {
            if (value)
            {
                GameContentModify.NamedHooks[detourName].Apply();
            }
            else
            {
                GameContentModify.NamedHooks[detourName].Undo();
            }
        }
    }
    public static void HandleNamedActionHook(bool value, string actionHookName)
    {
        if (value)
        {
            GameContentModify.NamedActionHooks[actionHookName].Register();
        }
        else
        {
            GameContentModify.NamedActionHooks[actionHookName].Unregister();
        }
    }
    public static bool NamedActionHookIsRegistered(string actionHookName) => GameContentModify.NamedActionHooks[actionHookName].Registered;
    public static T[] MakeArray<T>(this T item) where T : class => new T[1] { item };
    public static bool MembersValueAllEqualDefault(object target, params string[] names)
    {
        var type = target.GetType();
        foreach (string name in names)
        {
            var members = type.GetMember(name);
            if (members.Length == 0)
            {
                return false;
            }
            if (members.Length > 1)
            {
                return false;
            }
            var member = members[0];
            if (member.MemberType != MemberTypes.Field && member.MemberType != MemberTypes.Property)
            {
                return false;
            }
            var defaultAttr = member.GetCustomAttribute<DefaultValueAttribute>();
            if (defaultAttr is null)
            {
                return false;
            }
            if (!GetFieldOrPropertyValue(member, target)!.Equals(defaultAttr.Value))
            {
                return false;
            }
        }
        return true;
    }
    //public static bool MembersValueAllEqualDefault(object target, params object[] refMembers)
    //{
    //    var type = target.GetType();
    //    foreach (var refMember in refMembers)
    //    {
    //        MemberInfo? member;
    //        if(refMember is RuntimeFieldHandle)
    //        {
    //            member = FieldInfo.GetFieldFromHandle((RuntimeFieldHandle)refMember);
    //        }
    //        else
    //        {
    //            member = type.GetProperty((string)refMember);
    //        }
    //        if(member is null)
    //        {
    //            return false;
    //        }
    //        var defaultAttr = member.GetCustomAttribute<DefaultValueAttribute>();
    //        if (defaultAttr is null)
    //        {
    //            return false;
    //        }
    //        if (!GetFieldOrPropertyValue(member, target)!.Equals(defaultAttr.Value))
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    [Conditional("DEBUG")]
    public static void ArgumentWriteLine(bool value, [CallerArgumentExpression(nameof(value))] string expression = "")
    {
        Console.Write(expression);
        Console.Write(": ");
        Console.WriteLine(value);
    }
    [Conditional("DEBUG")]
    public static void ArgumentWriteLine(int value, [CallerArgumentExpression(nameof(value))] string expression = "")
    {
        Console.Write(expression);
        Console.Write(": ");
        Console.WriteLine(value);
    }
    public static bool hasUnknown;
    public static void WriteItemDropRuleInfo(IndentedTextWriter writer, IItemDropRule itemDropRule)
    {
        switch (itemDropRule)
        {
            case DropNothing:
                {
                    var rule = (DropNothing)itemDropRule;
                    writer.WriteLine(nameof(DropNothing));
                    writer.WriteLine("无掉落");
                }
                break;
            case ItemDropWithConditionRule:
                {
                    var rule = (ItemDropWithConditionRule)itemDropRule;
                    writer.WriteLine(nameof(ItemDropWithConditionRule));
                    TypeFullNameMatch(writer, typeof(ItemDropWithConditionRule), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    writer.WriteLine("条件: {0}", GetConditionDescription(rule.condition));
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case LeadingConditionRule:
                {
                    var rule = (LeadingConditionRule)itemDropRule;
                    writer.WriteLine(nameof(LeadingConditionRule));
                    TypeFullNameMatch(writer, typeof(LeadingConditionRule), itemDropRule);
                    writer.WriteLine("条件: {0}", GetConditionDescription(rule.condition));
                    writer.WriteLine("链式规则:");
                    writer.Indent++;
                    for(int i =0; i < rule.ChainedRules.Count; i++)
                    {
                        writer.Write("规则{0}: ", i + 1);
                        var chainedRule = rule.ChainedRules[i];
                        writer.Indent++;
                        WriteItemDropRuleInfo(writer, chainedRule.RuleToChain);
                        writer.Indent--;
                    }
                    writer.Indent--;
                }
                break;
            case DropLocalPerClientAndResetsNPCMoneyTo0:
                {
                    var rule = (DropLocalPerClientAndResetsNPCMoneyTo0)itemDropRule;
                    writer.WriteLine(nameof(DropLocalPerClientAndResetsNPCMoneyTo0));
                    TypeFullNameMatch(writer, typeof(DropLocalPerClientAndResetsNPCMoneyTo0), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    writer.WriteLine("条件: {0}", GetConditionDescription(rule.condition));
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case DropPerPlayerOnThePlayer:
                {
                    var rule = (DropPerPlayerOnThePlayer)itemDropRule;
                    writer.WriteLine(nameof(DropPerPlayerOnThePlayer));
                    TypeFullNameMatch(writer, typeof(DropPerPlayerOnThePlayer), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    writer.WriteLine("条件: {0}", GetConditionDescription(rule.condition));
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case CommonDropNotScalingWithLuck:
                {
                    var rule = (CommonDropNotScalingWithLuck)itemDropRule;
                    writer.WriteLine(nameof(CommonDropNotScalingWithLuck));
                    TypeFullNameMatch(writer, typeof(CommonDropNotScalingWithLuck), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case CommonDropWithRerolls:
                {
                    var rule = (CommonDropWithRerolls)itemDropRule;
                    writer.WriteLine(nameof(CommonDropWithRerolls));
                    TypeFullNameMatch(writer, typeof(CommonDropWithRerolls), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    writer.WriteLine("计算次数: {0}", rule.timesToRoll);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case CommonDrop:
                {
                    var rule = (CommonDrop)itemDropRule;
                    writer.WriteLine(nameof(CommonDrop));
                    TypeFullNameMatch(writer, typeof(CommonDrop), itemDropRule);
                    WriteCommonItemDropRuleInfo(writer, rule);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case DropBasedOnExpertMode:
                {
                    var rule = (DropBasedOnExpertMode)itemDropRule;
                    writer.WriteLine(nameof(DropBasedOnExpertMode));
                    TypeFullNameMatch(writer, typeof(DropBasedOnExpertMode), itemDropRule);

                    writer.Write("普通规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForNormalMode);
                    writer.Indent--;

                    writer.Write("专家规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForExpertMode);
                    writer.Indent--;
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case DropBasedOnMasterMode:
                {
                    var rule = (DropBasedOnMasterMode)itemDropRule;
                    writer.WriteLine(nameof(DropBasedOnMasterMode));
                    TypeFullNameMatch(writer, typeof(DropBasedOnMasterMode), itemDropRule);

                    writer.Write("默认规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForDefault);
                    writer.Indent--;

                    writer.Write("大师规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForMasterMode);
                    writer.Indent--;
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case DropBasedOnMasterAndExpertMode:
                {
                    var rule = (DropBasedOnMasterAndExpertMode)itemDropRule;
                    writer.WriteLine(nameof(DropBasedOnMasterAndExpertMode));
                    TypeFullNameMatch(writer, typeof(DropBasedOnMasterAndExpertMode), itemDropRule);

                    writer.Write("默认规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForDefault);
                    writer.Indent--;

                    writer.Write("专家规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForExpertmode);
                    writer.Indent--;

                    writer.Write("大师规则: ");
                    writer.Indent++;
                    WriteItemDropRuleInfo(writer, rule.ruleForMasterMode);
                    writer.Indent--;
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case DropOneByOne:
                {
                    var rule = (DropOneByOne)itemDropRule;
                    writer.WriteLine(nameof(DropOneByOne));
                    TypeFullNameMatch(writer, typeof(DropOneByOne), itemDropRule);
                    string leftStack, rightStack;
                    if(rule.parameters.BonusMinDropsPerChunkPerPlayer == 0)
                    {
                        leftStack = rule.parameters.MinimumStackPerChunkBase.ToString();
                    }
                    else
                    {
                        if (rule.parameters.BonusMinDropsPerChunkPerPlayer == 1)
                        {
                            leftStack = $"({rule.parameters.MinimumStackPerChunkBase} + 玩家数量)";
                        }
                        else
                        {
                            leftStack = $"({rule.parameters.MinimumStackPerChunkBase} + 玩家数量 * {rule.parameters.BonusMinDropsPerChunkPerPlayer})";
                        }
                    }
                    if (rule.parameters.BonusMaxDropsPerChunkPerPlayer == 0)
                    {
                        rightStack = rule.parameters.MaximumStackPerChunkBase.ToString();
                    }
                    else
                    {
                        if (rule.parameters.BonusMaxDropsPerChunkPerPlayer == 1)
                        {
                            rightStack = $"({rule.parameters.MaximumStackPerChunkBase} + 玩家数量)";
                        }
                        else
                        {
                            rightStack = $"({rule.parameters.MaximumStackPerChunkBase} + 玩家数量 * {rule.parameters.BonusMaxDropsPerChunkPerPlayer})";
                        }
                    }
                    WriteCommonItemDropInfo(writer, rule.itemId, $"{leftStack}-{rightStack}", rule.parameters.ChanceNumerator, rule.parameters.ChanceDenominator);
                    writer.WriteLine("掉落次数: {0}-{1}", rule.parameters.MinimumItemDropsCount, rule.parameters.MaximumItemDropsCount);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case OneFromOptionsDropRule:
                {
                    var rule = (OneFromOptionsDropRule)itemDropRule;
                    writer.WriteLine(nameof(OneFromOptionsDropRule));
                    TypeFullNameMatch(writer, typeof(OneFromOptionsDropRule), itemDropRule);
                    writer.WriteLine("物品ID:");
                    writer.Indent++;
                    foreach(int itemId in rule.dropIds)
                    {
                        writer.WriteLine("{0}[{1}]", itemId, Lang.GetItemNameValue(itemId));
                    }
                    writer.Indent--;
                    WriteCommonItemDropInfo(writer, null, "1", rule.chanceNumerator, rule.chanceDenominator);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case OneFromOptionsNotScaledWithLuckDropRule:
                {
                    var rule = (OneFromOptionsNotScaledWithLuckDropRule)itemDropRule;
                    writer.WriteLine(nameof(OneFromOptionsNotScaledWithLuckDropRule));
                    TypeFullNameMatch(writer, typeof(OneFromOptionsNotScaledWithLuckDropRule), itemDropRule);
                    writer.WriteLine("物品ID:");
                    writer.Indent++;
                    foreach (int itemId in rule.dropIds)
                    {
                        writer.WriteLine("{0}[{1}]", itemId, Lang.GetItemNameValue(itemId));
                    }
                    writer.Indent--;
                    WriteCommonItemDropInfo(writer, null, "1", rule.chanceNumerator, rule.chanceDenominator);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case OneFromRulesRule:
                {
                    var rule = (OneFromRulesRule)itemDropRule;
                    writer.WriteLine(nameof(OneFromRulesRule));
                    TypeFullNameMatch(writer, typeof(OneFromRulesRule), itemDropRule);
                    WriteCommonItemDropInfo(writer, null, null, 1, rule.chanceDenominator);
                    for(int i = 0; i < rule.options.Length; i++)
                    {
                        writer.WriteLine("规则{0}: ", i + 1);
                        writer.Indent++;
                        WriteItemDropRuleInfo(writer, rule.options[i]);
                        writer.Indent--;
                    }
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case FromOptionsWithoutRepeatsDropRule:
                {
                    var rule = (FromOptionsWithoutRepeatsDropRule)itemDropRule;
                    writer.WriteLine(nameof(FromOptionsWithoutRepeatsDropRule));
                    TypeFullNameMatch(writer, typeof(FromOptionsWithoutRepeatsDropRule), itemDropRule);
                    writer.WriteLine("物品ID:");
                    writer.Indent++;
                    foreach (int itemId in rule.dropIds)
                    {
                        writer.WriteLine("{0}[{1}]", itemId, Lang.GetItemNameValue(itemId));
                    }
                    writer.Indent--;
                    writer.WriteLine("掉落个数: {0}", rule.dropCount);
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case MechBossSpawnersDropRule:
                {
                    var rule = (MechBossSpawnersDropRule)itemDropRule;
                    writer.WriteLine(nameof(MechBossSpawnersDropRule));
                    TypeFullNameMatch(writer, typeof(MechBossSpawnersDropRule), itemDropRule);
                    WriteCommonItemDropInfo(writer, ItemID.MechanicalWorm, "1", 1, 2500);
                    writer.WriteLine("条件: 未击败毁灭者");
                    WriteCommonItemDropInfo(writer, ItemID.MechanicalEye, "1", 1, 2500);
                    writer.WriteLine("条件: 未击败双子魔眼");
                    WriteCommonItemDropInfo(writer, ItemID.MechanicalSkull, "1", 1, 2500);
                    writer.WriteLine("条件: 未击败机械骷髅王");
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            case SlimeBodyItemDropRule:
                {
                    var rule = (SlimeBodyItemDropRule)itemDropRule;
                    writer.WriteLine(nameof(SlimeBodyItemDropRule));
                    TypeFullNameMatch(writer, typeof(SlimeBodyItemDropRule), itemDropRule);
                    writer.WriteLine("物品ID: npc.ai[1]");
                    writer.WriteLine("条件: npc.type == 0");
                    writer.WriteLine("数量: 懒得写自己看");
                    WriteItemDropRuleChainInfo(writer, rule);
                }
                break;
            default:
                hasUnknown = true;
                writer.WriteLine("不知道({0})", itemDropRule.GetType().FullName);
                break;
        }
    }
    public static void WriteCommonItemDropInfo(TextWriter writer, int? itemId, string? amountText, int chanceNumerator, int chanceDenominator)
    {
        if (itemId.HasValue)
        {
            writer.WriteLine("物品ID: {0}[{1}]", itemId.Value, Lang.GetItemNameValue(itemId.Value));
        }
        if (!string.IsNullOrEmpty(amountText))
        {
            writer.WriteLine("掉落数量: {0}", amountText);
        }
        var str = (chanceNumerator / (float)chanceDenominator).ToString("P4");
        var chars = str.AsSpan();
        var pointIndex = chars.IndexOf('.');
        int i;
        for (i = chars.Length - 2; i > pointIndex; i--)
        {
            if (chars[i] != '0')
            {
                break;
            }
        }
        int newLength = i + 2;
        if (i == pointIndex)
        {
            newLength--;
        }
        var newSpan = (stackalloc char[newLength]);
        chars.Slice(0, newLength - 1).CopyTo(newSpan);
        newSpan[^1] = '%';
        writer.WriteLine("掉落概率: {0}/{1}[{2}]", chanceNumerator, chanceDenominator, newSpan.ToString());
    }
    public static void WriteCommonItemDropRuleInfo(IndentedTextWriter writer, CommonDrop rule)
    {
        WriteCommonItemDropInfo(writer, rule.itemId, rule.GetDropAmount(), rule.chanceNumerator, rule.chanceDenominator);
    }
    public static void WriteItemDropRuleChainInfo(TextWriter writer, IItemDropRuleChainAttempt chain)
    {
        switch (chain)
        {
            case Chains.TryIfSucceeded:
                writer.Write(nameof(Chains.TryIfSucceeded));
                writer.WriteLine("[成功时]");
                break;
            case Chains.TryIfDoesntFillConditions:
                writer.Write(nameof(Chains.TryIfDoesntFillConditions));
                writer.WriteLine("[无条件成功时]");
                break;
            case Chains.TryIfFailedRandomRoll:
                writer.Write(nameof(Chains.TryIfFailedRandomRoll));
                writer.WriteLine("[随机失败时]");
                break;
            default:
                hasUnknown = true;
                writer.WriteLine("不知道({0})", chain.GetType().FullName);
                break;
        }
    }
    public static void WriteItemDropRuleChainInfo(IndentedTextWriter writer, IItemDropRule rule)
    {
        if(rule.ChainedRules is null || rule.ChainedRules.Count == 0)
        {
            return;
        }
        writer.WriteLine("链式规则: ");
        writer.Indent++;
        for (int i = 0; i < rule.ChainedRules.Count; i++)
        {
            var chainRule = rule.ChainedRules[i];
            writer.Write("规则{0}: ", i + 1);
            WriteItemDropRuleChainInfo(writer, chainRule);
            writer.Indent++;
            WriteItemDropRuleInfo(writer, chainRule.RuleToChain);
            writer.Indent--;
        }
        writer.Indent--;
    }
    public static string GetDropAmount(this CommonDrop commonDrop)
    {
        if (commonDrop.amountDroppedMinimum == commonDrop.amountDroppedMaximum)
        {
            return commonDrop.amountDroppedMinimum.ToString();
        }
        return $"{commonDrop.amountDroppedMinimum}-{commonDrop.amountDroppedMaximum}";
    }
    public static string GetConditionDescription(IItemDropRuleCondition condition)
    {
        switch (condition)
        {
            case null:
                return "无";
            case Conditions.IsMasterMode:
                return "是大师模式";
            case Conditions.NotFromStatue:
                return "不是由雕像生成";
            case Conditions.LegacyHack_IsABoss:
                return "npc是boss";
            case Conditions.LegacyHack_IsBossAndExpert:
                return "npc是boss且是专家模式";
            case Conditions.LegacyHack_IsBossAndNotExpert:
                return "npc是boss且非专家模式";
            case Conditions.NotExpert:
                return "非专家模式";
            case Conditions.IsCorruption:
                return "腐化世界";
            case Conditions.IsCorruptionAndNotExpert:
                return "腐化世界且非专家模式";
            case Conditions.IsCrimson:
                return "猩红世界";
            case Conditions.IsCrimsonAndNotExpert:
                return "猩红世界且非专家模式";
            case Conditions.DontStarveIsUp:
                return "饥荒世界";
            case Conditions.DontStarveIsNotUp:
                return "非饥荒世界";
            case Conditions.RemixSeed:
                return "翻转世界";
            case Conditions.NotRemixSeed:
                return "非翻转世界";
            case Conditions.RemixSeedEasymode:
                return "肉前翻转世界";
            case Conditions.RemixSeedHardmode:
                return "肉后翻转世界";
            case Conditions.TenthAnniversaryIsUp:
                return "十周年世界";
            case Conditions.TenthAnniversaryIsNotUp:
                return "非十周年世界";
            case Conditions.MissingTwin:
                return "找不到另一半(双子魔眼)";
            case Conditions.DownedAllMechBosses:
                return "已击败所有机械Boss";
            case Conditions.MechdusaKill:
                return "美杜莎被击败";
            case Conditions.NamedNPC:
                return $"唯有名称为此时[{((Conditions.NamedNPC)condition).neededName}]";
            case Conditions.PumpkinMoonDropGateForTrophies:
                return "南瓜月15波以上";
            case Conditions.FrostMoonDropGateForTrophies:
                return "霜月15波以上";
            case Conditions.FirstTimeKillingPlantera:
                return "没打败世纪之花";
            case Conditions.DownedPlantera:
                return "已击败世纪之花";
            case Conditions.IsBloodMoonAndNotFromStatue:
                return "血月且不是由雕像生成";
            case Conditions.NeverTrue:
                return "不可能";
            case Conditions.IsHardmode:
                return "肉后";
        }
        var result = condition.GetConditionDescription();
        if (string.IsNullOrWhiteSpace(result))
        {
            hasUnknown = true;
            result = $"不知道({condition.GetType().FullName})";
        }
        else
        {
            result += "[来自游戏]";
        }
        return result;
    }
    public static void TypeFullNameMatch(IndentedTextWriter writer, Type type, object value)
    {
        var sourceName = value.GetType().FullName;
        var targetName = type.FullName!;
        if (!targetName.OrdinalEquals(sourceName))
        {
            writer.WriteLine("不完全匹配:");
            writer.Indent++;
            writer.WriteLine("目标类型: {0}", targetName);
            writer.WriteLine("来源类型: {0}", sourceName);
            writer.Indent--;
        }
    }
    public static void SetValueInWorld(ref int minX, ref int maxX, ref int minY, ref int maxY, int fluff = 0)
    {
        if(minX < fluff)
        {
            minX = fluff;
        }
        if(maxX > Main.maxTilesX - fluff)
        {
            maxX = Main.maxTilesX - fluff;
        }
        if(minY < fluff)
        {
            minY = fluff;
        }
        if(maxY > Main.maxTilesY - fluff)
        {
            maxY = Main.maxTilesY - fluff;
        }
    }
    public static bool InWorld(int minX, int maxX, int minY, int maxY, int fluff = 0)
    {
        if (minX < fluff)
        {
            return false;
        }
        if (maxX > Main.maxTilesX - fluff)
        {
            return false;
        }
        if (minY < fluff)
        {
            return false;
        }
        if (maxY > Main.maxTilesY - fluff)
        {
            return false;
        }
        return true;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRandomIntInRange(UnifiedRandom random, int value) => random.Next(-value, value + 1);
}