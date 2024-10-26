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
    private static Func<Module, object, object>?[]? shortOpcodeTransformFunc;
    public static Func<Module, object, object>?[] ShortOpcodeTransformFunc
    {
        get
        {
            if(shortOpcodeTransformFunc is null)
            {
                InitOpcodes();
            }
            return shortOpcodeTransformFunc;
        }
    }
    private static Func<Module, object, object>?[]? largeOpcodeTransformFunc;
    public static Func<Module, object, object>?[] LargeOpcodeTransformFunc
    {
        get
        {
            if(largeOpcodeTransformFunc is null)
            {
                InitOpcodes();
            }
            return largeOpcodeTransformFunc;
        }
    }
    [MemberNotNull(nameof(shortOpCodes), nameof(largeOpCodes), nameof(shortOpcodeTransformFunc), nameof(largeOpcodeTransformFunc))]
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
        shortOpcodeTransformFunc = new Func<Module, object, object>?[shortOpCodes.Length];
        largeOpcodeTransformFunc = new Func<Module, object, object>?[largeOpCodes.Length];
        var resolveField = static (Module module, object operand) => module.ResolveField((int)operand)!;
        var resolveMember = static (Module module, object operand) => module.ResolveMember((int)operand)!;
        var resolveMethod = static (Module module, object operand) => module.ResolveMethod((int)operand)!;
        var resolveSignature = static (Module module, object operand) => module.ResolveSignature((int)operand)!;
        var resolveString = static (Module module, object operand) => module.ResolveString((int)operand)!;
        var resolveType = static (Module module, object operand) => module.ResolveType((int)operand)!;
        foreach(var code in new ILOpCode[] { ILOpCode.Ldfld, ILOpCode.Ldflda, ILOpCode.Ldsfld, ILOpCode.Ldsflda })
        {
            shortOpcodeTransformFunc[(int)code] = resolveField;
        }
        foreach(var code in new ILOpCode[] { ILOpCode.Call, ILOpCode.Callvirt })
        {
            shortOpcodeTransformFunc[(int)code] = resolveMethod;
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
                case ILOpCode.Ldc_i4_m1:
                case ILOpCode.Ldc_i4_0:
                case ILOpCode.Ldc_i4_1:
                case ILOpCode.Ldc_i4_2:
                case ILOpCode.Ldc_i4_3:
                case ILOpCode.Ldc_i4_4:
                case ILOpCode.Ldc_i4_5:
                case ILOpCode.Ldc_i4_6:
                case ILOpCode.Ldc_i4_7:
                case ILOpCode.Ldc_i4_8:
                case ILOpCode.Ldc_i4:
                case ILOpCode.Dup:
                case ILOpCode.Pop:
                case ILOpCode.Ret:
                    result.Add(new(opcode, null, offset));
                    offset += GetOpCode(opcode).Size;
                    break;
                case ILOpCode.Ldc_i4_s:
                case ILOpCode.Brtrue_s:
                case ILOpCode.Brfalse_s:
                    result.Add(new(opcode, br.ReadSByte(), offset));
                    offset += GetOpCode(opcode).Size + 1;
                    break;
                case ILOpCode.Brfalse:
                case ILOpCode.Brtrue:
                    result.Add(new(opcode, br.ReadInt32(), offset));
                    offset += GetOpCode(opcode).Size + 4;
                    break;
                case ILOpCode.Call:
                case ILOpCode.Newobj:
                case ILOpCode.Ldftn:
                    result.Add(new(opcode, module.ResolveMethod(br.ReadInt32()), offset));
                    offset += GetOpCode(opcode).Size + 4;
                    break;
                case ILOpCode.Ldfld:
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
            var instruction = new Instruction(default, null, (int)ms.Position);
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
                default:
                    throw new NotSupportedException(opcode.OperandType.ToString());
            }
            result.Add(instruction);
        }
        return result;
    }
    public static void InstructionOperandTransform(Module module, List<Instruction> instructions)
    {
        foreach (var instruction in instructions)
        {
            if((ushort)instruction.OpCode >= 0xFE00)
            {
                if (LargeOpcodeTransformFunc[(ushort)instruction.OpCode - 0xFE00] is { } func)
                {
                    instruction.Operand = func(module, instruction.Operand!);
                }
            }
            else
            {
                if (ShortOpcodeTransformFunc[(ushort)instruction.OpCode] is { } func)
                {
                    instruction.Operand = func(module, instruction.Operand!);
                }
            }
            var opcode = GetOpCode(instruction.OpCode);
            if (opcode.OperandType == OperandType.InlineBrTarget)
            {
                instruction.Operand = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + (int)instruction.Operand!)];
                ((Instruction)instruction.Operand).IsBranchTarget = true;
            }
            else if (opcode.OperandType == OperandType.ShortInlineBrTarget)
            {
                instruction.Operand = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + (sbyte)instruction.Operand!)];
                ((Instruction)instruction.Operand).IsBranchTarget = true;
            }
            else if (opcode.OperandType == OperandType.InlineSwitch)
            {
                instruction.Operand = ((int[])instruction.Operand!).Select(offset => 
                {
                    var targetInstruction = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + offset)];
                    targetInstruction.IsBranchTarget = true;
                    return targetInstruction;
                }).ToArray();
            }
        }
    }
    public static OpCode GetOpCode(ILOpCode code)
    {
        if ((ushort)code >= 0xFE00)
        {
            return LargeOpCodes[(ushort)code - 0xFE00]!.Value;
        }
        else
        {
            return ShortOpCodes[(int)code]!.Value;
        }
    }
    public static int GetLocIndex(Instruction instruction)
    {
        switch (instruction.OpCode)
        {
            case ILOpCode.Ldloc_0:
            case ILOpCode.Ldloc_1:
            case ILOpCode.Ldloc_2:
            case ILOpCode.Ldloc_3:
                return instruction.OpCode - ILOpCode.Ldloc_0;
            case ILOpCode.Stloc_0:
            case ILOpCode.Stloc_1:
            case ILOpCode.Stloc_2:
            case ILOpCode.Stloc_3:
                return instruction.OpCode - ILOpCode.Stloc_0;
            case ILOpCode.Ldloc_s:
            case ILOpCode.Ldloca_s:
            case ILOpCode.Stloc_s:
                return (byte)instruction.Operand!;
            case ILOpCode.Ldloc:
            case ILOpCode.Ldloca:
            case ILOpCode.Stloc:
                return (ushort)instruction.Operand!;
            default:
                throw new NotSupportedException(instruction.OpCode.ToString());
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
static class Utils
{
    public static int BinarySearch<T, TValue>(this IList<T> values, Func<T, TValue, int> comparer, TValue value)
    {
        int lo = 0;
        int hi = values.Count - 1;
        while (lo <= hi)
        {
            int i = lo + ((hi - lo) >> 1);
            int order = comparer(values[i], value);
            if (order == 0)
            {
                return i;
            }
            if (order < 0)
            {
                lo = i + 1;
            }
            else
            {
                hi = i - 1;
            }
        }
        return ~lo;
    }
}