namespace VBY.TimedUnlockProgress;

public class Config
{
    public string StartedTime = "";
    public UnlockInfo[] UnlockInfos = Array.Empty<UnlockInfo>();
}
public class UnlockInfo
{
    public int[] IDs = Array.Empty<int>();
    public TimeSpan Time = TimeSpan.FromDays(1);
}
