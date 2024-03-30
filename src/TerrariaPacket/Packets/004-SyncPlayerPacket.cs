namespace Terraria.Net.Packets;

sealed partial class SyncPlayerPacket
{
    public byte WhoAmi;
    public byte SkinVariant;
    public byte Hair;
    public string Name = null!;
    public byte HairDye;
    public ushort AccessoryVisibility;
    public byte HideMisc;
    public RGBColor HairColor;
    public RGBColor SkinColor;
    public RGBColor EyeColor;
    public RGBColor ShirColor;
    public RGBColor UnderShirColor;
    public RGBColor PantsColor;
    public RGBColor ShoeColor;
    public BitsByte Flag1;
    public BitsByte Flag2;
    public BitsByte Flag3;
    public unsafe override byte[] GetRawData()
    {
        var ms = new MemoryStream(GetPacketBaseLength(this));
        var bw = new BinaryWriter(ms);
        ms.Position = 2;
        ms.WriteByte(PacketID);
        ms.WriteByte(WhoAmi);
        ms.WriteByte(SkinVariant);
        ms.WriteByte(Hair);
        bw.Write(Name);
        ms.WriteByte(HairDye);
        bw.Write(AccessoryVisibility);
        ms.WriteByte(HideMisc);
        ms.Write(HairColor);
        ms.Write(SkinColor);
        ms.Write(EyeColor);
        ms.Write(ShirColor);
        ms.Write(UnderShirColor);
        ms.Write(PantsColor);
        ms.Write(ShoeColor);
        ms.WriteByte(Flag1);
        ms.WriteByte(Flag2);
        ms.WriteByte(Flag3);
        var data = ms.ToArray();
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(data, checked((ushort)data.Length));
        return data;
    }
}
