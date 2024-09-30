using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

using VBY.Reflection.Emit;

namespace System.Reflection.Emit;

public static class EmitUtils
{
    private static OpCode?[]? shortOpCodes;
    private static OpCode?[]? largeOpCodes;
    public static OpCode?[] ShortOpCodes
    {
        get 
        {
            if(shortOpCodes is null)
            {
                InitOpcodes();
            }
            return shortOpCodes;
        }
    }
    public static OpCode?[] LargeOpCodes
    {
        get
        {
            if (largeOpCodes is null)
            {
                InitOpcodes();
            }
            return largeOpCodes;
        }
    }
    [MemberNotNull(nameof(shortOpCodes), nameof(largeOpCodes))]
    private static void InitOpcodes()
    {
        var opcodes = typeof(OpCodes).GetFields().Where(x => x.FieldType == typeof(OpCode)).Select(x => (OpCode)x.GetValue(null)!).ToArray();
        Array.Sort(opcodes, OpCodeComparer.Instance);
        var index = Array.FindIndex(opcodes, opcode => opcode.Value < 0);
        shortOpCodes = new OpCode?[256];
        largeOpCodes = new OpCode?[opcodes.Where(x => x.Value < 0).Select(x => (byte)(ushort)x.Value).Max() + 1];
        for (int i = 0; i < index; i++)
        {
            shortOpCodes[opcodes[i].Value] = opcodes[i];
        }
        for (int i = index; i < opcodes.Length; i++)
        {
            largeOpCodes[(byte)(ushort)opcodes[i].Value] = opcodes[i];
        }
    }
    public static List<Instruction> GetInstructionsFromBytes(Module module, byte[] bytes)
    {
        if (bytes is null || bytes.Length == 0)
        {
            throw new ArgumentNullException(nameof(bytes));
        }
        var result = new List<Instruction>();
        var ms = new MemoryStream(bytes);
        var br = new BinaryReader(ms);
        int offset = 0;
        while (ms.Position < ms.Length)
        {
            var opcode = (ILOpCode)br.ReadByte();
            if (opcode == (ILOpCode)0xfe)
            {
                opcode = (ILOpCode)(0xfe00 | br.ReadByte());
            }
            switch (opcode)
            {
                case ILOpCode.Nop:
                case ILOpCode.Ldarg_0:
                case ILOpCode.Ldnull:
                case ILOpCode.Dup:
                case ILOpCode.Pop:
                case ILOpCode.Ret:
                    result.Add(new(opcode, null, offset));
                    offset += GetOpCode(opcode).Size;
                    break;
                case ILOpCode.Call:
                case ILOpCode.Newobj:
                case ILOpCode.Ldftn:
                    result.Add(new(opcode, module.ResolveMethod(br.ReadInt32()), offset));
                    offset += GetOpCode(opcode).Size + 4;
                    break;
                case ILOpCode.Brtrue_s:
                    result.Add(new(opcode, br.ReadSByte(), offset));
                    offset += GetOpCode(opcode).Size + 1;
                    break;
                case ILOpCode.Ldsfld:
                case ILOpCode.Stsfld:
                    result.Add(new(opcode, module.ResolveField(br.ReadInt32()), offset));
                    offset += GetOpCode(opcode).Size + 4;
                    break;
                default:
                    throw new InvalidOperationException(opcode.ToString());
            }
        }
        for(int i = 0; i < result.Count; i++)
        {
            if (result[i].OpCode.IsBranch())
            {
                var findOffset = result[i].Offset + GetOpCode(result[i].OpCode).Size;
                if (result[i].OpCode == ILOpCode.Brtrue_s) 
                {
                    findOffset += 1 + (sbyte)result[i].Operand!;
                }
                else
                {
                    throw new InvalidDataException($"'{result[i].OpCode}' is invalid opcode of can get branch offset");
                }
                var index = result.FindIndex(0, result.Count, x => x.Offset == findOffset);
                if(index == -1)
                {
                    throw new InvalidDataException($"can not find offset: 0x{findOffset:X}");
                }
                result[i].IsBranchTarget = true;
            }
        }
        return result;
    }
    public static List<Instruction> GetInstructionsFromBytes(byte[] bytes)
    {
        var ilByteArray = bytes;
        var ms = new MemoryStream(ilByteArray);
        var br = new BinaryReader(ms);
        var result = new List<Instruction>();
        while (ms.Position != ms.Length)
        {
            var instruction = new Instruction(default, null);
            var opcodeValue = br.ReadByte();
            OpCode opcode; 
            if (opcodeValue == 0xFE)
            {
                opcode = LargeOpCodes[br.ReadByte()]!.Value;
            }
            else
            {
                opcode = ShortOpCodes[opcodeValue]!.Value;
            }
            instruction.OpCode = (ILOpCode)opcode.Value;
            switch (opcode.OperandType)
            {
                case OperandType.InlineBrTarget:
                case OperandType.InlineField:
                case OperandType.InlineMethod:
                case OperandType.InlineI:
                case OperandType.InlineSig:
                case OperandType.InlineString:
                case OperandType.InlineTok:
                case OperandType.InlineType:
                    instruction.Operand = br.ReadInt32();
                    break;
                case OperandType.InlineI8:
                    instruction.Operand = br.ReadInt64();
                    break;
                case OperandType.InlineNone:
                    break;
#pragma warning disable CS0618 // 类型或成员已过时
                case OperandType.InlinePhi:
                    throw new NotSupportedException(nameof(OperandType.InlinePhi));
#pragma warning restore CS0618 // 类型或成员已过时
                case OperandType.InlineR:
                    instruction.Operand = br.ReadDouble();
                    break;
                case OperandType.InlineSwitch:
                    var count = br.ReadUInt32();
                    var targets = new int[count];
                    for (int i = 0; i < count; i++)
                    {
                        targets[i] = br.ReadInt32();
                    }
                    instruction.Operand = targets;
                    break;
                case OperandType.InlineVar:
                    instruction.Operand = br.ReadUInt16();
                    break;
                case OperandType.ShortInlineBrTarget:
                case OperandType.ShortInlineI:
                    instruction.Operand = br.ReadSByte();
                    break;
                case OperandType.ShortInlineR:
                    instruction.Operand = br.ReadSingle();
                    break;
                case OperandType.ShortInlineVar:
                    instruction.Operand = br.ReadByte();
                    break;
            }
            result.Add(instruction);
        }
        return result;
    }
    public static OpCode GetOpCode(ILOpCode code)
    {
        switch (code)
        {
            case ILOpCode.Nop:
                return OpCodes.Nop;
            case ILOpCode.Ldarg_0:
                return OpCodes.Ldarg_0;
            case ILOpCode.Ldnull:
                return OpCodes.Ldnull;
            case ILOpCode.Dup:
                return OpCodes.Dup;
            case ILOpCode.Pop:
                return OpCodes.Pop;
            case ILOpCode.Call:
                return OpCodes.Call;
            case ILOpCode.Ret:
                return OpCodes.Ret;
            case ILOpCode.Brtrue_s:
                return OpCodes.Brtrue_S;
            case ILOpCode.Newobj:
                return OpCodes.Newobj;
            case ILOpCode.Ldsfld:
                return OpCodes.Ldsfld;
            case ILOpCode.Stsfld:
                return OpCodes.Stsfld;
            case ILOpCode.Ldftn:
                return OpCodes.Ldftn;
            default:
                throw new InvalidDataException(code.ToString());
        }
    }
    public static void Emit(this ILGenerator il, ILOpCode code, object? operand)
    {
        switch (code)
        {
            case ILOpCode.Nop:
            case ILOpCode.Ldarg_0:
            case ILOpCode.Ldnull:
            case ILOpCode.Dup:
            case ILOpCode.Pop:
            case ILOpCode.Ret:
                il.Emit(GetOpCode(code));
                break;
            case ILOpCode.Call:
            case ILOpCode.Ldftn:
                il.Emit(GetOpCode(code), (MethodInfo)operand!);
                break;
            case ILOpCode.Brtrue_s:
                il.Emit(GetOpCode(code), (Label)operand!);
                break;
            case ILOpCode.Newobj:
                il.Emit(GetOpCode(code), (ConstructorInfo)operand!);
                break;
            case ILOpCode.Ldsfld:
            case ILOpCode.Stsfld:
                il.Emit(GetOpCode(code), (FieldInfo)operand!);
                break;
            default:
                throw new InvalidOperationException(code.ToString());
        }
    }
    public static void Emit(ILGenerator il, Instruction instruction) => Emit(il, instruction.OpCode, instruction.Operand);
    private class OpCodeComparer : IComparer<OpCode>
    {
        public static OpCodeComparer Instance = new OpCodeComparer();
        private OpCodeComparer() { }
        public int Compare(OpCode x, OpCode y) => ((ushort)x.Value).CompareTo((ushort)y.Value);
    }
}
