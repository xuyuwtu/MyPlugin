using Microsoft.Xna.Framework;

using Terraria.ID;

using Newtonsoft.Json;
using TShockAPI;

namespace Game1;

internal class Config
{
    //60帧1秒 5分钟
    public int GameDuration = 60 * 60 * 5;
    public int MinPlayerCount = 2;
    public int[] TeamConvertColors = new int[TeamID.Count]
    {
        PaintID.None,
        PaintID.DeepRedPaint,
        PaintID.DeepGreenPaint,
        PaintID.DeepCyanPaint,
        PaintID.DeepYellowPaint,
        PaintID.DeepPinkPaint
    }; 
    public Point[] TeamSpawnPoint = new Point[TeamID.Count];
    public GameRegion GameRegion = new();
    public GameRegion[] TeamSpawneRegion = new GameRegion[TeamID.Count];
    public int[] CanSelectTeams = new int[]
    {
        TeamID.Blue,
        TeamID.Pink
    };
    public ProjectileConvertSizeInfo[] CanConvertProjectileInfos = new ProjectileConvertSizeInfo[]
    {
        new(ProjectileID.Bullet, 3),
        new(ProjectileID.Grenade, 4),
        new(ProjectileID.Flames, 4),
        new(ProjectileID.StickyGrenade, 4),
        new(ProjectileID.PainterPaintball, 3)
    };
    public int InitialHealMax = 400;
    public ItemInfo[] InitialBagItems = Array.Empty<ItemInfo>();
    public SwitchItemInfo[] SwitchItemInfos = Array.Empty<SwitchItemInfo>();
    public void Save(string path)
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
    }
    public static void Load(Config config)
    {
        MiniGame.DefGameDuration = config.GameDuration;
        Array.Fill(MiniGame.ProjectlileConvertSize, 0);
        for (int i = 0; i < config.CanConvertProjectileInfos.Length; i++)
        {
            ProjectileConvertSizeInfo info = config.CanConvertProjectileInfos[i];
            MiniGame.ProjectlileConvertSize[info.Type] = info.Size;
        }
        MiniGame.GameRegion = config.GameRegion;
        MiniGame.TeamConvertColors = config.TeamConvertColors.Select(x => (byte)x).ToArray();
        MiniGame.TeamSpawnPoint = config.TeamSpawnPoint;
        MiniGame.TeamSpawnRegion = config.TeamSpawneRegion.Select(x => (Rectangle)x).ToArray();
    }
}
internal class ProjectileConvertSizeInfo
{
    public int Type;
    public int Size;
    public ProjectileConvertSizeInfo(int type, int size)
    {
        Type = type;
        Size = size;
    }
}
internal class ItemInfo
{
    public int type;
    public int stack;
    public byte prefix;
    public int slot;
    public ItemInfo() { }
    public ItemInfo(int type, int stack, byte prefix, int slot)
    {
        this.type = type;
        this.stack = stack;
        this.prefix = prefix;
        this.slot = slot;
    }
    public ItemInfo(NetItem item, int slot) : this(item.NetId, item.Stack, item.PrefixId, slot) { }
}
internal class SwitchItemInfo
{
    public Point[] Points = Array.Empty<Point>();
    public ItemInfo[] ItemInfos = Array.Empty<ItemInfo>();
}

public struct GameRegion
{
    public int X;
    public int Y;
    public int Width;
    public int Height;
    public GameRegion()
    {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }
    public GameRegion(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public static implicit operator Rectangle(GameRegion region) => new(region.X, region.Y, region.Width, region.Height);
    public static implicit operator GameRegion(Rectangle rectangle) => new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
}