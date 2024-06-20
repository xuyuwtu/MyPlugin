using TShockAPI;
using TShockAPI.DB;

namespace FlowerSeaRPG;

public class FSPlayer
{
    public TSPlayer Player;
    public int Level;
    public int AttributePoints;
    public int Damage;
    public int KnockBack;
    public int Speed;
    public int Scale;
    public string? Title;
    public bool ShimmerAddDamage = true;
    public FSPlayer(TSPlayer tsPlayer)
    {
        Player = tsPlayer;
        using var reader = FlowerSeaRPG.DB.QueryReader(Utils.SelectStr[nameof(TableInfo.FSPlayerInfo)], Player.Name);
        if (reader.Read())
        {
            Level = reader.Get<int>(nameof(Level));
            AttributePoints = reader.Get<int>(nameof(AttributePoints));
            Damage = reader.Get<int>(nameof(Damage));
            KnockBack = reader.Get<int>(nameof(KnockBack));
            Speed = reader.Get<int>(nameof(Speed));
            Scale = reader.Get<int>(nameof(Scale));
        }
        else
        {
            FlowerSeaRPG.DB.Query(Utils.InsertStr[nameof(TableInfo.FSPlayerInfo)], Player.Name, Level, AttributePoints, Damage, KnockBack, Speed, Scale);
        }
        Title = FlowerSeaRPG.GradeInfos[Level].Title;
    }
    public void Clear()
    {
        Level = 0;
        AttributePoints = 0;
        Damage = 0;
        KnockBack = 0;
        Speed = 0;
        Scale = 0;
    }
    public void Save()
    {
        FlowerSeaRPG.DB.Query(Utils.UpdateStr[nameof(TableInfo.FSPlayerInfo)], Player.Name, Level, AttributePoints, Damage, KnockBack, Speed, Scale);
    }
    public CanUpgradeInfo CanUpgrade(string selectItemIndexStr, out int selectItemIndex)
    {
        selectItemIndex = -1;
        if (Level >= FlowerSeaRPG.MainConfig.MaxLevel)
        {
            return CanUpgradeInfo.LevelMax;
        }
        var gradeInfo = FlowerSeaRPG.GradeInfos[Level];
        if (gradeInfo.GiveItems is not null && gradeInfo.GiveItems.Length > 0)
        {
            if (string.IsNullOrEmpty(selectItemIndexStr))
            {
                if (gradeInfo.GiveItems.Length == 1)
                {
                    selectItemIndexStr = "0";
                }
                else
                {
                    return CanUpgradeInfo.SelectIndexStrIsNull;
                }
            }
            if (!int.TryParse(selectItemIndexStr, out selectItemIndex) || selectItemIndex < 0 || selectItemIndex >= gradeInfo.GiveItems.Length)
            {
                selectItemIndex = -1;
                return CanUpgradeInfo.SelectIndexStrIsError;
            }
        }
        var needItems = FlowerSeaRPG.GradeInfos[Level].UpgradeItems.Select(x => (x.type, x.stack)).ToArray();
        var itemEnought = new bool[needItems.Length];
        var inventory = Player.TPlayer.inventory;
        for (int i = 0; i < needItems.Length; i++)
        {
            itemEnought[i] = inventory.Where(x => x.type == needItems[i].type).Sum(x => x.stack) > needItems[i].stack;
        }
        return itemEnought.All(x => x) ? CanUpgradeInfo.ItemEnought : CanUpgradeInfo.ItemUnEnought;
    }
    public void Upgrade()
    {
        var gradeInfo = FlowerSeaRPG.GradeInfos[Level];
        var needItems = gradeInfo.UpgradeItems.Select(x => (x.type, x.stack)).ToArray();
        var inventory = Player.TPlayer.inventory;
        for (int i = 0; i < needItems.Length; i++)
        {
            for (int slot = 0; slot < inventory.Length; slot++)
            {
                var item = inventory[slot];
                var needItem = needItems[i];
                if (item.type == needItem.type)
                {
                    if (item.stack <= needItem.stack)
                    {
                        needItem.stack -= item.stack;
                        item.SetDefaults();
                        item.stack = 0;
                    }
                    else
                    {
                        item.stack -= needItem.stack;
                        needItem.stack = 0;
                    }
                    TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", Player.Index, slot, item.prefix);
                }
                if (needItem.stack == 0)
                {
                    break;
                }
            }
        }

        Level += 1;

        gradeInfo = FlowerSeaRPG.GradeInfos[Level];

        Title = gradeInfo.Title;
        AttributePoints += gradeInfo.AttributePoints;

        var tplayer = Player.TPlayer;
        UpdateLifeOrManaMax(ref tplayer.statLife, ref tplayer.statLifeMax, PacketTypes.PlayerHp, gradeInfo.Life);
        UpdateLifeOrManaMax(ref tplayer.statMana, ref tplayer.statManaMax, PacketTypes.PlayerMana, gradeInfo.Magic);

        if (gradeInfo.GiveItems is not null && gradeInfo.GiveItems.Length > 0)
        {
            foreach (var item in gradeInfo.GiveItems)
            {
                var stack = item.stack;
                item.SetDefaults(item.type);
                if (stack > item.maxStack)
                {
                    var quotient = Math.DivRem(item.stack, item.maxStack, out var remainder);
                }
            }
            gradeInfo.GiveItems.ForEach(x => Player.GiveItem(x.type, x.stack, x.prefix));
        }

        Player.SendSuccessMessage(
            $"升级成功\n" +
            $"等级 {Level - 1} => {Level}\n" +
            $"属性点 +{gradeInfo.AttributePoints}");
    }
    public void Upgrade(int selectItemIndex = -1)
    {
        var gradeInfo = FlowerSeaRPG.GradeInfos[Level];
        var needItems = gradeInfo.UpgradeItems.Select(x => (x.type, x.stack)).ToArray();
        var inventory = Player.TPlayer.inventory;
        for (int i = 0; i < needItems.Length; i++)
        {
            for (int slot = 0; slot < inventory.Length; slot++)
            {
                var item = inventory[slot];
                var needItem = needItems[i];
                if (item.type == needItem.type)
                {
                    if (item.stack <= needItem.stack)
                    {
                        needItem.stack -= item.stack;
                        item.SetDefaults();
                        item.stack = 0;
                    }
                    else
                    {
                        item.stack -= needItem.stack;
                        needItem.stack = 0;
                    }
                    TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", Player.Index, slot, item.prefix);
                }
                if (needItem.stack == 0)
                {
                    break;
                }
            }
        }

        if (selectItemIndex != -1)
        {
            var item = gradeInfo.GiveItems[selectItemIndex];
            Player.GiveItem(item.type, item.stack, item.prefix);
        }

        Level += 1;

        gradeInfo = FlowerSeaRPG.GradeInfos[Level];

        Title = gradeInfo.Title;
        AttributePoints += gradeInfo.AttributePoints;

        var tplayer = Player.TPlayer;
        UpdateLifeOrManaMax(ref tplayer.statLife, ref tplayer.statLifeMax, PacketTypes.PlayerHp, gradeInfo.Life);
        UpdateLifeOrManaMax(ref tplayer.statMana, ref tplayer.statManaMax, PacketTypes.PlayerMana, gradeInfo.Magic);


        Player.SendSuccessMessage(
            $"升级成功\n" +
            $"等级 {Level - 1} => {Level}\n" +
            $"属性点 +{gradeInfo.AttributePoints}");
    }
    //public void Upgrade(int level)
    //{
    //    var toLevel = Math.Min(FlowerSeaRPG.MainConfig.MaxLevel, level);
    //    for (int i = Level; i < toLevel; i++)
    //    {
    //        switch (CanUpgrade())
    //        {
    //            case CanUpgradeInfo.ItemEnought:
    //                Upgrade();
    //                break;
    //            case CanUpgradeInfo.ItemUnEnought:
    //                Player.SendInfoMessage("升级物品不足");
    //                return;
    //            case CanUpgradeInfo.LevelMax:
    //                Player.SendInfoMessage("已达最大等级");
    //                return;
    //        }
    //    }
    //}
    public void UpdateLifeOrManaMax(ref int stat, ref int statMax, PacketTypes packet, AddInfo info)
    {
        bool sendData = true;
        switch (info.Type)
        {
            case AddType.Add:
                stat += info.Value;
                statMax += info.Value;
                break;
            case AddType.Set:
                if (statMax >= info.Value)
                {
                    sendData = false;
                }
                else
                {
                    statMax = info.Value;
                    if (stat + info.Value > statMax)
                    {
                        stat = statMax;
                    }
                    else
                    {
                        stat += info.Value;
                    }
                }
                break;
            case AddType.None:
                sendData = false;
                break;
        }
        if (sendData)
        {
            TSPlayer.All.SendData(packet, "", Player.Index);
        }
    }
    public void CheckAttributePoint()
    {
        var sum = 0;
        for (int i = 1; i <= Level; i++) 
        {
            sum += FlowerSeaRPG.GradeInfos[i].AttributePoints;
        }
        var addInfo = FlowerSeaRPG.MainConfig.AttributeAddInfo;
        CheckMaxPoint(ref Damage, addInfo.DamageMaxCount);
        CheckMaxPoint(ref KnockBack, addInfo.KnockBackMaxCount);
        CheckMaxPoint(ref Scale, addInfo.ScaleMaxCount);
        CheckMaxPoint(ref Speed, addInfo.SpeedMaxCount);
        var curSum = Damage + KnockBack + Scale + Speed + AttributePoints;
        if (curSum != sum)
        {
            if (curSum > sum)
            {
                var cha = curSum - sum;
                if (cha <= AttributePoints)
                {
                    AttributePoints -= cha;
                }
                else
                {
                    Damage = 0;
                    KnockBack = 0;
                    Scale = 0;
                    Speed = 0;
                    AttributePoints = sum;
                }
            }
            else
            {
                AttributePoints += sum - curSum;
            }
        }
    }
    private void CheckMaxPoint(ref int attribute, int maxCount)
    {
        if (attribute > maxCount)
        {
            AttributePoints += attribute - maxCount;
            attribute = maxCount;
        }
    }
}
public enum CanUpgradeInfo
{
    ItemEnought = 0,
    ItemUnEnought = 1,
    LevelMax = 2,
    SelectIndexStrIsNull = 3,
    SelectIndexStrIsError = 4
}