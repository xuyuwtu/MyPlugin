namespace LifeArmor;

internal class Config
{
}
public class LifeArmorInfo
{
    public int type;
    public int life;
    public int prefix = -1;
    public int index = -1;
    public bool AnyIndex => index == -1;
    public bool AnyPrefix => prefix == -1;
    public bool IsMatch(int index, int prefix) => (AnyIndex || this.index == index) && (AnyPrefix || this.prefix == prefix);
}