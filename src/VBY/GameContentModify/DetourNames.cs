using Terraria;

namespace VBY.GameContentModify;

public static class DetourNames
{
    public const string Item_CheckLavaDeath = $"Terraria.Item.{nameof(Item.CheckLavaDeath)}";
    public const string Item_MechSpawn = "Terraria.Item.MechSpawn";
    public const string Main_UpdateTime_SpawnTownNPCs = "Terraria.Main.UpdateTime_SpawnTownNPCs";
    public const string MessageBuffer_GetData = "Terraria.MessageBuffer.GetData";
    public const string Liquid_DelWater = "Terraria.Liquid.DelWater";
    public const string NPC_CountKillForBannersAndDropThem = $"Terraria.NPC.{nameof(NPC.CountKillForBannersAndDropThem)}";
    public const string NPC_MechSpawn = "Terraria.NPC.MechSpawn";
    public const string NPC_SpawnNPC = "Terraria.NPC.SpawnNPC";
    public const string NPC_TransformElderSlime = "Terraria.NPC.TransformElderSlime";
    public const string ObjectData_TileObjectData_GetTileData = "Terraria.ObjectData.TileObjectData.GetTileData";
    public const string Wiring_HitWireSingle = "Terraria.Wiring.HitWireSingle";
    public const string WorldGen_ShakeTree = "Terraria.WorldGen.ShakeTree";
    public const string WorldGen_GrowAlch = "Terraria.WorldGen.GrowAlch";
    public const string WorldGen_SpawnThingsFromPot = "Terraria.WorldGen.SpawnThingsFromPot";
    public const string WorldGen_IsHarvestableHerbWithSeed = "Terraria.WorldGen.IsHarvestableHerbWithSeed";
}
