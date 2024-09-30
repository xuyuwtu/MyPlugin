namespace Terraria.Net.Packets;

sealed partial class HelloPacket
{
    public int Version = 279;
    public override byte[] GetRawData()
    {
        var ms = new MemoryStream();
        var bw = new BinaryWriter(ms);
        ms.Position = 2;
        ms.WriteByte(PacketID);
        //bw.Write("Terraria" + Version);
        bw.Write("Terraria279"u8);
        var data = ms.ToArray();
        System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(data, (ushort)data.Length);
        return data;
    }
}
