using Terraria;
using TerrariaApi.Server;

namespace VBY.EventFirstRecord;
[ApiVersion(2, 1)]
public class EventFirstRecord : TerrariaPlugin
{
    public override string Name => "VBY.EventFirstRecord";
    public override string Author => "yu";
    public override string Description => "记录第一次击败的";
    private StreamWriter LogFile = new(Path.Combine(TShockAPI.TShock.SavePath, "EventFirstRecord.txt")) { AutoFlush = true };
    public EventFirstRecord(Main game) : base(game)
    {
    }

    public override void Initialize()
    {
        On.Terraria.NPC.OnGameEventClearedForTheFirstTime += OnNPC_OnGameEventClearedForTheFirstTime;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            LogFile.Dispose();
            On.Terraria.NPC.OnGameEventClearedForTheFirstTime -= OnNPC_OnGameEventClearedForTheFirstTime;
        }
        base.Dispose(disposing);
    }
    private void OnNPC_OnGameEventClearedForTheFirstTime(On.Terraria.NPC.orig_OnGameEventClearedForTheFirstTime orig, int gameEventId)
    {
        orig(gameEventId);
        var name = "";
        switch (gameEventId)
        {
            case 0:
                name = "哥布林入侵";
                break;
            case 1:
                name = "霜月";
                break;
            case 2:
                name = "海盗入侵";
                break;
            case 3:
                name = "火星入侵";
                break;
            case 4:
                name = "哀木";
                break;
            case 5:
                name = "南瓜王";
                break;
            case 6:
                name = "石巨人";
                break;
            case 7:
                name = "猪龙鱼公爵";
                break;
            case 8:
                name = "蜂王";
                break;
            case 9:
                name = "拜月教邪教徒";
                break;
            case 10:
                name = "月亮领主";
                break;
            case 11:
                name = "史莱姆王";
                break;
            case 12:
                name = "世纪之花";
                break;
            case 13:
                name = "克苏鲁之眼";
                break;
            case 14:
                name = "邪恶Boss";
                break;
            case 15:
                name = "骷髅王";
                break;
            case 16:
                name = "毁灭者";
                break;
            case 17:
                name = "双子魔眼";
                break;
            case 18:
                name = "机械骷髅王";
                break;
            case 19:
                name = "血肉墙";
                break;
            case 20:
                name = "冰雪女王";
                break;
            case 21:
                name = "常绿尖叫怪";
                break;
            case 22:
                name = "圣诞坦克";
                break;
            case 23:
                name = "光之女皇";
                break;
            case 24:
                name = "史莱姆皇后";
                break;
            case 25:
                name = "独眼巨鹿";
                break;
        }
        if (!string.IsNullOrEmpty(name))
        {
            LogFile.WriteLine($"[{DateTime.Now}]{name}");
            LogFile.Flush();
        }
    }
}