using System.Reflection.Emit;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Net;
using OTAPI;

using VBY.GameContentModify.Config;

namespace VBY.GameContentModify;

[ReplaceType(typeof(MessageBuffer))]
public static class ReplaceMessageBuffer
{
    [DetourMethod]
    public static void GetData(On.Terraria.MessageBuffer.orig_GetData orig, MessageBuffer self, int start, int length, out int messageType)
    {
        if (self.whoAmI < 256)
        {
            Netplay.Clients[self.whoAmI].TimeOutTimer = 0;
        }
        else
        {
            Netplay.Connection.TimeOutTimer = 0;
        }
        int num = start + 1;
        byte b = (byte)(messageType = self.readBuffer[start]);
        if (!Hooks.MessageBuffer.InvokeGetData(self, ref b, ref num, ref start, ref length, ref messageType, MessageID.Count))
        {
            return;
        }
        if (b != 38 && Netplay.Clients[self.whoAmI].State == -1)
        {
            NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, Lang.mp[1].ToNetworkText());
            return;
        }
        if (Netplay.Clients[self.whoAmI].State < 10 && b > 12 && b != 93 && b != 16 && b != 42 && b != 50 && b != 38 && b != 68 && b != 147)
        {
            NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
        }
        if (Netplay.Clients[self.whoAmI].State == 0 && b != 1)
        {
            NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
        }
        if (self.reader == null)
        {
            self.ResetReader();
        }
        self.reader!.BaseStream.Position = num;
        switch (b)
        {
            case 1:
                if (Main.dedServ && Netplay.IsBanned(Netplay.Clients[self.whoAmI].Socket.GetRemoteAddress()))
                {
                    NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, Lang.mp[3].ToNetworkText());
                }
                else
                {
                    if (Netplay.Clients[self.whoAmI].State != 0)
                    {
                        break;
                    }
                    if (self.reader.ReadString() == "Terraria" + 279)
                    {
                        if (string.IsNullOrEmpty(Netplay.ServerPassword))
                        {
                            Netplay.Clients[self.whoAmI].State = 1;
                            NetMessage.TrySendData(MessageID.PlayerInfo, self.whoAmI);
                        }
                        else
                        {
                            Netplay.Clients[self.whoAmI].State = -1;
                            NetMessage.TrySendData(MessageID.RequestPassword, self.whoAmI);
                        }
                    }
                    else
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, Lang.mp[4].ToNetworkText());
                    }
                }
                break;
            case 4:
                {
                    self.reader.ReadByte();
                    int num203 = self.whoAmI;
                    if (num203 == Main.myPlayer && !Main.ServerSideCharacter)
                    {
                        break;
                    }
                    Player player13 = Main.player[num203];
                    player13.whoAmI = num203;
                    player13.skinVariant = self.reader.ReadByte();
                    player13.skinVariant = (int)MathHelper.Clamp(player13.skinVariant, 0f, PlayerVariantID.Count - 1);
                    player13.hair = self.reader.ReadByte();
                    if (player13.hair >= 165)
                    {
                        player13.hair = 0;
                    }
                    player13.name = self.reader.ReadString().Trim().Trim();
                    player13.hairDye = self.reader.ReadByte();
                    MessageBuffer.ReadAccessoryVisibility(self.reader, player13.hideVisibleAccessory);
                    player13.hideMisc = self.reader.ReadByte();
                    player13.hairColor = self.reader.ReadRGB();
                    player13.skinColor = self.reader.ReadRGB();
                    player13.eyeColor = self.reader.ReadRGB();
                    player13.shirtColor = self.reader.ReadRGB();
                    player13.underShirtColor = self.reader.ReadRGB();
                    player13.pantsColor = self.reader.ReadRGB();
                    player13.shoeColor = self.reader.ReadRGB();
                    BitsByte bitsByte12 = self.reader.ReadByte();
                    player13.difficulty = PlayerDifficultyID.SoftCore;
                    if (bitsByte12[0])
                    {
                        player13.difficulty = PlayerDifficultyID.MediumCore;
                    }
                    if (bitsByte12[1])
                    {
                        player13.difficulty = PlayerDifficultyID.Hardcore;
                    }
                    if (bitsByte12[3])
                    {
                        player13.difficulty = PlayerDifficultyID.Creative;
                    }
                    if (player13.difficulty > 3)
                    {
                        player13.difficulty = PlayerDifficultyID.Creative;
                    }
                    player13.extraAccessory = bitsByte12[2];
                    BitsByte bitsByte13 = self.reader.ReadByte();
                    player13.UsingBiomeTorches = bitsByte13[0];
                    player13.happyFunTorchTime = bitsByte13[1];
                    player13.unlockedBiomeTorches = bitsByte13[2];
                    player13.unlockedSuperCart = bitsByte13[3];
                    player13.enabledSuperCart = bitsByte13[4];
                    BitsByte bitsByte14 = self.reader.ReadByte();
                    player13.usedAegisCrystal = bitsByte14[0];
                    player13.usedAegisFruit = bitsByte14[1];
                    player13.usedArcaneCrystal = bitsByte14[2];
                    player13.usedGalaxyPearl = bitsByte14[3];
                    player13.usedGummyWorm = bitsByte14[4];
                    player13.usedAmbrosia = bitsByte14[5];
                    player13.ateArtisanBread = bitsByte14[6];
                    bool flag12 = false;
                    if (Netplay.Clients[self.whoAmI].State < 10)
                    {
                        for (int num204 = 0; num204 < 255; num204++)
                        {
                            if (num204 != num203 && player13.name == Main.player[num204].name && Netplay.Clients[num204].IsActive)
                            {
                                flag12 = true;
                            }
                        }
                    }
                    if (flag12 && Hooks.MessageBuffer.InvokeNameCollision(player13))
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, NetworkText.FromKey(Lang.mp[5].Key, player13.name));
                    }
                    else if (player13.name.Length > Player.nameLen)
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, NetworkText.FromKey("Net.NameTooLong"));
                    }
                    else if (player13.name == "")
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, NetworkText.FromKey("Net.EmptyName"));
                    }
                    else if (player13.difficulty == PlayerDifficultyID.Creative && !Main.GameModeInfo.IsJourneyMode)
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsCreativeAndWorldIsNotCreative"));
                    }
                    else if (player13.difficulty != PlayerDifficultyID.Creative && Main.GameModeInfo.IsJourneyMode)
                    {
                        NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, NetworkText.FromKey("Net.PlayerIsNotCreativeAndWorldIsCreative"));
                    }
                    else
                    {
                        Netplay.Clients[self.whoAmI].Name = player13.name;
                        NetMessage.TrySendData(MessageID.SyncPlayer, -1, self.whoAmI, null, num203);
                    }
                    break;
                }
            case 5:
                {
                    self.reader.ReadByte();
                    int num49 = self.whoAmI;
                    if (num49 == Main.myPlayer && !Main.ServerSideCharacter && !Main.player[num49].HasLockedInventory())
                    {
                        break;
                    }
                    Player player15 = Main.player[num49];
                    lock (player15)
                    {
                        int num50 = self.reader.ReadInt16();
                        int stack6 = self.reader.ReadInt16();
                        int num51 = self.reader.ReadByte();
                        int type14 = self.reader.ReadInt16();
                        Item[] array2 = null!;
                        Item[] array3 = null!;
                        int num52 = 0;
                        bool flag15 = false;
                        Player clientPlayer = Main.clientPlayer;
                        if (num50 >= PlayerItemSlotID.Loadout3_Dye_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout3_Dye_0;
                            array2 = player15.Loadouts[2].Dye;
                            array3 = clientPlayer.Loadouts[2].Dye;
                        }
                        else if (num50 >= PlayerItemSlotID.Loadout3_Armor_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout3_Armor_0;
                            array2 = player15.Loadouts[2].Armor;
                            array3 = clientPlayer.Loadouts[2].Armor;
                        }
                        else if (num50 >= PlayerItemSlotID.Loadout2_Dye_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout2_Dye_0;
                            array2 = player15.Loadouts[1].Dye;
                            array3 = clientPlayer.Loadouts[1].Dye;
                        }
                        else if (num50 >= PlayerItemSlotID.Loadout2_Armor_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout2_Armor_0;
                            array2 = player15.Loadouts[1].Armor;
                            array3 = clientPlayer.Loadouts[1].Armor;
                        }
                        else if (num50 >= PlayerItemSlotID.Loadout1_Dye_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout1_Dye_0;
                            array2 = player15.Loadouts[0].Dye;
                            array3 = clientPlayer.Loadouts[0].Dye;
                        }
                        else if (num50 >= PlayerItemSlotID.Loadout1_Armor_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Loadout1_Armor_0;
                            array2 = player15.Loadouts[0].Armor;
                            array3 = clientPlayer.Loadouts[0].Armor;
                        }
                        else if (num50 >= PlayerItemSlotID.Bank4_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Bank4_0;
                            array2 = player15.bank4.item;
                            array3 = clientPlayer.bank4.item;
                        }
                        else if (num50 >= PlayerItemSlotID.Bank3_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Bank3_0;
                            array2 = player15.bank3.item;
                            array3 = clientPlayer.bank3.item;
                        }
                        else if (num50 >= PlayerItemSlotID.TrashItem)
                        {
                            flag15 = true;
                        }
                        else if (num50 >= PlayerItemSlotID.Bank2_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Bank2_0;
                            array2 = player15.bank2.item;
                            array3 = clientPlayer.bank2.item;
                        }
                        else if (num50 >= PlayerItemSlotID.Bank1_0)
                        {
                            num52 = num50 - PlayerItemSlotID.Bank1_0;
                            array2 = player15.bank.item;
                            array3 = clientPlayer.bank.item;
                        }
                        else if (num50 >= PlayerItemSlotID.MiscDye0)
                        {
                            num52 = num50 - PlayerItemSlotID.MiscDye0;
                            array2 = player15.miscDyes;
                            array3 = clientPlayer.miscDyes;
                        }
                        else if (num50 >= PlayerItemSlotID.Misc0)
                        {
                            num52 = num50 - PlayerItemSlotID.Misc0;
                            array2 = player15.miscEquips;
                            array3 = clientPlayer.miscEquips;
                        }
                        else if (num50 >= PlayerItemSlotID.Dye0)
                        {
                            num52 = num50 - PlayerItemSlotID.Dye0;
                            array2 = player15.dye;
                            array3 = clientPlayer.dye;
                        }
                        else if (num50 >= PlayerItemSlotID.Armor0)
                        {
                            num52 = num50 - PlayerItemSlotID.Armor0;
                            array2 = player15.armor;
                            array3 = clientPlayer.armor;
                        }
                        else
                        {
                            num52 = num50 - PlayerItemSlotID.Inventory0;
                            array2 = player15.inventory;
                            array3 = clientPlayer.inventory;
                        }
                        if (flag15)
                        {
                            player15.trashItem = new Item();
                            player15.trashItem.netDefaults(type14);
                            player15.trashItem.stack = stack6;
                            player15.trashItem.Prefix(num51);
                            if (num49 == Main.myPlayer && !Main.ServerSideCharacter)
                            {
                                clientPlayer.trashItem = player15.trashItem.Clone();
                            }
                        }
                        else if (num50 <= 58)
                        {
                            array2[num52] = new Item();
                            array2[num52].netDefaults(type14);
                            array2[num52].stack = stack6;
                            array2[num52].Prefix(num51);
                            if (num49 == Main.myPlayer && !Main.ServerSideCharacter)
                            {
                                array3[num52] = array2[num52].Clone();
                            }
                            if (num49 == Main.myPlayer && num52 == 58)
                            {
                                Main.mouseItem = array2[num52].Clone();
                            }
                        }
                        else
                        {
                            array2[num52] = new Item();
                            array2[num52].netDefaults(type14);
                            array2[num52].stack = stack6;
                            array2[num52].Prefix(num51);
                            if (num49 == Main.myPlayer && !Main.ServerSideCharacter)
                            {
                                array3[num52] = array2[num52].Clone();
                            }
                        }
                        bool[] canRelay = PlayerItemSlotID.CanRelay;
                        if (num49 == self.whoAmI && canRelay.IndexInRange(num50) && canRelay[num50])
                        {
                            NetMessage.TrySendData(MessageID.SyncEquipment, -1, self.whoAmI, null, num49, num50, num51);
                        }
                        break;
                    }
                }
            case 6:
                if (Netplay.Clients[self.whoAmI].State == 1)
                {
                    Netplay.Clients[self.whoAmI].State = 2;
                }
                NetMessage.TrySendData(MessageID.WorldData, self.whoAmI);
                Main.SyncAnInvasion(self.whoAmI);
                break;
            case 8:
                {
                    NetMessage.TrySendData(MessageID.WorldData, self.whoAmI);
                    int num87 = self.reader.ReadInt32();
                    int num88 = self.reader.ReadInt32();
                    bool flag17 = true;
                    if (num87 == -1 || num88 == -1)
                    {
                        flag17 = false;
                    }
                    else if (num87 < 10 || num87 > Main.maxTilesX - 10)
                    {
                        flag17 = false;
                    }
                    else if (num88 < 10 || num88 > Main.maxTilesY - 10)
                    {
                        flag17 = false;
                    }
                    int num89 = Netplay.GetSectionX(Main.spawnTileX) - 2;
                    int num90 = Netplay.GetSectionY(Main.spawnTileY) - 1;
                    int num91 = num89 + 5;
                    int num92 = num90 + 3;
                    if (num89 < 0)
                    {
                        num89 = 0;
                    }
                    if (num91 >= Main.maxSectionsX)
                    {
                        num91 = Main.maxSectionsX;
                    }
                    if (num90 < 0)
                    {
                        num90 = 0;
                    }
                    if (num92 >= Main.maxSectionsY)
                    {
                        num92 = Main.maxSectionsY;
                    }
                    int num93 = (num91 - num89) * (num92 - num90);
                    List<Point> list = new();
                    for (int num94 = num89; num94 < num91; num94++)
                    {
                        for (int num96 = num90; num96 < num92; num96++)
                        {
                            list.Add(new Point(num94, num96));
                        }
                    }
                    int num97 = -1;
                    int num99 = -1;
                    if (flag17)
                    {
                        num87 = Netplay.GetSectionX(num87) - 2;
                        num88 = Netplay.GetSectionY(num88) - 1;
                        num97 = num87 + 5;
                        num99 = num88 + 3;
                        if (num87 < 0)
                        {
                            num87 = 0;
                        }
                        if (num97 >= Main.maxSectionsX)
                        {
                            num97 = Main.maxSectionsX - 1;
                        }
                        if (num88 < 0)
                        {
                            num88 = 0;
                        }
                        if (num99 >= Main.maxSectionsY)
                        {
                            num99 = Main.maxSectionsY - 1;
                        }
                        for (int num100 = num87; num100 <= num97; num100++)
                        {
                            for (int num101 = num88; num101 <= num99; num101++)
                            {
                                if (num100 < num89 || num100 >= num91 || num101 < num90 || num101 >= num92)
                                {
                                    list.Add(new Point(num100, num101));
                                    num93++;
                                }
                            }
                        }
                    }
                    PortalHelper.SyncPortalsOnPlayerJoin(self.whoAmI, 1, list, out var portalSections);
                    num93 += portalSections.Count;
                    if (Netplay.Clients[self.whoAmI].State == 2)
                    {
                        Netplay.Clients[self.whoAmI].State = 3;
                    }
                    NetMessage.TrySendData(MessageID.StatusTextSize, self.whoAmI, -1, Lang.inter[44].ToNetworkText(), num93);
                    Netplay.Clients[self.whoAmI].StatusText2 = Language.GetTextValue("Net.IsReceivingTileData");
                    Netplay.Clients[self.whoAmI].StatusMax += num93;
                    for (int num102 = num89; num102 < num91; num102++)
                    {
                        for (int num103 = num90; num103 < num92; num103++)
                        {
                            NetMessage.SendSection(self.whoAmI, num102, num103);
                        }
                    }
                    if (flag17)
                    {
                        for (int num104 = num87; num104 <= num97; num104++)
                        {
                            for (int num105 = num88; num105 <= num99; num105++)
                            {
                                NetMessage.SendSection(self.whoAmI, num104, num105);
                            }
                        }
                    }
                    for (int num108 = 0; num108 < portalSections.Count; num108++)
                    {
                        NetMessage.SendSection(self.whoAmI, portalSections[num108].X, portalSections[num108].Y);
                    }
                    int index;
                    if (NetMessageInfo.StaticSyncAllItem)
                    {
                        for (index = 0; index < 400;)
                        {
                            NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, index);
                            NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, index);
                            NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, index);
                            NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, index);
                            NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, index);
                            NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, index++);
                        }
                    }
                    else
                    {
                        for (int num109 = 0; num109 < 400; num109++)
                        {
                            if (Main.item[num109].active)
                            {
                                NetMessage.TrySendData(MessageID.SyncItem, self.whoAmI, -1, null, num109);
                                NetMessage.TrySendData(MessageID.ItemOwner, self.whoAmI, -1, null, num109);
                            }
                        }
                    }
                    if (NetMessageInfo.StaticSyncAllNPC)
                    {
                        for (index = 0; index < 200;)
                        {
                            NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, index++);
                        }
                    }
                    else
                    {
                        for (int num110 = 0; num110 < 200; num110++)
                        {
                            if (Main.npc[num110].active)
                            {
                                NetMessage.TrySendData(MessageID.SyncNPC, self.whoAmI, -1, null, num110);
                            }
                        }
                    }
                    if (NetMessageInfo.StaticSyncAllProjectile)
                    {
                        for (index = 0; index < 1000;)
                        {
                            NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, index++);
                            NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, index++);
                        }
                    }
                    else
                    {
                        for (int num112 = 0; num112 < 1000; num112++)
                        {
                            if (Main.projectile[num112].active && (Main.projPet[Main.projectile[num112].type] || Main.projectile[num112].netImportant))
                            {
                                NetMessage.TrySendData(MessageID.SyncProjectile, self.whoAmI, -1, null, num112);
                            }
                        }
                    }
                    for (int num113 = 0; num113 < 290; num113++)
                    {
                        NetMessage.TrySendData(MessageID.NPCKillCountDeathTally, self.whoAmI, -1, null, num113);
                    }
                    NetMessage.TrySendData(MessageID.Unknown57, self.whoAmI);
                    NetMessage.TrySendData(MessageID.MoonlordHorror);
                    NetMessage.TrySendData(MessageID.UpdateTowerShieldStrengths, self.whoAmI);
                    NetMessage.TrySendData(MessageID.SyncCavernMonsterType, self.whoAmI);
                    NetMessage.TrySendData(MessageID.InitialSpawn, self.whoAmI);
                    Main.BestiaryTracker.OnPlayerJoining(self.whoAmI);
                    CreativePowerManager.Instance.SyncThingsToJoiningPlayer(self.whoAmI);
                    Main.PylonSystem.OnPlayerJoining(self.whoAmI);
                    break;
                }
            case 12:
                {
                    self.reader.ReadByte();
                    int num114 = self.whoAmI;
                    Player player17 = Main.player[num114];
                    player17.SpawnX = self.reader.ReadInt16();
                    player17.SpawnY = self.reader.ReadInt16();
                    player17.respawnTimer = self.reader.ReadInt32();
                    player17.numberOfDeathsPVE = self.reader.ReadInt16();
                    player17.numberOfDeathsPVP = self.reader.ReadInt16();
                    if (player17.respawnTimer > 0)
                    {
                        player17.dead = true;
                    }
                    PlayerSpawnContext playerSpawnContext = (PlayerSpawnContext)self.reader.ReadByte();
                    player17.Spawn(playerSpawnContext);
                    if (Netplay.Clients[self.whoAmI].State < 3)
                    {
                        break;
                    }
                    if (Netplay.Clients[self.whoAmI].State == 3)
                    {
                        Netplay.Clients[self.whoAmI].State = 10;
                        NetMessage.buffer[self.whoAmI].broadcast = true;
                        NetMessage.SyncConnectedPlayer(self.whoAmI);
                        bool flag18 = NetMessage.DoesPlayerSlotCountAsAHost(self.whoAmI);
                        Main.countsAsHostForGameplay[self.whoAmI] = flag18;
                        if (NetMessage.DoesPlayerSlotCountAsAHost(self.whoAmI))
                        {
                            NetMessage.TrySendData(MessageID.SetCountsAsHostForGameplay, self.whoAmI, -1, null, self.whoAmI, flag18.ToInt());
                        }
                        NetMessage.TrySendData(MessageID.PlayerSpawn, -1, self.whoAmI, null, self.whoAmI, (int)(byte)playerSpawnContext);
                        NetMessage.TrySendData(MessageID.AnglerQuest, self.whoAmI, -1, NetworkText.FromLiteral(Main.player[self.whoAmI].name), Main.anglerQuest);
                        NetMessage.TrySendData(MessageID.FinishedConnectingToServer, self.whoAmI);
                        NetMessage.greetPlayer(self.whoAmI);
                        if (Main.player[num114].unlockedBiomeTorches)
                        {
                            NPC nPC2 = new();
                            nPC2.SetDefaults(NPCID.TorchGod);
                            Main.BestiaryTracker.Kills.RegisterKill(nPC2);
                        }
                    }
                    else
                    {
                        NetMessage.TrySendData(MessageID.PlayerSpawn, -1, self.whoAmI, null, self.whoAmI, (int)(byte)playerSpawnContext);
                    }
                    break;
                }
            case 13:
                {
                    int num138 = self.reader.ReadByte();
                    if (num138 != Main.myPlayer || Main.ServerSideCharacter)
                    {
                        num138 = self.whoAmI;
                        Player player10 = Main.player[num138];
                        BitsByte bitsByte21 = self.reader.ReadByte();
                        BitsByte bitsByte22 = self.reader.ReadByte();
                        BitsByte bitsByte23 = self.reader.ReadByte();
                        BitsByte bitsByte10 = self.reader.ReadByte();
                        player10.controlUp = bitsByte21[0];
                        player10.controlDown = bitsByte21[1];
                        player10.controlLeft = bitsByte21[2];
                        player10.controlRight = bitsByte21[3];
                        player10.controlJump = bitsByte21[4];
                        player10.controlUseItem = bitsByte21[5];
                        player10.direction = (bitsByte21[6] ? 1 : (-1));
                        if (bitsByte22[0])
                        {
                            player10.pulley = true;
                            player10.pulleyDir = (byte)((!bitsByte22[1]) ? 1u : 2u);
                        }
                        else
                        {
                            player10.pulley = false;
                        }
                        player10.vortexStealthActive = bitsByte22[3];
                        player10.gravDir = (bitsByte22[4] ? 1 : (-1));
                        player10.TryTogglingShield(bitsByte22[5]);
                        player10.ghost = bitsByte22[6];
                        player10.selectedItem = self.reader.ReadByte();
                        player10.position = self.reader.ReadVector2();
                        if (bitsByte22[2])
                        {
                            player10.velocity = self.reader.ReadVector2();
                        }
                        else
                        {
                            player10.velocity = Vector2.Zero;
                        }
                        if (bitsByte23[6])
                        {
                            player10.PotionOfReturnOriginalUsePosition = self.reader.ReadVector2();
                            player10.PotionOfReturnHomePosition = self.reader.ReadVector2();
                        }
                        else
                        {
                            player10.PotionOfReturnOriginalUsePosition = null;
                            player10.PotionOfReturnHomePosition = null;
                        }
                        player10.tryKeepingHoveringUp = bitsByte23[0];
                        player10.IsVoidVaultEnabled = bitsByte23[1];
                        player10.sitting.isSitting = bitsByte23[2];
                        player10.downedDD2EventAnyDifficulty = bitsByte23[3];
                        player10.isPettingAnimal = bitsByte23[4];
                        player10.isTheAnimalBeingPetSmall = bitsByte23[5];
                        player10.tryKeepingHoveringDown = bitsByte23[7];
                        player10.sleeping.SetIsSleepingAndAdjustPlayerRotation(player10, bitsByte10[0]);
                        player10.autoReuseAllWeapons = bitsByte10[1];
                        player10.controlDownHold = bitsByte10[2];
                        player10.isOperatingAnotherEntity = bitsByte10[3];
                        player10.controlUseTile = bitsByte10[4];
                        if (Netplay.Clients[self.whoAmI].State == 10)
                        {
                            NetMessage.TrySendData(MessageID.PlayerControls, -1, self.whoAmI, null, num138);
                        }
                    }
                    break;
                }
            case 16:
                {
                    int num139 = self.reader.ReadByte();
                    if (num139 != Main.myPlayer || Main.ServerSideCharacter)
                    {
                        num139 = self.whoAmI;
                        Player player11 = Main.player[num139];
                        player11.statLife = self.reader.ReadInt16();
                        player11.statLifeMax = self.reader.ReadInt16();
                        if (player11.statLifeMax < 100)
                        {
                            player11.statLifeMax = 100;
                        }
                        player11.dead = player11.statLife <= 0;
                        NetMessage.TrySendData(MessageID.PlayerLifeMana, -1, self.whoAmI, null, num139);
                    }
                    break;
                }
            case 17:
                {
                    byte action = self.reader.ReadByte();
                    int tileX = self.reader.ReadInt16();
                    int tileY = self.reader.ReadInt16();
                    short id = self.reader.ReadInt16();
                    int style = self.reader.ReadByte();
                    bool flag22 = id == 1;
                    if (!WorldGen.InWorld(tileX, tileY, 3))
                    {
                        break;
                    }
                    if (Main.tile[tileX, tileY] == null)
                    {
                        Main.tile[tileX, tileY] = Hooks.Tile.InvokeCreate();
                    }
                    if (!flag22)
                    {
                        if (action == 0 || action == 2 || action == 4)
                        {
                            Netplay.Clients[self.whoAmI].SpamDeleteBlock += 1f;
                        }
                        if (action == NetMessageID.TileManipulation.ActionPlaceTile || action == 3)
                        {
                            Netplay.Clients[self.whoAmI].SpamAddBlock += 1f;
                        }
                    }
                    if (!Netplay.Clients[self.whoAmI].TileSections[Netplay.GetSectionX(tileX), Netplay.GetSectionY(tileY)])
                    {
                        flag22 = true;
                    }
                    if (action == 0)
                    {
                        WorldGen.KillTile(tileX, tileY, flag22);
                    }
                    bool flag10 = false;
                    if (action == NetMessageID.TileManipulation.ActionPlaceTile)
                    {
                        bool forced = true;
                        if (forced && WorldGen.CheckTileBreakability2_ShouldTileSurvive(tileX, tileY))
                        {
                            flag10 = true;
                            forced = false;
                        }
                        WorldGen.PlaceTile(tileX, tileY, id, mute: false, forced, -1, style);
                    }
                    else if (action == 2)
                    {
                        WorldGen.KillWall(tileX, tileY, flag22);
                    }
                    else if (action == 3)
                    {
                        WorldGen.PlaceWall(tileX, tileY, id);
                    }
                    else if (action == 4)
                    {
                        WorldGen.KillTile(tileX, tileY, flag22, effectOnly: false, noItem: true);
                    }
                    else if (action == 5)
                    {
                        WorldGen.PlaceWire(tileX, tileY);
                    }
                    else if (action == 6)
                    {
                        WorldGen.KillWire(tileX, tileY);
                    }
                    else if (action == 7)
                    {
                        WorldGen.PoundTile(tileX, tileY);
                    }
                    else if (action == 8)
                    {
                        WorldGen.PlaceActuator(tileX, tileY);
                    }
                    else if (action == 9)
                    {
                        WorldGen.KillActuator(tileX, tileY);
                    }
                    else if (action == 10)
                    {
                        WorldGen.PlaceWire2(tileX, tileY);
                    }
                    else if (action == 11)
                    {
                        WorldGen.KillWire2(tileX, tileY);
                    }
                    else if (action == 12)
                    {
                        WorldGen.PlaceWire3(tileX, tileY);
                    }
                    else if (action == 13)
                    {
                        WorldGen.KillWire3(tileX, tileY);
                    }
                    else if (action == 14)
                    {
                        WorldGen.SlopeTile(tileX, tileY, id);
                    }
                    else if (action == 15)
                    {
                        Minecart.FrameTrack(tileX, tileY, pound: true);
                    }
                    else if (action == 16)
                    {
                        WorldGen.PlaceWire4(tileX, tileY);
                    }
                    else if (action == 17)
                    {
                        WorldGen.KillWire4(tileX, tileY);
                    }
                    switch (action)
                    {
                        case 18:
                            Wiring.SetCurrentUser(self.whoAmI);
                            Wiring.PokeLogicGate(tileX, tileY);
                            Wiring.SetCurrentUser();
                            return;
                        case 19:
                            Wiring.SetCurrentUser(self.whoAmI);
                            Wiring.Actuate(tileX, tileY);
                            Wiring.SetCurrentUser();
                            return;
                        case 20:
                            if (WorldGen.InWorld(tileX, tileY, 2))
                            {
                                int type10 = Main.tile[tileX, tileY].type;
                                WorldGen.KillTile(tileX, tileY, flag22);
                                id = (short)((Main.tile[tileX, tileY].active() && Main.tile[tileX, tileY].type == type10) ? 1 : 0);
                                NetMessage.TrySendData(MessageID.TileManipulation, -1, -1, null, action, tileX, tileY, id, style);
                            }
                            return;
                        case 21:
                            WorldGen.ReplaceTile(tileX, tileY, (ushort)id, style);
                            break;
                    }
                    if (action == 22)
                    {
                        WorldGen.ReplaceWall(tileX, tileY, (ushort)id);
                    }
                    else if (action == 23)
                    {
                        WorldGen.SlopeTile(tileX, tileY, id);
                        WorldGen.PoundTile(tileX, tileY);
                    }
                    if (flag10)
                    {
                        NetMessage.SendTileSquare(-1, tileX, tileY, 5);
                    }
                    else if ((action != 1 && action != 21) || !TileID.Sets.Falling[id] || Main.tile[tileX, tileY].active())
                    {
                        NetMessage.TrySendData(MessageID.TileManipulation, -1, self.whoAmI, null, action, tileX, tileY, id, style);
                    }
                    break;
                }
            case 19:
                {
                    byte b10 = self.reader.ReadByte();
                    int num190 = self.reader.ReadInt16();
                    int num192 = self.reader.ReadInt16();
                    if (WorldGen.InWorld(num190, num192, 3))
                    {
                        int num193 = ((self.reader.ReadByte() != 0) ? 1 : (-1));
                        switch (b10)
                        {
                            case 0:
                                WorldGen.OpenDoor(num190, num192, num193);
                                break;
                            case 1:
                                WorldGen.CloseDoor(num190, num192, forced: true);
                                break;
                            case 2:
                                WorldGen.ShiftTrapdoor(num190, num192, num193 == 1, 1);
                                break;
                            case 3:
                                WorldGen.ShiftTrapdoor(num190, num192, num193 == 1, 0);
                                break;
                            case 4:
                                WorldGen.ShiftTallGate(num190, num192, closing: false, forced: true);
                                break;
                            case 5:
                                WorldGen.ShiftTallGate(num190, num192, closing: true, forced: true);
                                break;
                        }
                        NetMessage.TrySendData(MessageID.ToggleDoorState, -1, self.whoAmI, null, b10, num190, num192, (num193 == 1) ? 1 : 0);
                    }
                    break;
                }
            case 20:
                {
                    int tileX = self.reader.ReadInt16();
                    int tileY = self.reader.ReadInt16();
                    ushort width = self.reader.ReadByte();
                    ushort height = self.reader.ReadByte();
                    byte tileChangeType = self.reader.ReadByte();
                    if (!WorldGen.InWorld(tileX, tileY, 3))
                    {
                        break;
                    }
                    TileChangeType type12 = TileChangeType.None;
                    if (Enum.IsDefined(typeof(TileChangeType), tileChangeType))
                    {
                        type12 = (TileChangeType)tileChangeType;
                    }
                    ReplaceMessageBuffer.GetOnTileChangeReceivedObjectFunc()?.Invoke(tileX, tileY, Math.Max(width, height), type12);
                    var hasError = false;
                    for (int num22 = tileX; num22 < tileX + width; num22++)
                    {
                        for (int num23 = tileY; num23 < tileY + height; num23++)
                        {
                            if (Main.tile[num22, num23] == null)
                            {
                                Main.tile[num22, num23] = Hooks.Tile.InvokeCreate();
                            }
                            ITile tile3 = Main.tile[num22, num23];
                            if (tile3.type == TileID.Dirt)
                            {
                                hasError = true;
                                BitsByte flag1 = self.reader.ReadByte();
                                BitsByte flag2 = self.reader.ReadByte();
                                self.readerStream.Seek(1, SeekOrigin.Current);
                                var active = flag1[0];
                                var wall = flag1[2] ? (ushort)1 : (ushort)0;
                                var hasLiquid = flag1[3];
                                if (flag2[2])
                                {
                                    self.readerStream.Seek(1, SeekOrigin.Current);
                                }
                                if (flag2[3])
                                {
                                    self.readerStream.Seek(1, SeekOrigin.Current);
                                }
                                if (active)
                                {
                                    var type = self.reader.ReadUInt16();
                                    if (Main.tileFrameImportant[type])
                                    {
                                        self.readerStream.Seek(4, SeekOrigin.Current);
                                    }
                                }
                                if (wall > 0)
                                {
                                    self.readerStream.Seek(2, SeekOrigin.Current);
                                }
                                if (hasLiquid)
                                {
                                    self.readerStream.Seek(2, SeekOrigin.Current);
                                }
                            }
                            else
                            {
                                bool oldActive = tile3.active();
                                BitsByte bitsByte15 = self.reader.ReadByte();
                                BitsByte bitsByte16 = self.reader.ReadByte();
                                BitsByte bitsByte18 = self.reader.ReadByte();
                                tile3.active(bitsByte15[0]);
                                tile3.wall = (bitsByte15[2] ? ((ushort)1) : ((ushort)0));
                                bool num208 = bitsByte15[3];
                                tile3.wire(bitsByte15[4]);
                                tile3.halfBrick(bitsByte15[5]);
                                tile3.actuator(bitsByte15[6]);
                                tile3.inActive(bitsByte15[7]);
                                tile3.wire2(bitsByte16[0]);
                                tile3.wire3(bitsByte16[1]);
                                if (bitsByte16[2])
                                {
                                    tile3.color(self.reader.ReadByte());
                                }
                                if (bitsByte16[3])
                                {
                                    tile3.wallColor(self.reader.ReadByte());
                                }
                                if (tile3.active())
                                {
                                    int oldType = tile3.type;
                                    tile3.type = self.reader.ReadUInt16();
                                    if (Main.tileFrameImportant[tile3.type])
                                    {
                                        tile3.frameX = self.reader.ReadInt16();
                                        tile3.frameY = self.reader.ReadInt16();
                                    }
                                    else if (!oldActive || tile3.type != oldType)
                                    {
                                        tile3.frameX = -1;
                                        tile3.frameY = -1;
                                    }
                                    byte b13 = 0;
                                    if (bitsByte16[4])
                                    {
                                        b13++;
                                    }
                                    if (bitsByte16[5])
                                    {
                                        b13 += 2;
                                    }
                                    if (bitsByte16[6])
                                    {
                                        b13 += 4;
                                    }
                                    tile3.slope(b13);
                                }
                                tile3.wire4(bitsByte16[7]);
                                tile3.fullbrightBlock(bitsByte18[0]);
                                tile3.fullbrightWall(bitsByte18[1]);
                                tile3.invisibleBlock(bitsByte18[2]);
                                tile3.invisibleWall(bitsByte18[3]);
                                if (tile3.wall > 0)
                                {
                                    tile3.wall = self.reader.ReadUInt16();
                                }
                                if (num208)
                                {
                                    tile3.liquid = self.reader.ReadByte();
                                    tile3.liquidType(self.reader.ReadByte());
                                }
                            }
                        }
                    }
                    if (hasError)
                    {
                        NetMessage.SendTileSquare(-1, tileX, tileY, width, height);
                    }
                    else
                    {
                        WorldGen.RangeFrame(tileX, tileY, tileX + width, tileY + height);
                        NetMessage.TrySendData(b, -1, self.whoAmI, null, tileX, tileY, width, height, tileChangeType);
                    }
                    break;
                }
            case 21:
            case 90:
            case 145:
            case 148:
                {
                    int num163 = self.reader.ReadInt16();
                    Vector2 position2 = self.reader.ReadVector2();
                    Vector2 velocity3 = self.reader.ReadVector2();
                    int stack2 = self.reader.ReadInt16();
                    int prefixWeWant2 = self.reader.ReadByte();
                    int num174 = self.reader.ReadByte();
                    int num185 = self.reader.ReadInt16();
                    bool shimmered = false;
                    float shimmerTime = 0f;
                    int timeLeftInWhichTheItemCannotBeTakenByEnemies = 0;
                    if (b == 145)
                    {
                        shimmered = self.reader.ReadBoolean();
                        shimmerTime = self.reader.ReadSingle();
                    }
                    if (b == 148)
                    {
                        timeLeftInWhichTheItemCannotBeTakenByEnemies = self.reader.ReadByte();
                    }
                    if (Main.timeItemSlotCannotBeReusedFor[num163] > 0)
                    {
                        break;
                    }
                    if (num185 == 0)
                    {
                        if (num163 < 400)
                        {
                            Main.item[num163].active = false;
                            NetMessage.TrySendData(MessageID.SyncItem, -1, -1, null, num163);
                        }
                        break;
                    }
                    bool flag19 = false;
                    if (num163 == 400)
                    {
                        flag19 = true;
                    }
                    if (flag19)
                    {
                        Item item3 = new();
                        item3.netDefaults(num185);
                        num163 = Item.NewItem(new EntitySource_Sync(), (int)position2.X, (int)position2.Y, item3.width, item3.height, item3.type, stack2, noBroadcast: true);
                    }
                    Item item4 = Main.item[num163];
                    item4.netDefaults(num185);
                    item4.Prefix(prefixWeWant2);
                    item4.stack = stack2;
                    item4.position = position2;
                    item4.velocity = velocity3;
                    item4.active = true;
                    item4.playerIndexTheItemIsReservedFor = Main.myPlayer;
                    item4.timeLeftInWhichTheItemCannotBeTakenByEnemies = timeLeftInWhichTheItemCannotBeTakenByEnemies;
                    if (b == 145)
                    {
                        item4.shimmered = shimmered;
                        item4.shimmerTime = shimmerTime;
                    }
                    if (flag19)
                    {
                        NetMessage.TrySendData(b, -1, -1, null, num163);
                        if (num174 == 0)
                        {
                            Main.item[num163].ownIgnore = self.whoAmI;
                            Main.item[num163].ownTime = 100;
                        }
                        Main.item[num163].FindOwner(num163);
                    }
                    else
                    {
                        NetMessage.TrySendData(b, -1, self.whoAmI, null, num163);
                    }
                    break;
                }
            case 22:
                {
                    int num68 = self.reader.ReadInt16();
                    int num69 = self.reader.ReadByte();
                    if (Main.item[num68].playerIndexTheItemIsReservedFor == self.whoAmI)
                    {
                        Main.item[num68].playerIndexTheItemIsReservedFor = num69;
                        if (num69 == Main.myPlayer)
                        {
                            Main.item[num68].keepTime = 15;
                        }
                        else
                        {
                            Main.item[num68].keepTime = 0;
                        }
                        Main.item[num68].playerIndexTheItemIsReservedFor = 255;
                        Main.item[num68].keepTime = 15;
                        NetMessage.TrySendData(MessageID.ItemOwner, -1, -1, null, num68);
                    }
                    break;
                }
            case 24:
                {
                    int num107 = self.reader.ReadInt16();
                    self.reader.ReadByte();
                    int num118 = self.whoAmI;
                    Player player20 = Main.player[num118];
                    Main.npc[num107].StrikeNPC(player20.inventory[player20.selectedItem].damage, player20.inventory[player20.selectedItem].knockBack, player20.direction, crit: false, noEffect: false, fromNet: false, Main.player[self.whoAmI]);
                    NetMessage.TrySendData(MessageID.UnusedMeleeStrike, -1, self.whoAmI, null, num107, num118);
                    NetMessage.TrySendData(MessageID.SyncNPC, -1, -1, null, num107);
                    break;
                }
            case 27:
                {
                    int num59 = self.reader.ReadInt16();
                    Vector2 position = self.reader.ReadVector2();
                    Vector2 velocity2 = self.reader.ReadVector2();
                    self.reader.ReadByte();
                    int num61 = self.reader.ReadInt16();
                    BitsByte bitsByte11 = self.reader.ReadByte();
                    BitsByte bitsByte17 = (byte)(bitsByte11[2] ? self.reader.ReadByte() : 0);
                    float[] array = self.ReUseTemporaryProjectileAI();
                    array[0] = (bitsByte11[0] ? self.reader.ReadSingle() : 0f);
                    array[1] = (bitsByte11[1] ? self.reader.ReadSingle() : 0f);
                    int bannerIdToRespondTo = (bitsByte11[3] ? self.reader.ReadUInt16() : 0);
                    int damage2 = (bitsByte11[4] ? self.reader.ReadInt16() : 0);
                    float knockBack2 = (bitsByte11[5] ? self.reader.ReadSingle() : 0f);
                    int originalDamage = (bitsByte11[6] ? self.reader.ReadInt16() : 0);
                    int num62 = (bitsByte11[7] ? self.reader.ReadInt16() : (-1));
                    if (num62 >= 1000)
                    {
                        num62 = -1;
                    }
                    array[2] = (bitsByte17[0] ? self.reader.ReadSingle() : 0f);
                    int num60;
                    if (num61 == 949)
                    {
                        num60 = 255;
                    }
                    else
                    {
                        num60 = self.whoAmI;
                        if (Main.projHostile[num61])
                        {
                            break;
                        }
                    }
                    int num64 = 1000;
                    for (int l = 0; l < 1000; l++)
                    {
                        if (Main.projectile[l].owner == num60 && Main.projectile[l].identity == num59 && Main.projectile[l].active)
                        {
                            num64 = l;
                            break;
                        }
                    }
                    if (num64 == 1000)
                    {
                        for (int num65 = 0; num65 < 1000; num65++)
                        {
                            if (!Main.projectile[num65].active)
                            {
                                num64 = num65;
                                break;
                            }
                        }
                    }
                    if (num64 == 1000)
                    {
                        num64 = Projectile.FindOldestProjectile();
                    }
                    Projectile projectile = Main.projectile[num64];
                    if (!projectile.active || projectile.type != num61)
                    {
                        projectile.SetDefaults(num61);
                        Netplay.Clients[self.whoAmI].SpamProjectile += 1f;
                    }
                    projectile.identity = num59;
                    projectile.position = position;
                    projectile.velocity = velocity2;
                    projectile.type = num61;
                    projectile.damage = damage2;
                    projectile.bannerIdToRespondTo = bannerIdToRespondTo;
                    projectile.originalDamage = originalDamage;
                    projectile.knockBack = knockBack2;
                    projectile.owner = num60;
                    for (int num66 = 0; num66 < Projectile.maxAI; num66++)
                    {
                        projectile.ai[num66] = array[num66];
                    }
                    if (num62 >= 0)
                    {
                        projectile.projUUID = num62;
                        Main.projectileIdentity[num60, num62] = num64;
                    }
                    projectile.ProjectileFixDesperation();
                    NetMessage.TrySendData(MessageID.SyncProjectile, -1, self.whoAmI, null, num64);
                    break;
                }
            case 28:
                {
                    int num186 = self.reader.ReadInt16();
                    int num187 = self.reader.ReadInt16();
                    float num188 = self.reader.ReadSingle();
                    int num189 = self.reader.ReadByte() - 1;
                    byte b9 = self.reader.ReadByte();
                    if (num187 < 0)
                    {
                        num187 = 0;
                    }
                    Main.npc[num186].PlayerInteraction(self.whoAmI);
                    if (num187 >= 0)
                    {
                        Main.npc[num186].StrikeNPC(num187, num188, num189, b9 == 1, noEffect: false, fromNet: true, Main.player[self.whoAmI]);
                    }
                    else
                    {
                        Main.npc[num186].life = 0;
                        Main.npc[num186].HitEffect();
                        Main.npc[num186].active = false;
                    }
                    NetMessage.TrySendData(MessageID.DamageNPC, -1, self.whoAmI, null, num186, num187, num188, num189, b9);
                    if (Main.npc[num186].life <= 0)
                    {
                        NetMessage.TrySendData(MessageID.SyncNPC, -1, -1, null, num186);
                    }
                    else
                    {
                        Main.npc[num186].netUpdate = true;
                    }
                    if (Main.npc[num186].realLife >= 0)
                    {
                        if (Main.npc[Main.npc[num186].realLife].life <= 0)
                        {
                            NetMessage.TrySendData(MessageID.SyncNPC, -1, -1, null, Main.npc[num186].realLife);
                        }
                        else
                        {
                            Main.npc[Main.npc[num186].realLife].netUpdate = true;
                        }
                    }
                    break;
                }
            case 29:
                {
                    int num120 = self.reader.ReadInt16();
                    self.reader.ReadByte();
                    int num121 = self.whoAmI;
                    for (int num130 = 0; num130 < 1000; num130++)
                    {
                        if (Main.projectile[num130].owner == num121 && Main.projectile[num130].identity == num120 && Main.projectile[num130].active)
                        {
                            Main.projectile[num130].Kill();
                            break;
                        }
                    }
                    NetMessage.TrySendData(MessageID.KillProjectile, -1, self.whoAmI, null, num120, num121);
                    break;
                }
            case 30:
                {
                    self.reader.ReadByte();
                    int num137 = self.whoAmI;
                    bool flag21 = self.reader.ReadBoolean();
                    Main.player[num137].hostile = flag21;
                    NetMessage.TrySendData(MessageID.TogglePVP, -1, self.whoAmI, null, num137);
                    LocalizedText obj3 = (flag21 ? Lang.mp[11] : Lang.mp[12]);
                    ChatHelper.BroadcastChatMessage(color: Main.teamColor[Main.player[num137].team], text: NetworkText.FromKey(obj3.Key, Main.player[num137].name));
                    break;
                }
            case 31:
                {
                    int num7 = self.reader.ReadInt16();
                    int num9 = self.reader.ReadInt16();
                    int num10 = Chest.FindChest(num7, num9);
                    if (num10 > -1 && Chest.UsingChest(num10) == -1)
                    {
                        for (int num11 = 0; num11 < 40; num11++)
                        {
                            NetMessage.TrySendData(MessageID.SyncChestItem, self.whoAmI, -1, null, num10, num11);
                        }
                        NetMessage.TrySendData(MessageID.SyncPlayerChest, self.whoAmI, -1, null, num10);
                        Main.player[self.whoAmI].chest = num10;
                        if (Main.myPlayer == self.whoAmI)
                        {
                            Main.recBigList = false;
                        }
                        NetMessage.TrySendData(MessageID.SyncPlayerChestIndex, -1, self.whoAmI, null, self.whoAmI, num10);
                        if (WorldGen.IsChestRigged(num7, num9))
                        {
                            Wiring.SetCurrentUser(self.whoAmI);
                            Wiring.HitSwitch(num7, num9);
                            Wiring.SetCurrentUser();
                            NetMessage.TrySendData(MessageID.HitSwitch, -1, self.whoAmI, null, num7, num9);
                        }
                    }
                    break;
                }
            case 32:
                {
                    int chestIndex = self.reader.ReadInt16();
                    int itemSlot = self.reader.ReadByte();
                    int itemStack = self.reader.ReadInt16();
                    int prefixWeWant = self.reader.ReadByte();
                    int itemType = self.reader.ReadInt16();
                    if (chestIndex >= 0 && chestIndex < 8000)
                    {
                        ref var chest = ref Main.chest[chestIndex];
                        chest ??= new();
                        ref var item = ref chest.item[itemSlot];
                        item ??= new();
                        item.netDefaults(itemType);
                        item.Prefix(prefixWeWant);
                        item.stack = itemStack;
                        //if (Main.chest[chestIndex] == null)
                        //{
                        //    Main.chest[chestIndex] = new Chest();
                        //}
                        //if (Main.chest[chestIndex].item[itemSlot] == null)
                        //{
                        //    Main.chest[chestIndex].item[itemSlot] = new Item();
                        //}
                        //Main.chest[chestIndex].item[itemSlot].netDefaults(itemType);
                        //Main.chest[chestIndex].item[itemSlot].Prefix(prefixWeWant);
                        //Main.chest[chestIndex].item[itemSlot].stack = itemStack;
                        //Recipe.FindRecipes(canDelayCheck: true);
                    }
                    break;
                }
            case 33:
                {
                    int num191 = self.reader.ReadInt16();
                    self.reader.ReadInt16();
                    self.reader.ReadInt16();
                    int num72 = self.reader.ReadByte();
                    string name = string.Empty;
                    if (num72 != 0)
                    {
                        if (num72 <= 20)
                        {
                            name = self.reader.ReadString();
                        }
                        else if (num72 != 255)
                        {
                            num72 = 0;
                        }
                    }
                    if (num72 != 0)
                    {
                        int chest = Main.player[self.whoAmI].chest;
                        Chest chest2 = Main.chest[chest];
                        chest2.name = name;
                        NetMessage.TrySendData(MessageID.ChestName, -1, self.whoAmI, null, chest, chest2.x, chest2.y);
                    }
                    Main.player[self.whoAmI].chest = num191;
                    Recipe.FindRecipes(canDelayCheck: true);
                    NetMessage.TrySendData(MessageID.SyncPlayerChestIndex, -1, self.whoAmI, null, self.whoAmI, num191);
                    break;
                }
            case 34:
                {
                    byte b11 = self.reader.ReadByte();
                    int num195 = self.reader.ReadInt16();
                    int num197 = self.reader.ReadInt16();
                    int num198 = self.reader.ReadInt16();
                    self.reader.ReadInt16();
                    switch (b11)
                    {
                        case 0:
                            {
                                int num201 = WorldGen.PlaceChest(num195, num197, TileID.Containers, notNearOtherChests: false, num198);
                                if (num201 == -1)
                                {
                                    NetMessage.TrySendData(MessageID.ChestUpdates, self.whoAmI, -1, null, b11, num195, num197, num198, num201);
                                    Item.NewItem(new EntitySource_TileBreak(num195, num197), num195 * 16, num197 * 16, 32, 32, Chest.chestItemSpawn[num198], 1, noBroadcast: true);
                                }
                                else
                                {
                                    NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, num198, num201);
                                }
                                break;
                            }
                        case 1:
                            if (Main.tile[num195, num197].type == TileID.Containers)
                            {
                                ITile tile4 = Main.tile[num195, num197];
                                if (tile4.frameX % 36 != 0)
                                {
                                    num195--;
                                }
                                if (tile4.frameY % 36 != 0)
                                {
                                    num197--;
                                }
                                int number = Chest.FindChest(num195, num197);
                                WorldGen.KillTile(num195, num197);
                                if (!tile4.active())
                                {
                                    NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, 0f, number);
                                }
                                break;
                            }
                            goto default;
                        default:
                            switch (b11)
                            {
                                case 2:
                                    {
                                        int num199 = WorldGen.PlaceChest(num195, num197, TileID.Dressers, notNearOtherChests: false, num198);
                                        if (num199 == -1)
                                        {
                                            NetMessage.TrySendData(MessageID.ChestUpdates, self.whoAmI, -1, null, b11, num195, num197, num198, num199);
                                            Item.NewItem(new EntitySource_TileBreak(num195, num197), num195 * 16, num197 * 16, 32, 32, Chest.dresserItemSpawn[num198], 1, noBroadcast: true);
                                        }
                                        else
                                        {
                                            NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, num198, num199);
                                        }
                                        break;
                                    }
                                case 3:
                                    if (Main.tile[num195, num197].type == TileID.Dressers)
                                    {
                                        ITile tile2 = Main.tile[num195, num197];
                                        num195 -= tile2.frameX % 54 / 18;
                                        if (tile2.frameY % 36 != 0)
                                        {
                                            num197--;
                                        }
                                        int number2 = Chest.FindChest(num195, num197);
                                        WorldGen.KillTile(num195, num197);
                                        if (!tile2.active())
                                        {
                                            NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, 0f, number2);
                                        }
                                        break;
                                    }
                                    goto default;
                                default:
                                    switch (b11)
                                    {
                                        case 4:
                                            {
                                                int num200 = WorldGen.PlaceChest(num195, num197, TileID.Containers2, notNearOtherChests: false, num198);
                                                if (num200 == -1)
                                                {
                                                    NetMessage.TrySendData(MessageID.ChestUpdates, self.whoAmI, -1, null, b11, num195, num197, num198, num200);
                                                    Item.NewItem(new EntitySource_TileBreak(num195, num197), num195 * 16, num197 * 16, 32, 32, Chest.chestItemSpawn2[num198], 1, noBroadcast: true);
                                                }
                                                else
                                                {
                                                    NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, num198, num200);
                                                }
                                                break;
                                            }
                                        case 5:
                                            if (Main.tile[num195, num197].type == TileID.Containers2)
                                            {
                                                ITile tile5 = Main.tile[num195, num197];
                                                if (tile5.frameX % 36 != 0)
                                                {
                                                    num195--;
                                                }
                                                if (tile5.frameY % 36 != 0)
                                                {
                                                    num197--;
                                                }
                                                int number3 = Chest.FindChest(num195, num197);
                                                WorldGen.KillTile(num195, num197);
                                                if (!tile5.active())
                                                {
                                                    NetMessage.TrySendData(MessageID.ChestUpdates, -1, -1, null, b11, num195, num197, 0f, number3);
                                                }
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    break;
                }
            case 35:
                {
                    self.reader.ReadByte();
                    int num146 = self.whoAmI;
                    int num147 = self.reader.ReadInt16();
                    if (num146 != Main.myPlayer || Main.ServerSideCharacter)
                    {
                        Main.player[num146].HealEffect(num147);
                    }
                    NetMessage.TrySendData(MessageID.PlayerHeal, -1, self.whoAmI, null, num146, num147);
                    break;
                }
            case 36:
                {
                    self.reader.ReadByte();
                    int num141 = self.whoAmI;
                    Player player18 = Main.player[num141];
                    bool num206 = player18.zone5[0];
                    player18.zone1 = self.reader.ReadByte();
                    player18.zone2 = self.reader.ReadByte();
                    player18.zone3 = self.reader.ReadByte();
                    player18.zone4 = self.reader.ReadByte();
                    player18.zone5 = self.reader.ReadByte();
                    if (!num206 && player18.zone5[0])
                    {
                        NPC.SpawnFaelings(num141);
                    }
                    NetMessage.TrySendData(MessageID.SyncPlayerZone, -1, self.whoAmI, null, num141);
                    break;
                }
            case 38:
                if (self.reader.ReadString() == Netplay.ServerPassword)
                {
                    Netplay.Clients[self.whoAmI].State = 1;
                    NetMessage.TrySendData(MessageID.PlayerInfo, self.whoAmI);
                }
                else
                {
                    NetMessage.TrySendData(MessageID.Kick, self.whoAmI, -1, Lang.mp[1].ToNetworkText());
                }
                break;
            case 40:
                {
                    self.reader.ReadByte();
                    int num47 = self.whoAmI;
                    int npcIndex = self.reader.ReadInt16();
                    Main.player[num47].SetTalkNPC(npcIndex, fromNet: true);
                    NetMessage.TrySendData(MessageID.SyncTalkNPC, -1, self.whoAmI, null, num47);
                    break;
                }
            case 41:
                {
                    self.reader.ReadByte();
                    int num16 = self.whoAmI;
                    Player player14 = Main.player[num16];
                    float itemRotation = self.reader.ReadSingle();
                    int itemAnimation = self.reader.ReadInt16();
                    player14.itemRotation = itemRotation;
                    player14.itemAnimation = itemAnimation;
                    player14.channel = player14.inventory[player14.selectedItem].channel;
                    NetMessage.TrySendData(MessageID.ShotAnimationAndSound, -1, self.whoAmI, null, num16);
                    break;
                }
            case 42:
                {
                    self.reader.ReadByte();
                    int num194 = self.whoAmI;
                    int statMana = self.reader.ReadInt16();
                    int statManaMax = self.reader.ReadInt16();
                    Main.player[num194].statMana = statMana;
                    Main.player[num194].statManaMax = statManaMax;
                    break;
                }
            case 43:
                {
                    self.reader.ReadByte();
                    int num153 = self.whoAmI;
                    int num154 = self.reader.ReadInt16();
                    if (num153 != Main.myPlayer)
                    {
                        Main.player[num153].ManaEffect(num154);
                    }
                    NetMessage.TrySendData(MessageID.Unknown43, -1, self.whoAmI, null, num153, num154);
                    break;
                }
            case 45:
                {
                    self.reader.ReadByte();
                    int num63 = self.whoAmI;
                    int num73 = self.reader.ReadByte();
                    Player player19 = Main.player[num63];
                    int team = player19.team;
                    player19.team = num73;
                    Color color = Main.teamColor[num73];
                    NetMessage.TrySendData(MessageID.Unknown45, -1, self.whoAmI, null, num63);
                    LocalizedText localizedText = Lang.mp[13 + num73];
                    if (num73 == 5)
                    {
                        localizedText = Lang.mp[22];
                    }
                    for (int num84 = 0; num84 < 255; num84++)
                    {
                        if (num84 == self.whoAmI || (team > 0 && Main.player[num84].team == team) || (num73 > 0 && Main.player[num84].team == num73))
                        {
                            ChatHelper.SendChatMessageToClient(NetworkText.FromKey(localizedText.Key, player19.name), color, num84);
                        }
                    }
                    break;
                }
            case 46:
                {
                    short i2 = self.reader.ReadInt16();
                    int j3 = self.reader.ReadInt16();
                    int num2 = Sign.ReadSign(i2, j3);
                    if (num2 >= 0)
                    {
                        NetMessage.TrySendData(MessageID.Unknown47, self.whoAmI, -1, null, num2, self.whoAmI);
                    }
                    break;
                }
            case 47:
                {
                    int num98 = self.reader.ReadInt16();
                    int x = self.reader.ReadInt16();
                    int y = self.reader.ReadInt16();
                    string text = self.reader.ReadString();
                    self.reader.ReadByte();
                    _ = (BitsByte)self.reader.ReadByte();
                    if (num98 >= 0 && num98 < 1000)
                    {
                        string? text2 = null;
                        if (Main.sign[num98] != null)
                        {
                            text2 = Main.sign[num98].text;
                        }
                        Main.sign[num98] = new Sign
                        {
                            x = x,
                            y = y
                        };
                        Sign.TextSign(num98, text);
                        if (text2 != text)
                        {
                            int num111 = self.whoAmI;
                            NetMessage.TrySendData(MessageID.Unknown47, -1, self.whoAmI, null, num98, num111);
                        }
                    }
                    break;
                }
            case 48:
                {
                    int num28 = self.reader.ReadInt16();
                    int num29 = self.reader.ReadInt16();
                    byte b14 = self.reader.ReadByte();
                    byte liquidType = self.reader.ReadByte();
                    if (Netplay.SpamCheck)
                    {
                        int num30 = self.whoAmI;
                        int num32 = (int)(Main.player[num30].position.X + (float)(Main.player[num30].width / 2));
                        int num207 = (int)(Main.player[num30].position.Y + (float)(Main.player[num30].height / 2));
                        int num33 = 10;
                        int num34 = num32 - num33;
                        int num36 = num32 + num33;
                        int num37 = num207 - num33;
                        int num38 = num207 + num33;
                        if (num28 < num34 || num28 > num36 || num29 < num37 || num29 > num38)
                        {
                            Netplay.Clients[self.whoAmI].SpamWater += 1f;
                        }
                    }
                    if (Main.tile[num28, num29] == null)
                    {
                        Main.tile[num28, num29] = Hooks.Tile.InvokeCreate();
                    }
                    lock (Main.tile[num28, num29])
                    {
                        Main.tile[num28, num29].liquid = b14;
                        Main.tile[num28, num29].liquidType(liquidType);
                        WorldGen.SquareTileFrame(num28, num29);
                        if (b14 == 0)
                        {
                            NetMessage.SendData(MessageID.LiquidUpdate, -1, self.whoAmI, null, num28, num29);
                        }
                        break;
                    }
                }
            case 49:
                if (Netplay.Connection.State == 6)
                {
                    Netplay.Connection.State = 10;
                    Main.player[Main.myPlayer].Spawn(PlayerSpawnContext.SpawningIntoWorld);
                }
                break;
            case 50:
                {
                    self.reader.ReadByte();
                    int num180 = self.whoAmI;
                    Player player12 = Main.player[num180];
                    for (int num181 = 0; num181 < Player.maxBuffs; num181++)
                    {
                        player12.buffType[num181] = self.reader.ReadUInt16();
                        if (player12.buffType[num181] > 0)
                        {
                            player12.buffTime[num181] = 60;
                        }
                        else
                        {
                            player12.buffTime[num181] = 0;
                        }
                    }
                    NetMessage.TrySendData(MessageID.PlayerBuffs, -1, self.whoAmI, null, num180);
                    break;
                }
            case 51:
                {
                    byte b7 = self.reader.ReadByte();
                    byte b8 = self.reader.ReadByte();
                    switch (b8)
                    {
                        case 1:
                            NPC.SpawnSkeletron(b7);
                            break;
                        case 2:
                            NetMessage.TrySendData(MessageID.MiscDataSync, -1, self.whoAmI, null, b7, (int)b8);
                            break;
                        case 3:
                            Main.Sundialing();
                            break;
                        case 4:
                            Main.npc[b7].BigMimicSpawnSmoke();
                            break;
                        case 5:
                            {
                                NPC nPC3 = new();
                                nPC3.SetDefaults(NPCID.TorchGod);
                                Main.BestiaryTracker.Kills.RegisterKill(nPC3);
                                break;
                            }
                        case 6:
                            Main.Moondialing();
                            break;
                    }
                    break;
                }
            case 52:
                {
                    int num148 = self.reader.ReadByte();
                    int num149 = self.reader.ReadInt16();
                    int num151 = self.reader.ReadInt16();
                    if (num148 == 1)
                    {
                        Chest.Unlock(num149, num151);
                        NetMessage.TrySendData(MessageID.LockAndUnlock, -1, self.whoAmI, null, 0, num148, num149, num151);
                        NetMessage.SendTileSquare(-1, num149, num151, 2);
                    }
                    if (num148 == 2)
                    {
                        WorldGen.UnlockDoor(num149, num151);
                        NetMessage.TrySendData(MessageID.LockAndUnlock, -1, self.whoAmI, null, 0, num148, num149, num151);
                        NetMessage.SendTileSquare(-1, num149, num151, 2);
                    }
                    if (num148 == 3)
                    {
                        Chest.Lock(num149, num151);
                        NetMessage.TrySendData(MessageID.LockAndUnlock, -1, self.whoAmI, null, 0, num148, num149, num151);
                        NetMessage.SendTileSquare(-1, num149, num151, 2);
                    }
                    break;
                }
            case 53:
                {
                    int num122 = self.reader.ReadInt16();
                    int type17 = self.reader.ReadUInt16();
                    int time2 = self.reader.ReadInt16();
                    Main.npc[num122].AddBuff(type17, time2, quiet: true);
                    NetMessage.TrySendData(MessageID.NPCBuffs, -1, -1, null, num122);
                    break;
                }
            case 55:
                {
                    int num70 = self.reader.ReadByte();
                    int num71 = self.reader.ReadUInt16();
                    int num74 = self.reader.ReadInt32();
                    if (num70 == self.whoAmI || Main.pvpBuff[num71])
                    {
                        NetMessage.TrySendData(MessageID.AddPlayerBuff, -1, -1, null, num70, num71, num74);
                    }
                    break;
                }
            case 56:
                {
                    int num53 = self.reader.ReadInt16();
                    if (num53 >= 0 && num53 < 200)
                    {
                        NetMessage.TrySendData(MessageID.UniqueTownNPCInfoSyncRequest, self.whoAmI, -1, null, num53);
                    }
                    break;
                }
            case 58:
                {
                    self.reader.ReadByte();
                    _ = self.whoAmI;
                    float num48 = self.reader.ReadSingle();
                    NetMessage.TrySendData(MessageID.InstrumentSound, -1, self.whoAmI, null, self.whoAmI, num48);
                    break;
                }
            case 59:
                {
                    int num106 = self.reader.ReadInt16();
                    int num196 = self.reader.ReadInt16();
                    Wiring.SetCurrentUser(self.whoAmI);
                    Wiring.HitSwitch(num106, num196);
                    Wiring.SetCurrentUser();
                    NetMessage.TrySendData(MessageID.HitSwitch, -1, self.whoAmI, null, num106, num196);
                    break;
                }
            case 60:
                {
                    int num43 = self.reader.ReadInt16();
                    int num44 = self.reader.ReadInt16();
                    int num45 = self.reader.ReadInt16();
                    byte b15 = self.reader.ReadByte();
                    if (num43 >= 200)
                    {
                        NetMessage.BootPlayer(self.whoAmI, NetworkText.FromKey("Net.CheatingInvalid"));
                    }
                    else if (Main.npc[num43].isLikeATownNPC)
                    {
                        if (b15 == 1)
                        {
                            WorldGen.kickOut(num43);
                        }
                        else
                        {
                            WorldGen.moveRoom(num44, num45, num43);
                        }
                    }
                    break;
                }
            case 61:
                {
                    int num4 = self.reader.ReadInt16();
                    int num5 = self.reader.ReadInt16();
                    if (num5 >= 0 && num5 < NPCID.Count && NPCID.Sets.MPAllowedEnemies[num5])
                    {
                        if (!NPC.AnyNPCs(num5))
                        {
                            NPC.SpawnOnPlayer(num4, num5);
                        }
                    }
                    else if (num5 == -4)
                    {
                        if (!Main.dayTime && !DD2Event.Ongoing)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[31].Key), new Color(50, 255, 130));
                            Main.startPumpkinMoon();
                            NetMessage.TrySendData(MessageID.WorldData);
                            NetMessage.TrySendData(MessageID.InvasionProgressReport, -1, -1, null, 0, 1f, 2f, 1f);
                        }
                    }
                    else if (num5 == -5)
                    {
                        if (!Main.dayTime && !DD2Event.Ongoing)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[34].Key), new Color(50, 255, 130));
                            Main.startSnowMoon();
                            NetMessage.TrySendData(MessageID.WorldData);
                            NetMessage.TrySendData(MessageID.InvasionProgressReport, -1, -1, null, 0, 1f, 1f, 1f);
                        }
                    }
                    else if (num5 == -6)
                    {
                        if (Main.dayTime && !Main.eclipse)
                        {
                            if (Main.remixWorld)
                            {
                                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[106].Key), new Color(50, 255, 130));
                            }
                            else
                            {
                                ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[20].Key), new Color(50, 255, 130));
                            }
                            Main.eclipse = true;
                            NetMessage.TrySendData(MessageID.WorldData);
                        }
                    }
                    else if (num5 == -7)
                    {
                        Main.invasionDelay = 0;
                        Main.StartInvasion(4);
                        NetMessage.TrySendData(MessageID.WorldData);
                        NetMessage.TrySendData(MessageID.InvasionProgressReport, -1, -1, null, 0, 1f, Main.invasionType + 3);
                    }
                    else if (num5 == -8)
                    {
                        if (NPC.downedGolemBoss && Main.hardMode && !NPC.AnyDanger() && !NPC.AnyoneNearCultists())
                        {
                            WorldGen.StartImpendingDoom(720);
                            NetMessage.TrySendData(MessageID.WorldData);
                        }
                    }
                    else if (num5 == -10)
                    {
                        if (!Main.dayTime && !Main.bloodMoon)
                        {
                            ChatHelper.BroadcastChatMessage(NetworkText.FromKey(Lang.misc[8].Key), new Color(50, 255, 130));
                            Main.bloodMoon = true;
                            if (Main.GetMoonPhase() == MoonPhase.Empty)
                            {
                                Main.moonPhase = 5;
                            }
                            AchievementsHelper.NotifyProgressionEvent(4);
                            NetMessage.TrySendData(MessageID.WorldData);
                        }
                    }
                    else if (num5 == -11)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookUsed"), new Color(50, 255, 130));
                        NPC.combatBookWasUsed = true;
                        NetMessage.TrySendData(MessageID.WorldData);
                    }
                    else if (num5 == -12)
                    {
                        NPC.UnlockOrExchangePet(ref NPC.boughtCat, NPCID.TownCat, "Misc.LicenseCatUsed", num5);
                    }
                    else if (num5 == -13)
                    {
                        NPC.UnlockOrExchangePet(ref NPC.boughtDog, NPCID.TownDog, "Misc.LicenseDogUsed", num5);
                    }
                    else if (num5 == -14)
                    {
                        NPC.UnlockOrExchangePet(ref NPC.boughtBunny, NPCID.TownBunny, "Misc.LicenseBunnyUsed", num5);
                    }
                    else if (num5 == -15)
                    {
                        NPC.UnlockOrExchangePet(ref NPC.unlockedSlimeBlueSpawn, NPCID.TownSlimeBlue, "Misc.LicenseSlimeUsed", num5);
                    }
                    else if (num5 == -16)
                    {
                        NPC.SpawnMechQueen(num4);
                    }
                    else if (num5 == -17)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.CombatBookVolumeTwoUsed"), new Color(50, 255, 130));
                        NPC.combatBookVolumeTwoWasUsed = true;
                        NetMessage.TrySendData(MessageID.WorldData);
                    }
                    else if (num5 == -18)
                    {
                        ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Misc.PeddlersSatchelUsed"), new Color(50, 255, 130));
                        NPC.peddlersSatchelWasUsed = true;
                        NetMessage.TrySendData(MessageID.WorldData);
                    }
                    else if (num5 < 0)
                    {
                        int num6 = 1;
                        if (num5 > -InvasionID.Count)
                        {
                            num6 = -num5;
                        }
                        if (num6 > 0 && Main.invasionType == InvasionID.None)
                        {
                            Main.invasionDelay = 0;
                            Main.StartInvasion(num6);
                        }
                        NetMessage.TrySendData(MessageID.InvasionProgressReport, -1, -1, null, 0, 1f, Main.invasionType + 3);
                    }
                    break;
                }
            case 62:
                {
                    self.reader.ReadByte();
                    int num164 = self.reader.ReadByte();
                    int num162 = self.whoAmI;
                    if (num164 == 1)
                    {
                        Main.player[num162].NinjaDodge();
                    }
                    if (num164 == 2)
                    {
                        Main.player[num162].ShadowDodge();
                    }
                    if (num164 == 4)
                    {
                        Main.player[num162].BrainOfConfusionDodge();
                    }
                    NetMessage.TrySendData(MessageID.Unknown62, -1, self.whoAmI, null, num162, num164);
                    break;
                }
            case 63:
                {
                    int num144 = self.reader.ReadInt16();
                    int num145 = self.reader.ReadInt16();
                    byte b4 = self.reader.ReadByte();
                    byte b5 = self.reader.ReadByte();
                    if (b5 == 0)
                    {
                        WorldGen.paintTile(num144, num145, b4);
                    }
                    else
                    {
                        WorldGen.paintCoatTile(num144, num145, b4);
                    }
                    NetMessage.TrySendData(MessageID.Unknown63, -1, self.whoAmI, null, num144, num145, (int)b4, (int)b5);
                    break;
                }
            case 64:
                {
                    int num131 = self.reader.ReadInt16();
                    int num132 = self.reader.ReadInt16();
                    byte b2 = self.reader.ReadByte();
                    byte b3 = self.reader.ReadByte();
                    if (b3 == 0)
                    {
                        WorldGen.paintWall(num131, num132, b2);
                    }
                    else
                    {
                        WorldGen.paintCoatWall(num131, num132, b2);
                    }
                    NetMessage.TrySendData(MessageID.Unknown64, -1, self.whoAmI, null, num131, num132, (int)b2, (int)b3);
                    break;
                }
            case 65:
                {
                    BitsByte bitsByte20 = self.reader.ReadByte();
                    self.reader.ReadInt16();
                    int num75 = self.whoAmI;
                    Vector2 vector = self.reader.ReadVector2();
                    int num76 = self.reader.ReadByte();
                    int num77 = 0;
                    if (bitsByte20[0])
                    {
                        num77++;
                    }
                    if (bitsByte20[1])
                    {
                        num77 += 2;
                    }
                    bool flag16 = false;
                    if (bitsByte20[2])
                    {
                        flag16 = true;
                    }
                    int num78 = 0;
                    if (bitsByte20[3])
                    {
                        num78 = self.reader.ReadInt32();
                    }
                    if (flag16)
                    {
                        vector = Main.player[num75].position;
                    }
                    switch (num77)
                    {
                        case 0:
                            Main.player[num75].Teleport(vector, num76, num78);
                            break;
                        case 1:
                            Main.npc[num75].Teleport(vector, num76, num78);
                            break;
                        case 2:
                            {
                                Main.player[num75].Teleport(vector, num76, num78);
                                RemoteClient.CheckSection(self.whoAmI, vector);
                                NetMessage.TrySendData(MessageID.TeleportEntity, -1, -1, null, 0, num75, vector.X, vector.Y, num76, flag16.ToInt(), num78);
                                int num79 = -1;
                                float num80 = 9999f;
                                for (int num81 = 0; num81 < 255; num81++)
                                {
                                    if (Main.player[num81].active && num81 != self.whoAmI)
                                    {
                                        Vector2 vector2 = Main.player[num81].position - Main.player[self.whoAmI].position;
                                        if (vector2.Length() < num80)
                                        {
                                            num80 = vector2.Length();
                                            num79 = num81;
                                        }
                                    }
                                }
                                if (num79 >= 0)
                                {
                                    ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Game.HasTeleportedTo", Main.player[self.whoAmI].name, Main.player[num79].name), new Color(250, 250, 0));
                                }
                                break;
                            }
                    }
                    if (num77 == 0)
                    {
                        NetMessage.TrySendData(MessageID.TeleportEntity, -1, self.whoAmI, null, num77, num75, vector.X, vector.Y, num76, flag16.ToInt(), num78);
                    }
                    break;
                }
            case 66:
                {
                    int num57 = self.reader.ReadByte();
                    int num58 = self.reader.ReadInt16();
                    if (num58 > 0)
                    {
                        Player player16 = Main.player[num57];
                        player16.statLife += num58;
                        if (player16.statLife > player16.statLifeMax2)
                        {
                            player16.statLife = player16.statLifeMax2;
                        }
                        player16.HealEffect(num58, broadcast: false);
                        NetMessage.TrySendData(MessageID.Unknown66, -1, self.whoAmI, null, num57, num58);
                    }
                    break;
                }
            case 68:
                Hooks.MessageBuffer.InvokeClientUUIDReceived(self, self.reader, start, length, ref messageType);
                break;
            case 69:
                {
                    int num39 = self.reader.ReadInt16();
                    int num40 = self.reader.ReadInt16();
                    int num41 = self.reader.ReadInt16();
                    if (num39 < -1 || num39 >= 8000)
                    {
                        break;
                    }
                    if (num39 == -1)
                    {
                        num39 = Chest.FindChest(num40, num41);
                        if (num39 == -1)
                        {
                            break;
                        }
                    }
                    Chest chest3 = Main.chest[num39];
                    if (chest3.x == num40 && chest3.y == num41)
                    {
                        NetMessage.TrySendData(MessageID.ChestName, self.whoAmI, -1, null, num39, num40, num41);
                    }
                    break;
                }
            case 70:
                {
                    int num17 = self.reader.ReadInt16();
                    self.reader.ReadByte();
                    int who = self.whoAmI;
                    if (num17 < 200 && num17 >= 0)
                    {
                        NPC.CatchNPC(num17, who);
                    }
                    break;
                }
            case 71:
                {
                    int x8 = self.reader.ReadInt32();
                    int y7 = self.reader.ReadInt32();
                    int type11 = self.reader.ReadInt16();
                    byte style3 = self.reader.ReadByte();
                    NPC.ReleaseNPC(x8, y7, type11, style3, self.whoAmI);
                    break;
                }
            case 73:
                switch (self.reader.ReadByte())
                {
                    case 0:
                        Main.player[self.whoAmI].TeleportationPotion();
                        break;
                    case 1:
                        Main.player[self.whoAmI].MagicConch();
                        break;
                    case 2:
                        Main.player[self.whoAmI].DemonConch();
                        break;
                    case 3:
                        Main.player[self.whoAmI].Shellphone_Spawn();
                        break;
                }
                break;
            case 75:
                {
                    string name2 = Main.player[self.whoAmI].name;
                    if (!Main.anglerWhoFinishedToday.Contains(name2))
                    {
                        Main.anglerWhoFinishedToday.Add(name2);
                    }
                    break;
                }
            case 76:
                {
                    int num169 = self.reader.ReadByte();
                    if (num169 != Main.myPlayer || Main.ServerSideCharacter)
                    {
                        num169 = self.whoAmI;
                        Player obj2 = Main.player[num169];
                        obj2.anglerQuestsFinished = self.reader.ReadInt32();
                        obj2.golferScoreAccumulated = self.reader.ReadInt32();
                        NetMessage.TrySendData(MessageID.QuestsCountSync, -1, self.whoAmI, null, num169);
                    }
                    break;
                }
            case 77:
                {
                    short type20 = self.reader.ReadInt16();
                    ushort tileType = self.reader.ReadUInt16();
                    short x2 = self.reader.ReadInt16();
                    short y5 = self.reader.ReadInt16();
                    Animation.NewTemporaryAnimation(type20, tileType, x2, y5);
                    break;
                }
            case 79:
                {
                    int x6 = self.reader.ReadInt16();
                    int y3 = self.reader.ReadInt16();
                    short type18 = self.reader.ReadInt16();
                    int style2 = self.reader.ReadInt16();
                    int num142 = self.reader.ReadByte();
                    int random = self.reader.ReadSByte();
                    int direction = (self.reader.ReadBoolean() ? 1 : (-1));
                    Netplay.Clients[self.whoAmI].SpamAddBlock += 1f;
                    if (WorldGen.InWorld(x6, y3, 10) && Netplay.Clients[self.whoAmI].TileSections[Netplay.GetSectionX(x6), Netplay.GetSectionY(y3)])
                    {
                        WorldGen.PlaceObject(x6, y3, type18, mute: false, style2, num142, random, direction);
                        NetMessage.SendObjectPlacement(self.whoAmI, x6, y3, type18, style2, num142, random, direction);
                    }
                    break;
                }
            case 82:
                NetManager.Instance.Read(self.reader, self.whoAmI, length);
                break;
            case 84:
                {
                    self.reader.ReadByte();
                    int num152 = self.whoAmI;
                    float stealth = self.reader.ReadSingle();
                    Main.player[num152].stealth = stealth;
                    NetMessage.TrySendData(MessageID.PlayerStealth, -1, self.whoAmI, null, num152);
                    break;
                }
            case 85:
                {
                    int whoAmI = self.whoAmI;
                    int slot = self.reader.ReadInt16();
                    if (whoAmI < 255)
                    {
                        Chest.ServerPlaceItem(self.whoAmI, slot);
                    }
                    break;
                }
            case 87:
                {
                    int x5 = self.reader.ReadInt16();
                    int y2 = self.reader.ReadInt16();
                    int type15 = self.reader.ReadByte();
                    if (WorldGen.InWorld(x5, y2) && !TileEntity.ByPosition.ContainsKey(new Point16(x5, y2)))
                    {
                        TileEntity.PlaceEntityNet(x5, y2, type15);
                    }
                    break;
                }
            case 89:
                {
                    short x11 = self.reader.ReadInt16();
                    int y6 = self.reader.ReadInt16();
                    int netId3 = self.reader.ReadInt16();
                    int prefix3 = self.reader.ReadByte();
                    int stack5 = self.reader.ReadInt16();
                    TEItemFrame.TryPlacing(x11, y6, netId3, prefix3, stack5);
                    break;
                }
            case 92:
                {
                    int num170 = self.reader.ReadInt16();
                    int num171 = self.reader.ReadInt32();
                    float num172 = self.reader.ReadSingle();
                    float num173 = self.reader.ReadSingle();
                    if (num170 >= 0 && num170 <= 200)
                    {
                        Main.npc[num170].extraValue += num171;
                        NetMessage.TrySendData(MessageID.SyncExtraValue, -1, -1, null, num170, Main.npc[num170].extraValue, num172, num173);
                    }
                    break;
                }
            case 95:
                {
                    ushort num165 = self.reader.ReadUInt16();
                    int num166 = self.reader.ReadByte();
                    for (int num168 = 0; num168 < 1000; num168++)
                    {
                        if (Main.projectile[num168].owner == num165 && Main.projectile[num168].active && Main.projectile[num168].type == ProjectileID.PortalGunGate && Main.projectile[num168].ai[1] == (float)num166)
                        {
                            Main.projectile[num168].Kill();
                            NetMessage.TrySendData(MessageID.KillProjectile, -1, -1, null, Main.projectile[num168].identity, (int)num165);
                            break;
                        }
                    }
                    break;
                }
            case 96:
                {
                    int num158 = self.reader.ReadByte();
                    Player obj6 = Main.player[num158];
                    int num159 = self.reader.ReadInt16();
                    Vector2 newPos2 = self.reader.ReadVector2();
                    Vector2 velocity5 = self.reader.ReadVector2();
                    int lastPortalColorIndex2 = num159 + ((num159 % 2 == 0) ? 1 : (-1));
                    obj6.lastPortalColorIndex = lastPortalColorIndex2;
                    obj6.Teleport(newPos2, 4, num159);
                    obj6.velocity = velocity5;
                    NetMessage.SendData(MessageID.TeleportPlayerThroughPortal, -1, -1, null, num158, newPos2.X, newPos2.Y, num159);
                    break;
                }
            case 99:
                {
                    self.reader.ReadByte();
                    int num143 = self.whoAmI;
                    Main.player[num143].MinionRestTargetPoint = self.reader.ReadVector2();
                    NetMessage.TrySendData(MessageID.MinionRestTargetUpdate, -1, self.whoAmI, null, num143);
                    break;
                }
            case 115:
                {
                    self.reader.ReadByte();
                    int num136 = self.whoAmI;
                    Main.player[num136].MinionAttackTargetNPC = self.reader.ReadInt16();
                    NetMessage.TrySendData(MessageID.MinionAttackTargetUpdate, -1, self.whoAmI, null, num136);
                    break;
                }
            case 100:
                {
                    int num127 = self.reader.ReadUInt16();
                    NPC obj5 = Main.npc[num127];
                    int num129 = self.reader.ReadInt16();
                    Vector2 newPos = self.reader.ReadVector2();
                    Vector2 velocity4 = self.reader.ReadVector2();
                    int lastPortalColorIndex = num129 + ((num129 % 2 == 0) ? 1 : (-1));
                    obj5.lastPortalColorIndex = lastPortalColorIndex;
                    obj5.Teleport(newPos, 4, num129);
                    obj5.velocity = velocity4;
                    obj5.netOffset *= 0f;
                    break;
                }
            case 102:
                {
                    self.reader.ReadByte();
                    ushort num83 = self.reader.ReadUInt16();
                    Vector2 other = self.reader.ReadVector2();
                    int num82 = self.whoAmI;
                    NetMessage.TrySendData(MessageID.NebulaLevelupRequest, -1, -1, null, num82, (int)num83, other.X, other.Y);
                    break;
                }
            case 105:
                {
                    short i3 = self.reader.ReadInt16();
                    int j2 = self.reader.ReadInt16();
                    bool on = self.reader.ReadBoolean();
                    WorldGen.ToggleGemLock(i3, j2, on);
                    break;
                }
            case 109:
                {
                    short x10 = self.reader.ReadInt16();
                    int y9 = self.reader.ReadInt16();
                    int x4 = self.reader.ReadInt16();
                    int y10 = self.reader.ReadInt16();
                    byte toolMode3 = self.reader.ReadByte();
                    int num56 = self.whoAmI;
                    WiresUI.Settings.MultiToolMode toolMode2 = WiresUI.Settings.ToolMode;
                    WiresUI.Settings.ToolMode = (WiresUI.Settings.MultiToolMode)toolMode3;
                    Wiring.MassWireOperation(new Point(x10, y9), new Point(x4, y10), Main.player[num56]);
                    WiresUI.Settings.ToolMode = toolMode2;
                    break;
                }
            case 111:
                BirthdayParty.ToggleManualParty();
                break;
            case 112:
                {
                    int num128 = self.reader.ReadByte();
                    int num140 = self.reader.ReadInt32();
                    int num150 = self.reader.ReadInt32();
                    int num157 = self.reader.ReadByte();
                    int num167 = self.reader.ReadInt16();
                    switch (num128)
                    {
                        case 1:
                            NetMessage.TrySendData(b, -1, -1, null, num128, num140, num150, num157, num167);
                            break;
                        case 2:
                            NPC.FairyEffects(new Vector2(num140, num150), num167);
                            break;
                    }
                    break;
                }
            case 113:
                {
                    int x3 = self.reader.ReadInt16();
                    int y8 = self.reader.ReadInt16();
                    if (!Main.snowMoon && !Main.pumpkinMoon)
                    {
                        if (DD2Event.WouldFailSpawningHere(x3, y8))
                        {
                            DD2Event.FailureMessage(self.whoAmI);
                        }
                        DD2Event.SummonCrystal(x3, y8, self.whoAmI);
                    }
                    break;
                }
            case 117:
                {
                    int num25 = self.reader.ReadByte();
                    if (self.whoAmI == num25 || (Main.player[num25].hostile && Main.player[self.whoAmI].hostile))
                    {
                        PlayerDeathReason playerDeathReason2 = PlayerDeathReason.FromReader(self.reader);
                        int damage3 = self.reader.ReadInt16();
                        int num26 = self.reader.ReadByte() - 1;
                        BitsByte bitsByte19 = self.reader.ReadByte();
                        bool flag14 = bitsByte19[0];
                        bool pvp2 = bitsByte19[1];
                        int num27 = self.reader.ReadSByte();
                        Main.player[num25].Hurt(playerDeathReason2, damage3, num26, pvp2, quiet: true, flag14, num27);
                        NetMessage.SendPlayerHurt(num25, playerDeathReason2, damage3, num26, flag14, pvp2, num27, -1, self.whoAmI);
                    }
                    break;
                }
            case 118:
                {
                    self.reader.ReadByte();
                    int num12 = self.whoAmI;
                    PlayerDeathReason playerDeathReason = PlayerDeathReason.FromReader(self.reader);
                    int num14 = self.reader.ReadInt16();
                    int num15 = self.reader.ReadByte() - 1;
                    bool pvp = ((BitsByte)self.reader.ReadByte())[0];
                    Main.player[num12].KillMe(playerDeathReason, num14, num15, pvp);
                    NetMessage.SendPlayerDeath(num12, playerDeathReason, num14, num15, pvp, -1, self.whoAmI);
                    break;
                }
            case 120:
                {
                    self.reader.ReadByte();
                    int num205 = self.whoAmI;
                    int num3 = self.reader.ReadByte();
                    if (num3 >= 0 && num3 < EmoteID.Count)
                    {
                        EmoteBubble.NewBubble(num3, new WorldUIAnchor(Main.player[num205]), 360);
                        EmoteBubble.CheckForNPCsToReactToEmoteBubble(num3, Main.player[num205]);
                    }
                    break;
                }
            case 121:
                {
                    self.reader.ReadByte();
                    int num182 = self.whoAmI;
                    int num183 = self.reader.ReadInt32();
                    int num184 = self.reader.ReadByte();
                    bool flag11 = false;
                    if (num184 >= 8)
                    {
                        flag11 = true;
                        num184 -= 8;
                    }
                    if (!TileEntity.ByID.TryGetValue(num183, out var value6))
                    {
                        self.reader.ReadInt32();
                        self.reader.ReadByte();
                        break;
                    }
                    if (num184 >= 8)
                    {
                        value6 = null;
                    }
                    if (value6 is TEDisplayDoll tEDisplayDoll)
                    {
                        tEDisplayDoll.ReadItem(num184, self.reader, flag11);
                    }
                    else
                    {
                        self.reader.ReadInt32();
                        self.reader.ReadByte();
                    }
                    NetMessage.TrySendData(b, -1, num182, null, num182, num183, num184, flag11.ToInt());
                    break;
                }
            case 122:
                {
                    int num155 = self.reader.ReadInt32();
                    self.reader.ReadByte();
                    int num156 = self.whoAmI;
                    if (num155 == -1)
                    {
                        Main.player[num156].tileEntityAnchor.Clear();
                        NetMessage.TrySendData(b, -1, -1, null, num155, num156);
                    }
                    else if (!TileEntity.IsOccupied(num155, out _) && TileEntity.ByID.TryGetValue(num155, out var value5))
                    {
                        Main.player[num156].tileEntityAnchor.Set(num155, value5.Position.X, value5.Position.Y);
                        NetMessage.TrySendData(b, -1, -1, null, num155, num156);
                    }
                    break;
                }
            case 123:
                {
                    short x9 = self.reader.ReadInt16();
                    int y4 = self.reader.ReadInt16();
                    int netId2 = self.reader.ReadInt16();
                    int prefix2 = self.reader.ReadByte();
                    int stack3 = self.reader.ReadInt16();
                    TEWeaponsRack.TryPlacing(x9, y4, netId2, prefix2, stack3);
                    break;
                }
            case 124:
                {
                    self.reader.ReadByte();
                    int num133 = self.whoAmI;
                    int num134 = self.reader.ReadInt32();
                    int num135 = self.reader.ReadByte();
                    bool flag20 = false;
                    if (num135 >= 2)
                    {
                        flag20 = true;
                        num135 -= 2;
                    }
                    if (!TileEntity.ByID.TryGetValue(num134, out var value4))
                    {
                        self.reader.ReadInt32();
                        self.reader.ReadByte();
                        break;
                    }
                    if (num135 >= 2)
                    {
                        value4 = null;
                    }
                    if (value4 is TEHatRack tEHatRack)
                    {
                        tEHatRack.ReadItem(num135, self.reader, flag20);
                    }
                    else
                    {
                        self.reader.ReadInt32();
                        self.reader.ReadByte();
                    }
                    NetMessage.TrySendData(b, -1, num133, null, num133, num134, num135, flag20.ToInt());
                    break;
                }
            case 125:
                {
                    self.reader.ReadByte();
                    int num124 = self.reader.ReadInt16();
                    int num125 = self.reader.ReadInt16();
                    int num126 = self.reader.ReadByte();
                    int num123 = self.whoAmI;
                    NetMessage.TrySendData(MessageID.SyncTilePicking, -1, num123, null, num123, num124, num125, num126);
                    break;
                }
            case 127:
                self.reader.ReadInt32();
                break;
            case 128:
                {
                    int num13 = self.reader.ReadByte();
                    int num24 = self.reader.ReadUInt16();
                    int num35 = self.reader.ReadUInt16();
                    int num46 = self.reader.ReadUInt16();
                    int num54 = self.reader.ReadUInt16();
                    NetMessage.SendData(MessageID.LandGolfBallInCup, -1, num13, null, num13, num46, num54, 0f, num24, num35);
                    break;
                }
            case 130:
                {
                    int num115 = self.reader.ReadUInt16();
                    int num116 = self.reader.ReadUInt16();
                    int num117 = self.reader.ReadInt16();
                    if (num117 == 682)
                    {
                        if (NPC.unlockedSlimeRedSpawn)
                        {
                            break;
                        }
                        NPC.unlockedSlimeRedSpawn = true;
                        NetMessage.TrySendData(MessageID.WorldData);
                    }
                    num115 *= 16;
                    num116 *= 16;
                    NPC nPC4 = new();
                    nPC4.SetDefaults(num117);
                    int type16 = nPC4.type;
                    int netID = nPC4.netID;
                    int num119 = NPC.NewNPC(new EntitySource_FishedOut(Main.player[self.whoAmI]), num115, num116, num117);
                    if (netID != type16)
                    {
                        Main.npc[num119].SetDefaults(netID);
                        NetMessage.TrySendData(MessageID.SyncNPC, -1, -1, null, num119);
                    }
                    if (num117 == 682)
                    {
                        WorldGen.CheckAchievement_RealEstateAndTownSlimes();
                    }
                    break;
                }
            case 133:
                {
                    short x7 = self.reader.ReadInt16();
                    int y11 = self.reader.ReadInt16();
                    int netId = self.reader.ReadInt16();
                    int prefix = self.reader.ReadByte();
                    int stack = self.reader.ReadInt16();
                    TEFoodPlatter.TryPlacing(x7, y11, netId, prefix, stack);
                    break;
                }
            case 134:
                {
                    self.reader.ReadByte();
                    int ladyBugLuckTimeLeft = self.reader.ReadInt32();
                    float torchLuck = self.reader.ReadSingle();
                    byte luckPotion = self.reader.ReadByte();
                    bool hasGardenGnomeNearby = self.reader.ReadBoolean();
                    float equipmentBasedLuckBonus = self.reader.ReadSingle();
                    float coinLuck = self.reader.ReadSingle();
                    int num67 = self.whoAmI;
                    Player obj4 = Main.player[num67];
                    obj4.ladyBugLuckTimeLeft = ladyBugLuckTimeLeft;
                    obj4.torchLuck = torchLuck;
                    obj4.luckPotion = luckPotion;
                    obj4.HasGardenGnomeNearby = hasGardenGnomeNearby;
                    obj4.equipmentBasedLuckBonus = equipmentBasedLuckBonus;
                    obj4.coinLuck = coinLuck;
                    obj4.RecalculateLuck();
                    NetMessage.SendData(MessageID.UpdatePlayerLuckFactors, -1, num67, null, num67);
                    break;
                }
            case 135:
                self.reader.ReadByte();
                break;
            case 136:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            NPC.cavernMonsterType[i, k] = self.reader.ReadUInt16();
                        }
                    }
                    break;
                }
            case 137:
                {
                    int num55 = self.reader.ReadInt16();
                    int buffTypeToRemove = self.reader.ReadUInt16();
                    if (num55 >= 0 && num55 < 200)
                    {
                        Main.npc[num55].RequestBuffRemoval(buffTypeToRemove);
                    }
                    break;
                }
            case 140:
                {
                    int num31 = self.reader.ReadByte();
                    int num42 = self.reader.ReadInt32();
                    switch (num31)
                    {
                        case 1:
                            NPC.TransformCopperSlime(num42);
                            break;
                        case 2:
                            NPC.TransformElderSlime(num42);
                            break;
                        case 0:
                            break;
                    }
                    break;
                }
            case 141:
                {
                    LucyAxeMessage.MessageSource messageSource = (LucyAxeMessage.MessageSource)self.reader.ReadByte();
                    byte b16 = self.reader.ReadByte();
                    Vector2 velocity = self.reader.ReadVector2();
                    int num202 = self.reader.ReadInt32();
                    int num8 = self.reader.ReadInt32();
                    NetMessage.SendData(MessageID.RequestLucyPopup, -1, self.whoAmI, null, (int)messageSource, (int)b16, velocity.X, velocity.Y, num202, num8);
                    break;
                }
            case 142:
                {
                    self.reader.ReadByte();
                    int num179 = self.whoAmI;
                    Player obj = Main.player[num179];
                    obj.piggyBankProjTracker.TryReading(self.reader);
                    obj.voidLensChest.TryReading(self.reader);
                    NetMessage.TrySendData(MessageID.SyncProjectileTrackers, -1, self.whoAmI, null, num179);
                    break;
                }
            case 143:
                DD2Event.AttemptToSkipWaitTime();
                break;
            case 144:
                NPC.HaveDryadDoStardewAnimation();
                break;
            case 146:
                switch (self.reader.ReadByte())
                {
                    case 0:
                        Item.ShimmerEffect(self.reader.ReadVector2());
                        break;
                    case 1:
                        {
                            Vector2 coinPosition = self.reader.ReadVector2();
                            int coinAmount = self.reader.ReadInt32();
                            Main.player[Main.myPlayer].AddCoinLuck(coinPosition, coinAmount);
                            break;
                        }
                    case 2:
                        {
                            int num95 = self.reader.ReadInt32();
                            Main.npc[num95].SetNetShimmerEffect();
                            break;
                        }
                }
                break;
            case 147:
                {
                    self.reader.ReadByte();
                    int num85 = self.whoAmI;
                    int num86 = self.reader.ReadByte();
                    Main.player[num85].TrySwitchingLoadout(num86);
                    MessageBuffer.ReadAccessoryVisibility(self.reader, Main.player[num85].hideVisibleAccessory);
                    NetMessage.TrySendData(b, -1, num85, null, num85, num86);
                    break;
                }
            default:
                if (Netplay.Clients[self.whoAmI].State == 0)
                {
                    NetMessage.BootPlayer(self.whoAmI, Lang.mp[2].ToNetworkText());
                }
                break;
            case 2:
            case 3:
            case 7:
            case 9:
            case 10:
            case 11:
            case 14:
            case 15:
            case 18:
            case 23:
            case 25:
            case 26:
            case 37:
            case 39:
            case 44:
            case 54:
            case 57:
            case 67:
            case 72:
            case 74:
            case 78:
            case 80:
            case 81:
            case 83:
            case 86:
            case 88:
            case 91:
            case 93:
            case 97:
            case 98:
            case 101:
            case 103:
            case 104:
            case 106:
            case 107:
            case 108:
            case 110:
            case 114:
            case 116:
            case 119:
            case 126:
            case 129:
            case 131:
            case 132:
            case 139:
                break;
        }
    }

    private static readonly Func<TileChangeReceivedEvent?> GetOnTileChangeReceivedObjectFunc;
    static ReplaceMessageBuffer()
    {
        var method = new DynamicMethod(nameof(GetOnTileChangeReceivedObjectFunc), typeof(TileChangeReceivedEvent), Type.EmptyTypes);
        var il = method.GetILGenerator();
        il.Emit(OpCodes.Ldsfld, typeof(MessageBuffer).GetField("OnTileChangeReceived", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!);
        il.Emit(OpCodes.Ret);
        GetOnTileChangeReceivedObjectFunc = method.CreateDelegate<Func<TileChangeReceivedEvent>>();
    }
}
