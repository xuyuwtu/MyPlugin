using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using OTAPI;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Social;

namespace VBY.GameContentModify;

public static class ReplaceNetMessage
{
    internal static ushort[] PacketLength = new ushort[MessageID.Count];
    static ReplaceNetMessage()
    {
        //1 text
        //2 text
        PacketLength[3] = 3 + sizeof(byte) * 2;
        //4 text name
        PacketLength[5] = 3 + sizeof(byte) + sizeof(short) * 2 + sizeof(byte) + sizeof(short);
        //
        //7 text worldName
        PacketLength[8] = 3 + sizeof(int) * 2;
        //9 text
        //10 to long
        PacketLength[11] = 3 + sizeof(short) * 4;
        PacketLength[12] = 3 + sizeof(byte) + sizeof(short) * 2 + sizeof(int) + sizeof(short) * 2 + sizeof(byte);
        PacketLength[13] = 3 + sizeof(byte) * 6 + SizeOf.Vector2 * 4;
        PacketLength[14] = 3 + sizeof(byte) * 2;
        //
        PacketLength[16] = 3 + sizeof(byte) + sizeof(short) * 2;
        PacketLength[17] = 3 + sizeof(byte) + sizeof(short) * 3 + sizeof(byte);
        PacketLength[18] = 3 + sizeof(byte) + sizeof(int) + sizeof(short) * 2;
        PacketLength[19] = 3 + sizeof(byte) + sizeof(short) * 2 + sizeof(byte);
        //20 to long
        PacketLength[21] = 3 + sizeof(short) + SizeOf.Vector2 * 2 + sizeof(short) + sizeof(byte) * 2 + sizeof(short);
        PacketLength[22] = 3 + sizeof(short) + sizeof(byte);
        PacketLength[23] = (ushort)(3 + sizeof(short) + SizeOf.Vector2 * 2 + sizeof(ushort) + sizeof(byte) * 2 + sizeof(float) * NPC.maxAI + sizeof(short) + sizeof(byte) + sizeof(float) + sizeof(byte) + sizeof(int) + sizeof(byte));
        PacketLength[24] = 3 + sizeof(short) + sizeof(byte);
        //
        //
        PacketLength[27] = 3 + sizeof(short) + SizeOf.Vector2 * 2 + sizeof(byte) + sizeof(short) + sizeof(byte) * 2 + sizeof(float) * 2 + sizeof(ushort) + sizeof(short) + sizeof(float) + sizeof(short) * 2 + sizeof(float);
        PacketLength[28] = 3 + sizeof(short) * 2 + sizeof(float) + sizeof(byte) * 2;
        PacketLength[29] = 3 + sizeof(short) + sizeof(byte);
        PacketLength[30] = 3 + sizeof(byte) * 2;
        PacketLength[31] = 3 + sizeof(short) * 2;
        PacketLength[32] = 3 + sizeof(short) + sizeof(byte) + sizeof(short) + sizeof(byte) + sizeof(short);
        //33 text chestName
        PacketLength[34] = 3 + sizeof(byte) + sizeof(short) * 3 + sizeof(int) * 2 + sizeof(short);
        PacketLength[35] = 3 + sizeof(byte) + sizeof(short);
        PacketLength[36] = 3 + sizeof(byte) * 6;
        //
        //38 text password
        PacketLength[39] = 3 + sizeof(short);
        PacketLength[40] = 3 + sizeof(byte) + sizeof(short);
        PacketLength[41] = 3 + sizeof(byte) + sizeof(float) + sizeof(short);
        PacketLength[42] = 3 + sizeof(byte) + sizeof(short) * 2;
        PacketLength[43] = 3 + sizeof(byte) + sizeof(short);
        //
        PacketLength[45] = 3 + sizeof(byte) * 2;
        PacketLength[46] = 3 + sizeof(short) * 2;
        //47 text signText
        PacketLength[48] = 3 + sizeof(short) * 2 + sizeof(byte) * 2;
        //
        PacketLength[50] = (ushort)(3 + sizeof(byte) + sizeof(ushort) * Player.maxBuffs);
        PacketLength[51] = 3 + sizeof(byte) * 2;
        PacketLength[52] = 3 + sizeof(byte) + sizeof(short) * 2;
        PacketLength[53] = 3 + sizeof(short) + sizeof(ushort) + sizeof(short);
        PacketLength[54] = (ushort)(3 + sizeof(short) + (sizeof(ushort) + sizeof(short)) * NPC.maxBuffs);
        PacketLength[55] = 3 + sizeof(byte) + sizeof(ushort) + sizeof(int);
        //56 text name
        PacketLength[57] = 3 + sizeof(byte) * 3;
        PacketLength[58] = 3 + sizeof(byte) + sizeof(float);
        PacketLength[59] = 3 + sizeof(short) * 2;
        PacketLength[60] = 3 + sizeof(short) * 3 + sizeof(byte);
        PacketLength[61] = 3 + sizeof(short) * 2;
        PacketLength[62] = 3 + sizeof(byte) * 2;
        PacketLength[63] = 3 + sizeof(short) * 2 + sizeof(byte) * 2;
        PacketLength[64] = PacketLength[63];
        PacketLength[65] = 3 + sizeof(byte) + sizeof(short) + sizeof(float) * 2 + sizeof(byte) + sizeof(int);
        PacketLength[66] = 3 + sizeof(byte) + sizeof(short);
        //
        //68 text clientUUID
        //69 text chestName
        PacketLength[70] = 3 + sizeof(short) + sizeof(byte);
        PacketLength[71] = 3 + sizeof(short) + sizeof(byte);
        PacketLength[72] = 3 + sizeof(short) * 40;
        PacketLength[73] = 3 + sizeof(byte);
        PacketLength[74] = 3 + sizeof(byte) * 2;
        //
        PacketLength[76] = 3 + sizeof(byte) + sizeof(int) * 2;
        PacketLength[77] = 3 + sizeof(short) + sizeof(ushort) + sizeof(short) * 2;
        PacketLength[78] = 3 + sizeof(int) * 2 + sizeof(sbyte) * 2;
        PacketLength[79] = 3 + sizeof(short) * 4 + sizeof(byte) * 3;
        PacketLength[80] = 3 + sizeof(byte) + sizeof(short);
        PacketLength[81] = 3 + sizeof(float) * 2 + SizeOf.Color + sizeof(int);
        //
        PacketLength[83] = 3 + sizeof(short) + sizeof(int);
        PacketLength[84] = 3 + sizeof(byte) + sizeof(float);

        PacketLength[85] = 3 + sizeof(short);
        //86 ... virtual
        PacketLength[87] = 3 + sizeof(short) * 2 + sizeof(byte);
        PacketLength[88] = 3 + sizeof(short) + sizeof(byte) + sizeof(uint) + sizeof(ushort) + sizeof(float) + sizeof(ushort) * 2 + sizeof(short) + sizeof(float) + sizeof(byte) + sizeof(ushort) * 2 + sizeof(float) + sizeof(short) * 2 + sizeof(byte);
        PacketLength[89] = 3 + sizeof(short) * 3 + sizeof(byte) + sizeof(short);
        PacketLength[90] = PacketLength[21];
        PacketLength[91] = 3 + sizeof(int) + sizeof(byte) + sizeof(ushort) * 2 + sizeof(byte) + sizeof(short);
        PacketLength[92] = 3 + sizeof(short) + sizeof(int) + sizeof(float) * 2;
        //
        //
        PacketLength[95] = 3 + sizeof(ushort) + sizeof(byte);
        PacketLength[96] = 3 + sizeof(byte) + sizeof(short) + sizeof(float) * 2 + SizeOf.Vector2;
        PacketLength[97] = 3 + sizeof(short);
        PacketLength[98] = 3 + sizeof(short);
        PacketLength[99] = 3 + sizeof(byte) + SizeOf.Vector2;
        PacketLength[100] = 3 + sizeof(ushort) + sizeof(short) + sizeof(float) * 2 + SizeOf.Vector2;
        PacketLength[101] = 3 + sizeof(ushort) * 4;
        PacketLength[102] = 3 + sizeof(byte) + sizeof(ushort) + sizeof(float) * 2;
        PacketLength[103] = 3 + sizeof(int) * 2;
        PacketLength[104] = 3 + sizeof(byte) + sizeof(short) * 2 + sizeof(byte) + sizeof(float) + sizeof(byte);
        PacketLength[105] = 3 + sizeof(short) * 2 + sizeof(byte);
        PacketLength[106] = 3 + sizeof(uint);
        //
        PacketLength[108] = 3 + sizeof(short) + sizeof(float) + sizeof(short) * 4 + sizeof(byte);
        PacketLength[109] = 3 + sizeof(short) * 4 + sizeof(byte);
        PacketLength[110] = 3 + sizeof(short) * 2 + sizeof(byte);
        //
        PacketLength[112] = 3 + sizeof(byte) + sizeof(int) * 2 + sizeof(byte) + sizeof(short);
        PacketLength[113] = 3 + sizeof(short) * 2;
        //
        PacketLength[115] = 3 + sizeof(byte) + sizeof(short);
        PacketLength[116] = 3 + sizeof(int);
        //117 text 
        //118 text
        //119 text
        PacketLength[120] = 3 + sizeof(byte) * 2;
        PacketLength[121] = 3 + sizeof(byte) + sizeof(int) +sizeof(byte)+ SizeOf.TEDisplayDoll + sizeof(int) + sizeof(byte);
        PacketLength[122] = 3 + sizeof(int) + sizeof(byte);
        PacketLength[123] = 3 + sizeof(short) * 3 + sizeof(byte) + sizeof(short);
        PacketLength[124] = 3 + sizeof(byte) + sizeof(int) +sizeof(byte)+ SizeOf.TEHatRack + sizeof(int) + sizeof(byte);
        PacketLength[125] = 3 + sizeof(byte) + sizeof(short) * 2 + sizeof(byte);
        PacketLength[126] = 3 + sizeof(int) + sizeof(float) * 2 + sizeof(int) + sizeof(float) + sizeof(int) * 3 + sizeof(float) + sizeof(byte);
        PacketLength[127] = 3 + sizeof(int);
        PacketLength[128] = 3 + sizeof(byte) + sizeof(ushort) * 4;
        //
        PacketLength[130] = 3 + sizeof(ushort) * 2 + sizeof(short);
        PacketLength[131] = 3 + sizeof(ushort) + sizeof(byte) + sizeof(int) + sizeof(short);
        PacketLength[132] = 3 + SizeOf.Vector2 + sizeof(ushort) + sizeof(byte) + sizeof(int) + sizeof(float) * 2;
        PacketLength[133] = 3 + sizeof(short) * 3 + sizeof(byte) + sizeof(short);
        PacketLength[134] = 3 + sizeof(byte) + sizeof(int) + sizeof(float) + sizeof(byte) * 2 + sizeof(float) + sizeof(float) * 2;
        PacketLength[135] = 3 + sizeof(byte);
        PacketLength[136] = 3 + sizeof(ushort) * 3 * 2;
        PacketLength[137] = 3 + sizeof(short) + sizeof(ushort);
        //
        PacketLength[139] = 3 + sizeof(byte) * 2;
        PacketLength[140] = 3 + sizeof(byte) + sizeof(int);
        PacketLength[141] = 3 + sizeof(byte) * 2 + sizeof(float) * 4;
        PacketLength[142] = 3 + sizeof(byte) + SizeOf.TrackedProjectileReference * 2;
        //
        //
        PacketLength[145] = (ushort)(PacketLength[21] + sizeof(byte) + sizeof(float));
        PacketLength[146] = 3 + sizeof(byte) + SizeOf.Vector2 + sizeof(int);
        PacketLength[147] = 3 + sizeof(byte) * 2 + sizeof(ushort);
        PacketLength[148] = (ushort)(PacketLength[21] + sizeof(byte));
    }
//#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
#pragma warning disable IDE1006 // 命名样式
    public static void orig_SendData(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText? text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
    {
        if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
        {
            msgType = 145;
        }
        int num = 256;
        text ??= NetworkText.Empty;
        if (remoteClient >= 0)
        {
            num = remoteClient;
        }
        using IPacketWriter packetWriter = PacketLength[msgType] != 0 ? new FixedLengthPacketWriter(PacketLength[msgType]) : new TerrariaPacketWriter(new MemoryStream());
        //using IPacketWriter packetWriter = new TerrariaPacketWriter(new MemoryStream());
        packetWriter.Position = 2L;
        packetWriter.Write((byte)msgType); 
        switch (msgType)
        {
            case 1:
                packetWriter.Write("Terraria" + 279);
                break;
            case 2:
                text.Serialize((BinaryWriter)packetWriter);
                if (Main.dedServ)
                {
                    Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", Netplay.Clients[num].Socket.GetRemoteAddress().ToString(), text));
                }
                break;
            case 3:
                packetWriter.Write((byte)remoteClient);
                packetWriter.Write(value: false);
                break;
            case 4:
                {
                    Player player4 = Main.player[number];
                    packetWriter.Write((byte)number);
                    packetWriter.Write((byte)player4.skinVariant);
                    packetWriter.Write((byte)player4.hair);
                    packetWriter.Write(player4.name);
                    packetWriter.Write(player4.hairDye);
                    NetMessageUtils.WriteAccessoryVisibility(packetWriter, player4.hideVisibleAccessory);
                    packetWriter.Write(player4.hideMisc);
                    packetWriter.WriteRGB(player4.hairColor);
                    packetWriter.WriteRGB(player4.skinColor);
                    packetWriter.WriteRGB(player4.eyeColor);
                    packetWriter.WriteRGB(player4.shirtColor);
                    packetWriter.WriteRGB(player4.underShirtColor);
                    packetWriter.WriteRGB(player4.pantsColor);
                    packetWriter.WriteRGB(player4.shoeColor);
                    BitsByte bitsByte16 = (byte)0;
                    if (player4.difficulty == 1)
                    {
                        bitsByte16[0] = true;
                    }
                    else if (player4.difficulty == 2)
                    {
                        bitsByte16[1] = true;
                    }
                    else if (player4.difficulty == 3)
                    {
                        bitsByte16[3] = true;
                    }
                    bitsByte16[2] = player4.extraAccessory;
                    packetWriter.Write(bitsByte16);
                    BitsByte bitsByte17 = (byte)0;
                    bitsByte17[0] = player4.UsingBiomeTorches;
                    bitsByte17[1] = player4.happyFunTorchTime;
                    bitsByte17[2] = player4.unlockedBiomeTorches;
                    bitsByte17[3] = player4.unlockedSuperCart;
                    bitsByte17[4] = player4.enabledSuperCart;
                    packetWriter.Write(bitsByte17);
                    BitsByte bitsByte18 = (byte)0;
                    bitsByte18[0] = player4.usedAegisCrystal;
                    bitsByte18[1] = player4.usedAegisFruit;
                    bitsByte18[2] = player4.usedArcaneCrystal;
                    bitsByte18[3] = player4.usedGalaxyPearl;
                    bitsByte18[4] = player4.usedGummyWorm;
                    bitsByte18[5] = player4.usedAmbrosia;
                    bitsByte18[6] = player4.ateArtisanBread;
                    packetWriter.Write(bitsByte18);
                    break;
                }
            case 5:
                {
                    packetWriter.Write((byte)number);
                    packetWriter.Write((short)number2);
                    Player player5 = Main.player[number];
                    int num11 = 0;
                    int num12 = 0;
                    Item item6 = ((number2 >= (float)PlayerItemSlotID.Loadout3_Dye_0) ? player5.Loadouts[2].Dye[(int)number2 - PlayerItemSlotID.Loadout3_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout3_Armor_0) ? player5.Loadouts[2].Armor[(int)number2 - PlayerItemSlotID.Loadout3_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Loadout2_Dye_0) ? player5.Loadouts[1].Dye[(int)number2 - PlayerItemSlotID.Loadout2_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout2_Armor_0) ? player5.Loadouts[1].Armor[(int)number2 - PlayerItemSlotID.Loadout2_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Loadout1_Dye_0) ? player5.Loadouts[0].Dye[(int)number2 - PlayerItemSlotID.Loadout1_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout1_Armor_0) ? player5.Loadouts[0].Armor[(int)number2 - PlayerItemSlotID.Loadout1_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Bank4_0) ? player5.bank4.item[(int)number2 - PlayerItemSlotID.Bank4_0] : ((number2 >= (float)PlayerItemSlotID.Bank3_0) ? player5.bank3.item[(int)number2 - PlayerItemSlotID.Bank3_0] : ((number2 >= (float)PlayerItemSlotID.TrashItem) ? player5.trashItem : ((number2 >= (float)PlayerItemSlotID.Bank2_0) ? player5.bank2.item[(int)number2 - PlayerItemSlotID.Bank2_0] : ((number2 >= (float)PlayerItemSlotID.Bank1_0) ? player5.bank.item[(int)number2 - PlayerItemSlotID.Bank1_0] : ((number2 >= (float)PlayerItemSlotID.MiscDye0) ? player5.miscDyes[(int)number2 - PlayerItemSlotID.MiscDye0] : ((number2 >= (float)PlayerItemSlotID.Misc0) ? player5.miscEquips[(int)number2 - PlayerItemSlotID.Misc0] : ((number2 >= (float)PlayerItemSlotID.Dye0) ? player5.dye[(int)number2 - PlayerItemSlotID.Dye0] : ((!(number2 >= (float)PlayerItemSlotID.Armor0)) ? player5.inventory[(int)number2 - PlayerItemSlotID.Inventory0] : player5.armor[(int)number2 - PlayerItemSlotID.Armor0])))))))))))))));
                    if (item6.Name == "" || item6.stack == 0 || item6.type == 0)
                    {
                        item6.SetDefaults(0, noMatCheck: true);
                    }
                    num11 = item6.stack;
                    num12 = item6.netID;
                    if (num11 < 0)
                    {
                        num11 = 0;
                    }
                    packetWriter.Write((short)num11);
                    packetWriter.Write((byte)number3);
                    packetWriter.Write((short)num12);
                    break;
                }
            case 7:
                {
                    packetWriter.Write((int)Main.time);
                    BitsByte bitsByte5 = (byte)0;
                    bitsByte5[0] = Main.dayTime;
                    bitsByte5[1] = Main.bloodMoon;
                    bitsByte5[2] = Main.eclipse;
                    packetWriter.Write(bitsByte5);
                    packetWriter.Write((byte)Main.moonPhase);
                    packetWriter.Write((short)Main.maxTilesX);
                    packetWriter.Write((short)Main.maxTilesY);
                    packetWriter.Write((short)Main.spawnTileX);
                    packetWriter.Write((short)Main.spawnTileY);
                    packetWriter.Write((short)Main.worldSurface);
                    packetWriter.Write((short)Main.rockLayer);
                    packetWriter.Write(Main.worldID);
                    packetWriter.Write(Main.worldName);
                    packetWriter.Write((byte)Main.GameMode);
                    packetWriter.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
                    packetWriter.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
                    packetWriter.Write((byte)Main.moonType);
                    packetWriter.Write((byte)WorldGen.treeBG1);
                    packetWriter.Write((byte)WorldGen.treeBG2);
                    packetWriter.Write((byte)WorldGen.treeBG3);
                    packetWriter.Write((byte)WorldGen.treeBG4);
                    packetWriter.Write((byte)WorldGen.corruptBG);
                    packetWriter.Write((byte)WorldGen.jungleBG);
                    packetWriter.Write((byte)WorldGen.snowBG);
                    packetWriter.Write((byte)WorldGen.hallowBG);
                    packetWriter.Write((byte)WorldGen.crimsonBG);
                    packetWriter.Write((byte)WorldGen.desertBG);
                    packetWriter.Write((byte)WorldGen.oceanBG);
                    packetWriter.Write((byte)WorldGen.mushroomBG);
                    packetWriter.Write((byte)WorldGen.underworldBG);
                    packetWriter.Write((byte)Main.iceBackStyle);
                    packetWriter.Write((byte)Main.jungleBackStyle);
                    packetWriter.Write((byte)Main.hellBackStyle);
                    packetWriter.Write(Main.windSpeedTarget);
                    packetWriter.Write((byte)Main.numClouds);
                    for (int n = 0; n < 3; n++)
                    {
                        packetWriter.Write(Main.treeX[n]);
                    }
                    for (int num8 = 0; num8 < 4; num8++)
                    {
                        packetWriter.Write((byte)Main.treeStyle[num8]);
                    }
                    for (int num9 = 0; num9 < 3; num9++)
                    {
                        packetWriter.Write(Main.caveBackX[num9]);
                    }
                    for (int num10 = 0; num10 < 4; num10++)
                    {
                        packetWriter.Write((byte)Main.caveBackStyle[num10]);
                    }
                    WorldGen.TreeTops.SyncSend((BinaryWriter)packetWriter);
                    if (!Main.raining)
                    {
                        Main.maxRaining = 0f;
                    }
                    packetWriter.Write(Main.maxRaining);
                    BitsByte bitsByte6 = (byte)0;
                    bitsByte6[0] = WorldGen.shadowOrbSmashed;
                    bitsByte6[1] = NPC.downedBoss1;
                    bitsByte6[2] = NPC.downedBoss2;
                    bitsByte6[3] = NPC.downedBoss3;
                    bitsByte6[4] = Main.hardMode;
                    bitsByte6[5] = NPC.downedClown;
                    bitsByte6[6] = Main.ServerSideCharacter;
                    bitsByte6[7] = NPC.downedPlantBoss;
                    packetWriter.Write(bitsByte6);
                    BitsByte bitsByte7 = (byte)0;
                    bitsByte7[0] = NPC.downedMechBoss1;
                    bitsByte7[1] = NPC.downedMechBoss2;
                    bitsByte7[2] = NPC.downedMechBoss3;
                    bitsByte7[3] = NPC.downedMechBossAny;
                    bitsByte7[4] = Main.cloudBGActive >= 1f;
                    bitsByte7[5] = WorldGen.crimson;
                    bitsByte7[6] = Main.pumpkinMoon;
                    bitsByte7[7] = Main.snowMoon;
                    packetWriter.Write(bitsByte7);
                    BitsByte bitsByte8 = (byte)0;
                    bitsByte8[1] = Main.fastForwardTimeToDawn;
                    bitsByte8[2] = Main.slimeRain;
                    bitsByte8[3] = NPC.downedSlimeKing;
                    bitsByte8[4] = NPC.downedQueenBee;
                    bitsByte8[5] = NPC.downedFishron;
                    bitsByte8[6] = NPC.downedMartians;
                    bitsByte8[7] = NPC.downedAncientCultist;
                    packetWriter.Write(bitsByte8);
                    BitsByte bitsByte9 = (byte)0;
                    bitsByte9[0] = NPC.downedMoonlord;
                    bitsByte9[1] = NPC.downedHalloweenKing;
                    bitsByte9[2] = NPC.downedHalloweenTree;
                    bitsByte9[3] = NPC.downedChristmasIceQueen;
                    bitsByte9[4] = NPC.downedChristmasSantank;
                    bitsByte9[5] = NPC.downedChristmasTree;
                    bitsByte9[6] = NPC.downedGolemBoss;
                    bitsByte9[7] = BirthdayParty.PartyIsUp;
                    packetWriter.Write(bitsByte9);
                    BitsByte bitsByte10 = (byte)0;
                    bitsByte10[0] = NPC.downedPirates;
                    bitsByte10[1] = NPC.downedFrost;
                    bitsByte10[2] = NPC.downedGoblins;
                    bitsByte10[3] = Sandstorm.Happening;
                    bitsByte10[4] = DD2Event.Ongoing;
                    bitsByte10[5] = DD2Event.DownedInvasionT1;
                    bitsByte10[6] = DD2Event.DownedInvasionT2;
                    bitsByte10[7] = DD2Event.DownedInvasionT3;
                    packetWriter.Write(bitsByte10);
                    BitsByte bitsByte11 = (byte)0;
                    bitsByte11[0] = NPC.combatBookWasUsed;
                    bitsByte11[1] = LanternNight.LanternsUp;
                    bitsByte11[2] = NPC.downedTowerSolar;
                    bitsByte11[3] = NPC.downedTowerVortex;
                    bitsByte11[4] = NPC.downedTowerNebula;
                    bitsByte11[5] = NPC.downedTowerStardust;
                    bitsByte11[6] = Main.forceHalloweenForToday;
                    bitsByte11[7] = Main.forceXMasForToday;
                    packetWriter.Write(bitsByte11);
                    BitsByte bitsByte12 = (byte)0;
                    bitsByte12[0] = NPC.boughtCat;
                    bitsByte12[1] = NPC.boughtDog;
                    bitsByte12[2] = NPC.boughtBunny;
                    bitsByte12[3] = NPC.freeCake;
                    bitsByte12[4] = Main.drunkWorld;
                    bitsByte12[5] = NPC.downedEmpressOfLight;
                    bitsByte12[6] = NPC.downedQueenSlime;
                    bitsByte12[7] = Main.getGoodWorld;
                    packetWriter.Write(bitsByte12);
                    BitsByte bitsByte13 = (byte)0;
                    bitsByte13[0] = Main.tenthAnniversaryWorld;
                    bitsByte13[1] = Main.dontStarveWorld;
                    bitsByte13[2] = NPC.downedDeerclops;
                    bitsByte13[3] = Main.notTheBeesWorld;
                    bitsByte13[4] = Main.remixWorld;
                    bitsByte13[5] = NPC.unlockedSlimeBlueSpawn;
                    bitsByte13[6] = NPC.combatBookVolumeTwoWasUsed;
                    bitsByte13[7] = NPC.peddlersSatchelWasUsed;
                    packetWriter.Write(bitsByte13);
                    BitsByte bitsByte14 = (byte)0;
                    bitsByte14[0] = NPC.unlockedSlimeGreenSpawn;
                    bitsByte14[1] = NPC.unlockedSlimeOldSpawn;
                    bitsByte14[2] = NPC.unlockedSlimePurpleSpawn;
                    bitsByte14[3] = NPC.unlockedSlimeRainbowSpawn;
                    bitsByte14[4] = NPC.unlockedSlimeRedSpawn;
                    bitsByte14[5] = NPC.unlockedSlimeYellowSpawn;
                    bitsByte14[6] = NPC.unlockedSlimeCopperSpawn;
                    bitsByte14[7] = Main.fastForwardTimeToDusk;
                    packetWriter.Write(bitsByte14);
                    BitsByte bitsByte15 = (byte)0;
                    bitsByte15[0] = Main.noTrapsWorld;
                    bitsByte15[1] = Main.zenithWorld;
                    bitsByte15[2] = NPC.unlockedTruffleSpawn;
                    packetWriter.Write(bitsByte15);
                    packetWriter.Write((byte)Main.sundialCooldown);
                    packetWriter.Write((byte)Main.moondialCooldown);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Copper);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Iron);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Silver);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Gold);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Cobalt);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Mythril);
                    packetWriter.Write((short)WorldGen.SavedOreTiers.Adamantite);
                    packetWriter.Write((sbyte)Main.invasionType);
                    if (SocialAPI.Network != null)
                    {
                        packetWriter.Write(SocialAPI.Network.GetLobbyId());
                    }
                    else
                    {
                        packetWriter.Write(0uL);
                    }
                    packetWriter.Write(Sandstorm.IntendedSeverity);
                    break;
                }
            case 8:
                packetWriter.Write(number);
                packetWriter.Write((int)number2);
                break;
            case 9:
                {
                    packetWriter.Write(number);
                    text.Serialize((BinaryWriter)packetWriter);
                    BitsByte bitsByte22 = (byte)number2;
                    packetWriter.Write(bitsByte22);
                    break;
                }
            case 10:
                NetMessage.CompressTileBlock(number, (int)number2, (short)number3, (short)number4, ((BinaryWriter)packetWriter).BaseStream);
                break;
            case 11:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                break;
            case 12:
                {
                    Player player6 = Main.player[number];
                    packetWriter.Write((byte)number);
                    packetWriter.Write((short)player6.SpawnX);
                    packetWriter.Write((short)player6.SpawnY);
                    packetWriter.Write(player6.respawnTimer);
                    packetWriter.Write((short)player6.numberOfDeathsPVE);
                    packetWriter.Write((short)player6.numberOfDeathsPVP);
                    packetWriter.Write((byte)number2);
                    break;
                }
            case 13:
                {
                    Player player7 = Main.player[number];
                    packetWriter.Write((byte)number);
                    BitsByte bitsByte25 = (byte)0;
                    bitsByte25[0] = player7.controlUp;
                    bitsByte25[1] = player7.controlDown;
                    bitsByte25[2] = player7.controlLeft;
                    bitsByte25[3] = player7.controlRight;
                    bitsByte25[4] = player7.controlJump;
                    bitsByte25[5] = player7.controlUseItem;
                    bitsByte25[6] = player7.direction == 1;
                    packetWriter.Write(bitsByte25);
                    BitsByte bitsByte26 = (byte)0;
                    bitsByte26[0] = player7.pulley;
                    bitsByte26[1] = player7.pulley && player7.pulleyDir == 2;
                    bitsByte26[2] = player7.velocity != Vector2.Zero;
                    bitsByte26[3] = player7.vortexStealthActive;
                    bitsByte26[4] = player7.gravDir == 1f;
                    bitsByte26[5] = player7.shieldRaised;
                    bitsByte26[6] = player7.ghost;
                    packetWriter.Write(bitsByte26);
                    BitsByte bitsByte27 = (byte)0;
                    bitsByte27[0] = player7.tryKeepingHoveringUp;
                    bitsByte27[1] = player7.IsVoidVaultEnabled;
                    bitsByte27[2] = player7.sitting.isSitting;
                    bitsByte27[3] = player7.downedDD2EventAnyDifficulty;
                    bitsByte27[4] = player7.isPettingAnimal;
                    bitsByte27[5] = player7.isTheAnimalBeingPetSmall;
                    bitsByte27[6] = player7.PotionOfReturnOriginalUsePosition.HasValue;
                    bitsByte27[7] = player7.tryKeepingHoveringDown;
                    packetWriter.Write(bitsByte27);
                    BitsByte bitsByte28 = (byte)0;
                    bitsByte28[0] = player7.sleeping.isSleeping;
                    bitsByte28[1] = player7.autoReuseAllWeapons;
                    bitsByte28[2] = player7.controlDownHold;
                    bitsByte28[3] = player7.isOperatingAnotherEntity;
                    bitsByte28[4] = player7.controlUseTile;
                    packetWriter.Write(bitsByte28);
                    packetWriter.Write((byte)player7.selectedItem);
                    packetWriter.WriteVector2(player7.position);
                    if (bitsByte26[2])
                    {
                        packetWriter.WriteVector2(player7.velocity);
                    }
                    if (bitsByte27[6])
                    {
                        packetWriter.WriteVector2(player7.PotionOfReturnOriginalUsePosition!.Value);
                        packetWriter.WriteVector2(player7.PotionOfReturnHomePosition!.Value);
                    }
                    break;
                }
            case 14:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                break;
            case 16:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)Main.player[number].statLife);
                packetWriter.Write((short)Main.player[number].statLifeMax);
                break;
            case 17:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                packetWriter.Write((byte)number5);
                break;
            case 18:
                packetWriter.Write((byte)(Main.dayTime ? 1u : 0u));
                packetWriter.Write((int)Main.time);
                packetWriter.Write(Main.sunModY);
                packetWriter.Write(Main.moonModY);
                break;
            case 19:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((number4 == 1f) ? ((byte)1) : ((byte)0));
                break;
            case 20:
                {
                    int num13 = number;
                    int num14 = (int)number2;
                    int num15 = (int)number3;
                    if (num15 < 0)
                    {
                        num15 = 0;
                    }
                    int num16 = (int)number4;
                    if (num16 < 0)
                    {
                        num16 = 0;
                    }
                    if (num13 < num15)
                    {
                        num13 = num15;
                    }
                    if (num13 >= Main.maxTilesX + num15)
                    {
                        num13 = Main.maxTilesX - num15 - 1;
                    }
                    if (num14 < num16)
                    {
                        num14 = num16;
                    }
                    if (num14 >= Main.maxTilesY + num16)
                    {
                        num14 = Main.maxTilesY - num16 - 1;
                    }
                    packetWriter.Write((short)num13);
                    packetWriter.Write((short)num14);
                    packetWriter.Write((byte)num15);
                    packetWriter.Write((byte)num16);
                    packetWriter.Write((byte)number5);
                    for (int num17 = num13; num17 < num13 + num15; num17++)
                    {
                        for (int num18 = num14; num18 < num14 + num16; num18++)
                        {
                            BitsByte bitsByte19 = (byte)0;
                            BitsByte bitsByte20 = (byte)0;
                            BitsByte bitsByte21 = (byte)0;
                            byte b3 = 0;
                            byte b4 = 0;
                            ITile tile2 = Main.tile[num17, num18];
                            bitsByte19[0] = tile2.active();
                            bitsByte19[2] = tile2.wall > 0;
                            bitsByte19[3] = tile2.liquid > 0 && Main.netMode == 2;
                            bitsByte19[4] = tile2.wire();
                            bitsByte19[5] = tile2.halfBrick();
                            bitsByte19[6] = tile2.actuator();
                            bitsByte19[7] = tile2.inActive();
                            bitsByte20[0] = tile2.wire2();
                            bitsByte20[1] = tile2.wire3();
                            if (tile2.active() && tile2.color() > 0)
                            {
                                bitsByte20[2] = true;
                                b3 = tile2.color();
                            }
                            if (tile2.wall > 0 && tile2.wallColor() > 0)
                            {
                                bitsByte20[3] = true;
                                b4 = tile2.wallColor();
                            }
                            bitsByte20 = (byte)((byte)bitsByte20 + (byte)(tile2.slope() << 4));
                            bitsByte20[7] = tile2.wire4();
                            bitsByte21[0] = tile2.fullbrightBlock();
                            bitsByte21[1] = tile2.fullbrightWall();
                            bitsByte21[2] = tile2.invisibleBlock();
                            bitsByte21[3] = tile2.invisibleWall();
                            packetWriter.Write(bitsByte19);
                            packetWriter.Write(bitsByte20);
                            packetWriter.Write(bitsByte21);
                            if (b3 > 0)
                            {
                                packetWriter.Write(b3);
                            }
                            if (b4 > 0)
                            {
                                packetWriter.Write(b4);
                            }
                            if (tile2.active())
                            {
                                packetWriter.Write(tile2.type);
                                if (Main.tileFrameImportant[tile2.type])
                                {
                                    packetWriter.Write(tile2.frameX);
                                    packetWriter.Write(tile2.frameY);
                                }
                            }
                            if (tile2.wall > 0)
                            {
                                packetWriter.Write(tile2.wall);
                            }
                            if (tile2.liquid > 0 && Main.netMode == 2)
                            {
                                packetWriter.Write(tile2.liquid);
                                packetWriter.Write(tile2.liquidType());
                            }
                        }
                    }
                    break;
                }
            case 21:
            case 90:
            case 145:
            case 148:
                {
                    Item item3 = Main.item[number];
                    packetWriter.Write((short)number);
                    packetWriter.WriteVector2(item3.position);
                    packetWriter.WriteVector2(item3.velocity);
                    packetWriter.Write((short)item3.stack);
                    packetWriter.Write(item3.prefix);
                    packetWriter.Write((byte)number2);
                    short value2 = 0;
                    if (item3.active && item3.stack > 0)
                    {
                        value2 = (short)item3.netID;
                    }
                    packetWriter.Write(value2);
                    if (msgType == 145)
                    {
                        packetWriter.Write(item3.shimmered);
                        packetWriter.Write(item3.shimmerTime);
                    }
                    if (msgType == 148)
                    {
                        packetWriter.Write((byte)MathHelper.Clamp(item3.timeLeftInWhichTheItemCannotBeTakenByEnemies, 0f, 255f));
                    }
                    break;
                }
            case 22:
                packetWriter.Write((short)number);
                packetWriter.Write((byte)Main.item[number].playerIndexTheItemIsReservedFor);
                break;
            case 23:
                {
                    NPC nPC2 = Main.npc[number];
                    packetWriter.Write((short)number);
                    packetWriter.WriteVector2(nPC2.position);
                    packetWriter.WriteVector2(nPC2.velocity);
                    packetWriter.Write((ushort)nPC2.target);
                    int num4 = nPC2.life;
                    if (!nPC2.active)
                    {
                        num4 = 0;
                    }
                    if (!nPC2.active || nPC2.life <= 0)
                    {
                        nPC2.netSkip = 0;
                    }
                    short value3 = (short)nPC2.netID;
                    bool[] array = new bool[4];
                    BitsByte bitsByte3 = (byte)0;
                    bitsByte3[0] = nPC2.direction > 0;
                    bitsByte3[1] = nPC2.directionY > 0;
                    bitsByte3[2] = (array[0] = nPC2.ai[0] != 0f);
                    bitsByte3[3] = (array[1] = nPC2.ai[1] != 0f);
                    bitsByte3[4] = (array[2] = nPC2.ai[2] != 0f);
                    bitsByte3[5] = (array[3] = nPC2.ai[3] != 0f);
                    bitsByte3[6] = nPC2.spriteDirection > 0;
                    bitsByte3[7] = num4 == nPC2.lifeMax;
                    packetWriter.Write(bitsByte3);
                    BitsByte bitsByte4 = (byte)0;
                    bitsByte4[0] = nPC2.statsAreScaledForThisManyPlayers > 1;
                    bitsByte4[1] = nPC2.SpawnedFromStatue;
                    bitsByte4[2] = nPC2.strengthMultiplier != 1f;
                    packetWriter.Write(bitsByte4);
                    for (int m = 0; m < NPC.maxAI; m++)
                    {
                        if (array[m])
                        {
                            packetWriter.Write(nPC2.ai[m]);
                        }
                    }
                    packetWriter.Write(value3);
                    if (bitsByte4[0])
                    {
                        packetWriter.Write((byte)nPC2.statsAreScaledForThisManyPlayers);
                    }
                    if (bitsByte4[2])
                    {
                        packetWriter.Write(nPC2.strengthMultiplier);
                    }
                    if (!bitsByte3[7])
                    {
                        byte b2 = 1;
                        if (nPC2.lifeMax > 32767)
                        {
                            b2 = 4;
                        }
                        else if (nPC2.lifeMax > 127)
                        {
                            b2 = 2;
                        }
                        packetWriter.Write(b2);
                        switch (b2)
                        {
                            case 2:
                                packetWriter.Write((short)num4);
                                break;
                            case 4:
                                packetWriter.Write(num4);
                                break;
                            default:
                                packetWriter.Write((sbyte)num4);
                                break;
                        }
                    }
                    if (nPC2.type >= 0 && nPC2.type < NPCID.Count && Main.npcCatchable[nPC2.type])
                    {
                        packetWriter.Write((byte)nPC2.releaseOwner);
                    }
                    break;
                }
            case 24:
                packetWriter.Write((short)number);
                packetWriter.Write((byte)number2);
                break;
            case 107:
                packetWriter.Write((byte)number2);
                packetWriter.Write((byte)number3);
                packetWriter.Write((byte)number4);
                text.Serialize((BinaryWriter)packetWriter);
                packetWriter.Write((short)number5);
                break;
            case 27:
                {
                    Projectile projectile = Main.projectile[number];
                    packetWriter.Write((short)projectile.identity);
                    packetWriter.WriteVector2(projectile.position);
                    packetWriter.WriteVector2(projectile.velocity);
                    packetWriter.Write((byte)projectile.owner);
                    packetWriter.Write((short)projectile.type);
                    BitsByte bitsByte23 = (byte)0;
                    BitsByte bitsByte24 = (byte)0;
                    bitsByte23[0] = projectile.ai[0] != 0f;
                    bitsByte23[1] = projectile.ai[1] != 0f;
                    bitsByte24[0] = projectile.ai[2] != 0f;
                    if (projectile.bannerIdToRespondTo != 0)
                    {
                        bitsByte23[3] = true;
                    }
                    if (projectile.damage != 0)
                    {
                        bitsByte23[4] = true;
                    }
                    if (projectile.knockBack != 0f)
                    {
                        bitsByte23[5] = true;
                    }
                    if (projectile.type > 0 && projectile.type < ProjectileID.Count && ProjectileID.Sets.NeedsUUID[projectile.type])
                    {
                        bitsByte23[7] = true;
                    }
                    if (projectile.originalDamage != 0)
                    {
                        bitsByte23[6] = true;
                    }
                    if ((byte)bitsByte24 != 0)
                    {
                        bitsByte23[2] = true;
                    }
                    packetWriter.Write(bitsByte23);
                    if (bitsByte23[2])
                    {
                        packetWriter.Write(bitsByte24);
                    }
                    if (bitsByte23[0])
                    {
                        packetWriter.Write(projectile.ai[0]);
                    }
                    if (bitsByte23[1])
                    {
                        packetWriter.Write(projectile.ai[1]);
                    }
                    if (bitsByte23[3])
                    {
                        packetWriter.Write((ushort)projectile.bannerIdToRespondTo);
                    }
                    if (bitsByte23[4])
                    {
                        packetWriter.Write((short)projectile.damage);
                    }
                    if (bitsByte23[5])
                    {
                        packetWriter.Write(projectile.knockBack);
                    }
                    if (bitsByte23[6])
                    {
                        packetWriter.Write((short)projectile.originalDamage);
                    }
                    if (bitsByte23[7])
                    {
                        packetWriter.Write((short)projectile.projUUID);
                    }
                    if (bitsByte24[0])
                    {
                        packetWriter.Write(projectile.ai[2]);
                    }
                    break;
                }
            case 28:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write(number3);
                packetWriter.Write((byte)(number4 + 1f));
                packetWriter.Write((byte)number5);
                break;
            case 29:
                packetWriter.Write((short)number);
                packetWriter.Write((byte)number2);
                break;
            case 30:
                packetWriter.Write((byte)number);
                packetWriter.Write(Main.player[number].hostile);
                break;
            case 31:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                break;
            case 32:
                {
                    Item item7 = Main.chest[number].item[(byte)number2];
                    packetWriter.Write((short)number);
                    packetWriter.Write((byte)number2);
                    short value4 = (short)item7.netID;
                    if (item7.Name == null)
                    {
                        value4 = 0;
                    }
                    packetWriter.Write((short)item7.stack);
                    packetWriter.Write(item7.prefix);
                    packetWriter.Write(value4);
                    break;
                }
            case 33:
                {
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    string? text2 = null;
                    if (number > -1)
                    {
                        num5 = Main.chest[number].x;
                        num6 = Main.chest[number].y;
                    }
                    if (number2 == 1f)
                    {
                        string text3 = text.ToString();
                        num7 = (byte)text3.Length;
                        if (num7 == 0 || num7 > 20)
                        {
                            num7 = 255;
                        }
                        else
                        {
                            text2 = text3;
                        }
                    }
                    packetWriter.Write((short)number);
                    packetWriter.Write((short)num5);
                    packetWriter.Write((short)num6);
                    packetWriter.Write((byte)num7);
                    if (text2 != null)
                    {
                        packetWriter.Write(text2);
                    }
                    break;
                }
            case 34:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                if (Main.netMode == 2)
                {
                    Netplay.GetSectionX((int)number2);
                    Netplay.GetSectionY((int)number3);
                    packetWriter.Write((short)number5);
                }
                else
                {
                    packetWriter.Write((short)0);
                }
                break;
            case 35:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                break;
            case 36:
                {
                    Player player3 = Main.player[number];
                    packetWriter.Write((byte)number);
                    packetWriter.Write(player3.zone1);
                    packetWriter.Write(player3.zone2);
                    packetWriter.Write(player3.zone3);
                    packetWriter.Write(player3.zone4);
                    packetWriter.Write(player3.zone5);
                    break;
                }
            case 38:
                packetWriter.Write(Netplay.ServerPassword);
                break;
            case 39:
                packetWriter.Write((short)number);
                break;
            case 40:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)Main.player[number].talkNPC);
                break;
            case 41:
                packetWriter.Write((byte)number);
                packetWriter.Write(Main.player[number].itemRotation);
                packetWriter.Write((short)Main.player[number].itemAnimation);
                break;
            case 42:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)Main.player[number].statMana);
                packetWriter.Write((short)Main.player[number].statManaMax);
                break;
            case 43:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                break;
            case 45:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)Main.player[number].team);
                break;
            case 46:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                break;
            case 47:
                packetWriter.Write((short)number);
                packetWriter.Write((short)Main.sign[number].x);
                packetWriter.Write((short)Main.sign[number].y);
                packetWriter.Write(Main.sign[number].text);
                packetWriter.Write((byte)number2);
                packetWriter.Write((byte)number3);
                break;
            case 48:
                {
                    ITile tile = Main.tile[number, (int)number2];
                    packetWriter.Write((short)number);
                    packetWriter.Write((short)number2);
                    packetWriter.Write(tile.liquid);
                    packetWriter.Write(tile.liquidType());
                    break;
                }
            case 50:
                {
                    packetWriter.Write((byte)number);
                    for (int l = 0; l < Player.maxBuffs; l++)
                    {
                        packetWriter.Write((ushort)Main.player[number].buffType[l]);
                    }
                    break;
                }
            case 51:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                break;
            case 52:
                packetWriter.Write((byte)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                break;
            case 53:
                packetWriter.Write((short)number);
                packetWriter.Write((ushort)number2);
                packetWriter.Write((short)number3);
                break;
            case 54:
                {
                    packetWriter.Write((short)number);
                    for (int k = 0; k < NPC.maxBuffs; k++)
                    {
                        packetWriter.Write((ushort)Main.npc[number].buffType[k]);
                        packetWriter.Write((short)Main.npc[number].buffTime[k]);
                    }
                    break;
                }
            case 55:
                packetWriter.Write((byte)number);
                packetWriter.Write((ushort)number2);
                packetWriter.Write((int)number3);
                break;
            case 56:
                packetWriter.Write((short)number);
                if (Main.netMode == 2)
                {
                    string givenName = Main.npc[number].GivenName;
                    packetWriter.Write(givenName);
                    packetWriter.Write(Main.npc[number].townNpcVariationIndex);
                }
                break;
            case 57:
                packetWriter.Write(WorldGen.tGood);
                packetWriter.Write(WorldGen.tEvil);
                packetWriter.Write(WorldGen.tBlood);
                break;
            case 58:
                packetWriter.Write((byte)number);
                packetWriter.Write(number2);
                break;
            case 59:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                break;
            case 60:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((byte)number4);
                break;
            case 61:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                break;
            case 62:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                break;
            case 63:
            case 64:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((byte)number3);
                packetWriter.Write((byte)number4);
                break;
            case 65:
                {
                    BitsByte bitsByte29 = (byte)0;
                    bitsByte29[0] = (number & 1) == 1;
                    bitsByte29[1] = (number & 2) == 2;
                    bitsByte29[2] = number6 == 1;
                    bitsByte29[3] = number7 != 0;
                    packetWriter.Write(bitsByte29);
                    packetWriter.Write((short)number2);
                    packetWriter.Write(number3);
                    packetWriter.Write(number4);
                    packetWriter.Write((byte)number5);
                    if (bitsByte29[3])
                    {
                        packetWriter.Write(number7);
                    }
                    break;
                }
            case 66:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                break;
            case 68:
                packetWriter.Write(Main.clientUUID);
                break;
            case 69:
                Netplay.GetSectionX((int)number2);
                Netplay.GetSectionY((int)number3);
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write(Main.chest[(short)number].name);
                break;
            case 70:
                packetWriter.Write((short)number);
                packetWriter.Write((byte)number2);
                break;
            case 71:
                packetWriter.Write(number);
                packetWriter.Write((int)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((byte)number4);
                break;
            case 72:
                {
                    for (int num20 = 0; num20 < 40; num20++)
                    {
                        packetWriter.Write((short)Main.travelShop[num20]);
                    }
                    break;
                }
            case 73:
                packetWriter.Write((byte)number);
                break;
            case 74:
                {
                    packetWriter.Write((byte)Main.anglerQuest);
                    bool value7 = Main.anglerWhoFinishedToday.Contains(text.ToString());
                    packetWriter.Write(value7);
                    break;
                }
            case 76:
                packetWriter.Write((byte)number);
                packetWriter.Write(Main.player[number].anglerQuestsFinished);
                packetWriter.Write(Main.player[number].golferScoreAccumulated);
                break;
            case 77:
                packetWriter.Write((short)number);
                packetWriter.Write((ushort)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                break;
            case 78:
                packetWriter.Write(number);
                packetWriter.Write((int)number2);
                packetWriter.Write((sbyte)number3);
                packetWriter.Write((sbyte)number4);
                break;
            case 79:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                packetWriter.Write((byte)number5);
                packetWriter.Write((sbyte)number6);
                packetWriter.Write(number7 == 1);
                break;
            case 80:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                break;
            case 81:
                {
                    packetWriter.Write(number2);
                    packetWriter.Write(number3);
                    Color c2 = default;
                    c2.PackedValue = (uint)number;
                    packetWriter.WriteRGB(c2);
                    packetWriter.Write((int)number4);
                    break;
                }
            case 119:
                {
                    packetWriter.Write(number2);
                    packetWriter.Write(number3);
                    packetWriter.WriteRGB(new Color((uint)number));
                    text.Serialize((BinaryWriter)packetWriter);
                    break;
                }
            case 83:
                {
                    int num19 = number;
                    if (num19 < 0 && num19 >= 290)
                    {
                        num19 = 1;
                    }
                    int value6 = NPC.killCount[num19];
                    packetWriter.Write((short)num19);
                    packetWriter.Write(value6);
                    break;
                }
            case 84:
                {
                    byte b5 = (byte)number;
                    float stealth = Main.player[b5].stealth;
                    packetWriter.Write(b5);
                    packetWriter.Write(stealth);
                    break;
                }
            case 85:
                {
                    short value5 = (short)number;
                    packetWriter.Write(value5);
                    break;
                }
            case 86:
                {
                    packetWriter.Write(number);
                    bool flag3 = TileEntity.ByID.ContainsKey(number);
                    packetWriter.Write(flag3);
                    if (flag3)
                    {
                        TileEntity.Write((BinaryWriter)packetWriter, TileEntity.ByID[number], networkSend: true);
                    }
                    break;
                }
            case 87:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((byte)number3);
                break;
            case 88:
                {
                    BitsByte bitsByte = (byte)number2;
                    BitsByte bitsByte2 = (byte)number3;
                    packetWriter.Write((short)number);
                    packetWriter.Write(bitsByte);
                    Item item5 = Main.item[number];
                    if (bitsByte[0])
                    {
                        packetWriter.Write(item5.color.PackedValue);
                    }
                    if (bitsByte[1])
                    {
                        packetWriter.Write((ushort)item5.damage);
                    }
                    if (bitsByte[2])
                    {
                        packetWriter.Write(item5.knockBack);
                    }
                    if (bitsByte[3])
                    {
                        packetWriter.Write((ushort)item5.useAnimation);
                    }
                    if (bitsByte[4])
                    {
                        packetWriter.Write((ushort)item5.useTime);
                    }
                    if (bitsByte[5])
                    {
                        packetWriter.Write((short)item5.shoot);
                    }
                    if (bitsByte[6])
                    {
                        packetWriter.Write(item5.shootSpeed);
                    }
                    if (bitsByte[7])
                    {
                        packetWriter.Write(bitsByte2);
                        if (bitsByte2[0])
                        {
                            packetWriter.Write((ushort)item5.width);
                        }
                        if (bitsByte2[1])
                        {
                            packetWriter.Write((ushort)item5.height);
                        }
                        if (bitsByte2[2])
                        {
                            packetWriter.Write(item5.scale);
                        }
                        if (bitsByte2[3])
                        {
                            packetWriter.Write((short)item5.ammo);
                        }
                        if (bitsByte2[4])
                        {
                            packetWriter.Write((short)item5.useAmmo);
                        }
                        if (bitsByte2[5])
                        {
                            packetWriter.Write(item5.notAmmo);
                        }
                    }
                    break;
                }
            case 89:
                {
                    packetWriter.Write((short)number);
                    packetWriter.Write((short)number2);
                    Item item4 = Main.player[(int)number4].inventory[(int)number3];
                    packetWriter.Write((short)item4.netID);
                    packetWriter.Write(item4.prefix);
                    packetWriter.Write((short)number5);
                    break;
                }
            case 91:
                packetWriter.Write(number);
                packetWriter.Write((byte)number2);
                if (number2 != 255f)
                {
                    packetWriter.Write((ushort)number3);
                    packetWriter.Write((ushort)number4);
                    packetWriter.Write((byte)number5);
                    if (number5 < 0)
                    {
                        packetWriter.Write((short)number6);
                    }
                }
                break;
            case 92:
                packetWriter.Write((short)number);
                packetWriter.Write((int)number2);
                packetWriter.Write(number3);
                packetWriter.Write(number4);
                break;
            case 95:
                packetWriter.Write((ushort)number);
                packetWriter.Write((byte)number2);
                break;
            case 96:
                {
                    packetWriter.Write((byte)number);
                    Player player2 = Main.player[number];
                    packetWriter.Write((short)number4);
                    packetWriter.Write(number2);
                    packetWriter.Write(number3);
                    packetWriter.WriteVector2(player2.velocity);
                    break;
                }
            case 97:
                packetWriter.Write((short)number);
                break;
            case 98:
                packetWriter.Write((short)number);
                break;
            case 99:
                packetWriter.Write((byte)number);
                packetWriter.WriteVector2(Main.player[number].MinionRestTargetPoint);
                break;
            case 115:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)Main.player[number].MinionAttackTargetNPC);
                break;
            case 100:
                {
                    packetWriter.Write((ushort)number);
                    NPC nPC = Main.npc[number];
                    packetWriter.Write((short)number4);
                    packetWriter.Write(number2);
                    packetWriter.Write(number3);
                    packetWriter.WriteVector2(nPC.velocity);
                    break;
                }
            case 101:
                packetWriter.Write((ushort)NPC.ShieldStrengthTowerSolar);
                packetWriter.Write((ushort)NPC.ShieldStrengthTowerVortex);
                packetWriter.Write((ushort)NPC.ShieldStrengthTowerNebula);
                packetWriter.Write((ushort)NPC.ShieldStrengthTowerStardust);
                break;
            case 102:
                packetWriter.Write((byte)number);
                packetWriter.Write((ushort)number2);
                packetWriter.Write(number3);
                packetWriter.Write(number4);
                break;
            case 103:
                packetWriter.Write(NPC.MaxMoonLordCountdown);
                packetWriter.Write(NPC.MoonLordCountdown);
                break;
            case 104:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                packetWriter.Write(((short)number3 < 0) ? 0f : number3);
                packetWriter.Write((byte)number4);
                packetWriter.Write(number5);
                packetWriter.Write((byte)number6);
                break;
            case 105:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write(number3 == 1f);
                break;
            case 106:
                packetWriter.Write(new HalfVector2(number, number2).PackedValue);
                break;
            case 108:
                packetWriter.Write((short)number);
                packetWriter.Write(number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                packetWriter.Write((short)number5);
                packetWriter.Write((short)number6);
                packetWriter.Write((byte)number7);
                break;
            case 109:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((short)number4);
                packetWriter.Write((byte)number5);
                break;
            case 110:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((byte)number3);
                break;
            case 112:
                packetWriter.Write((byte)number);
                packetWriter.Write((int)number2);
                packetWriter.Write((int)number3);
                packetWriter.Write((byte)number4);
                packetWriter.Write((short)number5);
                break;
            case 113:
                packetWriter.Write((short)number);
                packetWriter.Write((short)number2);
                break;
            case 116:
                packetWriter.Write(number);
                break;
            case 117:
                packetWriter.Write((byte)number);
                NetMessage._currentPlayerDeathReason.WriteSelfTo((BinaryWriter)packetWriter);
                packetWriter.Write((short)number2);
                packetWriter.Write((byte)(number3 + 1f));
                packetWriter.Write((byte)number4);
                packetWriter.Write((sbyte)number5);
                break;
            case 118:
                packetWriter.Write((byte)number);
                NetMessage._currentPlayerDeathReason.WriteSelfTo((BinaryWriter)packetWriter);
                packetWriter.Write((short)number2);
                packetWriter.Write((byte)(number3 + 1f));
                packetWriter.Write((byte)number4);
                break;
            case 120:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                break;
            case 121:
                {
                    int num3 = (int)number3;
                    bool flag2 = number4 == 1f;
                    if (flag2)
                    {
                        num3 += 8;
                    }
                    packetWriter.Write((byte)number);
                    packetWriter.Write((int)number2);
                    packetWriter.Write((byte)num3);
                    if (TileEntity.ByID[(int)number2] is TEDisplayDoll tEDisplayDoll)
                    {
                        tEDisplayDoll.WriteItem((int)number3, packetWriter, flag2);
                        break;
                    }
                    packetWriter.Write(0);
                    packetWriter.Write((byte)0);
                    break;
                }
            case 122:
                packetWriter.Write(number);
                packetWriter.Write((byte)number2);
                break;
            case 123:
                {
                    packetWriter.Write((short)number);
                    packetWriter.Write((short)number2);
                    Item item2 = Main.player[(int)number4].inventory[(int)number3];
                    packetWriter.Write((short)item2.netID);
                    packetWriter.Write(item2.prefix);
                    packetWriter.Write((short)number5);
                    break;
                }
            case 124:
                {
                    int num2 = (int)number3;
                    bool flag = number4 == 1f;
                    if (flag)
                    {
                        num2 += 2;
                    }
                    packetWriter.Write((byte)number);
                    packetWriter.Write((int)number2);
                    packetWriter.Write((byte)num2);
                    if (TileEntity.ByID[(int)number2] is TEHatRack tEHatRack)
                    {
                        tEHatRack.WriteItem((int)number3, packetWriter, flag);
                        break;
                    }
                    packetWriter.Write(0);
                    packetWriter.Write((byte)0);
                    break;
                }
            case 125:
                packetWriter.Write((byte)number);
                packetWriter.Write((short)number2);
                packetWriter.Write((short)number3);
                packetWriter.Write((byte)number4);
                break;
            case 126:
                NetMessage._currentRevengeMarker.WriteSelfTo(packetWriter);
                break;
            case 127:
                packetWriter.Write(number);
                break;
            case 128:
                packetWriter.Write((byte)number);
                packetWriter.Write((ushort)number5);
                packetWriter.Write((ushort)number6);
                packetWriter.Write((ushort)number2);
                packetWriter.Write((ushort)number3);
                break;
            case 130:
                packetWriter.Write((ushort)number);
                packetWriter.Write((ushort)number2);
                packetWriter.Write((short)number3);
                break;
            case 131:
                {
                    packetWriter.Write((ushort)number);
                    packetWriter.Write((byte)number2);
                    byte b = (byte)number2;
                    if (b == 1)
                    {
                        packetWriter.Write((int)number3);
                        packetWriter.Write((short)number4);
                    }
                    break;
                }
            case 132:
                NetMessage._currentNetSoundInfo.WriteSelfTo(packetWriter);
                break;
            case 133:
                {
                    packetWriter.Write((short)number);
                    packetWriter.Write((short)number2);
                    Item item = Main.player[(int)number4].inventory[(int)number3];
                    packetWriter.Write((short)item.netID);
                    packetWriter.Write(item.prefix);
                    packetWriter.Write((short)number5);
                    break;
                }
            case 134:
                {
                    packetWriter.Write((byte)number);
                    Player player = Main.player[number];
                    packetWriter.Write(player.ladyBugLuckTimeLeft);
                    packetWriter.Write(player.torchLuck);
                    packetWriter.Write(player.luckPotion);
                    packetWriter.Write(player.HasGardenGnomeNearby);
                    packetWriter.Write(player.equipmentBasedLuckBonus);
                    packetWriter.Write(player.coinLuck);
                    break;
                }
            case 135:
                packetWriter.Write((byte)number);
                break;
            case 136:
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            packetWriter.Write((ushort)NPC.cavernMonsterType[i, j]);
                        }
                    }
                    break;
                }
            case 137:
                packetWriter.Write((short)number);
                packetWriter.Write((ushort)number2);
                break;
            case 139:
                {
                    packetWriter.Write((byte)number);
                    bool value = number2 == 1f;
                    packetWriter.Write(value);
                    break;
                }
            case 140:
                packetWriter.Write((byte)number);
                packetWriter.Write((int)number2);
                break;
            case 141:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                packetWriter.Write(number3);
                packetWriter.Write(number4);
                packetWriter.Write(number5);
                packetWriter.Write(number6);
                break;
            case 142:
                {
                    packetWriter.Write((byte)number);
                    Player obj = Main.player[number];
                    obj.piggyBankProjTracker.Write(packetWriter);
                    obj.voidLensChest.Write(packetWriter);
                    break;
                }
            case 146:
                packetWriter.Write((byte)number);
                switch (number)
                {
                    case 0:
                        packetWriter.WriteVector2(new Vector2((int)number2, (int)number3));
                        break;
                    case 1:
                        packetWriter.WriteVector2(new Vector2((int)number2, (int)number3));
                        packetWriter.Write((int)number4);
                        break;
                    case 2:
                        packetWriter.Write((int)number2);
                        break;
                }
                break;
            case 147:
                packetWriter.Write((byte)number);
                packetWriter.Write((byte)number2);
                NetMessageUtils.WriteAccessoryVisibility(packetWriter, Main.player[number].hideVisibleAccessory);
                break;
        }
        int size = (int)packetWriter.Position;
        if (size > ushort.MaxValue)
        {
            throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + size);
        }
        packetWriter.WriteLength();
        var sendData = packetWriter.GetData();
        if (remoteClient == -1)
        {
            switch (msgType)
            {
                case 34:
                case 69:
                    {
                        for (int num23 = 0; num23 < 256; num23++)
                        {
                            if (num23 != ignoreClient && NetMessage.buffer[num23].broadcast && Netplay.Clients[num23].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num23].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num23].Socket, sendData, 0, size, Netplay.Clients[num23].ServerWriteCallBack, null, num23);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    }
                case 20:
                    {
                        for (int num27 = 0; num27 < 256; num27++)
                        {
                            if (num27 != ignoreClient && NetMessage.buffer[num27].broadcast && Netplay.Clients[num27].IsConnected() && Netplay.Clients[num27].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
                            {
                                try
                                {
                                    NetMessage.buffer[num27].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num27].Socket, sendData, 0, size, Netplay.Clients[num27].ServerWriteCallBack, null, num27);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    }
                case 23:
                    {
                        NPC nPC4 = Main.npc[number];
                        for (int num28 = 0; num28 < 256; num28++)
                        {
                            if (num28 == ignoreClient || !NetMessage.buffer[num28].broadcast || !Netplay.Clients[num28].IsConnected())
                            {
                                continue;
                            }
                            bool flag6 = false;
                            if (nPC4.boss || nPC4.netAlways || nPC4.townNPC || !nPC4.active)
                            {
                                flag6 = true;
                            }
                            else if (nPC4.netSkip <= 0)
                            {
                                Rectangle rect5 = Main.player[num28].getRect();
                                Rectangle rect6 = nPC4.getRect();
                                rect6.X -= 2500;
                                rect6.Y -= 2500;
                                rect6.Width += 5000;
                                rect6.Height += 5000;
                                if (rect5.Intersects(rect6))
                                {
                                    flag6 = true;
                                }
                            }
                            else
                            {
                                flag6 = true;
                            }
                            if (flag6)
                            {
                                try
                                {
                                    NetMessage.buffer[num28].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num28].Socket, sendData, 0, size, Netplay.Clients[num28].ServerWriteCallBack, null, num28);
                                }
                                catch
                                {
                                }
                            }
                        }
                        nPC4.netSkip++;
                        if (nPC4.netSkip > 4)
                        {
                            nPC4.netSkip = 0;
                        }
                        break;
                    }
                case 28:
                    {
                        NPC nPC3 = Main.npc[number];
                        for (int num25 = 0; num25 < 256; num25++)
                        {
                            if (num25 == ignoreClient || !NetMessage.buffer[num25].broadcast || !Netplay.Clients[num25].IsConnected())
                            {
                                continue;
                            }
                            bool flag5 = false;
                            if (nPC3.life <= 0)
                            {
                                flag5 = true;
                            }
                            else
                            {
                                Rectangle rect3 = Main.player[num25].getRect();
                                Rectangle rect4 = nPC3.getRect();
                                rect4.X -= 3000;
                                rect4.Y -= 3000;
                                rect4.Width += 6000;
                                rect4.Height += 6000;
                                if (rect3.Intersects(rect4))
                                {
                                    flag5 = true;
                                }
                            }
                            if (flag5)
                            {
                                try
                                {
                                    NetMessage.buffer[num25].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num25].Socket, sendData, 0, size, Netplay.Clients[num25].ServerWriteCallBack, null, num25);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    }
                case 13:
                    {
                        for (int num26 = 0; num26 < 256; num26++)
                        {
                            if (num26 != ignoreClient && NetMessage.buffer[num26].broadcast && Netplay.Clients[num26].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num26].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num26].Socket, sendData, 0, size, Netplay.Clients[num26].ServerWriteCallBack, null, num26);
                                }
                                catch
                                {
                                }
                            }
                        }
                        Main.player[number].netSkip++;
                        if (Main.player[number].netSkip > 2)
                        {
                            Main.player[number].netSkip = 0;
                        }
                        break;
                    }
                case 27:
                    {
                        Projectile projectile2 = Main.projectile[number];
                        for (int num24 = 0; num24 < 256; num24++)
                        {
                            if (num24 == ignoreClient || !NetMessage.buffer[num24].broadcast || !Netplay.Clients[num24].IsConnected())
                            {
                                continue;
                            }
                            bool flag4 = false;
                            if (projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant)
                            {
                                flag4 = true;
                            }
                            else
                            {
                                Rectangle rect = Main.player[num24].getRect();
                                Rectangle rect2 = projectile2.getRect();
                                rect2.X -= 5000;
                                rect2.Y -= 5000;
                                rect2.Width += 10000;
                                rect2.Height += 10000;
                                if (rect.Intersects(rect2))
                                {
                                    flag4 = true;
                                }
                            }
                            if (flag4)
                            {
                                try
                                {
                                    NetMessage.buffer[num24].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num24].Socket, sendData, 0, size, Netplay.Clients[num24].ServerWriteCallBack, null, num24);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        for (int num22 = 0; num22 < 256; num22++)
                        {
                            if (num22 != ignoreClient && (NetMessage.buffer[num22].broadcast || (Netplay.Clients[num22].State >= 3 && msgType == 10)) && Netplay.Clients[num22].IsConnected())
                            {
                                try
                                {
                                    NetMessage.buffer[num22].spamCount++;
                                    Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                                    Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[num22].Socket, sendData, 0, size, Netplay.Clients[num22].ServerWriteCallBack, null, num22);
                                }
                                catch
                                {
                                }
                            }
                        }
                        break;
                    }
            }
        }
        else if (Netplay.Clients[remoteClient].IsConnected())
        {
            try
            {
                NetMessage.buffer[remoteClient].spamCount++;
                Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, size);
                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[remoteClient].Socket, sendData, 0, size, Netplay.Clients[remoteClient].ServerWriteCallBack, null, remoteClient);
            }
            catch
            {
            }
        }
        NetMessage.buffer[num].writeLocked = false;
        if (msgType == 2 && Main.netMode == 2)
        {
            Netplay.Clients[num].PendingTermination = true;
            Netplay.Clients[num].PendingTerminationApproved = true;
        }
    }

    #region OldSendData
    //public static void orig_SendData(int msgType, int remoteClient = -1, int ignoreClient = -1, NetworkText? text = null, int number = 0, float number2 = 0f, float number3 = 0f, float number4 = 0f, int number5 = 0, int number6 = 0, int number7 = 0)
    //{
    //    if (msgType == 21 && (Main.item[number].shimmerTime > 0f || Main.item[number].shimmered))
    //    {
    //        msgType = 145;
    //    }
    //    int num = 256;
    //    text ??= NetworkText.Empty;
    //    if (remoteClient >= 0)
    //    {
    //        num = remoteClient;
    //    }
    //    using FixedLengthPacketWriter lengthPacket = PacketLength[msgType] != 0 ? new(PacketLength[msgType], (byte)msgType) : null;
    //    MemoryStream memoryStream = null;
    //    PacketWriter packetWriter = null;
    //    long firstPosition = 0;
    //    if (lengthPacket is null)
    //    {
    //        memoryStream = new();
    //        packetWriter = Hooks.NetMessage.InvokeCreatePacketWriter(memoryStream);
    //        if (packetWriter == null)
    //        {
    //            NetMessage.buffer[num].ResetWriter();
    //            packetWriter = (PacketWriter)NetMessage.buffer[num].writer;
    //        }
    //        packetWriter.BaseStream.Position = 0L;
    //        firstPosition = packetWriter.BaseStream.Position;
    //        packetWriter.BaseStream.Position += 2L;
    //        packetWriter.Write((byte)msgType);
    //    }
    //    switch (msgType)
    //    {
    //        case 1:
    //            packetWriter.Write("Terraria" + 279);
    //            break;
    //        case 2:
    //            text.Serialize(packetWriter);
    //            if (Main.dedServ)
    //            {
    //                Console.WriteLine(Language.GetTextValue("CLI.ClientWasBooted", Netplay.Clients[num].Socket.GetRemoteAddress().ToString(), text));
    //            }
    //            break;
    //        case 3:
    //            lengthPacket!.Write((byte)remoteClient);
    //            lengthPacket!.Write(false);
    //            break;
    //        case 4:
    //            {
    //                Player player4 = Main.player[number];
    //                packetWriter.Write((byte)number);
    //                packetWriter.Write((byte)player4.skinVariant);
    //                packetWriter.Write((byte)player4.hair);
    //                packetWriter.Write(player4.name);
    //                packetWriter.Write(player4.hairDye);
    //                NetMessage.WriteAccessoryVisibility(packetWriter, player4.hideVisibleAccessory);
    //                packetWriter.Write(player4.hideMisc);
    //                packetWriter.WriteRGB(player4.hairColor);
    //                packetWriter.WriteRGB(player4.skinColor);
    //                packetWriter.WriteRGB(player4.eyeColor);
    //                packetWriter.WriteRGB(player4.shirtColor);
    //                packetWriter.WriteRGB(player4.underShirtColor);
    //                packetWriter.WriteRGB(player4.pantsColor);
    //                packetWriter.WriteRGB(player4.shoeColor);
    //                BitsByte bitsByte16 = (byte)0;
    //                switch (player4.difficulty)
    //                {
    //                    case 1:
    //                    case 2:
    //                        bitsByte16[player4.difficulty - 1] = true;
    //                        break;
    //                    case 3:
    //                        bitsByte16[3] = true;
    //                        break;
    //                }
    //                bitsByte16[2] = player4.extraAccessory;
    //                packetWriter.Write(bitsByte16);
    //                packetWriter.Write(new BitsByte()
    //                {
    //                    [0] = player4.UsingBiomeTorches,
    //                    [1] = player4.happyFunTorchTime,
    //                    [2] = player4.unlockedBiomeTorches,
    //                    [3] = player4.unlockedSuperCart,
    //                    [4] = player4.enabledSuperCart
    //                });
    //                packetWriter.Write(new BitsByte()
    //                {
    //                    [0] = player4.usedAegisCrystal,
    //                    [1] = player4.usedAegisFruit,
    //                    [2] = player4.usedArcaneCrystal,
    //                    [3] = player4.usedGalaxyPearl,
    //                    [4] = player4.usedGummyWorm,
    //                    [5] = player4.usedAmbrosia,
    //                    [6] = player4.ateArtisanBread
    //                });
    //                break;
    //            }
    //        case 5:
    //            {
    //                lengthPacket!.Write((byte)number);
    //                lengthPacket.Write((short)number2);
    //                Player player5 = Main.player[number];
    //                Item item6 = ((number2 >= (float)PlayerItemSlotID.Loadout3_Dye_0) ? player5.Loadouts[2].Dye[(int)number2 - PlayerItemSlotID.Loadout3_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout3_Armor_0) ? player5.Loadouts[2].Armor[(int)number2 - PlayerItemSlotID.Loadout3_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Loadout2_Dye_0) ? player5.Loadouts[1].Dye[(int)number2 - PlayerItemSlotID.Loadout2_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout2_Armor_0) ? player5.Loadouts[1].Armor[(int)number2 - PlayerItemSlotID.Loadout2_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Loadout1_Dye_0) ? player5.Loadouts[0].Dye[(int)number2 - PlayerItemSlotID.Loadout1_Dye_0] : ((number2 >= (float)PlayerItemSlotID.Loadout1_Armor_0) ? player5.Loadouts[0].Armor[(int)number2 - PlayerItemSlotID.Loadout1_Armor_0] : ((number2 >= (float)PlayerItemSlotID.Bank4_0) ? player5.bank4.item[(int)number2 - PlayerItemSlotID.Bank4_0] : ((number2 >= (float)PlayerItemSlotID.Bank3_0) ? player5.bank3.item[(int)number2 - PlayerItemSlotID.Bank3_0] : ((number2 >= (float)PlayerItemSlotID.TrashItem) ? player5.trashItem : ((number2 >= (float)PlayerItemSlotID.Bank2_0) ? player5.bank2.item[(int)number2 - PlayerItemSlotID.Bank2_0] : ((number2 >= (float)PlayerItemSlotID.Bank1_0) ? player5.bank.item[(int)number2 - PlayerItemSlotID.Bank1_0] : ((number2 >= (float)PlayerItemSlotID.MiscDye0) ? player5.miscDyes[(int)number2 - PlayerItemSlotID.MiscDye0] : ((number2 >= (float)PlayerItemSlotID.Misc0) ? player5.miscEquips[(int)number2 - PlayerItemSlotID.Misc0] : ((number2 >= (float)PlayerItemSlotID.Dye0) ? player5.dye[(int)number2 - PlayerItemSlotID.Dye0] : ((!(number2 >= (float)PlayerItemSlotID.Armor0)) ? player5.inventory[(int)number2 - PlayerItemSlotID.Inventory0] : player5.armor[(int)number2 - PlayerItemSlotID.Armor0])))))))))))))));
    //                if (item6.Name == "" || item6.stack == 0 || item6.type == 0)
    //                {
    //                    item6.SetDefaults(0, noMatCheck: true);
    //                }
    //                int stack = item6.stack;
    //                if (stack < 0)
    //                {
    //                    stack = 0;
    //                }
    //                lengthPacket.Write((short)stack);
    //                lengthPacket.Write((byte)number3);
    //                lengthPacket.Write((short)item6.netID);
    //                break;
    //            }
    //        case 7:
    //            {
    //                packetWriter.Write((int)Main.time);
    //                BitsByte bitsByte5 = (byte)0;
    //                bitsByte5[0] = Main.dayTime;
    //                bitsByte5[1] = Main.bloodMoon;
    //                bitsByte5[2] = Main.eclipse;
    //                packetWriter.Write(bitsByte5);
    //                packetWriter.Write((byte)Main.moonPhase);
    //                packetWriter.Write((short)Main.maxTilesX);
    //                packetWriter.Write((short)Main.maxTilesY);
    //                packetWriter.Write((short)Main.spawnTileX);
    //                packetWriter.Write((short)Main.spawnTileY);
    //                packetWriter.Write((short)Main.worldSurface);
    //                packetWriter.Write((short)Main.rockLayer);
    //                packetWriter.Write(Main.worldID);
    //                packetWriter.Write(Main.worldName);
    //                packetWriter.Write((byte)Main.GameMode);
    //                packetWriter.Write(Main.ActiveWorldFileData.UniqueId.ToByteArray());
    //                packetWriter.Write(Main.ActiveWorldFileData.WorldGeneratorVersion);
    //                packetWriter.Write((byte)Main.moonType);
    //                packetWriter.Write((byte)WorldGen.treeBG1);
    //                packetWriter.Write((byte)WorldGen.treeBG2);
    //                packetWriter.Write((byte)WorldGen.treeBG3);
    //                packetWriter.Write((byte)WorldGen.treeBG4);
    //                packetWriter.Write((byte)WorldGen.corruptBG);
    //                packetWriter.Write((byte)WorldGen.jungleBG);
    //                packetWriter.Write((byte)WorldGen.snowBG);
    //                packetWriter.Write((byte)WorldGen.hallowBG);
    //                packetWriter.Write((byte)WorldGen.crimsonBG);
    //                packetWriter.Write((byte)WorldGen.desertBG);
    //                packetWriter.Write((byte)WorldGen.oceanBG);
    //                packetWriter.Write((byte)WorldGen.mushroomBG);
    //                packetWriter.Write((byte)WorldGen.underworldBG);
    //                packetWriter.Write((byte)Main.iceBackStyle);
    //                packetWriter.Write((byte)Main.jungleBackStyle);
    //                packetWriter.Write((byte)Main.hellBackStyle);
    //                packetWriter.Write(Main.windSpeedTarget);
    //                packetWriter.Write((byte)Main.numClouds);
    //                for (int n = 0; n < 3; n++)
    //                {
    //                    packetWriter.Write(Main.treeX[n]);
    //                }
    //                for (int num8 = 0; num8 < 4; num8++)
    //                {
    //                    packetWriter.Write((byte)Main.treeStyle[num8]);
    //                }
    //                for (int num9 = 0; num9 < 3; num9++)
    //                {
    //                    packetWriter.Write(Main.caveBackX[num9]);
    //                }
    //                for (int num10 = 0; num10 < 4; num10++)
    //                {
    //                    packetWriter.Write((byte)Main.caveBackStyle[num10]);
    //                }
    //                WorldGen.TreeTops.SyncSend(packetWriter);
    //                if (!Main.raining)
    //                {
    //                    Main.maxRaining = 0f;
    //                }
    //                packetWriter.Write(Main.maxRaining);
    //                BitsByte bitsByte6 = (byte)0;
    //                bitsByte6[0] = WorldGen.shadowOrbSmashed;
    //                bitsByte6[1] = NPC.downedBoss1;
    //                bitsByte6[2] = NPC.downedBoss2;
    //                bitsByte6[3] = NPC.downedBoss3;
    //                bitsByte6[4] = Main.hardMode;
    //                bitsByte6[5] = NPC.downedClown;
    //                bitsByte6[6] = Main.ServerSideCharacter;
    //                bitsByte6[7] = NPC.downedPlantBoss;
    //                packetWriter.Write(bitsByte6);
    //                BitsByte bitsByte7 = (byte)0;
    //                bitsByte7[0] = NPC.downedMechBoss1;
    //                bitsByte7[1] = NPC.downedMechBoss2;
    //                bitsByte7[2] = NPC.downedMechBoss3;
    //                bitsByte7[3] = NPC.downedMechBossAny;
    //                bitsByte7[4] = Main.cloudBGActive >= 1f;
    //                bitsByte7[5] = WorldGen.crimson;
    //                bitsByte7[6] = Main.pumpkinMoon;
    //                bitsByte7[7] = Main.snowMoon;
    //                packetWriter.Write(bitsByte7);
    //                BitsByte bitsByte8 = (byte)0;
    //                bitsByte8[1] = Main.fastForwardTimeToDawn;
    //                bitsByte8[2] = Main.slimeRain;
    //                bitsByte8[3] = NPC.downedSlimeKing;
    //                bitsByte8[4] = NPC.downedQueenBee;
    //                bitsByte8[5] = NPC.downedFishron;
    //                bitsByte8[6] = NPC.downedMartians;
    //                bitsByte8[7] = NPC.downedAncientCultist;
    //                packetWriter.Write(bitsByte8);
    //                BitsByte bitsByte9 = (byte)0;
    //                bitsByte9[0] = NPC.downedMoonlord;
    //                bitsByte9[1] = NPC.downedHalloweenKing;
    //                bitsByte9[2] = NPC.downedHalloweenTree;
    //                bitsByte9[3] = NPC.downedChristmasIceQueen;
    //                bitsByte9[4] = NPC.downedChristmasSantank;
    //                bitsByte9[5] = NPC.downedChristmasTree;
    //                bitsByte9[6] = NPC.downedGolemBoss;
    //                bitsByte9[7] = BirthdayParty.PartyIsUp;
    //                packetWriter.Write(bitsByte9);
    //                BitsByte bitsByte10 = (byte)0;
    //                bitsByte10[0] = NPC.downedPirates;
    //                bitsByte10[1] = NPC.downedFrost;
    //                bitsByte10[2] = NPC.downedGoblins;
    //                bitsByte10[3] = Sandstorm.Happening;
    //                bitsByte10[4] = DD2Event.Ongoing;
    //                bitsByte10[5] = DD2Event.DownedInvasionT1;
    //                bitsByte10[6] = DD2Event.DownedInvasionT2;
    //                bitsByte10[7] = DD2Event.DownedInvasionT3;
    //                packetWriter.Write(bitsByte10);
    //                BitsByte bitsByte11 = (byte)0;
    //                bitsByte11[0] = NPC.combatBookWasUsed;
    //                bitsByte11[1] = LanternNight.LanternsUp;
    //                bitsByte11[2] = NPC.downedTowerSolar;
    //                bitsByte11[3] = NPC.downedTowerVortex;
    //                bitsByte11[4] = NPC.downedTowerNebula;
    //                bitsByte11[5] = NPC.downedTowerStardust;
    //                bitsByte11[6] = Main.forceHalloweenForToday;
    //                bitsByte11[7] = Main.forceXMasForToday;
    //                packetWriter.Write(bitsByte11);
    //                BitsByte bitsByte12 = (byte)0;
    //                bitsByte12[0] = NPC.boughtCat;
    //                bitsByte12[1] = NPC.boughtDog;
    //                bitsByte12[2] = NPC.boughtBunny;
    //                bitsByte12[3] = NPC.freeCake;
    //                bitsByte12[4] = Main.drunkWorld;
    //                bitsByte12[5] = NPC.downedEmpressOfLight;
    //                bitsByte12[6] = NPC.downedQueenSlime;
    //                bitsByte12[7] = Main.getGoodWorld;
    //                packetWriter.Write(bitsByte12);
    //                packetWriter.Write(new BitsByte()
    //                {
    //                    [0] = Main.tenthAnniversaryWorld,
    //                    [1] = Main.dontStarveWorld,
    //                    [2] = NPC.downedDeerclops,
    //                    [3] = Main.notTheBeesWorld,
    //                    [4] = Main.remixWorld,
    //                    [5] = NPC.unlockedSlimeBlueSpawn,
    //                    [6] = NPC.combatBookVolumeTwoWasUsed,
    //                    [7] = NPC.peddlersSatchelWasUsed
    //                });
    //                packetWriter.Write(new BitsByte()
    //                {
    //                    [0] = NPC.unlockedSlimeGreenSpawn,
    //                    [1] = NPC.unlockedSlimeOldSpawn,
    //                    [2] = NPC.unlockedSlimePurpleSpawn,
    //                    [3] = NPC.unlockedSlimeRainbowSpawn,
    //                    [4] = NPC.unlockedSlimeRedSpawn,
    //                    [5] = NPC.unlockedSlimeYellowSpawn,
    //                    [6] = NPC.unlockedSlimeCopperSpawn,
    //                    [7] = Main.fastForwardTimeToDusk
    //                });
    //                packetWriter.Write(new BitsByte()
    //                {
    //                    [0] = Main.noTrapsWorld,
    //                    [1] = Main.zenithWorld,
    //                    [2] = NPC.unlockedTruffleSpawn
    //                });
    //                packetWriter.Write((byte)Main.sundialCooldown);
    //                packetWriter.Write((byte)Main.moondialCooldown);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Copper);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Iron);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Silver);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Gold);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Cobalt);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Mythril);
    //                packetWriter.Write((short)WorldGen.SavedOreTiers.Adamantite);
    //                packetWriter.Write((sbyte)Main.invasionType);
    //                if (SocialAPI.Network != null)
    //                {
    //                    packetWriter.Write(SocialAPI.Network.GetLobbyId());
    //                }
    //                else
    //                {
    //                    packetWriter.Write(0uL);
    //                }
    //                packetWriter.Write(Sandstorm.IntendedSeverity);
    //                break;
    //            }
    //        case 8:
    //            lengthPacket!.Write(number);
    //            lengthPacket.Write((int)number2);
    //            break;
    //        case 9:
    //            {
    //                packetWriter.Write(number);
    //                text.Serialize(packetWriter);
    //                BitsByte bitsByte22 = (byte)number2;
    //                packetWriter.Write(bitsByte22);
    //                break;
    //            }
    //        case 10:
    //            NetMessage.CompressTileBlock(number, (int)number2, (short)number3, (short)number4, packetWriter.BaseStream);
    //            break;
    //        case 11:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            break;
    //        case 12:
    //            {
    //                Player player6 = Main.player[number];
    //                lengthPacket!.Write((byte)number);
    //                lengthPacket.Write((short)player6.SpawnX);
    //                lengthPacket.Write((short)player6.SpawnY);
    //                lengthPacket.Write(player6.respawnTimer);
    //                lengthPacket.Write((short)player6.numberOfDeathsPVE);
    //                lengthPacket.Write((short)player6.numberOfDeathsPVP);
    //                lengthPacket.Write((byte)number2);
    //                break;
    //            }
    //        case 13:
    //            {
    //                Player player7 = Main.player[number];
    //                lengthPacket.Write((byte)number);
    //                BitsByte bitsByte25 = (byte)0;
    //                bitsByte25[0] = player7.controlUp;
    //                bitsByte25[1] = player7.controlDown;
    //                bitsByte25[2] = player7.controlLeft;
    //                bitsByte25[3] = player7.controlRight;
    //                bitsByte25[4] = player7.controlJump;
    //                bitsByte25[5] = player7.controlUseItem;
    //                bitsByte25[6] = player7.direction == 1;
    //                lengthPacket.Write(bitsByte25);
    //                BitsByte bitsByte26 = (byte)0;
    //                bitsByte26[0] = player7.pulley;
    //                bitsByte26[1] = player7.pulley && player7.pulleyDir == 2;
    //                bitsByte26[2] = player7.velocity != Vector2.Zero;
    //                bitsByte26[3] = player7.vortexStealthActive;
    //                bitsByte26[4] = player7.gravDir == 1f;
    //                bitsByte26[5] = player7.shieldRaised;
    //                bitsByte26[6] = player7.ghost;
    //                lengthPacket.Write(bitsByte26);
    //                BitsByte bitsByte27 = (byte)0;
    //                bitsByte27[0] = player7.tryKeepingHoveringUp;
    //                bitsByte27[1] = player7.IsVoidVaultEnabled;
    //                bitsByte27[2] = player7.sitting.isSitting;
    //                bitsByte27[3] = player7.downedDD2EventAnyDifficulty;
    //                bitsByte27[4] = player7.isPettingAnimal;
    //                bitsByte27[5] = player7.isTheAnimalBeingPetSmall;
    //                bitsByte27[6] = player7.PotionOfReturnOriginalUsePosition.HasValue;
    //                bitsByte27[7] = player7.tryKeepingHoveringDown;
    //                lengthPacket.Write(bitsByte27);
    //                BitsByte bitsByte28 = (byte)0;
    //                bitsByte28[0] = player7.sleeping.isSleeping;
    //                bitsByte28[1] = player7.autoReuseAllWeapons;
    //                bitsByte28[2] = player7.controlDownHold;
    //                bitsByte28[3] = player7.isOperatingAnotherEntity;
    //                bitsByte28[4] = player7.controlUseTile;
    //                lengthPacket.Write(bitsByte28);
    //                lengthPacket.Write((byte)player7.selectedItem);
    //                lengthPacket.WriteVector2(player7.position);
    //                if (bitsByte26[2])
    //                {
    //                    lengthPacket.WriteVector2(player7.velocity);
    //                }
    //                if (bitsByte27[6])
    //                {
    //                    lengthPacket.WriteVector2(player7.PotionOfReturnOriginalUsePosition!.Value);
    //                    lengthPacket.WriteVector2(player7.PotionOfReturnHomePosition!.Value);
    //                }
    //                break;
    //            }
    //        case 14:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 16:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)Main.player[number].statLife);
    //            lengthPacket.Write((short)Main.player[number].statLifeMax);
    //            break;
    //        case 17:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            lengthPacket.Write((byte)number5);
    //            break;
    //        case 18:
    //            lengthPacket!.Write((byte)(Main.dayTime ? 1u : 0u));
    //            lengthPacket.Write((int)Main.time);
    //            lengthPacket.Write(Main.sunModY);
    //            lengthPacket.Write(Main.moonModY);
    //            break;
    //        case 19:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((number4 == 1f) ? ((byte)1) : ((byte)0));
    //            break;
    //        case 20:
    //            {
    //                int num13 = number;
    //                int num14 = (int)number2;
    //                int num15 = (int)number3;
    //                if (num15 < 0)
    //                {
    //                    num15 = 0;
    //                }
    //                int num16 = (int)number4;
    //                if (num16 < 0)
    //                {
    //                    num16 = 0;
    //                }
    //                if (num13 < num15)
    //                {
    //                    num13 = num15;
    //                }
    //                if (num13 >= Main.maxTilesX + num15)
    //                {
    //                    num13 = Main.maxTilesX - num15 - 1;
    //                }
    //                if (num14 < num16)
    //                {
    //                    num14 = num16;
    //                }
    //                if (num14 >= Main.maxTilesY + num16)
    //                {
    //                    num14 = Main.maxTilesY - num16 - 1;
    //                }
    //                packetWriter.Write((short)num13);
    //                packetWriter.Write((short)num14);
    //                packetWriter.Write((byte)num15);
    //                packetWriter.Write((byte)num16);
    //                packetWriter.Write((byte)number5);
    //                for (int num17 = num13; num17 < num13 + num15; num17++)
    //                {
    //                    for (int num18 = num14; num18 < num14 + num16; num18++)
    //                    {
    //                        BitsByte bitsByte19 = (byte)0;
    //                        BitsByte bitsByte20 = (byte)0;
    //                        BitsByte bitsByte21 = (byte)0;
    //                        byte b3 = 0;
    //                        byte b4 = 0;
    //                        ITile tile2 = Main.tile[num17, num18];
    //                        bitsByte19[0] = tile2.active();
    //                        bitsByte19[2] = tile2.wall > 0;
    //                        bitsByte19[3] = tile2.liquid > 0 && Main.netMode == 2;
    //                        bitsByte19[4] = tile2.wire();
    //                        bitsByte19[5] = tile2.halfBrick();
    //                        bitsByte19[6] = tile2.actuator();
    //                        bitsByte19[7] = tile2.inActive();
    //                        bitsByte20[0] = tile2.wire2();
    //                        bitsByte20[1] = tile2.wire3();
    //                        if (tile2.active() && tile2.color() > 0)
    //                        {
    //                            bitsByte20[2] = true;
    //                            b3 = tile2.color();
    //                        }
    //                        if (tile2.wall > 0 && tile2.wallColor() > 0)
    //                        {
    //                            bitsByte20[3] = true;
    //                            b4 = tile2.wallColor();
    //                        }
    //                        bitsByte20 = (byte)((byte)bitsByte20 + (byte)(tile2.slope() << 4));
    //                        bitsByte20[7] = tile2.wire4();
    //                        bitsByte21[0] = tile2.fullbrightBlock();
    //                        bitsByte21[1] = tile2.fullbrightWall();
    //                        bitsByte21[2] = tile2.invisibleBlock();
    //                        bitsByte21[3] = tile2.invisibleWall();
    //                        packetWriter.Write(bitsByte19);
    //                        packetWriter.Write(bitsByte20);
    //                        packetWriter.Write(bitsByte21);
    //                        if (b3 > 0)
    //                        {
    //                            packetWriter.Write(b3);
    //                        }
    //                        if (b4 > 0)
    //                        {
    //                            packetWriter.Write(b4);
    //                        }
    //                        if (tile2.active())
    //                        {
    //                            packetWriter.Write(tile2.type);
    //                            if (Main.tileFrameImportant[tile2.type])
    //                            {
    //                                packetWriter.Write(tile2.frameX);
    //                                packetWriter.Write(tile2.frameY);
    //                            }
    //                        }
    //                        if (tile2.wall > 0)
    //                        {
    //                            packetWriter.Write(tile2.wall);
    //                        }
    //                        if (tile2.liquid > 0 && Main.netMode == 2)
    //                        {
    //                            packetWriter.Write(tile2.liquid);
    //                            packetWriter.Write(tile2.liquidType());
    //                        }
    //                    }
    //                }
    //                break;
    //            }
    //        case 21:
    //        case 90:
    //        case 145:
    //        case 148:
    //            {
    //                Item item3 = Main.item[number];
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.WriteVector2(item3.position);
    //                lengthPacket.WriteVector2(item3.velocity);
    //                lengthPacket.Write((short)item3.stack);
    //                lengthPacket.Write(item3.prefix);
    //                lengthPacket.Write((byte)number2);
    //                short value2 = 0;
    //                if (item3.active && item3.stack > 0)
    //                {
    //                    value2 = (short)item3.netID;
    //                }
    //                lengthPacket.Write(value2);
    //                if (msgType == 145)
    //                {
    //                    lengthPacket.Write(item3.shimmered);
    //                    lengthPacket.Write(item3.shimmerTime);
    //                }
    //                if (msgType == 148)
    //                {
    //                    lengthPacket.Write((byte)MathHelper.Clamp(item3.timeLeftInWhichTheItemCannotBeTakenByEnemies, 0f, 255f));
    //                }
    //                break;
    //            }
    //        case 22:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((byte)Main.item[number].playerIndexTheItemIsReservedFor);
    //            break;
    //        case 23:
    //            {
    //                NPC nPC2 = Main.npc[number];
    //                lengthPacket.Write((short)number);
    //                lengthPacket.WriteVector2(nPC2.position);
    //                lengthPacket.WriteVector2(nPC2.velocity);
    //                lengthPacket.Write((ushort)nPC2.target);
    //                int num4 = nPC2.life;
    //                if (!nPC2.active)
    //                {
    //                    num4 = 0;
    //                }
    //                if (!nPC2.active || nPC2.life <= 0)
    //                {
    //                    nPC2.netSkip = 0;
    //                }
    //                short value3 = (short)nPC2.netID;
    //                bool[] array = new bool[4];
    //                BitsByte bitsByte3 = (byte)0;
    //                bitsByte3[0] = nPC2.direction > 0;
    //                bitsByte3[1] = nPC2.directionY > 0;
    //                bitsByte3[2] = (array[0] = nPC2.ai[0] != 0f);
    //                bitsByte3[3] = (array[1] = nPC2.ai[1] != 0f);
    //                bitsByte3[4] = (array[2] = nPC2.ai[2] != 0f);
    //                bitsByte3[5] = (array[3] = nPC2.ai[3] != 0f);
    //                bitsByte3[6] = nPC2.spriteDirection > 0;
    //                bitsByte3[7] = num4 == nPC2.lifeMax;
    //                lengthPacket.Write(bitsByte3);
    //                BitsByte bitsByte4 = (byte)0;
    //                bitsByte4[0] = nPC2.statsAreScaledForThisManyPlayers > 1;
    //                bitsByte4[1] = nPC2.SpawnedFromStatue;
    //                bitsByte4[2] = nPC2.strengthMultiplier != 1f;
    //                lengthPacket.Write(bitsByte4);
    //                for (int m = 0; m < NPC.maxAI; m++)
    //                {
    //                    if (array[m])
    //                    {
    //                        lengthPacket.Write(nPC2.ai[m]);
    //                    }
    //                }
    //                lengthPacket.Write(value3);
    //                if (bitsByte4[0])
    //                {
    //                    lengthPacket.Write((byte)nPC2.statsAreScaledForThisManyPlayers);
    //                }
    //                if (bitsByte4[2])
    //                {
    //                    lengthPacket.Write(nPC2.strengthMultiplier);
    //                }
    //                if (!bitsByte3[7])
    //                {
    //                    byte b2 = 1;
    //                    if (nPC2.lifeMax > 32767)
    //                    {
    //                        b2 = 4;
    //                    }
    //                    else if (nPC2.lifeMax > 127)
    //                    {
    //                        b2 = 2;
    //                    }
    //                    lengthPacket.Write(b2);
    //                    switch (b2)
    //                    {
    //                        case 2:
    //                            lengthPacket.Write((short)num4);
    //                            break;
    //                        case 4:
    //                            lengthPacket.Write(num4);
    //                            break;
    //                        default:
    //                            lengthPacket.Write((sbyte)num4);
    //                            break;
    //                    }
    //                }
    //                if (nPC2.type >= 0 && nPC2.type < NPCID.Count && Main.npcCatchable[nPC2.type])
    //                {
    //                    lengthPacket.Write((byte)nPC2.releaseOwner);
    //                }
    //                break;
    //            }
    //        case 24:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 107:
    //            packetWriter.Write((byte)number2);
    //            packetWriter.Write((byte)number3);
    //            packetWriter.Write((byte)number4);
    //            text.Serialize(packetWriter);
    //            packetWriter.Write((short)number5);
    //            break;
    //        case 27:
    //            {
    //                Projectile projectile = Main.projectile[number];
    //                lengthPacket.Write((short)projectile.identity);
    //                lengthPacket.WriteVector2(projectile.position);
    //                lengthPacket.WriteVector2(projectile.velocity);
    //                lengthPacket.Write((byte)projectile.owner);
    //                lengthPacket.Write((short)projectile.type);
    //                BitsByte bitsByte23 = (byte)0;
    //                BitsByte bitsByte24 = (byte)0;
    //                bitsByte23[0] = projectile.ai[0] != 0f;
    //                bitsByte23[1] = projectile.ai[1] != 0f;
    //                bitsByte24[0] = projectile.ai[2] != 0f;
    //                if (projectile.bannerIdToRespondTo != 0)
    //                {
    //                    bitsByte23[3] = true;
    //                }
    //                if (projectile.damage != 0)
    //                {
    //                    bitsByte23[4] = true;
    //                }
    //                if (projectile.knockBack != 0f)
    //                {
    //                    bitsByte23[5] = true;
    //                }
    //                if (projectile.type > 0 && projectile.type < ProjectileID.Count && ProjectileID.Sets.NeedsUUID[projectile.type])
    //                {
    //                    bitsByte23[7] = true;
    //                }
    //                if (projectile.originalDamage != 0)
    //                {
    //                    bitsByte23[6] = true;
    //                }
    //                if ((byte)bitsByte24 != 0)
    //                {
    //                    bitsByte23[2] = true;
    //                }
    //                lengthPacket.Write(bitsByte23);
    //                if (bitsByte23[2])
    //                {
    //                    lengthPacket.Write(bitsByte24);
    //                }
    //                if (bitsByte23[0])
    //                {
    //                    lengthPacket.Write(projectile.ai[0]);
    //                }
    //                if (bitsByte23[1])
    //                {
    //                    lengthPacket.Write(projectile.ai[1]);
    //                }
    //                if (bitsByte23[3])
    //                {
    //                    lengthPacket.Write((ushort)projectile.bannerIdToRespondTo);
    //                }
    //                if (bitsByte23[4])
    //                {
    //                    lengthPacket.Write((short)projectile.damage);
    //                }
    //                if (bitsByte23[5])
    //                {
    //                    lengthPacket.Write(projectile.knockBack);
    //                }
    //                if (bitsByte23[6])
    //                {
    //                    lengthPacket.Write((short)projectile.originalDamage);
    //                }
    //                if (bitsByte23[7])
    //                {
    //                    lengthPacket.Write((short)projectile.projUUID);
    //                }
    //                if (bitsByte24[0])
    //                {
    //                    lengthPacket.Write(projectile.ai[2]);
    //                }
    //                break;
    //            }
    //        case 28:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write(number3);
    //            lengthPacket.Write((byte)(number4 + 1f));
    //            lengthPacket.Write((byte)number5);
    //            break;
    //        case 29:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 30:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write(Main.player[number].hostile);
    //            break;
    //        case 31:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 32:
    //            {
    //                Item item7 = Main.chest[number].item[(byte)number2];
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.Write((byte)number2);
    //                short value4 = (short)item7.netID;
    //                if (item7.Name == null)
    //                {
    //                    value4 = 0;
    //                }
    //                lengthPacket.Write((short)item7.stack);
    //                lengthPacket.Write(item7.prefix);
    //                lengthPacket.Write(value4);
    //                break;
    //            }
    //        case 33:
    //            {
    //                int num5 = 0;
    //                int num6 = 0;
    //                int num7 = 0;
    //                string text2 = null!;
    //                if (number > -1)
    //                {
    //                    num5 = Main.chest[number].x;
    //                    num6 = Main.chest[number].y;
    //                }
    //                if (number2 == 1f)
    //                {
    //                    string text3 = text.ToString();
    //                    num7 = (byte)text3.Length;
    //                    if (num7 == 0 || num7 > 20)
    //                    {
    //                        num7 = 255;
    //                    }
    //                    else
    //                    {
    //                        text2 = text3;
    //                    }
    //                }
    //                packetWriter.Write((short)number);
    //                packetWriter.Write((short)num5);
    //                packetWriter.Write((short)num6);
    //                packetWriter.Write((byte)num7);
    //                if (text2 != null)
    //                {
    //                    packetWriter.Write(text2);
    //                }
    //                break;
    //            }
    //        case 34:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            if (Main.netMode == 2)
    //            {
    //                Netplay.GetSectionX((int)number2);
    //                Netplay.GetSectionY((int)number3);
    //                lengthPacket.Write((short)number5);
    //            }
    //            else
    //            {
    //                lengthPacket.Write((short)0);
    //            }
    //            break;
    //        case 35:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 36:
    //            {
    //                Player player3 = Main.player[number];
    //                lengthPacket!.Write((byte)number);
    //                lengthPacket.Write(player3.zone1);
    //                lengthPacket.Write(player3.zone2);
    //                lengthPacket.Write(player3.zone3);
    //                lengthPacket.Write(player3.zone4);
    //                lengthPacket.Write(player3.zone5);
    //                break;
    //            }
    //        case 38:
    //            packetWriter.Write(Netplay.ServerPassword);
    //            break;
    //        case 39:
    //            lengthPacket!.Write((short)number);
    //            break;
    //        case 40:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)Main.player[number].talkNPC);
    //            break;
    //        case 41:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write(Main.player[number].itemRotation);
    //            lengthPacket.Write((short)Main.player[number].itemAnimation);
    //            break;
    //        case 42:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)Main.player[number].statMana);
    //            lengthPacket.Write((short)Main.player[number].statManaMax);
    //            break;
    //        case 43:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 45:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)Main.player[number].team);
    //            break;
    //        case 46:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 47:
    //            packetWriter.Write((short)number);
    //            packetWriter.Write((short)Main.sign[number].x);
    //            packetWriter.Write((short)Main.sign[number].y);
    //            packetWriter.Write(Main.sign[number].text);
    //            packetWriter.Write((byte)number2);
    //            packetWriter.Write((byte)number3);
    //            break;
    //        case 48:
    //            {
    //                ITile tile = Main.tile[number, (int)number2];
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.Write((short)number2);
    //                lengthPacket.Write(tile.liquid);
    //                lengthPacket.Write(tile.liquidType());
    //                break;
    //            }
    //        case 50:
    //            {
    //                lengthPacket!.Write((byte)number);
    //                for (int l = 0; l < Player.maxBuffs; l++)
    //                {
    //                    lengthPacket.Write((ushort)Main.player[number].buffType[l]);
    //                }
    //                break;
    //            }
    //        case 51:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 52:
    //            lengthPacket!.Write((byte)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            break;
    //        case 53:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write((short)number3);
    //            break;
    //        case 54:
    //            {
    //                lengthPacket!.Write((short)number);
    //                for (int k = 0; k < NPC.maxBuffs; k++)
    //                {
    //                    lengthPacket.Write((ushort)Main.npc[number].buffType[k]);
    //                    lengthPacket.Write((short)Main.npc[number].buffTime[k]);
    //                }
    //                break;
    //            }
    //        case 55:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write((int)number3);
    //            break;
    //        case 56:
    //            packetWriter.Write((short)number);
    //            if (Main.netMode == 2)
    //            {
    //                string givenName = Main.npc[number].GivenName;
    //                packetWriter.Write(givenName);
    //                packetWriter.Write(Main.npc[number].townNpcVariationIndex);
    //            }
    //            break;
    //        case 57:
    //            lengthPacket!.Write(WorldGen.tGood);
    //            lengthPacket.Write(WorldGen.tEvil);
    //            lengthPacket.Write(WorldGen.tBlood);
    //            break;
    //        case 58:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write(number2);
    //            break;
    //        case 59:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 60:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((byte)number4);
    //            break;
    //        case 61:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 62:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 63:
    //        case 64:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((byte)number3);
    //            lengthPacket.Write((byte)number4);
    //            break;
    //        case 65:
    //            {
    //                BitsByte bitsByte29 = (byte)0;
    //                bitsByte29[0] = (number & 1) == 1;
    //                bitsByte29[1] = (number & 2) == 2;
    //                bitsByte29[2] = number6 == 1;
    //                bitsByte29[3] = number7 != 0;
    //                lengthPacket.Write(bitsByte29);
    //                lengthPacket.Write((short)number2);
    //                lengthPacket.Write(number3);
    //                lengthPacket.Write(number4);
    //                lengthPacket.Write((byte)number5);
    //                if (bitsByte29[3])
    //                {
    //                    lengthPacket.Write(number7);
    //                }
    //                break;
    //            }
    //        case 66:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 68:
    //            packetWriter.Write(Main.clientUUID);
    //            break;
    //        case 69:
    //            Netplay.GetSectionX((int)number2);
    //            Netplay.GetSectionY((int)number3);
    //            packetWriter.Write((short)number);
    //            packetWriter.Write((short)number2);
    //            packetWriter.Write((short)number3);
    //            packetWriter.Write(Main.chest[(short)number].name);
    //            break;
    //        case 70:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 71:
    //            lengthPacket!.Write(number);
    //            lengthPacket.Write((int)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((byte)number4);
    //            break;
    //        case 72:
    //            {
    //                for (int num20 = 0; num20 < 40; num20++)
    //                {
    //                    lengthPacket!.Write((short)Main.travelShop[num20]);
    //                }
    //                break;
    //            }
    //        case 73:
    //            lengthPacket!.Write((byte)number);
    //            break;
    //        case 74:
    //            {
    //                lengthPacket!.Write((byte)Main.anglerQuest);
    //                bool value7 = Main.anglerWhoFinishedToday.Contains(text.ToString());
    //                lengthPacket.Write(value7);
    //                break;
    //            }
    //        case 76:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write(Main.player[number].anglerQuestsFinished);
    //            lengthPacket.Write(Main.player[number].golferScoreAccumulated);
    //            break;
    //        case 77:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            break;
    //        case 78:
    //            lengthPacket!.Write(number);
    //            lengthPacket.Write((int)number2);
    //            lengthPacket.Write((sbyte)number3);
    //            lengthPacket.Write((sbyte)number4);
    //            break;
    //        case 79:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            lengthPacket.Write((byte)number5);
    //            lengthPacket.Write((sbyte)number6);
    //            lengthPacket.Write(number7 == 1);
    //            break;
    //        case 80:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 81:
    //            {
    //                lengthPacket!.Write(number2);
    //                lengthPacket.Write(number3);
    //                lengthPacket.WriteRGB(new Color((uint)number));
    //                lengthPacket.Write((int)number4);
    //                break;
    //            }
    //        case 119:
    //            {
    //                packetWriter.Write(number2);
    //                packetWriter.Write(number3);
    //                Color c = default(Color);
    //                c.PackedValue = (uint)number;
    //                packetWriter.WriteRGB(c);
    //                text.Serialize(packetWriter);
    //                break;
    //            }
    //        case 83:
    //            {
    //                int num19 = number;
    //                if (num19 < 0 && num19 >= 290)
    //                {
    //                    num19 = 1;
    //                }
    //                lengthPacket!.Write((short)num19);
    //                lengthPacket.Write(NPC.killCount[num19]);
    //                break;
    //            }
    //        case 84:
    //            {
    //                byte b5 = (byte)number;
    //                lengthPacket!.Write(b5);
    //                lengthPacket.Write(Main.player[b5].stealth);
    //                break;
    //            }
    //        case 85:
    //            {
    //                lengthPacket!.Write((short)number);
    //                break;
    //            }
    //        case 86:
    //            {
    //                packetWriter.Write(number);
    //                bool flag3 = TileEntity.ByID.ContainsKey(number);
    //                packetWriter.Write(flag3);
    //                if (flag3)
    //                {
    //                    TileEntity.Write(packetWriter, TileEntity.ByID[number], networkSend: true);
    //                }
    //                break;
    //            }
    //        case 87:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((byte)number3);
    //            break;
    //        case 88:
    //            {
    //                BitsByte bitsByte = (byte)number2;
    //                BitsByte bitsByte2 = (byte)number3;
    //                lengthPacket.Write((short)number);
    //                lengthPacket.Write(bitsByte);
    //                Item item5 = Main.item[number];
    //                if (bitsByte[0])
    //                {
    //                    lengthPacket.Write(item5.color.PackedValue);
    //                }
    //                if (bitsByte[1])
    //                {
    //                    lengthPacket.Write((ushort)item5.damage);
    //                }
    //                if (bitsByte[2])
    //                {
    //                    lengthPacket.Write(item5.knockBack);
    //                }
    //                if (bitsByte[3])
    //                {
    //                    lengthPacket.Write((ushort)item5.useAnimation);
    //                }
    //                if (bitsByte[4])
    //                {
    //                    lengthPacket.Write((ushort)item5.useTime);
    //                }
    //                if (bitsByte[5])
    //                {
    //                    lengthPacket.Write((short)item5.shoot);
    //                }
    //                if (bitsByte[6])
    //                {
    //                    lengthPacket.Write(item5.shootSpeed);
    //                }
    //                if (bitsByte[7])
    //                {
    //                    lengthPacket.Write(bitsByte2);
    //                    if (bitsByte2[0])
    //                    {
    //                        lengthPacket.Write((ushort)item5.width);
    //                    }
    //                    if (bitsByte2[1])
    //                    {
    //                        lengthPacket.Write((ushort)item5.height);
    //                    }
    //                    if (bitsByte2[2])
    //                    {
    //                        lengthPacket.Write(item5.scale);
    //                    }
    //                    if (bitsByte2[3])
    //                    {
    //                        lengthPacket.Write((short)item5.ammo);
    //                    }
    //                    if (bitsByte2[4])
    //                    {
    //                        lengthPacket.Write((short)item5.useAmmo);
    //                    }
    //                    if (bitsByte2[5])
    //                    {
    //                        lengthPacket.Write(item5.notAmmo);
    //                    }
    //                }
    //                break;
    //            }
    //        case 89:
    //            {
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.Write((short)number2);
    //                Item item4 = Main.player[(int)number4].inventory[(int)number3];
    //                lengthPacket.Write((short)item4.netID);
    //                lengthPacket.Write(item4.prefix);
    //                lengthPacket.Write((short)number5);
    //                break;
    //            }
    //        case 91:
    //            packetWriter.Write(number);
    //            packetWriter.Write((byte)number2);
    //            if (number2 != 255f)
    //            {
    //                packetWriter.Write((ushort)number3);
    //                packetWriter.Write((ushort)number4);
    //                packetWriter.Write((byte)number5);
    //                if (number5 < 0)
    //                {
    //                    packetWriter.Write((short)number6);
    //                }
    //            }
    //            break;
    //        case 92:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((int)number2);
    //            lengthPacket.Write(number3);
    //            lengthPacket.Write(number4);
    //            break;
    //        case 95:
    //            lengthPacket!.Write((ushort)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 96:
    //            {
    //                lengthPacket!.Write((byte)number);
    //                Player player2 = Main.player[number];
    //                lengthPacket.Write((short)number4);
    //                lengthPacket.Write(number2);
    //                lengthPacket.Write(number3);
    //                lengthPacket.WriteVector2(player2.velocity);
    //                break;
    //            }
    //        case 97:
    //            lengthPacket!.Write((short)number);
    //            break;
    //        case 98:
    //            lengthPacket!.Write((short)number);
    //            break;
    //        case 99:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.WriteVector2(Main.player[number].MinionRestTargetPoint);
    //            break;
    //        case 115:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)Main.player[number].MinionAttackTargetNPC);
    //            break;
    //        case 100:
    //            {
    //                lengthPacket!.Write((ushort)number);
    //                NPC nPC = Main.npc[number];
    //                lengthPacket.Write((short)number4);
    //                lengthPacket.Write(number2);
    //                lengthPacket.Write(number3);
    //                lengthPacket.WriteVector2(nPC.velocity);
    //                break;
    //            }
    //        case 101:
    //            lengthPacket!.Write((ushort)NPC.ShieldStrengthTowerSolar);
    //            lengthPacket.Write((ushort)NPC.ShieldStrengthTowerVortex);
    //            lengthPacket.Write((ushort)NPC.ShieldStrengthTowerNebula);
    //            lengthPacket.Write((ushort)NPC.ShieldStrengthTowerStardust);
    //            break;
    //        case 102:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write(number3);
    //            lengthPacket.Write(number4);
    //            break;
    //        case 103:
    //            lengthPacket!.Write(NPC.MaxMoonLordCountdown);
    //            lengthPacket.Write(NPC.MoonLordCountdown);
    //            break;
    //        case 104:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write(((short)number3 < 0) ? 0f : number3);
    //            lengthPacket.Write((byte)number4);
    //            lengthPacket.Write(number5);
    //            lengthPacket.Write((byte)number6);
    //            break;
    //        case 105:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write(number3 == 1f);
    //            break;
    //        case 106:
    //            lengthPacket!.Write(new HalfVector2(number, number2).PackedValue);
    //            break;
    //        case 108:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write(number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            lengthPacket.Write((short)number5);
    //            lengthPacket.Write((short)number6);
    //            lengthPacket.Write((byte)number7);
    //            break;
    //        case 109:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((short)number4);
    //            lengthPacket.Write((byte)number5);
    //            break;
    //        case 110:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((byte)number3);
    //            break;
    //        case 112:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((int)number2);
    //            lengthPacket.Write((int)number3);
    //            lengthPacket.Write((byte)number4);
    //            lengthPacket.Write((short)number5);
    //            break;
    //        case 113:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((short)number2);
    //            break;
    //        case 116:
    //            lengthPacket!.Write(number);
    //            break;
    //        case 117:
    //            packetWriter.Write((byte)number);
    //            NetMessage._currentPlayerDeathReason.WriteSelfTo(packetWriter);
    //            packetWriter.Write((short)number2);
    //            packetWriter.Write((byte)(number3 + 1f));
    //            packetWriter.Write((byte)number4);
    //            packetWriter.Write((sbyte)number5);
    //            break;
    //        case 118:
    //            packetWriter.Write((byte)number);
    //            NetMessage._currentPlayerDeathReason.WriteSelfTo(packetWriter);
    //            packetWriter.Write((short)number2);
    //            packetWriter.Write((byte)(number3 + 1f));
    //            packetWriter.Write((byte)number4);
    //            break;
    //        case 120:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 121:
    //            {
    //                int num3 = (int)number3;
    //                bool flag2 = number4 == 1f;
    //                if (flag2)
    //                {
    //                    num3 += 8;
    //                }
    //                packetWriter.Write((byte)number);
    //                packetWriter.Write((int)number2);
    //                packetWriter.Write((byte)num3);
    //                if (TileEntity.ByID[(int)number2] is TEDisplayDoll tEDisplayDoll)
    //                {
    //                    tEDisplayDoll.WriteItem((int)number3, packetWriter, flag2);
    //                    break;
    //                }
    //                packetWriter.Write(0);
    //                packetWriter.Write((byte)0);
    //                break;
    //            }
    //        case 122:
    //            lengthPacket!.Write(number);
    //            lengthPacket.Write((byte)number2);
    //            break;
    //        case 123:
    //            {
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.Write((short)number2);
    //                Item item2 = Main.player[(int)number4].inventory[(int)number3];
    //                lengthPacket.Write((short)item2.netID);
    //                lengthPacket.Write(item2.prefix);
    //                lengthPacket.Write((short)number5);
    //                break;
    //            }
    //        case 124:
    //            {
    //                int num2 = (int)number3;
    //                bool flag = number4 == 1f;
    //                if (flag)
    //                {
    //                    num2 += 2;
    //                }
    //                packetWriter.Write((byte)number);
    //                packetWriter.Write((int)number2);
    //                packetWriter.Write((byte)num2);
    //                if (TileEntity.ByID[(int)number2] is TEHatRack tEHatRack)
    //                {
    //                    tEHatRack.WriteItem((int)number3, packetWriter, flag);
    //                    break;
    //                }
    //                packetWriter.Write(0);
    //                packetWriter.Write((byte)0);
    //                break;
    //            }
    //        case 125:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((short)number2);
    //            lengthPacket.Write((short)number3);
    //            lengthPacket.Write((byte)number4);
    //            break;
    //        case 126:
    //            NetMessage._currentRevengeMarker.WriteSelfTo(packetWriter);
    //            break;
    //        case 127:
    //            lengthPacket!.Write(number);
    //            break;
    //        case 128:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((ushort)number5);
    //            lengthPacket.Write((ushort)number6);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write((ushort)number3);
    //            break;
    //        case 130:
    //            lengthPacket!.Write((ushort)number);
    //            lengthPacket.Write((ushort)number2);
    //            lengthPacket.Write((short)number3);
    //            break;
    //        case 131:
    //            {
    //                packetWriter.Write((ushort)number);
    //                packetWriter.Write((byte)number2);
    //                byte b = (byte)number2;
    //                if (b == 1)
    //                {
    //                    packetWriter.Write((int)number3);
    //                    packetWriter.Write((short)number4);
    //                }
    //                break;
    //            }
    //        case 132:
    //            NetMessage._currentNetSoundInfo.WriteSelfTo(packetWriter);
    //            break;
    //        case 133:
    //            {
    //                lengthPacket!.Write((short)number);
    //                lengthPacket.Write((short)number2);
    //                Item item = Main.player[(int)number4].inventory[(int)number3];
    //                lengthPacket.Write((short)item.netID);
    //                lengthPacket.Write(item.prefix);
    //                lengthPacket.Write((short)number5);
    //                break;
    //            }
    //        case 134:
    //            {
    //                lengthPacket!.Write((byte)number);
    //                Player player = Main.player[number];
    //                lengthPacket.Write(player.ladyBugLuckTimeLeft);
    //                lengthPacket.Write(player.torchLuck);
    //                lengthPacket.Write(player.luckPotion);
    //                lengthPacket.Write(player.HasGardenGnomeNearby);
    //                lengthPacket.Write(player.equipmentBasedLuckBonus);
    //                lengthPacket.Write(player.coinLuck);
    //                break;
    //            }
    //        case 135:
    //            lengthPacket!.Write((byte)number);
    //            break;
    //        case 136:
    //            {
    //                for (int i = 0; i < 2; i++)
    //                {
    //                    for (int j = 0; j < 3; j++)
    //                    {
    //                        lengthPacket!.Write((ushort)NPC.cavernMonsterType[i, j]);
    //                    }
    //                }
    //                break;
    //            }
    //        case 137:
    //            lengthPacket!.Write((short)number);
    //            lengthPacket.Write((ushort)number2);
    //            break;
    //        case 139:
    //            {
    //                lengthPacket!.Write((byte)number);
    //                lengthPacket.Write(number2 == 1f);
    //                break;
    //            }
    //        case 140:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((int)number2);
    //            break;
    //        case 141:
    //            lengthPacket!.Write((byte)number);
    //            lengthPacket.Write((byte)number2);
    //            lengthPacket.Write(number3);
    //            lengthPacket.Write(number4);
    //            lengthPacket.Write(number5);
    //            lengthPacket.Write(number6);
    //            break;
    //        case 142:
    //            {
    //                packetWriter.Write((byte)number);
    //                Player obj = Main.player[number];
    //                obj.piggyBankProjTracker.Write(packetWriter);
    //                obj.voidLensChest.Write(packetWriter);
    //                break;
    //            }
    //        case 146:
    //            packetWriter.Write((byte)number);
    //            switch (number)
    //            {
    //                case 0:
    //                    packetWriter.WriteVector2(new Vector2((int)number2, (int)number3));
    //                    break;
    //                case 1:
    //                    packetWriter.WriteVector2(new Vector2((int)number2, (int)number3));
    //                    packetWriter.Write((int)number4);
    //                    break;
    //                case 2:
    //                    packetWriter.Write((int)number2);
    //                    break;
    //            }
    //            break;
    //        case 147:
    //            packetWriter.Write((byte)number);
    //            packetWriter.Write((byte)number2);
    //            NetMessage.WriteAccessoryVisibility(packetWriter, Main.player[number].hideVisibleAccessory);
    //            break;
    //    }
    //    int packteLength = (int)(lengthPacket?.Position ?? (int)packetWriter.BaseStream.Position);
    //    if (lengthPacket is null)
    //    {
    //        packetWriter.BaseStream.Position = firstPosition;
    //        packetWriter.Write((ushort)packteLength);
    //        packetWriter.BaseStream.Position = packteLength;
    //    }
    //    if (packteLength > 65535)
    //    {
    //        throw new Exception("Maximum packet length exceeded. id: " + msgType + " length: " + packteLength);
    //    }
    //    //NetMessage.OnPacketWrite(num, memoryStream, packetWriter, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
    //    byte[] sendArray;
    //    if (lengthPacket is null)
    //    {
    //        sendArray = memoryStream.ToArray();
    //    }
    //    else
    //    {
    //        lengthPacket.WriteLength();
    //        sendArray = lengthPacket.Data;
    //    }
    //    if (remoteClient == -1)
    //    {
    //        switch (msgType)
    //        {
    //            case 34:
    //            case 69:
    //                {
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex != ignoreClient && NetMessage.buffer[clientIndex].broadcast && Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //            case 20:
    //                {
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex != ignoreClient && NetMessage.buffer[clientIndex].broadcast && Netplay.Clients[clientIndex].IsConnected() && Netplay.Clients[clientIndex].SectionRange((int)Math.Max(number3, number4), number, (int)number2))
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //            case 23:
    //                {
    //                    NPC nPC4 = Main.npc[number];
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex == ignoreClient || !NetMessage.buffer[clientIndex].broadcast || !Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            continue;
    //                        }
    //                        bool flag6 = false;
    //                        if (nPC4.boss || nPC4.netAlways || nPC4.townNPC || !nPC4.active)
    //                        {
    //                            flag6 = true;
    //                        }
    //                        else if (nPC4.netSkip <= 0)
    //                        {
    //                            Rectangle rect5 = Main.player[clientIndex].getRect();
    //                            Rectangle rect6 = nPC4.getRect();
    //                            rect6.X -= 2500;
    //                            rect6.Y -= 2500;
    //                            rect6.Width += 5000;
    //                            rect6.Height += 5000;
    //                            if (rect5.Intersects(rect6))
    //                            {
    //                                flag6 = true;
    //                            }
    //                        }
    //                        else
    //                        {
    //                            flag6 = true;
    //                        }
    //                        if (flag6)
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    nPC4.netSkip++;
    //                    if (nPC4.netSkip > 4)
    //                    {
    //                        nPC4.netSkip = 0;
    //                    }
    //                    break;
    //                }
    //            case 28:
    //                {
    //                    NPC nPC3 = Main.npc[number];
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex == ignoreClient || !NetMessage.buffer[clientIndex].broadcast || !Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            continue;
    //                        }
    //                        bool flag5 = false;
    //                        if (nPC3.life <= 0)
    //                        {
    //                            flag5 = true;
    //                        }
    //                        else
    //                        {
    //                            Rectangle rect3 = Main.player[clientIndex].getRect();
    //                            Rectangle rect4 = nPC3.getRect();
    //                            rect4.X -= 3000;
    //                            rect4.Y -= 3000;
    //                            rect4.Width += 6000;
    //                            rect4.Height += 6000;
    //                            if (rect3.Intersects(rect4))
    //                            {
    //                                flag5 = true;
    //                            }
    //                        }
    //                        if (flag5)
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //            case 13:
    //                {
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex != ignoreClient && NetMessage.buffer[clientIndex].broadcast && Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    Main.player[number].netSkip++;
    //                    if (Main.player[number].netSkip > 2)
    //                    {
    //                        Main.player[number].netSkip = 0;
    //                    }
    //                    break;
    //                }
    //            case 27:
    //                {
    //                    Projectile projectile2 = Main.projectile[number];
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex == ignoreClient || !NetMessage.buffer[clientIndex].broadcast || !Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            continue;
    //                        }
    //                        bool flag4 = false;
    //                        if (projectile2.type == 12 || Main.projPet[projectile2.type] || projectile2.aiStyle == 11 || projectile2.netImportant)
    //                        {
    //                            flag4 = true;
    //                        }
    //                        else
    //                        {
    //                            Rectangle rect = Main.player[clientIndex].getRect();
    //                            Rectangle rect2 = projectile2.getRect();
    //                            rect2.X -= 5000;
    //                            rect2.Y -= 5000;
    //                            rect2.Width += 10000;
    //                            rect2.Height += 10000;
    //                            if (rect.Intersects(rect2))
    //                            {
    //                                flag4 = true;
    //                            }
    //                        }
    //                        if (flag4)
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //            default:
    //                {
    //                    for (int clientIndex = 0; clientIndex < 256; clientIndex++)
    //                    {
    //                        if (clientIndex != ignoreClient && (NetMessage.buffer[clientIndex].broadcast || (Netplay.Clients[clientIndex].State >= 3 && msgType == 10)) && Netplay.Clients[clientIndex].IsConnected())
    //                        {
    //                            try
    //                            {
    //                                NetMessage.buffer[clientIndex].spamCount++;
    //                                //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //                                Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[clientIndex].Socket, sendArray, 0, packteLength, Netplay.Clients[clientIndex].ServerWriteCallBack, null, clientIndex);
    //                            }
    //                            catch
    //                            {
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //        }
    //    }
    //    else if (Netplay.Clients[remoteClient].IsConnected())
    //    {
    //        try
    //        {
    //            NetMessage.buffer[remoteClient].spamCount++;
    //            //Main.ActiveNetDiagnosticsUI.CountSentMessage(msgType, endPosition);
    //            Hooks.NetMessage.InvokeSendBytes(Netplay.Clients[remoteClient].Socket, sendArray, 0, packteLength, Netplay.Clients[remoteClient].ServerWriteCallBack, null, remoteClient);
    //            //Netplay.Clients[remoteClient].Socket.AsyncSend(sendArray, 0, endPosition, Netplay.Clients[remoteClient].ServerWriteCallBack, null);
    //        }
    //        catch
    //        {
    //        }
    //    }
    //    NetMessage.buffer[num].writeLocked = false;
    //    if (msgType == 2)
    //    {
    //        Netplay.Clients[num].PendingTermination = true;
    //        Netplay.Clients[num].PendingTerminationApproved = true;
    //    }
    //}
    #endregion
}
#region Extension
public static class NetMessageUtils
{
    public static void WriteSelfTo(this Terraria.GameContent.CoinLossRevengeSystem.RevengeMarker self, IPacketWriter writer)
    {
        writer.Write(self._uniqueID);
        writer.WriteVector2(self._location);
        writer.Write(self._npcNetID);
        writer.Write(self._npcHPPercent);
        writer.Write(self._npcTypeAgainstDiscouragement);
        writer.Write(self._npcAIStyleAgainstDiscouragement);
        writer.Write(self._coinsValue);
        writer.Write(self._baseValue);
        writer.Write(self._spawnedFromStatue);
    }
    public static void WriteSelfTo(this NetMessage.NetSoundInfo self, IPacketWriter writer)
    {
        writer.WriteVector2(self.position);
        writer.Write(self.soundIndex);
        BitsByte bitsByte = new BitsByte(self.style != -1, self.volume != -1f, self.pitchOffset != -1f);
        writer.Write(bitsByte);
        if (bitsByte[0])
        {
            writer.Write(self.style);
        }

        if (bitsByte[1])
        {
            writer.Write(self.volume);
        }

        if (bitsByte[2])
        {
            writer.Write(self.pitchOffset);
        }
    }
    public static void Write(this TrackedProjectileReference self, IPacketWriter writer)
    {
        writer.Write((short)self.ProjectileOwnerIndex);
        if (self.ProjectileOwnerIndex != -1)
        {
            writer.Write((short)self.ProjectileIdentity);
            writer.Write((short)self.ProjectileType);
        }
    }
    public static void WriteAccessoryVisibility(IPacketWriter writer, bool[] hideVisibleAccessory)
    {
        ushort num = 0;
        for (int i = 0; i < hideVisibleAccessory.Length; i++)
        {
            if (hideVisibleAccessory[i])
            {
                num = (ushort)(num | (ushort)(1 << i));
            }
        }

        writer.Write(num);
    }
    public static void WriteItem(this TEDisplayDoll self, int itemIndex, IPacketWriter writer, bool dye)
    {
        Item item = self._items[itemIndex];
        if (dye)
        {
            item = self._dyes[itemIndex];
        }

        writer.Write((ushort)item.netID);
        writer.Write((ushort)item.stack);
        writer.Write(item.prefix);
    }
    public static void WriteItem(this TEHatRack self, int itemIndex, IPacketWriter writer, bool dye)
    {
        Item item = self._items[itemIndex];
        if (dye)
        {
            item = self._dyes[itemIndex];
        }

        writer.Write((ushort)item.netID);
        writer.Write((ushort)item.stack);
        writer.Write(item.prefix);
    }
}
public static class SizeOf
{
    public const ushort Vector2 = sizeof(float) * 2;
    public const ushort Color = sizeof(byte) * 3;
    public const ushort TrackedProjectileReference = sizeof(short) * 3;
    public const ushort TEDisplayDoll = sizeof(ushort) * 2 + sizeof(byte);
    public const ushort TEHatRack = sizeof(ushort) * 2 + sizeof(byte);
}
#endregion