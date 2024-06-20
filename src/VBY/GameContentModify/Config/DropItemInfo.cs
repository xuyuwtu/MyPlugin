using Terraria;
using Terraria.DataStructures;

namespace VBY.GameContentModify.Config;

public class DropItemInfo
{
    public int RandomValue = 1;
    public RandomItemInfo[] Items = Array.Empty<RandomItemInfo>();
    public DropItemInfo[]? Else;
    public bool Continute = true;
    public bool TryDrop(int x, int y)
    {
        if(RandomValue == -1)
        {
            Utils.SelectRandom(Main.rand, Items).NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x, y);
            return true;
        }
        if (Main.rand.NextIsZero(RandomValue))
        {
            foreach (var itemInfo in Items)
            {
                itemInfo.NewItem(WorldGen.GetItemSource_FromTileBreak(x, y), x, y);
            }
            return true;
        }
        if (Else is not null)
        {
            for(int i = 0; i < Else.Length; i++)
            {
                if (Else[i].TryDrop(x, y) && !Else[i].Continute)
                {
                    break;
                }
            }
        }
        return false;
    }
}

public class RandomItemInfo : ItemInfo
{
    public int? stackEnd;
    public RandomItemInfo() { }
    public RandomItemInfo(int type, int stack = 1, int prefix = -1) : base(type, stack, prefix) { }
    public override int NewItem(IEntitySource source, int tileX, int tileY)
    {
        int newStack;
        if (!stackEnd.HasValue || stack == stackEnd)
        {
            newStack = stack;
        }
        else
        {
            newStack = WorldGen.genRand.Next(stack, stackEnd.Value + 1);
        }
        return Item.NewItem(source, tileX * 16, tileY * 16, 32, 32, type, newStack, false, prefix);
    }
}
