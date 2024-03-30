namespace VBY.Common;

public interface ILoader
{
 
   public bool OnPost { get; }
    public void Initialize();
    public void Clear();
    public static void Initialize(ILoader clearable) => clearable.Initialize();
    public static void Clear(ILoader clearable) => clearable.Clear();
    public static bool PostInitialize(ILoader clearable) => clearable.OnPost;
    public static bool NotPostInitialize(ILoader clearable) => !clearable.OnPost;
}
