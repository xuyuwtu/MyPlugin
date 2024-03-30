using System.Buffers.Binary;

namespace VBY.MoreSpawnPoint;

internal struct BytePacket
{
    internal ref struct WorldData
    {
        public byte[] Data { get; }
        private Span<byte> DataSpan;
        public WorldData(byte[] byteData)
        {
            Data = byteData;
            DataSpan = new Span<byte>(Data);
        }
        public readonly ushort PacketLength => BinaryPrimitives.ReadUInt16LittleEndian(DataSpan);
        public readonly byte MessageID => Data[2];
        public readonly short SpawnX
        {
            get => BinaryPrimitives.ReadInt16LittleEndian(DataSpan.Slice(13));
            set => BinaryPrimitives.WriteInt16LittleEndian(DataSpan.Slice(13), value);
        }
        public readonly short SpawnY
        {
            get => BinaryPrimitives.ReadInt16LittleEndian(DataSpan.Slice(15));
            set => BinaryPrimitives.WriteInt16LittleEndian(DataSpan.Slice(15), value);
        }
    }
}