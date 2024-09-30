using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace IDAnalyzer;

public static class IDs
{
    internal const short NPCID = 0;
    internal const short ItemID = 0;
    internal const ushort TileID = 0;
    internal const ushort WallID = 0;
    internal const byte MessageID = 0;
    internal const short InvasionID = 0;
    internal const short ProjectileID = 0;
    internal const byte PlayerDifficultyID = 0;

    private static readonly Dictionary<string, IDictionary> AllID = [];
    static IDs()
    {
        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("OTAPI.dll");
        using var peReader = new PEReader(stream);
        var mdReader = peReader.GetMetadataReader();
        AddID(mdReader, NPCID);
        AddID(mdReader, ItemID);
        AddID(mdReader, TileID);
        AddID(mdReader, WallID);
        AddID(mdReader, MessageID);
        AddID(mdReader, InvasionID, static name => name.StartsWith("Cache"));
        AddID(mdReader, ProjectileID);
        AddID(mdReader, PlayerDifficultyID);
    }
    static IDictionary AddDirectionary<T>(MetadataReader mdReader, string typeName, Func<byte[], T> reader, Func<string, bool>? fieldNameFilter = null) where T : notnull
    {
        var dict = new Dictionary<T, string>();
        foreach (var typeDefHandle in mdReader.TypeDefinitions)
        {
            var typeDef = mdReader.GetTypeDefinition(typeDefHandle);
            if (mdReader.GetString(typeDef.Namespace) == "Terraria.ID" && mdReader.GetString(typeDef.Name) == typeName)
            {
                foreach (var fieldDefHandle in typeDef.GetFields())
                {
                    var fieldDef = mdReader.GetFieldDefinition(fieldDefHandle);
                    var fieldName = mdReader.GetString(fieldDef.Name);
                    if(fieldNameFilter is not null && fieldNameFilter(fieldName))
                    {
                        continue;
                    }
                    if (fieldDef.Attributes.HasFlag(FieldAttributes.Literal))
                    {
                        dict.Add(reader(mdReader.GetBlobBytes(mdReader.GetConstant(fieldDef.GetDefaultValue()).Value)), fieldName);
                    }
                }
                break;
            }
        }
        if (dict.Count == 0)
        {
            throw new Exception("");
        }
        return dict.ToFrozenDictionary();
    }
    static void AddID(MetadataReader mdReader, short value, Func<string, bool>? fieldNameFilter = null, [CallerArgumentExpression(nameof(value))] string typeName = "") => AllID.Add(typeName, AddDirectionary(mdReader, typeName, static bytes => BinaryPrimitives.ReadInt16LittleEndian(bytes), fieldNameFilter));
    static void AddID(MetadataReader mdReader, ushort value, Func<string, bool>? fieldNameFilter = null, [CallerArgumentExpression(nameof(value))] string typeName = "") => AllID.Add(typeName, AddDirectionary(mdReader, typeName, static bytes => BinaryPrimitives.ReadUInt16LittleEndian(bytes), fieldNameFilter));
    static void AddID(MetadataReader mdReader, byte value, Func<string, bool>? fieldNameFilter = null, [CallerArgumentExpression(nameof(value))] string typeName = "") => AllID.Add(typeName, AddDirectionary(mdReader, typeName, static bytes => bytes[0], fieldNameFilter));
    public static FrozenDictionary<short, string> GetInt16ID([CallerMemberName] string type = "")
    {
        var sDict = AllID[type];
        if (sDict is FrozenDictionary<ushort, string> usDict)
        {
            return FrozenDictionary.ToFrozenDictionary(usDict, static x => (short)x.Key, static x => x.Value);
        }
        else if(sDict is FrozenDictionary<byte, string> bDict)
        {
            return FrozenDictionary.ToFrozenDictionary(bDict, static x => (short)x.Key, static x => x.Value);
        }
        return (FrozenDictionary<short, string>)sDict;
    }
}
