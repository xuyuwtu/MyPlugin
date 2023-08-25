using Microsoft.Xna.Framework;
using System.Buffers;
using System.Runtime.CompilerServices;

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
    public void Write(ulong value);
    public void Write(float value);
    public void Write(string value);
    public void Write(byte[] buffer);
    public void WriteVector2(Vector2 value);
    public void WriteRGB(Color value);
    public void WriteLength();
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

    public void WriteRGB(Color value)
    {
        Write(value.R);
        Write(value.G);
        Write(value.B);
    }

    public void WriteVector2(Vector2 value)
    {
        Write(value.X);
        Write(value.Y);
    }
}
public sealed class FixedLengthPacketWriter : IPacketWriter
{
    private byte[] data;
    public long Position { get => position; set => position = (int)value; }
    public byte[] GetData() => data;
    private int position;
    private bool disposedValue;

    //public FixedLengthPacketWriter(ushort length, byte msgType)
    public FixedLengthPacketWriter(ushort length)
    {
        if (length < 3)
        {
            throw new ArgumentException("length can't less then 3");
        }
        //position = 3;
        //Data = new byte[length];
        //Unsafe.As<byte, ushort>(ref Data[0]) = length;
        //Data[2] = msgType;
        data = ArrayPool<byte>.Shared.Rent(length);
        //position = 0;
        //position = 2;
        //Write(length);
        //Write(msgType);
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
        Unsafe.As<byte, short>(ref data[position]) = value;
        position += sizeof(short);
    }
    public void Write(ushort value)
    {
        Unsafe.As<byte, ushort>(ref data[position]) = value;
        position += sizeof(ushort);
    }
    public void Write(int value)
    {
        Unsafe.As<byte, int>(ref data[position]) = value;
        position += sizeof(int);
    }
    public void Write(uint value)
    {
        Unsafe.As<byte, uint>(ref data[position]) = value;
        position += sizeof(uint);
    }
    public void Write(ulong value)
    {
        Unsafe.As<byte, ulong>(ref data[position]) = value;
        position += sizeof(ulong);
    }
    public void Write(float value)
    {
        Unsafe.As<byte, float>(ref data[position]) = value;
        position += sizeof(float);
    }
    public void WriteVector2(Vector2 vector2)
    {
        Write(vector2.X);
        Write(vector2.Y);
    }
    public void WriteRGB(Color vector2)
    {
        data[position++] = vector2.R;
        data[position++] = vector2.G;
        data[position++] = vector2.B;
    }
    public void WriteLength()
    {
        Unsafe.As<byte, ushort>(ref data[0]) = (ushort)position;
    }
    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                ArrayPool<byte>.Shared.Return(data);
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
        throw new NotImplementedException();
    }
}