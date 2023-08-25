using VBY.Common.Config;

namespace TempGroup;
public class Config : ConfigBase<Root> { }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
public class Root : RootBase 
{
    public DatabaseInfo DBInfo;
    public List<AddInfo> AddInfos;
    public double Interval = 60000;
}
public class AddInfo
{
    public string GroupName;
    public int GroupLevel;
    public string MethodName;
    public double Value;
}