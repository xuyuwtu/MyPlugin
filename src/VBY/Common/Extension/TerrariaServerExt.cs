namespace VBY.Common.Extension;

public static class TerrariaServerExt
{
    public static BinaryReader GetBinaryReader(this TerrariaApi.Server.GetDataEventArgs args) => new(new MemoryStream(args.Msg.readBuffer, args.Index, args.Length));
}
