using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.GameContent.Events;

using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Item))]
public static class ReplaceItem
{
    public static bool CanShimmer(Item item)
    {
        var func = ShimmerItemReplaceInfo.CanShimmerFuncs[item.type];
        if (func is not null)
        {
            return func();
        }

        int shimmerEquivalentType = item.GetShimmerEquivalentType();
        if (Terraria.GameContent.ShimmerTransforms.IsItemTransformLocked(shimmerEquivalentType))
        {
            return false;
        }

        bool flag = false;
        //if ((type == 1326 || type == 779 || type == 3031 || type == 5364) && NPC.downedMoonlord)
        //{
        //    flag = true;
        //}

        if (item.type == 4986 && !NPC.unlockedSlimeRainbowSpawn)
        {
            flag = true;
        }

        if (item.type == 3461 || item.createTile == TileID.MusicBoxes)
        {
            flag = true;
        }

        if (!flag && ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] <= 0 && (MainConfigInfo.StaticDisableShimmerDecrafte | item.FindDecraftAmount() <= 0) && !ItemID.Sets.CommonCoin[item.type])
        {
            return item.makeNPC > 0;
        }

        return true;
    }
    public static void GetShimmered(Item item)
    {
        int shimmerEquivalentType = item.GetShimmerEquivalentType();
        int decraftingRecipeIndex = MainConfigInfo.StaticDisableShimmerDecrafte ? -1 : Terraria.GameContent.ShimmerTransforms.GetDecraftingRecipeIndex(shimmerEquivalentType);
        if (ItemID.Sets.CommonCoin[shimmerEquivalentType])
        {
            switch (shimmerEquivalentType)
            {
                case ItemID.SilverCoin:
                    item.stack *= 100;
                    break;
                case ItemID.GoldCoin:
                    item.stack *= 10000;
                    break;
                case ItemID.PlatinumCoin:
                    if (item.stack > 1)
                    {
                        item.stack = 1;
                    }
                    item.stack *= 1000000;
                    break;
            }
            Main.player[Main.myPlayer].AddCoinLuck(item.Center, item.stack);
            NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 1, (int)item.Center.X, (int)item.Center.Y, item.stack);
            item.type = 0;
            item.stack = 0;
        }
        else if (shimmerEquivalentType == ItemID.LunarBrick)
        {
            short num5 = Main.GetMoonPhase() switch
            {
                MoonPhase.QuarterAtRight => ItemID.StarRoyaleBrick,
                MoonPhase.HalfAtRight => ItemID.CryocoreBrick,
                MoonPhase.ThreeQuartersAtRight => ItemID.CosmicEmberBrick,
                MoonPhase.Full => ItemID.HeavenforgeBrick,
                MoonPhase.ThreeQuartersAtLeft => ItemID.LunarRustBrick,
                MoonPhase.HalfAtLeft => ItemID.AstraBrick,
                MoonPhase.QuarterAtLeft => ItemID.DarkCelestialBrick,
                _ => ItemID.MercuryBrick,
            };
            int num6 = item.stack;
            item.SetDefaults(num5);
            item.stack = num6;
            item.shimmered = true;
        }
        else if (item.createTile == TileID.MusicBoxes)
        {
            int num7 = item.stack;
            item.SetDefaults(ItemID.MusicBox);
            item.stack = num7;
            item.shimmered = true;
        }
        else if (ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] > 0)
        {
            //int num8 = item.stack;
            //item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            //item.stack = num8;
            //item.shimmered = true;
            var oldstack = item.stack;
            item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            if (oldstack > item.maxStack)
            {
                item.stack = item.maxStack;
                Main.item[Item.NewItem(null, item.Center, item.velocity, shimmerEquivalentType, oldstack - item.maxStack)].shimmered = true;
            }
            else
            {
                item.stack = oldstack;
            }
            item.shimmered = true;
        }
        else if (item.type == ItemID.GelBalloon)
        {
            if (NPC.unlockedSlimeRainbowSpawn)
            {
                return;
            }
            NPC.unlockedSlimeRainbowSpawn = true;
            NetMessage.SendData(MessageID.WorldData);
            int num9 = NPC.NewNPC(item.GetNPCSource_FromThis(), (int)item.Center.X + 4, (int)item.Center.Y, 681);
            if (num9 >= 0)
            {
                NPC obj = Main.npc[num9];
                obj.velocity = item.velocity;
                obj.netUpdate = true;
                obj.shimmerTransparency = 1f;
                NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num9);
            }
            WorldGen.CheckAchievement_RealEstateAndTownSlimes();
            item.stack--;
            if (item.stack <= 0)
            {
                item.type = 0;
            }
        }
        else if (item.makeNPC > 0)
        {
            int num10 = 50;
            int highestNPCSlotIndexWeWillPick = 200;
            int num11 = NPC.GetAvailableAmountOfNPCsToSpawnUpToSlot(item.stack, highestNPCSlotIndexWeWillPick);
            while (num10 > 0 && num11 > 0 && item.stack > 0)
            {
                num10--;
                num11--;
                item.stack--;
                int num12 = ((NPCID.Sets.ShimmerTransformToNPC[item.makeNPC] < 0) ? NPC.ReleaseNPC((int)item.Center.X, (int)item.Bottom.Y, item.makeNPC, item.placeStyle, Main.myPlayer) : NPC.ReleaseNPC((int)item.Center.X, (int)item.Bottom.Y, NPCID.Sets.ShimmerTransformToNPC[item.makeNPC], 0, Main.myPlayer));
                if (num12 >= 0)
                {
                    Main.npc[num12].shimmerTransparency = 1f;
                    NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num12);
                }
            }
            item.shimmered = true;
            if (item.stack <= 0)
            {
                item.type = 0;
            }
        }
        else if (decraftingRecipeIndex >= 0)
        {
            int decraftAmount = item.FindDecraftAmount();
            Recipe recipe = Main.recipe[decraftingRecipeIndex];
            int num14 = 0;
            bool flag = recipe.requiredItem[1].stack > 0;
            IEnumerable<Item> enumerable = recipe.requiredItem;
            if (recipe.customShimmerResults != null)
            {
                enumerable = recipe.customShimmerResults;
            }
            int num15 = 0;
            foreach (Item requiredItem in enumerable)
            {
                if (requiredItem.type <= 0)
                {
                    break;
                }
                num15++;
                int needSpawnStack = decraftAmount * requiredItem.stack;
                if (recipe.alchemy)
                {
                    for (int num17 = needSpawnStack; num17 > 0; num17--)
                    {
                        if (Main.rand.Next(3) == 0)
                        {
                            needSpawnStack--;
                        }
                    }
                }
                while (needSpawnStack > 0)
                {
                    int stack = needSpawnStack;
                    //if (stack > 9999)
                    //{
                    //    stack = 9999;
                    //}
                    if (stack > requiredItem.maxStack)
                    {
                        stack = requiredItem.maxStack;
                    }
                    needSpawnStack -= stack;
                    int newItemIndex = Item.NewItem(item.GetItemSource_Misc(8), (int)item.position.X, (int)item.position.Y, item.width, item.height, requiredItem.type);
                    Item newItem = Main.item[newItemIndex];
                    newItem.stack = stack;
                    newItem.shimmerTime = 1f;
                    newItem.shimmered = true;
                    newItem.shimmerWet = true;
                    newItem.wet = true;
                    newItem.velocity *= 0.1f;
                    newItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
                    if (flag)
                    {
                        newItem.velocity.X = 1f * num15;
                        newItem.velocity.X *= 1f + num15 * 0.05f;
                        if (num14 % 2 == 0)
                        {
                            newItem.velocity.X *= -1f;
                        }
                    }
                    NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, newItemIndex, 1f);
                }
            }
            item.stack -= decraftAmount * recipe.createItem.stack;
            if (item.stack <= 0)
            {
                item.stack = 0;
                item.type = 0;
            }
        }
        item.shimmerTime = item.stack > 0 ? 1f : 0f;
        item.shimmerWet = true;
        item.wet = true;
        item.velocity *= 0.1f;
        //if (Main.netMode == 0)
        //{
        //    Item.ShimmerEffect(item.Center);
        //}
        //else
        //{
        NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 0, (int)item.Center.X, (int)item.Center.Y);
        NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, item.whoAmI, 1f);
        //}
        Terraria.GameContent.Achievements.AchievementsHelper.NotifyProgressionEvent(AchievementHelperID.Events.TransmuteItem);
        if (item.stack == 0)
        {
            item.makeNPC = -1;
            item.active = false;
        }
    }
}
