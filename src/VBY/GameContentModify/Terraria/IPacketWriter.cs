using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;

namespace VBY.GameContentModify;

public interface IPacketWriter : IDisposable
{
    public long Position { get; set; }
    public byte[] GetData();
    public void Write(byte value);
    public void Write(sbyte value);
    public void Write(bool value);
    public void Write(short value);
    public void Write(ushort value);
    public void Write(int value);
    public void Write(uint value);
    public void Write(long value);
    public void Write(ulong value);
    public void Write(float value);
    public void Write(double value);
    public void Write(string value);
    public void Write(byte[] buffer);
    public void WriteVector2(in Vector2 value);
    public void WriteRGB(in Color value);
    public void WriteLength();
    public Span<byte> GetDataHead();
}
public sealed class TerrariaPacketWriter : BinaryWriter, IPacketWriter
{
    public TerrariaPacketWriter(MemoryStream stream) : base(stream) { }
    public long Position { get => BaseStream.Position; set => BaseStream.Position = value; }
    public byte[] GetData() => ((MemoryStream)BaseStream).ToArray();

    public void WriteLength()
    {
        var position = Position;
        BaseStream.Position = 0;
        Write((ushort)position);
        BaseStream.Position = position;
    }
    public unsafe void WriteRGB(in Color value)
    {
        Write(value.R);
        Write(value.G);
        Write(value.B);
    }
    public unsafe void WriteVector2(in Vector2 value)
    {
        Span<byte> span = stackalloc byte[8];
        BinaryPrimitives.WriteSingleLittleEndian(span, value.X);
        BinaryPrimitives.WriteSingleLittleEndian(span.Slice(4), value.Y);
        Write(span);
    }

    Span<byte> IPacketWriter.GetDataHead() => throw new InvalidOperationException();
}
public class FixedLengthPacketWriter : IPacketWriter
{
    private readonly static ArrayPool<byte> arrayPool = ArrayPool<byte>.Create();
    private byte[] data;
    public long Position { get => position; set => position = value; }
    public byte[] GetData()
    {
        var array = GC.AllocateUninitializedArray<byte>(data.Length);
        data.AsSpan().CopyTo(array);
        return array;
    }
    private long position = 0;
    private bool disposedValue = false;

    public FixedLengthPacketWriter(ushort length)
    {
        if (length < 3)
        {
            throw new ArgumentException("length can't less then 3");
        }
        data = arrayPool.Rent(length);
    }
    public void Write(bool value)
    {
        data[position++] = value ? (byte)1 : (byte)0;
    }
    public void Write(byte value)
    {
        data[position++] = value;
    }
    public void Write(sbyte value)
    {
        data[position++] = (byte)value;
    }
    public void Write(short value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(short);
    }
    public void Write(ushort value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(ushort);
    }
    public void Write(int value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(int);
    }
    public void Write(uint value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(uint);
    }
    public void Write(long value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(long);
    }
    public void Write(ulong value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(ulong);
    }
    public void Write(float value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(float);
    }
    public void Write(double value)
    {
        Unsafe.WriteUnaligned(ref data[position], value);
        position += sizeof(double);
    }
    public void WriteVector2(in Vector2 value)
    {
        Unsafe.WriteUnaligned(ref data[position], value.X);
        Unsafe.WriteUnaligned(ref data[position + 4], value.Y);
        position += 8;
    }
    public unsafe void WriteRGB(in Color value)
    {
        Unsafe.CopyBlockUnaligned(ref data[position], ref Unsafe.As<uint, byte>(ref Unsafe.AsRef(value).packedValue), 3);
        position += 3;
    }
    public void WriteLength() => Unsafe.WriteUnaligned(ref data[0], (ushort)position);
    public void WriteLength(ushort length) => Unsafe.WriteUnaligned(ref data[0], length);
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                arrayPool.Return(data);
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Write(string value)
    {
        throw new NotImplementedException();
    }

    public void Write(byte[] buffer)
    {
        buffer.AsSpan().CopyTo(data.AsSpan()[(int)position..]);
    }

    unsafe Span<byte> IPacketWriter.GetDataHead() => data.AsSpan()[3..];
}