using Terraria;

namespace VBY.GameContentModify.Config;

public enum ChestSpawnAction
{
    SpawnNPC,
    SpawnTile
}
public class ChestSpawnInfo
{
    public ChestSpawnAction Action;
    public int ItemType;
    public int ItemStack = 1;
    public bool OnlyEqualItemStack = true;
    public virtual bool CanExecute(Chest chest, Player user)
    {
        var dic = new Dictionary<int, int>();
        for (int i = 0; i < 40; i++)
        {
            Item chestItem = chest.item[i];
            if (chestItem is not null && chestItem.type > 0)
            {
                if (dic.TryGetValue(chestItem.type, out var stack))
                {
                    dic[chestItem.type] = stack + chestItem.stack;
                }
                else
                {
                    dic[chestItem.type] = chestItem.stack;
                }
            }
        }
        return dic.Count == 1 && dic.TryGetValue(ItemType, out var itemStack) && (OnlyEqualItemStack ? itemStack == ItemStack : itemStack >= ItemStack);
    }
    public virtual void Execute(int x, int y, Player user) { }
}
public class ChestSpawnNPCInfo : ChestSpawnInfo
{
    public int NPCType;
    private int _CrimsonNPCType;
    public int CrimsonNPCType
    {
        get => _CrimsonNPCType;
        set => _CrimsonNPCType = value == 0 ? NPCType : value;
    }
    public ChestSpawnNPCInfo()
    {
        Action = ChestSpawnAction.SpawnNPC;
    }

    public ChestSpawnNPCInfo(int itemType, int npcType, int crimsonNPCType = 0)
    {
        Action = ChestSpawnAction.SpawnNPC;
        ItemType = itemType;
        NPCType = npcType;
        CrimsonNPCType = crimsonNPCType;
    }
    public override void Execute(int x, int y, Player user)
    {
        int npcType = WorldGen.crimson ? CrimsonNPCType : NPCType;
        int npcIndex = NPC.NewNPC(user.GetNPCSource_TileInteraction(x, y), x * 16 + 16, y * 16 + 32, npcType);
        Main.npc[npcIndex].whoAmI = npcIndex;
        NetMessage.SendData(23, -1, -1, null, npcIndex);
        Main.npc[npcIndex].BigMimicSpawnSmoke();
    }
}
public class ChestSpawnTileInfo : ChestSpawnInfo
{
    public string Size = "22";
    public ushort TileType;
    public int Style;

    public ChestSpawnTileInfo()
    {
        Action = ChestSpawnAction.SpawnTile;
    }

    public override void Execute(int x, int y, Player user)
    {
        switch (Size)
        {
            case "22":
                WorldGen.Place2x2(x + 1, y + 1, TileType, Style);
                break;
            case "33":
                WorldGen.Place3x3(x, y + 1, TileType, Style);
                break;
        }
        NetMessage.SendTileSquare(-1, x, y, 5);
    }
}