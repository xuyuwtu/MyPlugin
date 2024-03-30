namespace Terraria.Net.Packets;

sealed partial class SetTimePacket
{
    public bool DayTime;
    public int Time;
    public byte SunModY;
    public byte MoonModY;
}
