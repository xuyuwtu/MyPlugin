using System.Data;

using Terraria;
using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.DB;

using MySql.Data.MySqlClient;

using VBY.Common.Command;
using VBY.Common.Extension;

namespace TempGroup;

[ApiVersion(2, 1)]
public class TempGroup : TerrariaPlugin
{
    SubCmdRoot CtlCommand;
    Config ReadConig = new();
    IDbConnection DB;
    System.Timers.Timer timer;
    // 玩家名 旧组名 新组名 结束时间
    List<Dictionary<string, DateTime>> NameAndEndTime = new();
    Dictionary<string, SetGroupInfo> Info = new();
    public TempGroup(Main game) : base(game)
    {
        DB = ReadConig.Root!.DBInfo.GetDbConnection();
        var creator = DB.GetTableCreator();
        creator.EnsureTableStructure(new SqlTable(nameof(TempGroup), new List<SqlColumn>() {
                new(nameof(SetGroupInfo.PlayerName), MySqlDbType.String),
                new(nameof(SetGroupInfo.NewGroupName),  MySqlDbType.String),
                new(nameof(SetGroupInfo.EndTime),    MySqlDbType.DateTime)
            }));
        timer = new(ReadConig.Root.Interval);
        timer.Elapsed += OnTimer;
        CtlCommand = new("tg");
        CtlCommand.AllowInfo.SetInfo(null, null, "tempgroup");
    }
    public override void Initialize()
    {
        ServerApi.Hooks.ServerJoin.Register(this, OnServerJoin);
        ServerApi.Hooks.ServerLeave.Register(this, OnServerLeave);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.ServerJoin.Deregister(this, OnServerJoin);
            ServerApi.Hooks.ServerLeave.Deregister(this, OnServerLeave);
            timer.Elapsed -= OnTimer;
            timer.Dispose();
        }
        base.Dispose(disposing);
    }
    void OnServerJoin(JoinEventArgs e)
    {
        var player = TShock.Players[e.Who];
        if (player is null)
            return;
        using var reader = DB.QueryReader($"SELECT {SetGroupInfo.Headers} FROM {nameof(TempGroup)} WHERE PlayerName = @0", player.Name);
        if (reader.Read())
        {
            //var dic = new Dictionary<string, DateTime>();
            //reader.Reader.DoForEach<SetGroupInfo>(x => dic.Add(x.Get(x => x.NewGroupName), x.Get(x => x.EndTime)));
            reader.Reader.DoForEach<SetGroupInfo>(x => Info.Add(player.Name, new SetGroupInfo(player.Name, x.Get(x => x.OriginalGroup), x.Get(x => x.NewGroupName), x.Get(x => x.EndTime))));
            
            //NameAndEndTime.Add(dic);
            if (!timer.Enabled)
                timer.Start();
        }
    }
    void OnServerLeave(LeaveEventArgs e)
    {
        if (TShock.Utils.GetActivePlayerCount() == 0 && timer.Enabled) 
            timer.Stop();
    }
    private void OnTimer(object? sender, System.Timers.ElapsedEventArgs e)
    {
        var now = DateTime.Now;
        foreach(var info in NameAndEndTime)
        {
            foreach (var (name,time) in info)
            {
                if(time < now)
                {

                }
            }
        }
    }
}
record class SetGroupInfo
{
    internal string PlayerName;
    internal string OriginalGroup;
    internal string NewGroupName;
    internal DateTime EndTime;
    internal const string Headers = "PlayerName,OriginalGroup,NewGroupName,EndTime";

    public SetGroupInfo(string playerName, string originalGroup, string newGroupName, DateTime endTime)
    {
        PlayerName = playerName;
        OriginalGroup = originalGroup;
        NewGroupName = newGroupName;
        EndTime = endTime;
    }
}