using Microsoft.Xna.Framework;
using System.Runtime.Intrinsics.X86;
using Terraria;

namespace VBY.GameContentModify;

internal struct NPCUpdate
{
    public short Index;
    public Vector2 Position;
    public Vector2 Velocity;
    public ushort Target;
    public BitsByte Flag1;
    public BitsByte Flag2;
    public float[] AI;
    public short NetID;
    public byte? StatsAreScaledForThisManyPlayers;
    public float? StrengthMultiplier;
    public byte? LifeType = null;
    public int? Life;
    public byte? ReleaseOwner;
    public NPCUpdate(int number)
    {
        Index = (short)number;
        var npc = Main.npc[number];
        Position = npc.position;
        Velocity = npc.velocity;
        Target = (ushort)npc.target; 
        bool[] array = new bool[4];
        Flag1 = new()
        {
            [0] = npc.direction > 0,
            [1] = npc.directionY > 0,
            [2] = (array[0] = npc.ai[0] != 0f),
            [3] = (array[1] = npc.ai[1] != 0f),
            [4] = (array[2] = npc.ai[2] != 0f),
            [5] = (array[3] = npc.ai[3] != 0f),
            [6] = npc.spriteDirection > 0,
            [7] = npc.life == npc.lifeMax
        };
        Flag2 = new()
        {
            [0] = npc.statsAreScaledForThisManyPlayers > 1,
            [1] = npc.SpawnedFromStatue,
            [2] = npc.strengthMultiplier != 1f
        };
        NetID = (short)npc.netID;
        StatsAreScaledForThisManyPlayers = Flag2[0] ? (byte)npc.statsAreScaledForThisManyPlayers : null;
        StrengthMultiplier = Flag2[2] ? npc.strengthMultiplier : null;
        if (!Flag1[7])
        {
            LifeType = 1;
            if(npc.lifeMax > short.MaxValue)
            {
                LifeType = 4;
            }
            else if(npc.lifeMax > 127)
            {
                LifeType = 2;
            }
            Life = npc.life;
        }
    }
}
