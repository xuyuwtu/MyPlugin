using Microsoft.Xna.Framework;

using OTAPI;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.GameContent.Events;

using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

[ReplaceType(typeof(Item))]
public static class ReplaceItem
{
    [DetourMethod]
    public static bool CanShimmer(On.Terraria.Item.orig_CanShimmer orig, Item self)
    {
        var func = ShimmerItemReplaceInfo.CanShimmerFuncs[self.type];
        if (func is not null)
        {
            return func();
        }

        int shimmerEquivalentType = self.GetShimmerEquivalentType();
        if (Terraria.GameContent.ShimmerTransforms.IsItemTransformLocked(shimmerEquivalentType))
        {
            return false;
        }

        bool flag = false;
        //if ((type == 1326 || type == 779 || type == 3031 || type == 5364) && NPC.downedMoonlord)
        //{
        //    flag = true;
        //}

        if (self.type == ItemID.GelBalloon && !NPC.unlockedSlimeRainbowSpawn)
        {
            flag = true;
        }

        if (self.type == ItemID.LunarBrick || self.createTile == TileID.MusicBoxes)
        {
            flag = true;
        }

        if (!flag && ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] <= 0 && (MainConfigInfo.StaticDisableShimmerDecrafte | self.FindDecraftAmount() <= 0) && !ItemID.Sets.CommonCoin[self.type])
        {
            return self.makeNPC > 0;
        }

        return true;
    }
    [DetourMethod]
    public static void GetShimmered(On.Terraria.Item.orig_GetShimmered orig, Item self)
    {
        int shimmerEquivalentType = self.GetShimmerEquivalentType();
        int decraftingRecipeIndex = MainConfigInfo.StaticDisableShimmerDecrafte ? -1 : Terraria.GameContent.ShimmerTransforms.GetDecraftingRecipeIndex(shimmerEquivalentType);
        if (ItemID.Sets.CommonCoin[shimmerEquivalentType])
        {
            switch (shimmerEquivalentType)
            {
                case ItemID.SilverCoin:
                    self.stack *= 100;
                    break;
                case ItemID.GoldCoin:
                    self.stack *= 10000;
                    break;
                case ItemID.PlatinumCoin:
                    if (self.stack > 1)
                    {
                        self.stack = 1;
                    }
                    self.stack *= 1000000;
                    break;
            }
            Main.player[Main.myPlayer].AddCoinLuck(self.Center, self.stack);
            NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 1, (int)self.Center.X, (int)self.Center.Y, self.stack);
            self.type = ItemID.None;
            self.stack = 0;
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
            int num6 = self.stack;
            self.SetDefaults(num5);
            self.stack = num6;
            self.shimmered = true;
        }
        else if (self.createTile == TileID.MusicBoxes)
        {
            int num7 = self.stack;
            self.SetDefaults(ItemID.MusicBox);
            self.stack = num7;
            self.shimmered = true;
        }
        else if (ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] > 0)
        {
            //int num8 = item.stack;
            //item.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            //item.stack = num8;
            //item.shimmered = true;
            var oldstack = self.stack;
            self.SetDefaults(ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType]);
            if (oldstack > self.maxStack)
            {
                self.stack = self.maxStack;
                Main.item[Item.NewItem(null, self.Center, self.velocity, shimmerEquivalentType, oldstack - self.maxStack)].shimmered = true;
            }
            else
            {
                self.stack = oldstack;
            }
            self.shimmered = true;
        }
        else if (self.type == ItemID.GelBalloon)
        {
            if (NPC.unlockedSlimeRainbowSpawn)
            {
                return;
            }
            NPC.unlockedSlimeRainbowSpawn = true;
            NetMessage.SendData(MessageID.WorldData);
            int num9 = NPC.NewNPC(self.GetNPCSource_FromThis(), (int)self.Center.X + 4, (int)self.Center.Y, NPCID.TownSlimeRainbow);
            if (num9 >= 0)
            {
                NPC obj = Main.npc[num9];
                obj.velocity = self.velocity;
                obj.netUpdate = true;
                obj.shimmerTransparency = 1f;
                NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num9);
            }
            WorldGen.CheckAchievement_RealEstateAndTownSlimes();
            self.stack--;
            if (self.stack <= 0)
            {
                self.type = ItemID.None;
            }
        }
        else if (self.makeNPC > 0)
        {
            int num10 = 50;
            int highestNPCSlotIndexWeWillPick = 200;
            int num11 = NPC.GetAvailableAmountOfNPCsToSpawnUpToSlot(self.stack, highestNPCSlotIndexWeWillPick);
            while (num10 > 0 && num11 > 0 && self.stack > 0)
            {
                num10--;
                num11--;
                self.stack--;
                int num12 = ((NPCID.Sets.ShimmerTransformToNPC[self.makeNPC] < 0) ? NPC.ReleaseNPC((int)self.Center.X, (int)self.Bottom.Y, self.makeNPC, self.placeStyle, Main.myPlayer) : NPC.ReleaseNPC((int)self.Center.X, (int)self.Bottom.Y, NPCID.Sets.ShimmerTransformToNPC[self.makeNPC], 0, Main.myPlayer));
                if (num12 >= 0)
                {
                    Main.npc[num12].shimmerTransparency = 1f;
                    NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 2, num12);
                }
            }
            self.shimmered = true;
            if (self.stack <= 0)
            {
                self.type = ItemID.None;
            }
        }
        else if (decraftingRecipeIndex >= 0)
        {
            int decraftAmount = self.FindDecraftAmount();
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
                    int newItemIndex = Item.NewItem(self.GetItemSource_Misc(8), (int)self.position.X, (int)self.position.Y, self.width, self.height, requiredItem.type);
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
            self.stack -= decraftAmount * recipe.createItem.stack;
            if (self.stack <= 0)
            {
                self.stack = 0;
                self.type = ItemID.None;
            }
        }
        self.shimmerTime = self.stack > 0 ? 1f : 0f;
        self.shimmerWet = true;
        self.wet = true;
        self.velocity *= 0.1f;
        //if (Main.netMode == 0)
        //{
        //    Item.ShimmerEffect(item.Center);
        //}
        //else
        //{
        NetMessage.SendData(MessageID.ShimmerActions, -1, -1, null, 0, (int)self.Center.X, (int)self.Center.Y);
        NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, self.whoAmI, 1f);
        //}
        Terraria.GameContent.Achievements.AchievementsHelper.NotifyProgressionEvent(AchievementHelperID.Events.TransmuteItem);
        if (self.stack == 0)
        {
            self.makeNPC = -1;
            self.active = false;
        }
    }
    public static bool MechSpawn(float x, float y, int type)
    {
        int numOfAll = 0;
        int numOf300 = 0;
        int numOf800 = 0;
        for (int i = 0; i < 200; i++)
        {
            if (Main.item[i].active && Main.item[i].type == type)
            {
                numOfAll++;
                float num6 = (Main.item[i].position - new Vector2(x, y)).Length();
                if (num6 < 300f)
                {
                    if (MechInfo.StaticItemSpawnLimitUseStack)
                    {
                        numOf300 += Main.item[i].stack;
                    }
                    else
                    {
                        numOf300++;
                    }
                }
                if (num6 < 800f)
                {
                    if (MechInfo.StaticItemSpawnLimitUseStack)
                    {
                        numOf800 += Main.item[i].stack;
                    }
                    else
                    {
                        numOf800++;
                    }
                }
            }
        }
        if (numOf300 >= MechInfo.StaticItemSpawnLimitOfRange300 || numOf800 >= MechInfo.StaticItemSpawnLimitOfRange800 || numOfAll >= MechInfo.StaticItemSpawnLimitOfWorld)
        {
            return Hooks.Item.InvokeMechSpawn(result: false, x, y, type, numOfAll, numOf300, numOf800);
        }
        return Hooks.Item.InvokeMechSpawn(result: true, x, y, type, numOfAll, numOf300, numOf800);
    }
    public static void CheckLavaDeath(Item self, int i)
    {
        if (self.type == ItemID.GuideVoodooDoll)
        {
            int num = self.stack;
            self.active = false;
            self.type = ItemID.None;
            self.stack = 0;
            bool flag = false;
            for (int j = 0; j < 200; j++)
            {
                if (Main.npc[j].active && Main.npc[j].type == NPCID.Guide)
                {
                    int num2 = -Main.npc[j].direction;
                    if (Main.npc[j].IsNPCValidForBestiaryKillCredit())
                    {
                        Main.BestiaryTracker.Kills.RegisterKill(Main.npc[j]);
                    }
                    Main.npc[j].StrikeNPCNoInteraction(9999, 10f, -num2);
                    num--;
                    flag = true;
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, j, 9999f, 10f, -num2);
                    NPC.SpawnWOF(self.position);
                }
            }
            if (!flag) //&& MainConfigInfo.StaticGuideVoodooDollSpawnWOFCanWithoutGuide)
            {
                NPC.SpawnWOF(self.position);
            }
            if (flag)
            {
                var list = new List<int>();
                for (int k = 0; k < 200; k++)
                {
                    if (num <= 0)
                    {
                        break;
                    }
                    NPC nPC = Main.npc[k];
                    if (nPC.active && nPC.isLikeATownNPC)
                    {
                        list.Add(k);
                    }
                }
                while (num > 0 && list.Count > 0)
                {
                    int index = Main.rand.Next(list.Count);
                    int num3 = list[index];
                    list.RemoveAt(index);
                    int num4 = -Main.npc[num3].direction;
                    if (Main.npc[num3].IsNPCValidForBestiaryKillCredit())
                    {
                        Main.BestiaryTracker.Kills.RegisterKill(Main.npc[num3]);
                    }
                    Main.npc[num3].StrikeNPCNoInteraction(9999, 10f, -num4);
                    num--;
                    if (Main.netMode == 2)
                    {
                        NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, num3, 9999f, 10f, -num4);
                    }
                }
            }
            NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
        }
        else if (self.playerIndexTheItemIsReservedFor == Main.myPlayer && self.rare == 0 && self.type >= 0 && self.type < ItemID.Count && !ItemID.Sets.IsLavaImmuneRegardlessOfRarity[self.type])
        {
            self.active = false;
            self.type = ItemID.None;
            self.stack = 0;
            if (Main.netMode != 0)
            {
                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, i);
            }
        }
    }

}
