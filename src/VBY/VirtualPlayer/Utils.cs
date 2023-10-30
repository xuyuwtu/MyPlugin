using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;

using MonoMod.RuntimeDetour.HookGen;

using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.GameContent.Creative;
using Terraria.Localization;

using TShockAPI;

namespace VBY.VirtualPlayer;

internal static class Utils
{
    public static bool FindByNameOrID(string search, [MaybeNullWhen(false)]out TSPlayer player)
    {
        var list = TSPlayer.FindByNameOrID(search);
        player = list.Count == 1 ? list[0] : null;
        return list.Count == 1;
    }
    public static void ClearOwner(Delegate hook)
    {
        var dic = (IDictionary)typeof(HookEndpointManager).GetField("OwnedHookLists", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var owner = HookEndpointManager.GetOwner(hook);
        if (owner is not null)
        {
            dic.Remove(owner);
        }
    }
    public static Vector2 Normalize(this Vector2 vector2, float num)
    {
        return vector2 * num / (float)Math.Sqrt(vector2.X * vector2.X + vector2.Y * vector2.Y);
    }

    public static PlayerFileData LoadPlayer(string playerPath)
    {
        PlayerFileData playerFileData = new(playerPath, false);
        Player player = new();
        var myAes = Aes.Create();
        using (MemoryStream stream = new(File.ReadAllBytes(playerPath)))
        {
            using CryptoStream input = new(stream, myAes.CreateDecryptor(Player.ENCRYPTION_KEY, Player.ENCRYPTION_KEY), CryptoStreamMode.Read);
            using BinaryReader binaryReader = new(input);
            int num = binaryReader.ReadInt32();
            if (num >= 135)
            {
                playerFileData.Metadata = FileMetadata.Read(binaryReader, FileType.Player);
            }
            else
            {
                playerFileData.Metadata = FileMetadata.FromCurrentSettings(FileType.Player);
            }
            if (num > 279)
            {
                player.loadStatus = 1;
                player.name = binaryReader.ReadString();
                playerFileData.Player = player;
                return playerFileData;
            }
            //Player.Deserialize(playerFileData, player, binaryReader, num, out _);
            Deserialize(playerFileData, player, binaryReader, num, out _);
        }
        player.PlayerFrame();
        player.loadStatus = 0;
        playerFileData.Player = player;
        return playerFileData;
    }
    public static Item GetItemUseSlot(Player player, int slot)
    {
        return (slot >= PlayerItemSlotID.Loadout3_Dye_0) 
            ? player.Loadouts[2].Dye[slot - PlayerItemSlotID.Loadout3_Dye_0] 
            : ((slot >= PlayerItemSlotID.Loadout3_Armor_0) 
            ? player.Loadouts[2].Armor[slot - PlayerItemSlotID.Loadout3_Armor_0] 
            : ((slot >= PlayerItemSlotID.Loadout2_Dye_0) 
            ? player.Loadouts[1].Dye[slot - PlayerItemSlotID.Loadout2_Dye_0] 
            : ((slot >= PlayerItemSlotID.Loadout2_Armor_0) 
            ? player.Loadouts[1].Armor[slot - PlayerItemSlotID.Loadout2_Armor_0] 
            : ((slot >= PlayerItemSlotID.Loadout1_Dye_0) 
            ? player.Loadouts[0].Dye[slot - PlayerItemSlotID.Loadout1_Dye_0] 
            : ((slot >= PlayerItemSlotID.Loadout1_Armor_0) 
            ? player.Loadouts[0].Armor[slot - PlayerItemSlotID.Loadout1_Armor_0] 
            : ((slot >= PlayerItemSlotID.Bank4_0) 
            ? player.bank4.item[slot - PlayerItemSlotID.Bank4_0] 
            : ((slot >= PlayerItemSlotID.Bank3_0) 
            ? player.bank3.item[slot - PlayerItemSlotID.Bank3_0] 
            : ((slot >= PlayerItemSlotID.TrashItem) 
            ? player.trashItem 
            : ((slot >= PlayerItemSlotID.Bank2_0) 
            ? player.bank2.item[slot - PlayerItemSlotID.Bank2_0] 
            : ((slot >= PlayerItemSlotID.Bank1_0) 
            ? player.bank.item[slot - PlayerItemSlotID.Bank1_0] 
            : ((slot >= PlayerItemSlotID.MiscDye0) 
            ? player.miscDyes[slot - PlayerItemSlotID.MiscDye0] 
            : ((slot >= PlayerItemSlotID.Misc0) 
            ? player.miscEquips[slot - PlayerItemSlotID.Misc0] 
            : ((slot >= PlayerItemSlotID.Dye0) 
            ? player.dye[slot - PlayerItemSlotID.Dye0] 
            : ((!(slot >= PlayerItemSlotID.Armor0)) 
            ? player.inventory[slot - PlayerItemSlotID.Inventory0] 
            : player.armor[slot - PlayerItemSlotID.Armor0]))))))))))))));
    }
    public static void SavePlayer(Player player,string path)
    {
        Aes myAes = Aes.Create();
        using Stream stream = new FileStream(path, FileMode.Create);
        using CryptoStream cryptoStream = new(stream, myAes.CreateEncryptor(Player.ENCRYPTION_KEY, Player.ENCRYPTION_KEY), CryptoStreamMode.Write);
        PlayerFileData playerFileData = new()
        {
            Metadata = FileMetadata.FromCurrentSettings(FileType.Player),
            Player = player,
            _isCloudSave = false,
            _path = path
        };
        Main.LocalFavoriteData.ClearEntry(playerFileData);
        using BinaryWriter binaryWriter = new(cryptoStream);
        //230 1.4.0.5
        //269 1.4.4.0
        binaryWriter.Write(269);
        playerFileData.Metadata.Write(binaryWriter);
        binaryWriter.Write(player.name);
        binaryWriter.Write(player.difficulty);
        binaryWriter.Write(playerFileData.GetPlayTime().Ticks);
        binaryWriter.Write(player.hair);
        binaryWriter.Write(player.hairDye);
        BitsByte bitsByte = 0;
        for (int i = 0; i < 8; i++)
        {
            bitsByte[i] = player.hideVisibleAccessory[i];
        }
        binaryWriter.Write(bitsByte);
        bitsByte = 0;
        for (int j = 0; j < 2; j++)
        {
            bitsByte[j] = player.hideVisibleAccessory[j + 8];
        }
        binaryWriter.Write(bitsByte);
        binaryWriter.Write(player.hideMisc);
        binaryWriter.Write((byte)player.skinVariant);
        binaryWriter.Write(player.statLife);
        binaryWriter.Write(player.statLifeMax);
        binaryWriter.Write(player.statMana);
        binaryWriter.Write(player.statManaMax);
        binaryWriter.Write(player.extraAccessory);
        binaryWriter.Write(player.unlockedBiomeTorches);
        binaryWriter.Write(player.UsingBiomeTorches);

        binaryWriter.Write(player.ateArtisanBread);
        binaryWriter.Write(player.usedAegisCrystal);
        binaryWriter.Write(player.usedAegisFruit);
        binaryWriter.Write(player.usedArcaneCrystal);
        binaryWriter.Write(player.usedGalaxyPearl);
        binaryWriter.Write(player.usedGummyWorm);
        binaryWriter.Write(player.usedAmbrosia);

        binaryWriter.Write(player.downedDD2EventAnyDifficulty);
        binaryWriter.Write(player.taxMoney);

        binaryWriter.Write(player.numberOfDeathsPVE);
        binaryWriter.Write(player.numberOfDeathsPVP);

        binaryWriter.WriteRGB(player.hairColor);
        binaryWriter.WriteRGB(player.skinColor);
        binaryWriter.WriteRGB(player.eyeColor);
        binaryWriter.WriteRGB(player.shirtColor);
        binaryWriter.WriteRGB(player.underShirtColor);
        binaryWriter.WriteRGB(player.pantsColor);
        binaryWriter.WriteRGB(player.shoeColor);
        for (int k = 0; k < player.armor.Length; k++)
        {
            binaryWriter.Write(player.armor[k].netID);
            binaryWriter.Write(player.armor[k].prefix);
        }
        for (int l = 0; l < player.dye.Length; l++)
        {
            binaryWriter.Write(player.dye[l].netID);
            binaryWriter.Write(player.dye[l].prefix);
        }
        for (int m = 0; m < 58; m++)
        {
            binaryWriter.Write(player.inventory[m].netID);
            binaryWriter.Write(player.inventory[m].stack);
            binaryWriter.Write(player.inventory[m].prefix);
            binaryWriter.Write(player.inventory[m].favorited);
        }
        for (int n = 0; n < player.miscEquips.Length; n++)
        {
            binaryWriter.Write(player.miscEquips[n].netID);
            binaryWriter.Write(player.miscEquips[n].prefix);
            binaryWriter.Write(player.miscDyes[n].netID);
            binaryWriter.Write(player.miscDyes[n].prefix);
        }
        for (int num = 0; num < 40; num++)
        {
            binaryWriter.Write(player.bank.item[num].netID);
            binaryWriter.Write(player.bank.item[num].stack);
            binaryWriter.Write(player.bank.item[num].prefix);
        }
        for (int num2 = 0; num2 < 40; num2++)
        {
            binaryWriter.Write(player.bank2.item[num2].netID);
            binaryWriter.Write(player.bank2.item[num2].stack);
            binaryWriter.Write(player.bank2.item[num2].prefix);
        }
        for (int num3 = 0; num3 < 40; num3++)
        {
            binaryWriter.Write(player.bank3.item[num3].netID);
            binaryWriter.Write(player.bank3.item[num3].stack);
            binaryWriter.Write(player.bank3.item[num3].prefix);
        }
        for (int num4 = 0; num4 < 40; num4++)
        {
            binaryWriter.Write(player.bank4.item[num4].netID);
            binaryWriter.Write(player.bank4.item[num4].stack);
            binaryWriter.Write(player.bank4.item[num4].prefix);
            binaryWriter.Write(player.bank4.item[num4].favorited);
        }
        binaryWriter.Write(player.voidVaultInfo);
        for (int num5 = 0; num5 < 44; num5++)
        {
            if (Main.buffNoSave[player.buffType[num5]])
            {
                binaryWriter.Write(0);
                binaryWriter.Write(0);
            }
            else
            {
                binaryWriter.Write(player.buffType[num5]);
                binaryWriter.Write(player.buffTime[num5]);
            }
        }
        for (int num6 = 0; num6 < 200; num6++)
        {
            if (player.spN[num6] == null)
            {
                binaryWriter.Write(-1);
                break;
            }
            binaryWriter.Write(player.spX[num6]);
            binaryWriter.Write(player.spY[num6]);
            binaryWriter.Write(player.spI[num6]);
            binaryWriter.Write(player.spN[num6]);
        }
        binaryWriter.Write(player.hbLocked);
        for (int num7 = 0; num7 < player.hideInfo.Length; num7++)
        {
            binaryWriter.Write(player.hideInfo[num7]);
        }
        binaryWriter.Write(player.anglerQuestsFinished);
        for (int num8 = 0; num8 < player.DpadRadial.Bindings.Length; num8++)
        {
            binaryWriter.Write(player.DpadRadial.Bindings[num8]);
        }
        for (int num9 = 0; num9 < player.builderAccStatus.Length; num9++)
        {
            binaryWriter.Write(player.builderAccStatus[num9]);
        }
        binaryWriter.Write(player.bartenderQuestLog);
        binaryWriter.Write(player.dead);
        if (player.dead)
        {
            binaryWriter.Write(player.respawnTimer);
        }
        long value = DateTime.UtcNow.ToBinary();
        binaryWriter.Write(value);
        binaryWriter.Write(player.golferScoreAccumulated);
        //SaveSacrifice(binaryWriter);
        player.SaveTemporaryItemSlotContents(binaryWriter);
        CreativePowerManager.Instance.SaveToPlayer(player, binaryWriter);
        BitsByte bitsByte2 = default;
        bitsByte2[0] = player.unlockedSuperCart;
        bitsByte2[1] = player.enabledSuperCart;
        binaryWriter.Write(bitsByte2);
        binaryWriter.Write(player.CurrentLoadoutIndex);
        for (int num10 = 0; num10 < player.Loadouts.Length; num10++)
        {
            player.Loadouts[num10].Serialize(binaryWriter);
        }

        binaryWriter.Flush();
        cryptoStream.FlushFinalBlock();
        stream.Flush();
    }
    public static void SyncEquipments(int whoAmI, ref int slot, int count, Item[] items)
    {
        for (int i = 0; i < count; i++) 
        {
            NetSender.SyncEquipment(whoAmI, slot, items[i]);
            slot++;
        }
    }
    public static void RestoreInventory(Player player)
    {
        NetSender.SyncLoadout((byte)player.whoAmI, (byte)player.CurrentLoadoutIndex, ushort.MaxValue);
        int slot = 0;
        SyncEquipments(player.whoAmI, ref slot, NetItem.InventorySlots, player.inventory);
        SyncEquipments(player.whoAmI, ref slot, NetItem.ArmorSlots, player.armor);
        SyncEquipments(player.whoAmI, ref slot, NetItem.DyeSlots, player.dye);
        SyncEquipments(player.whoAmI, ref slot, NetItem.MiscEquipSlots, player.miscEquips);
        SyncEquipments(player.whoAmI, ref slot, NetItem.MiscDyeSlots, player.miscDyes);
    }
    public static void Deserialize(PlayerFileData data, Player newPlayer, BinaryReader fileIO, int release, out bool gotToReadName)
    {
        newPlayer.name = fileIO.ReadString();
        gotToReadName = true;
        if (release >= 10)
        {
            if (release >= 17)
            {
                newPlayer.difficulty = fileIO.ReadByte();
            }
            else if (fileIO.ReadBoolean())
            {
                newPlayer.difficulty = 2;
            }
        }
        if (release >= 138)
        {
            data.SetPlayTime(new TimeSpan(fileIO.ReadInt64()));
        }
        else
        {
            data.SetPlayTime(TimeSpan.Zero);
        }
        newPlayer.hair = fileIO.ReadInt32();
        if (release >= 82)
        {
            newPlayer.hairDye = fileIO.ReadByte();
        }
        if (release >= 124)
        {
            BitsByte bitsByte = fileIO.ReadByte();
            for (int i = 0; i < 8; i++)
            {
                newPlayer.hideVisibleAccessory[i] = bitsByte[i];
            }
            bitsByte = fileIO.ReadByte();
            for (int j = 0; j < 2; j++)
            {
                newPlayer.hideVisibleAccessory[j + 8] = bitsByte[j];
            }
        }
        else if (release >= 83)
        {
            BitsByte bitsByte2 = fileIO.ReadByte();
            for (int k = 0; k < 8; k++)
            {
                newPlayer.hideVisibleAccessory[k] = bitsByte2[k];
            }
        }
        if (release >= 119)
        {
            newPlayer.hideMisc = fileIO.ReadByte();
        }
        if (release <= 17)
        {
            if (newPlayer.hair == 5 || newPlayer.hair == 6 || newPlayer.hair == 9 || newPlayer.hair == 11)
            {
                newPlayer.Male = false;
            }
            else
            {
                newPlayer.Male = true;
            }
        }
        else if (release < 107)
        {
            newPlayer.Male = fileIO.ReadBoolean();
        }
        else
        {
            newPlayer.skinVariant = fileIO.ReadByte();
        }
        if (release < 161 && newPlayer.skinVariant == 7)
        {
            newPlayer.skinVariant = 9;
        }
        newPlayer.statLife = fileIO.ReadInt32();
        newPlayer.statLifeMax = fileIO.ReadInt32();
        if (newPlayer.statLifeMax > 500)
        {
            newPlayer.statLifeMax = 500;
        }
        newPlayer.statMana = fileIO.ReadInt32();
        newPlayer.statManaMax = fileIO.ReadInt32();
        if (newPlayer.statManaMax > 200)
        {
            newPlayer.statManaMax = 200;
        }
        if (newPlayer.statMana > 400)
        {
            newPlayer.statMana = 400;
        }
        if (release >= 125)
        {
            newPlayer.extraAccessory = fileIO.ReadBoolean();
        }
        if (release >= 229)
        {
            newPlayer.unlockedBiomeTorches = fileIO.ReadBoolean();
            newPlayer.UsingBiomeTorches = fileIO.ReadBoolean();
            if (release >= 256)
            {
                newPlayer.ateArtisanBread = fileIO.ReadBoolean();
            }
            if (release >= 260)
            {
                newPlayer.usedAegisCrystal = fileIO.ReadBoolean();
                newPlayer.usedAegisFruit = fileIO.ReadBoolean();
                newPlayer.usedArcaneCrystal = fileIO.ReadBoolean();
                newPlayer.usedGalaxyPearl = fileIO.ReadBoolean();
                newPlayer.usedGummyWorm = fileIO.ReadBoolean();
                newPlayer.usedAmbrosia = fileIO.ReadBoolean();
            }
        }
        if (release >= 182)
        {
            newPlayer.downedDD2EventAnyDifficulty = fileIO.ReadBoolean();
        }
        if (release >= 128)
        {
            newPlayer.taxMoney = fileIO.ReadInt32();
        }
        if (release >= 254)
        {
            newPlayer.numberOfDeathsPVE = fileIO.ReadInt32();
        }
        if (release >= 254)
        {
            newPlayer.numberOfDeathsPVP = fileIO.ReadInt32();
        }
        newPlayer.hairColor = fileIO.ReadRGB();
        newPlayer.skinColor = fileIO.ReadRGB();
        newPlayer.eyeColor = fileIO.ReadRGB();
        newPlayer.shirtColor = fileIO.ReadRGB();
        newPlayer.underShirtColor = fileIO.ReadRGB();
        newPlayer.pantsColor = fileIO.ReadRGB();
        newPlayer.shoeColor = fileIO.ReadRGB();
        if (release >= 38)
        {
            if (release < 124)
            {
                int num = 11;
                if (release >= 81)
                {
                    num = 16;
                }
                for (int l = 0; l < num; l++)
                {
                    int num2 = l;
                    if (num2 >= 8)
                    {
                        num2 += 2;
                    }
                    newPlayer.armor[num2].netDefaults(fileIO.ReadInt32());
                    newPlayer.armor[num2].Prefix(fileIO.ReadByte());
                }
            }
            else
            {
                int num3 = 20;
                for (int m = 0; m < num3; m++)
                {
                    newPlayer.armor[m].netDefaults(fileIO.ReadInt32());
                    newPlayer.armor[m].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 47)
            {
                int num4 = 3;
                if (release >= 81)
                {
                    num4 = 8;
                }
                if (release >= 124)
                {
                    num4 = 10;
                }
                for (int n = 0; n < num4; n++)
                {
                    int num5 = n;
                    newPlayer.dye[num5].netDefaults(fileIO.ReadInt32());
                    newPlayer.dye[num5].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 58)
            {
                for (int num6 = 0; num6 < 58; num6++)
                {
                    int num7 = fileIO.ReadInt32();
                    if (num7 >= ItemID.Count)
                    {
                        newPlayer.inventory[num6].netDefaults(0);
                        fileIO.ReadInt32();
                        fileIO.ReadByte();
                        if (release >= 114)
                        {
                            fileIO.ReadBoolean();
                        }
                    }
                    else
                    {
                        newPlayer.inventory[num6].netDefaults(num7);
                        newPlayer.inventory[num6].stack = fileIO.ReadInt32();
                        newPlayer.inventory[num6].Prefix(fileIO.ReadByte());
                        if (release >= 114)
                        {
                            newPlayer.inventory[num6].favorited = fileIO.ReadBoolean();
                        }
                    }
                }
            }
            else
            {
                for (int num8 = 0; num8 < 48; num8++)
                {
                    int num9 = fileIO.ReadInt32();
                    if (num9 >= ItemID.Count)
                    {
                        newPlayer.inventory[num8].netDefaults(0);
                        fileIO.ReadInt32();
                        fileIO.ReadByte();
                    }
                    else
                    {
                        newPlayer.inventory[num8].netDefaults(num9);
                        newPlayer.inventory[num8].stack = fileIO.ReadInt32();
                        newPlayer.inventory[num8].Prefix(fileIO.ReadByte());
                    }
                }
            }
            if (release >= 117)
            {
                if (release < 136)
                {
                    for (int num10 = 0; num10 < 5; num10++)
                    {
                        if (num10 != 1)
                        {
                            int num11 = fileIO.ReadInt32();
                            if (num11 >= ItemID.Count)
                            {
                                newPlayer.miscEquips[num10].netDefaults(0);
                                fileIO.ReadByte();
                            }
                            else
                            {
                                newPlayer.miscEquips[num10].netDefaults(num11);
                                newPlayer.miscEquips[num10].Prefix(fileIO.ReadByte());
                            }
                            num11 = fileIO.ReadInt32();
                            if (num11 >= ItemID.Count)
                            {
                                newPlayer.miscDyes[num10].netDefaults(0);
                                fileIO.ReadByte();
                            }
                            else
                            {
                                newPlayer.miscDyes[num10].netDefaults(num11);
                                newPlayer.miscDyes[num10].Prefix(fileIO.ReadByte());
                            }
                        }
                    }
                }
                else
                {
                    for (int num12 = 0; num12 < 5; num12++)
                    {
                        int num13 = fileIO.ReadInt32();
                        if (num13 >= ItemID.Count)
                        {
                            newPlayer.miscEquips[num12].netDefaults(0);
                            fileIO.ReadByte();
                        }
                        else
                        {
                            newPlayer.miscEquips[num12].netDefaults(num13);
                            newPlayer.miscEquips[num12].Prefix(fileIO.ReadByte());
                        }
                        num13 = fileIO.ReadInt32();
                        if (num13 >= ItemID.Count)
                        {
                            newPlayer.miscDyes[num12].netDefaults(0);
                            fileIO.ReadByte();
                        }
                        else
                        {
                            newPlayer.miscDyes[num12].netDefaults(num13);
                            newPlayer.miscDyes[num12].Prefix(fileIO.ReadByte());
                        }
                    }
                }
            }
            if (release >= 58)
            {
                for (int num14 = 0; num14 < 40; num14++)
                {
                    newPlayer.bank.item[num14].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank.item[num14].stack = fileIO.ReadInt32();
                    newPlayer.bank.item[num14].Prefix(fileIO.ReadByte());
                }
                for (int num15 = 0; num15 < 40; num15++)
                {
                    newPlayer.bank2.item[num15].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank2.item[num15].stack = fileIO.ReadInt32();
                    newPlayer.bank2.item[num15].Prefix(fileIO.ReadByte());
                }
            }
            else
            {
                for (int num16 = 0; num16 < 20; num16++)
                {
                    newPlayer.bank.item[num16].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank.item[num16].stack = fileIO.ReadInt32();
                    newPlayer.bank.item[num16].Prefix(fileIO.ReadByte());
                }
                for (int num17 = 0; num17 < 20; num17++)
                {
                    newPlayer.bank2.item[num17].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank2.item[num17].stack = fileIO.ReadInt32();
                    newPlayer.bank2.item[num17].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 182)
            {
                for (int num18 = 0; num18 < 40; num18++)
                {
                    newPlayer.bank3.item[num18].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank3.item[num18].stack = fileIO.ReadInt32();
                    newPlayer.bank3.item[num18].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 198)
            {
                for (int num19 = 0; num19 < 40; num19++)
                {
                    newPlayer.bank4.item[num19].netDefaults(fileIO.ReadInt32());
                    newPlayer.bank4.item[num19].stack = fileIO.ReadInt32();
                    newPlayer.bank4.item[num19].Prefix(fileIO.ReadByte());
                    if (release >= 255)
                    {
                        newPlayer.bank4.item[num19].favorited = fileIO.ReadBoolean();
                    }
                }
            }
            if (release >= 199)
            {
                newPlayer.voidVaultInfo = fileIO.ReadByte();
            }
        }
        else
        {
            for (int num20 = 0; num20 < 8; num20++)
            {
                newPlayer.armor[num20].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                if (release >= 36)
                {
                    newPlayer.armor[num20].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 6)
            {
                for (int num21 = 8; num21 < 11; num21++)
                {
                    newPlayer.armor[num21].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                    if (release >= 36)
                    {
                        newPlayer.armor[num21].Prefix(fileIO.ReadByte());
                    }
                }
            }
            for (int num22 = 0; num22 < 44; num22++)
            {
                newPlayer.inventory[num22].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                newPlayer.inventory[num22].stack = fileIO.ReadInt32();
                if (release >= 36)
                {
                    newPlayer.inventory[num22].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 15)
            {
                for (int num23 = 44; num23 < 48; num23++)
                {
                    newPlayer.inventory[num23].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                    newPlayer.inventory[num23].stack = fileIO.ReadInt32();
                    if (release >= 36)
                    {
                        newPlayer.inventory[num23].Prefix(fileIO.ReadByte());
                    }
                }
            }
            for (int num24 = 0; num24 < 20; num24++)
            {
                newPlayer.bank.item[num24].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                newPlayer.bank.item[num24].stack = fileIO.ReadInt32();
                if (release >= 36)
                {
                    newPlayer.bank.item[num24].Prefix(fileIO.ReadByte());
                }
            }
            if (release >= 20)
            {
                for (int num25 = 0; num25 < 20; num25++)
                {
                    newPlayer.bank2.item[num25].SetDefaults(ItemID.FromLegacyName(fileIO.ReadString(), release));
                    newPlayer.bank2.item[num25].stack = fileIO.ReadInt32();
                    if (release >= 36)
                    {
                        newPlayer.bank2.item[num25].Prefix(fileIO.ReadByte());
                    }
                }
            }
        }
        if (release < 58)
        {
            for (int num26 = 40; num26 < 48; num26++)
            {
                newPlayer.inventory[num26 + 10] = newPlayer.inventory[num26].Clone();
                newPlayer.inventory[num26].SetDefaults();
            }
        }
        if (release >= 11)
        {
            int num27 = 22;
            if (release < 74)
            {
                num27 = 10;
            }
            if (release >= 252)
            {
                num27 = 44;
            }
            for (int num28 = 0; num28 < num27; num28++)
            {
                newPlayer.buffType[num28] = fileIO.ReadInt32();
                newPlayer.buffTime[num28] = fileIO.ReadInt32();
                if (newPlayer.buffType[num28] == 0)
                {
                    num28--;
                    num27--;
                }
            }
        }
        for (int num29 = 0; num29 < 200; num29++)
        {
            int num30 = fileIO.ReadInt32();
            if (num30 == -1)
            {
                break;
            }
            newPlayer.spX[num29] = num30;
            newPlayer.spY[num29] = fileIO.ReadInt32();
            newPlayer.spI[num29] = fileIO.ReadInt32();
            newPlayer.spN[num29] = fileIO.ReadString();
        }
        if (release >= 16)
        {
            newPlayer.hbLocked = fileIO.ReadBoolean();
        }
        if (release >= 115)
        {
            int num31 = 13;
            for (int num32 = 0; num32 < num31; num32++)
            {
                newPlayer.hideInfo[num32] = fileIO.ReadBoolean();
            }
        }
        if (release >= 98)
        {
            newPlayer.anglerQuestsFinished = fileIO.ReadInt32();
        }
        if (release >= 162)
        {
            for (int num33 = 0; num33 < 4; num33++)
            {
                newPlayer.DpadRadial.Bindings[num33] = fileIO.ReadInt32();
            }
        }
        if (release >= 164)
        {
            int num34 = 8;
            if (release >= 167)
            {
                num34 = 10;
            }
            if (release >= 197)
            {
                num34 = 11;
            }
            if (release >= 230)
            {
                num34 = 12;
            }
            for (int num35 = 0; num35 < num34; num35++)
            {
                newPlayer.builderAccStatus[num35] = fileIO.ReadInt32();
            }
            if (release < 210)
            {
                newPlayer.builderAccStatus[0] = 1;
            }
            if (release < 249)
            {
                bool flag = false;
                for (int num36 = 0; num36 < 58; num36++)
                {
                    if (newPlayer.inventory[num36].type == 3611)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    newPlayer.builderAccStatus[1] = 1;
                }
            }
        }
        if (release >= 181)
        {
            newPlayer.bartenderQuestLog = fileIO.ReadInt32();
        }
        if (release >= 200)
        {
            newPlayer.dead = fileIO.ReadBoolean();
            if (newPlayer.dead)
            {
                newPlayer.respawnTimer = Terraria.Utils.Clamp(fileIO.ReadInt32(), 0, 60000);
            }
        }
        newPlayer.lastTimePlayerWasSaved = 0L;
        if (release >= 202)
        {
            newPlayer.lastTimePlayerWasSaved = fileIO.ReadInt64();
        }
        else
        {
            newPlayer.lastTimePlayerWasSaved = DateTime.UtcNow.ToBinary();
        }
        if (release >= 206)
        {
            newPlayer.golferScoreAccumulated = fileIO.ReadInt32();
        }
        if (release >= 218)
        {
            newPlayer.creativeTracker.Load(fileIO, release);
        }
        if (release >= 214)
        {
            newPlayer.LoadTemporaryItemSlotContents(fileIO);
        }
        newPlayer.savedPerPlayerFieldsThatArentInThePlayerClass = new Player.SavedPlayerDataWithAnnoyingRules();
        CreativePowerManager.Instance.ResetDataForNewPlayer(newPlayer);
        if (release >= 220)
        {
            CreativePowerManager.Instance.LoadToPlayer(newPlayer, fileIO, release);
        }
        if (release >= 253)
        {
            BitsByte bitsByte3 = fileIO.ReadByte();
            newPlayer.unlockedSuperCart = bitsByte3[0];
            newPlayer.enabledSuperCart = bitsByte3[1];
        }
        else
        {
            newPlayer.unlockedSuperCart = newPlayer.HasItemInAnyInventory(3353);
        }
        if (release >= 262)
        {
            int value = fileIO.ReadInt32();
            newPlayer.CurrentLoadoutIndex = Terraria.Utils.Clamp(value, 0, newPlayer.Loadouts.Length - 1);
            for (int num37 = 0; num37 < newPlayer.Loadouts.Length; num37++)
            {
                newPlayer.Loadouts[num37].Deserialize(fileIO, release);
            }
        }
        Player.LoadPlayer_LastMinuteFixes(newPlayer);
    }
    #region Extension
    public static void RestoreCharacter(this PlayerData playerData, Player player)
    {
        player.statLife = playerData.health;
        player.statLifeMax = playerData.maxHealth;
        player.statMana = playerData.maxMana;
        player.statManaMax = playerData.maxMana;
        player.SpawnX = playerData.spawnX;
        player.SpawnY = playerData.spawnY;
        player.hairDye = playerData.hairDye;
        player.anglerQuestsFinished = playerData.questsCompleted;
        player.UsingBiomeTorches = playerData.usingBiomeTorches == 1;
        player.happyFunTorchTime = playerData.happyFunTorchTime == 1;
        player.unlockedBiomeTorches = playerData.unlockedBiomeTorches == 1;
        player.CurrentLoadoutIndex = playerData.currentLoadoutIndex;
        player.ateArtisanBread = playerData.ateArtisanBread == 1;
        player.usedAegisCrystal = playerData.usedAegisCrystal == 1;
        player.usedAegisFruit = playerData.usedAegisFruit == 1;
        player.usedArcaneCrystal = playerData.usedArcaneCrystal == 1;
        player.usedGalaxyPearl = playerData.usedGalaxyPearl == 1;
        player.usedGummyWorm = playerData.usedGummyWorm == 1;
        player.usedAmbrosia = playerData.usedAmbrosia == 1;
        player.unlockedSuperCart = playerData.unlockedSuperCart == 1;
        player.enabledSuperCart = playerData.enabledSuperCart == 1;
        if (playerData.extraSlot.HasValue)
        {
            player.extraAccessory = playerData.extraSlot.Value == 1;
        }
        if (playerData.skinVariant.HasValue)
        {
            player.skinVariant = playerData.skinVariant.Value;
        }
        if (playerData.hair.HasValue)
        {
            player.hair = playerData.hair.Value;
        }
        if (playerData.hairColor.HasValue)
        {
            player.hairColor = playerData.hairColor.Value;
        }
        if (playerData.pantsColor.HasValue)
        {
            player.pantsColor = playerData.pantsColor.Value;
        }
        if (playerData.shirtColor.HasValue)
        {
            player.shirtColor = playerData.shirtColor.Value;
        }
        if (playerData.underShirtColor.HasValue)
        {
            player.underShirtColor = playerData.underShirtColor.Value;
        }
        if (playerData.shoeColor.HasValue)
        {
            player.shoeColor = playerData.shoeColor.Value;
        }
        if (playerData.skinColor.HasValue)
        {
            player.skinColor = playerData.skinColor.Value;
        }
        if (playerData.eyeColor.HasValue)
        {
            player.eyeColor = playerData.eyeColor.Value;
        }
        if (playerData.hideVisuals != null)
        {
            player.hideVisibleAccessory = playerData.hideVisuals;
        }
        else
        {
            player.hideVisibleAccessory = new bool[player.hideVisibleAccessory.Length];
        }
        for (int i = 0; i < NetItem.MaxInventory; i++)
        {
            if (i < NetItem.InventoryIndex.Item2)
            {
                player.inventory[i].netDefaults(playerData.inventory[i].NetId);
                if (player.inventory[i].netID != 0)
                {
                    player.inventory[i].stack = playerData.inventory[i].Stack;
                    player.inventory[i].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.ArmorIndex.Item2)
            {
                int num = i - NetItem.ArmorIndex.Item1;
                player.armor[num].netDefaults(playerData.inventory[i].NetId);
                if (player.armor[num].netID != 0)
                {
                    player.armor[num].stack = playerData.inventory[i].Stack;
                    player.armor[num].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.DyeIndex.Item2)
            {
                int num2 = i - NetItem.DyeIndex.Item1;
                player.dye[num2].netDefaults(playerData.inventory[i].NetId);
                if (player.dye[num2].netID != 0)
                {
                    player.dye[num2].stack = playerData.inventory[i].Stack;
                    player.dye[num2].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.MiscEquipIndex.Item2)
            {
                int num3 = i - NetItem.MiscEquipIndex.Item1;
                player.miscEquips[num3].netDefaults(playerData.inventory[i].NetId);
                if (player.miscEquips[num3].netID != 0)
                {
                    player.miscEquips[num3].stack = playerData.inventory[i].Stack;
                    player.miscEquips[num3].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.MiscDyeIndex.Item2)
            {
                int num4 = i - NetItem.MiscDyeIndex.Item1;
                player.miscDyes[num4].netDefaults(playerData.inventory[i].NetId);
                if (player.miscDyes[num4].netID != 0)
                {
                    player.miscDyes[num4].stack = playerData.inventory[i].Stack;
                    player.miscDyes[num4].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.PiggyIndex.Item2)
            {
                int num5 = i - NetItem.PiggyIndex.Item1;
                player.bank.item[num5].netDefaults(playerData.inventory[i].NetId);
                if (player.bank.item[num5].netID != 0)
                {
                    player.bank.item[num5].stack = playerData.inventory[i].Stack;
                    player.bank.item[num5].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.SafeIndex.Item2)
            {
                int num6 = i - NetItem.SafeIndex.Item1;
                player.bank2.item[num6].netDefaults(playerData.inventory[i].NetId);
                if (player.bank2.item[num6].netID != 0)
                {
                    player.bank2.item[num6].stack = playerData.inventory[i].Stack;
                    player.bank2.item[num6].prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.TrashIndex.Item2)
            {
                _ = NetItem.TrashIndex.Item1;
                player.trashItem.netDefaults(playerData.inventory[i].NetId);
                if (player.trashItem.netID != 0)
                {
                    player.trashItem.stack = playerData.inventory[i].Stack;
                    player.trashItem.prefix = playerData.inventory[i].PrefixId;
                }
            }
            else if (i < NetItem.ForgeIndex.Item2)
            {
                int num7 = i - NetItem.ForgeIndex.Item1;
                player.bank3.item[num7].netDefaults(playerData.inventory[i].NetId);
                if (player.bank3.item[num7].netID != 0)
                {
                    player.bank3.item[num7].stack = playerData.inventory[i].Stack;
                    player.bank3.item[num7].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.VoidIndex.Item2)
            {
                int num8 = i - NetItem.VoidIndex.Item1;
                player.bank4.item[num8].netDefaults(playerData.inventory[i].NetId);
                if (player.bank4.item[num8].netID != 0)
                {
                    player.bank4.item[num8].stack = playerData.inventory[i].Stack;
                    player.bank4.item[num8].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout1Armor.Item2)
            {
                int num9 = i - NetItem.Loadout1Armor.Item1;
                player.Loadouts[0].Armor[num9].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[0].Armor[num9].netID != 0)
                {
                    player.Loadouts[0].Armor[num9].stack = playerData.inventory[i].Stack;
                    player.Loadouts[0].Armor[num9].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout1Dye.Item2)
            {
                int num10 = i - NetItem.Loadout1Dye.Item1;
                player.Loadouts[0].Dye[num10].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[0].Dye[num10].netID != 0)
                {
                    player.Loadouts[0].Dye[num10].stack = playerData.inventory[i].Stack;
                    player.Loadouts[0].Dye[num10].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout2Armor.Item2)
            {
                int num11 = i - NetItem.Loadout2Armor.Item1;
                player.Loadouts[1].Armor[num11].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[1].Armor[num11].netID != 0)
                {
                    player.Loadouts[1].Armor[num11].stack = playerData.inventory[i].Stack;
                    player.Loadouts[1].Armor[num11].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout2Dye.Item2)
            {
                int num12 = i - NetItem.Loadout2Dye.Item1;
                player.Loadouts[1].Dye[num12].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[1].Dye[num12].netID != 0)
                {
                    player.Loadouts[1].Dye[num12].stack = playerData.inventory[i].Stack;
                    player.Loadouts[1].Dye[num12].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout3Armor.Item2)
            {
                int num13 = i - NetItem.Loadout3Armor.Item1;
                player.Loadouts[2].Armor[num13].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[2].Armor[num13].netID != 0)
                {
                    player.Loadouts[2].Armor[num13].stack = playerData.inventory[i].Stack;
                    player.Loadouts[2].Armor[num13].Prefix(playerData.inventory[i].PrefixId);
                }
            }
            else if (i < NetItem.Loadout3Dye.Item2)
            {
                int num14 = i - NetItem.Loadout3Dye.Item1;
                player.Loadouts[2].Dye[num14].netDefaults(playerData.inventory[i].NetId);
                if (player.Loadouts[2].Dye[num14].netID != 0)
                {
                    player.Loadouts[2].Dye[num14].stack = playerData.inventory[i].Stack;
                    player.Loadouts[2].Dye[num14].Prefix(playerData.inventory[i].PrefixId);
                }
            }
        }
        NetSender.SendData(147, player);
        int slot = 0;
        for (int j = 0; j < NetItem.InventorySlots; j++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int k = 0; k < NetItem.ArmorSlots; k++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int l = 0; l < NetItem.DyeSlots; l++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int m = 0; m < NetItem.MiscEquipSlots; m++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int n = 0; n < NetItem.MiscDyeSlots; n++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num16 = 0; num16 < NetItem.PiggySlots; num16++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num17 = 0; num17 < NetItem.SafeSlots; num17++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        NetSender.SendData(5, player, slot);
        for (int num18 = 0; num18 < NetItem.ForgeSlots; num18++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num19 = 0; num19 < NetItem.VoidSlots; num19++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num20 = 0; num20 < NetItem.LoadoutArmorSlots; num20++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num21 = 0; num21 < NetItem.LoadoutDyeSlots; num21++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num22 = 0; num22 < NetItem.LoadoutArmorSlots; num22++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num23 = 0; num23 < NetItem.LoadoutDyeSlots; num23++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num24 = 0; num24 < NetItem.LoadoutArmorSlots; num24++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num25 = 0; num25 < NetItem.LoadoutDyeSlots; num25++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        NetSender.SendData(4, player);
        NetSender.SendData(42, player);
        NetSender.SendData(16, player);
        slot = 0;
        for (int num26 = 0; num26 < NetItem.InventorySlots; num26++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num27 = 0; num27 < NetItem.ArmorSlots; num27++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num28 = 0; num28 < NetItem.DyeSlots; num28++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num29 = 0; num29 < NetItem.MiscEquipSlots; num29++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num30 = 0; num30 < NetItem.MiscDyeSlots; num30++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num31 = 0; num31 < NetItem.PiggySlots; num31++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num32 = 0; num32 < NetItem.SafeSlots; num32++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        NetSender.SendData(5, player, slot);
        for (int num33 = 0; num33 < NetItem.ForgeSlots; num33++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num34 = 0; num34 < NetItem.VoidSlots; num34++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num35 = 0; num35 < NetItem.LoadoutArmorSlots; num35++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num36 = 0; num36 < NetItem.LoadoutDyeSlots; num36++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num37 = 0; num37 < NetItem.LoadoutArmorSlots; num37++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num38 = 0; num38 < NetItem.LoadoutDyeSlots; num38++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num39 = 0; num39 < NetItem.LoadoutArmorSlots; num39++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        for (int num40 = 0; num40 < NetItem.LoadoutDyeSlots; num40++)
        {
            NetSender.SendData(5, player, slot);
            slot += 1;
        }
        NetSender.SendData(4, player);
        NetSender.SendData(42, player);
        NetSender.SendData(16, player);
        for (int i = 0; i < Player.maxBuffs; i++)
        {
            player.buffType[i] = 0;
        }
        NetSender.SendData(50, player);
        NetSender.SendData(76, player);
        //NetSender.SendData(39, player, 400);
        //if (!Main.GameModeInfo.IsJourneyMode)
        //{
        //    return;
        //}
        //Dictionary<int, int> sacrificedItems = TShock.ResearchDatastore.GetSacrificedItems();
        //for (int num42 = 0; num42 < ItemID.Count; num42++)
        //{
        //    int sacrificeCount = 0;
        //    if (sacrificedItems.ContainsKey(num42))
        //    {
        //        sacrificeCount = sacrificedItems[num42];
        //    }
        //    NetPacket packet = NetCreativeUnlocksModule.SerializeItemSacrifice(num42, sacrificeCount);
        //    NetManager.Instance.SendToClient(packet, player.whoAmI);
        //}
    }

    #endregion
}
