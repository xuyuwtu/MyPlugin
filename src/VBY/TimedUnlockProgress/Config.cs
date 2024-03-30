namespace VBY.TimedUnlockProgress;

public class Config
{
    public string StartedTime = "";
    public LockType LockType = LockType.NotSpawn;
    public float StrenghtValue = 100.0f;
    public UnlockInfo[] UnlockInfos = Array.Empty<UnlockInfo>();
    public object[] NoSendInfoIDs = Array.Empty<object>();
    public bool AddCion = true;
}
public class UnlockInfo
{
    public object[] IDs = Array.Empty<object>();
    public TimeSpan Time = TimeSpan.FromDays(1);
}
public enum LockType
{
    NotSpawn, Strengthen
}