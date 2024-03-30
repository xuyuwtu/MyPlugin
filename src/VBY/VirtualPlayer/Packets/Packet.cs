using System.Runtime.CompilerServices;

namespace VBY.VirtualPlayer.Packets;


[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal abstract class Packet
{
    protected ushort PacketLength;
    protected byte PacketID;
    public Packet(byte messageID)
    {
        PacketID = messageID;
        if (PacketLengths[messageID] != 0)
        {
            PacketLength = PacketLengths[messageID];
        }
        else
        {
            var type = GetType();
            PacketLength = (ushort)Marshal.SizeOf(type);
            PacketLength -= (ushort)(type.GetFields().Where(x => x.FieldType == typeof(bool)).Count() * 3);
            PacketLengths[messageID] = PacketLength;
        }
    }
    public Packet(PacketTypes packetType) : this((byte)packetType) { }
    public void Clear() => GetSpan(this).Clear();
    public void CopyTo(Packet target) => GetSpan(this).CopyTo(GetSpan(target));
    public unsafe byte[] GetData() => GetSpan(this).ToArray();

    private static ushort[] PacketLengths = new ushort[MessageID.Count];
    private static unsafe Span<byte> GetSpan(Packet packet) => new(Unsafe.AsPointer(ref Unsafe.As<Packet, RawData>(ref packet).Data), packet.PacketLength);
    private static Dictionary<byte, Packet> packetCache = new();
    public static T GetPacket<T>(byte messageID) where T : Packet, new()
    {
        if (packetCache.TryGetValue(messageID, out var packet))
        {
            packet.Clear();
            return (T)packet;
        }
        packet = new T();
        packetCache[messageID] = packet;
        packet.Clear();
        return (T)packet;
    }
    public static unsafe void CopyTo(ReadOnlySpan<byte> bytes, Packet target)
    {
        if (bytes[0] != target.PacketID)
        {
            throw new ArgumentException("PacketID not equal", nameof(bytes));
        }
        if (System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes) != target.PacketLength)
        {
            throw new ArgumentException("PacketLength not equal", nameof(bytes));
        }
        bytes.CopyTo(GetSpan(target));
    }
}
internal sealed class RawData
{
    public byte Data;
}