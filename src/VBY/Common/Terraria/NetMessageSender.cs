using Terraria;
using TShockAPI;

namespace VBY.Common.Terraria;

public static class NetMessageSender
{
    /// <summary>
    /// ID = 5;
    /// 玩家背包、盔甲等个人存储内容
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="playerIndex"><see cref="TSPlayer.Index"/></param>
    /// <param name="slotIndex"></param>
    /// <param name="prefix"><see cref="Item.prefix"/></param>
    public static void PlayerSlot(int playerIndex, short slotIndex, byte prefix)
    {
        TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", playerIndex, slotIndex, prefix);
    }
    /// <summary>
    /// ID = 7;
    /// 世界信息
    /// 理应发送给所有玩家
    /// </summary>
    public static void WorldInfo()
    {
        TSPlayer.All.SendData(PacketTypes.WorldInfo);
    }
    /// <summary>
    /// ID = 16;
    /// 玩家血量和最大生命值
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="playerIndex"><see cref="TSPlayer.Index"/></param>
    public static void PlayerHp(int playerIndex)
    {
        TSPlayer.All.SendData(PacketTypes.PlayerHp, "", playerIndex);
    }
    /// <summary>
    /// ID = 21 或 ID = 90;
    /// 物品掉落
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="itemIndex"></param>
    public static void ItemDrop(int itemIndex, byte noDelay = 0)
    {
        TSPlayer.All.SendData(PacketTypes.ItemDrop, "", itemIndex, noDelay);
    }
    /// <summary>
    /// ID = 22
    /// 物品所有者
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="itemIndex"></param>
    public static void ItemOwner(int itemIndex)
    {
        TSPlayer.All.SendData(PacketTypes.ItemOwner, "", itemIndex);
    }
    /// <summary>
    /// ID = 23
    /// NPC更新
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="npcIndex"></param>
    public static void NPCUpdate(int npcIndex)
    {
        TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", npcIndex);
    }
    /// <summary>
    /// ID = 34
    /// 箱子更新
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="npcIndex"></param>
    public static void PlaceChest(int action,int x,int y,int style,int chestIndex)
    {
        TSPlayer.All.SendData(PacketTypes.PlaceChest, "", action, x, y, style, chestIndex);
    }
    /// <summary>
    /// ID = 42;
    /// 玩家魔力和最大魔力值
    /// 理应发送给所有玩家
    /// </summary>
    /// <param name="playerIndex"><see cref="TSPlayer.Index"/></param>
    public static void PlayerMana(int playerIndex)
    {
        TSPlayer.All.SendData(PacketTypes.PlayerHp, "", playerIndex);
    }
    /// <summary>
    /// ID = 88;
    /// 掉落在地上的物品属性修改
    /// 根据情况发送给所有玩家或是特定玩家
    /// </summary>
    /// <param name="itemIndex"><see cref="TSPlayer.Index"/></param>
    /// <param name="flag1">BitFlags: 1 = Color, 2 = Damage, 4 = Knockback, 8 = UseAnimation, 16 = UseTime, 32 = Shoot, 64 = ShootSpeed, 128 = NextFlags</param>
    /// <param name="flag2">if Flags1.NextFlags BitFlags: 1 = Width, 2 = Height, 4 = Scale, 8 = Ammo, 16 = UseAmmo, 32 = NotAmmo</param>
    public static void TweakItem(int itemIndex, byte flag1, byte flag2)
    {
        TSPlayer.All.SendData(PacketTypes.TweakItem, "", itemIndex, flag1, flag2);
    }
}
