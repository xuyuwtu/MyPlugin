using System.Text;

using TShockAPI;

namespace Game1;

internal class Vote
{
    public string Name;
    public List<string> Voters = new();
    public int TrueCount;
    public int FalseCount;
    public int GiveupCount;
    public int MinVoters = 2;
    public Func<Vote, bool> SuccessFunc = vote => vote.Voters.Count > vote.MinVoters && vote.TrueCount > vote.FalseCount;
    public List<string>? SuccessCommands;
    public Action<Vote>? SuccessAction;
    public Vote(string name)
    {
        Name = name;
    }
    public void VoteTrue(TSPlayer player)
    {
        Voters.Add(player.Name);
        TrueCount++;
    }
    public void VoteFalse(TSPlayer player)
    {
        Voters.Add(player.Name);
        FalseCount++;
    }
    public void VoteGiveUp(TSPlayer player)
    {
        Voters.Add(player.Name); 
        GiveupCount++;
    }
    public (bool success,string message) GetResult()
    {
        var sb = new StringBuilder();
        sb.AppendFormat("同意:{0}票\n", TrueCount);
        sb.AppendFormat("拒绝:{0}票\n", FalseCount);
        sb.AppendFormat("放弃:{0}票\n", GiveupCount);
        var success = SuccessFunc(this);
        sb.AppendLine($"投票结果:{success.ToString("成功", "失败")}");
        if (success)
        {
            SuccessCommands?.ForEach(x => Commands.HandleCommand(TSPlayer.Server, x));
            SuccessAction?.Invoke(this);
        }
        return (success, sb.ToString());
    }
    public void Reset()
    {
        TrueCount = 0;
        FalseCount = 0;
        GiveupCount = 0;
        Voters.Clear();
    }
}
