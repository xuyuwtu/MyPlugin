﻿using Terraria;
using Terraria.ID;

namespace VBY.SpawnRandomDrop;

public class Config
{
    public Dictionary<string, object[]> IDGroup = new();
    public Dictionary<string, object[]> AdditionalGroup = new();
    public List<object> InitAddIDs = new();
    public List<object> NotRandomDropNPCIDs = new();
    public List<object> CanDropItemIDs = new();
    public List<object> CanUseTileIDs = new();
    public bool SkipEmptyDrop = true;
    public bool SkipStatue = true;
    public DropType DropType = DropType.Replace;
    public int RandomDropRandomNumber = 4;

    public static Config GetDefault()
    {
        var config = new Config();
        var npc = new NPC();
        for (int i = 1; i < NPCID.Count; i++)
        {
            npc.SetDefaults(i);
            if (npc.boss)
            {
                config.NotRandomDropNPCIDs.Add(i);
            }
        }
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsHead);
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsBody);
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsTail);
        config.CanUseTileIDs.Add(TileID.DemonAltar);
        return config;
    }
}

public enum DropType
{
    Replace, Add
}