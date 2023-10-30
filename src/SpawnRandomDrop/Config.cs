using Terraria;
using Terraria.ID;

namespace VBY.SpawnRandomDrop;

public class Config
{
    public List<object> NotRandomDropNPCIDs = new();
    public List<object> CanDropItemIDs = new();
    public bool SkipEmptyDrop = false;
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
            npc.boss = false;
        }
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsHead);
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsBody);
        config.NotRandomDropNPCIDs.Add(NPCID.EaterofWorldsTail);
        return config;
    }
}
