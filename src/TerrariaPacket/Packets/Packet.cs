using System.Runtime.CompilerServices;

namespace Terraria.Net.Packets;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public abstract class Packet
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
    public unsafe virtual byte[] GetRawData() => GetSpan(this).ToArray();

    private static ushort[] PacketLengths = new ushort[MessageID.Count];
    private static ushort[] PacketBaseLengths = new ushort[MessageID.Count];
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
    public static ushort GetPacketBaseLength(Packet packet)
    {
        var id = packet.PacketID;
        if (PacketBaseLengths[id] == 0)
        {
            ushort length = 0;
            var fields = packet.GetType().GetFields();
            foreach(var field in fields)
            {
                if (field.FieldType == typeof(byte) || field.FieldType == typeof(sbyte) || field.FieldType == typeof(bool))
                {
                    length++;
                }
                else if (field.FieldType == typeof(ushort) || field.FieldType == typeof(short))
                {
                    length += 2;
                }
                else if(field.FieldType == typeof(int) || field.FieldType == typeof(uint) || field.FieldType == typeof(float))
                {
                    length += 4;
                }
                else if (field.FieldType == typeof(long) || field.FieldType == typeof(ulong) || field.FieldType == typeof(double))
                {
                    length += 8;
                }
                else if(field.FieldType == typeof(Color))
                {
                    length += 3;
                }
                else if(field.FieldType == typeof(Guid))
                {
                    length += 16;
                }
            }
            PacketBaseLengths[id] = length;
        }
        return PacketBaseLengths[id];
    }
    public static unsafe void CopyTo(Span<byte> bytes, Packet target)
    {
        if (bytes[0] != target.PacketID)
        {
            throw new ArgumentException("PacketID not equal", nameof(bytes));
        }
        if (System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(1)) != target.PacketLength)
        {
            throw new ArgumentException("PacketLength not equal", nameof(bytes));
        }
        bytes.CopyTo(GetSpan(target));
    }
}