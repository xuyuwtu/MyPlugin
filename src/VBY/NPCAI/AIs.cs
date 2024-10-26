using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace VBY.NPCAI;
public static partial class AIs
{
    public static void AI(NPC npc) => _NPCAIs[npc.aiStyle].Invoke(npc);

    private static readonly Type dType = typeof(Action<NPC>);
    public static void SetMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_tempNPCAIs[index] is null)
        {
            _tempNPCAIs[index] = _NPCAIs[index];
            _NPCAIs[index] = (Action<NPC>)Delegate.CreateDelegate(dType, method);
            //Console.WriteLine($"aiStyle:{index} add success. method:{method.Name}");
        }
    }
    public static void SetMethod(Action<NPC> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_tempNPCAIs[index] is null)
        {
            _tempNPCAIs[index] = _NPCAIs[index];
            _NPCAIs[index] = action;
            //Console.WriteLine($"aiStyle:{index} add success. method:{action.Method.Name}");
        }
    }
    public static void RemoveMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_NPCAIs[index].Method == method)
        {
            _NPCAIs[index] = _tempNPCAIs[index]!;
            _tempNPCAIs[index] = null; 
            //Console.WriteLine($"aiStyle:{index} remove success. method:{method.Name}");
        }
    }
    public static void RemoveMethod(Action<NPC> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_NPCAIs[index].Method == action.Method)
        {
            _NPCAIs[index] = _tempNPCAIs[index]!;
            _tempNPCAIs[index] = null;
            //Console.WriteLine($"aiStyle:{index} remove success. method:{action.Method.Name}");
        }
    }
    internal static Action<NPC>[] _NPCAIs = new Action<NPC>[126];
    internal static Action<NPC>?[] _tempNPCAIs = new Action<NPC>[126];
    static AIs()
    {
        var type = typeof(NPC);
        var aiMethod = type.GetMethod(nameof(NPC.AI))!;
        var aiMethodBody = aiMethod.GetMethodBody()!;
        var instructions = EmitUtils.GetInstructionsFromBytes(aiMethodBody.GetILAsByteArray()!);
        EmitUtils.InstructionOperandTransform(type.Module, instructions);
        var aiStyleField = type.GetField(nameof(NPC.aiStyle))!;
        var findOpcodeFuncs = new Func<Instruction, bool>[]
        {
            static x => x.OpCode == ILOpCode.Ldarg_0,
            x => x.OpCode == ILOpCode.Ldfld && (FieldInfo)x.Operand! == aiStyleField,
            static x => (ushort)x.OpCode is (>= (ushort)ILOpCode.Ldc_i4_0 and <= (ushort)ILOpCode.Ldc_i4) or (ushort)ILOpCode.Brtrue
        };
        var foundIndices = new List<int>();
        var aiStyleValues = new List<int>();
        for (int i = 0; i < instructions.Count; i++)
        {
            var firstIns = instructions[i];
            if (!findOpcodeFuncs[0](firstIns))
            {
                continue;
            }
            var match = true;
            for (int j = 1; j < findOpcodeFuncs.Length; j++)
            {
                var checkIns = instructions[i + j];
                if (!findOpcodeFuncs[j](checkIns))
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                foundIndices.Add(i);
                var ldci4Ins = instructions[i + 2];
                int ldci4Value;
                if (ldci4Ins.OpCode == ILOpCode.Brtrue)
                {
                    ldci4Value = 0;
                }
                else if (ldci4Ins.OpCode == ILOpCode.Ldc_i4)
                {
                    ldci4Value = (int)ldci4Ins.Operand!;
                }
                else if (ldci4Ins.OpCode == ILOpCode.Ldc_i4_s)
                {
                    ldci4Value = (sbyte)ldci4Ins.Operand!;
                }
                else
                {
                    ldci4Value = ldci4Ins.OpCode - ILOpCode.Ldc_i4_0;
                }
                aiStyleValues.Add(ldci4Value);
                i += findOpcodeFuncs.Length - 1;
                if (ldci4Ins.OpCode == ILOpCode.Brtrue)
                {
                    foundIndices[^1]--;
                }
            }
        }
        var end = foundIndices.Count - 1;
        var ranges = new List<Range>(foundIndices.Count);
        for (int i = 0; i < end; i++)
        {
            // [ldarg.0] [ldfld aiStyle] [ldc.i4] [_](branch) [_](start) = +4
            ranges.Add(new(foundIndices[i] + 4, foundIndices[i + 1]));
        }
        ranges.Add(new(foundIndices[^1] + 4, instructions.Count));
        var aiActionParamTypes = new Type[] { type };
        var exceptionHandlerInfos = new ExceptionHandlerInfo[aiMethodBody.ExceptionHandlingClauses.Count];
        var exceptionIndices = new Queue<int>(aiMethodBody.ExceptionHandlingClauses.Count * 4);
        for (int i = 0; i < aiMethodBody.ExceptionHandlingClauses.Count; i++)
        {
            var handler = aiMethodBody.ExceptionHandlingClauses[i];
            var handlerInfo = new ExceptionHandlerInfo()
            {
                TryStartIndex = instructions.BinarySearch(InstructionOffsetCompare, handler.TryOffset),
                TryEndIndex = instructions.BinarySearch(InstructionEndOffsetCompare, handler.TryOffset + handler.TryLength),
                HandlerStartIndex = instructions.BinarySearch(InstructionOffsetCompare, handler.HandlerOffset),
                HandlerEndIndex = instructions.BinarySearch(InstructionEndOffsetCompare, handler.HandlerOffset + handler.HandlerLength),
                Flags = handler.Flags,
            };
            exceptionHandlerInfos[i] = handlerInfo;
            if (handler.Flags != ExceptionHandlingClauseOptions.Filter && handler.Flags != ExceptionHandlingClauseOptions.Finally)
            {
                handlerInfo.CatchType = handler.CatchType;
            }
            if (handler.Flags != ExceptionHandlingClauseOptions.Finally && handler.Flags != ExceptionHandlingClauseOptions.Clause)
            {
                throw new NotSupportedException(handler.Flags.ToString());
            }
            static int InstructionOffsetCompare(Instruction instruction, int value) => instruction.Offset.CompareTo(value);
            static int InstructionEndOffsetCompare(Instruction instruction, int value) => (instruction.Offset + instruction.GetSize()).CompareTo(value);
        }
        var tryBlocks = new List<TryBlockInfo>();
        var groups = exceptionHandlerInfos.GroupBy(static x => x.TryStartIndex);
        var offsetComparer = new FuncComparer<Instruction>(static (x, y) => x!.Offset.CompareTo(y!.Offset));
        foreach (var infos in groups)
        {
            if (infos.Count() == 1)
            {
                var info = infos.First();
                if (info.Flags == ExceptionHandlingClauseOptions.Clause)
                {
                    var block = new TryBlockInfo() { TryStartIndex = info.TryStartIndex, TryEndIndex = info.TryEndIndex };
                    tryBlocks.Add(block);
                    block.Catch.Add((info.HandlerStartIndex, info.HandlerEndIndex, info.CatchType!));
                    tryBlocks.Add(block);
                }
                else if (info.Flags == ExceptionHandlingClauseOptions.Finally)
                {
                    tryBlocks.Add(new TryBlockInfo() { TryStartIndex = info.TryStartIndex, TryEndIndex = info.TryEndIndex, Finally = (info.HandlerStartIndex, info.HandlerEndIndex) });
                }
                else
                {
                    throw new NotSupportedException(info.Flags.ToString());
                }
            }
            else
            {
                var info2s = infos.ToArray();
                Array.Sort(info2s, static (x, y) =>
                {
                    var result = x.TryStartIndex.CompareTo(y.TryStartIndex);
                    if (result == 0)
                    {
                        result = x.TryEndIndex.CompareTo(y.TryEndIndex);
                    }
                    if (result == 0)
                    {
                        result = x.HandlerStartIndex.CompareTo(y.HandlerStartIndex);
                    }
                    if (result == 0)
                    {
                        result = x.HandlerEndIndex.CompareTo(y.HandlerEndIndex);
                    }
                    return result;
                });
                for (int i = 0; i < info2s.Length; i++)
                {
                    if (info2s[i].Flags == ExceptionHandlingClauseOptions.Filter && i + 1 != info2s.Length)
                    {
                        throw new InvalidDataException("invalid exceptionHandler. Filter must at last");
                    }
                }
                var info = info2s[0];
                var block = new TryBlockInfo() { TryStartIndex = info.TryStartIndex, TryEndIndex = info.TryEndIndex };
                exceptionIndices.Enqueue(block.TryStartIndex);
                end = info2s.Length;
                if (info2s[^1].Flags == ExceptionHandlingClauseOptions.Filter)
                {
                    end--;
                }
                for (int i = 0; i < end; i++)
                {
                    info = info2s[i];
                    block.Catch.Add((info.HandlerStartIndex, info.HandlerEndIndex, info.CatchType!));
                    exceptionIndices.Enqueue(info.HandlerStartIndex);
                }
                if (end != info2s.Length)
                {
                    info = info2s[end];
                    block.Finally = (info.HandlerStartIndex, info.HandlerEndIndex);
                    exceptionIndices.Enqueue(info.HandlerStartIndex);
                }
                tryBlocks.Add(block);
                exceptionIndices.Enqueue(block.EndIndex);
            }
        }
        var hasExceptionIndex = exceptionIndices.Count != 0;
        for (int i = 0; i < ranges.Count; i++)
        {
            var aiStyle = aiStyleValues[i];
            var rangeInstructions = CollectionsMarshal.AsSpan(instructions)[ranges[i]];
            if (rangeInstructions is [{ OpCode: ILOpCode.Ldarg_0 }, { OpCode: ILOpCode.Callvirt }, _])
            {
                _NPCAIs[aiStyle] = (Action<NPC>)Delegate.CreateDelegate(typeof(Action<NPC>), null, (MethodInfo)rangeInstructions[1].Operand!);
                continue;
            }
            var rangeLastInstruction = rangeInstructions[^1];
            var rangeLastIsRet = rangeLastInstruction.OpCode == ILOpCode.Ret;
            var rangeLastIsBranchToRet = EmitUtils.GetOpCode(rangeLastInstruction.OpCode).OperandType is OperandType.InlineBrTarget or OperandType.ShortInlineBrTarget && ((Instruction)rangeLastInstruction.Operand!).OpCode == ILOpCode.Ret;
            if (!rangeLastIsRet && !rangeLastIsBranchToRet)
            {
                throw new InvalidDataException("!rangeLastIsRet && !rangeLastIsBranchToRet");
            }
            var retInstruction = rangeLastIsRet ? rangeLastInstruction : new Instruction(ILOpCode.Ret, null, rangeLastInstruction.Offset + rangeLastInstruction.GetSize());
            var newAiMethod = new DynamicMethod($"AI_{aiStyle.ToString().PadLeft(3, '0')}", typeof(void), aiActionParamTypes);
            var oplocInstructions = new List<Instruction>();
            foreach (var instruction in rangeInstructions)
            {
                if (instruction.OpCode is (>= ILOpCode.Ldloc_0 and <= ILOpCode.Ldloc_3) or ILOpCode.Ldloc_s or ILOpCode.Ldloca_s or ILOpCode.Ldloc or ILOpCode.Ldloca
                                        or (>= ILOpCode.Stloc_0 and <= ILOpCode.Stloc_3) or ILOpCode.Stloc_s or ILOpCode.Stloc)
                {
                    oplocInstructions.Add(instruction);
                }
            }
            var il = newAiMethod.GetILGenerator();
            var locBuilders = new Dictionary<int, LocalBuilder>();
            var localVars = aiMethodBody.LocalVariables;
            var selLocIndices = oplocInstructions.Select(EmitUtils.GetLocIndex);
            foreach (var index in selLocIndices)
            {
                if (!locBuilders.TryGetValue(index, out var builder))
                {
                    locBuilders[index] = il.DeclareLocal(localVars[index].LocalType);
                }
            }
            var hasLeaveOrCondBrToRet = false;
            var branchTargetInstructions = new List<Instruction>();
            foreach (var instruction in rangeInstructions)
            {
                if (instruction.IsBranchTarget)
                {
                    branchTargetInstructions.Add(instruction);
                }
                if (!hasLeaveOrCondBrToRet 
                && (instruction is { OpCode: ILOpCode.Leave or ILOpCode.Leave_s, Operand: Instruction targetInstruction } 
                    && targetInstruction.Offset == instructions[^1].Offset || EmitUtils.GetOpCode(instruction.OpCode).FlowControl == FlowControl.Cond_Branch))
                {
                    hasLeaveOrCondBrToRet = true;
                }
            }
            if ((rangeLastIsBranchToRet || hasLeaveOrCondBrToRet) && !retInstruction.IsBranchTarget)
            {
                retInstruction.IsBranchTarget = true;
                branchTargetInstructions.Add(retInstruction);
            }
            var branchTargetLabels = new Label[branchTargetInstructions.Count];
            for (int j = 0; j < branchTargetInstructions.Count; j++)
            {
                branchTargetLabels[j] = il.DefineLabel();
            }
            var hasException = false;
            var curRange = ranges[i];
            foreach (var handlerInfo in exceptionHandlerInfos)
            {
                if (curRange.Start.Value <= handlerInfo.TryStartIndex && handlerInfo.HandlerEndIndex < curRange.End.Value)
                {
                    hasException = true;
                    break;
                }
            }
            if (hasException)
            {
                if (hasExceptionIndex && exceptionIndices.Peek() == curRange.Start.Value + i)
                {
                    var index = exceptionIndices.Dequeue();
                    foreach (var block in tryBlocks)
                    {
                        if (index == block.TryStartIndex)
                        {
                            il.BeginExceptionBlock();
                            break;
                        }
                        foreach (var catchInfo in block.Catch)
                        {
                            if (index == catchInfo.startIndex)
                            {
                                il.BeginCatchBlock(catchInfo.catchType);
                                break;
                            }
                        }
                        if (block.Finally.HasValue && index == block.Finally.Value.startIndex)
                        {
                            il.BeginFinallyBlock();
                        }
                        else if (index == block.EndIndex)
                        {
                            il.EndExceptionBlock();
                        }
                    }
                    hasExceptionIndex = exceptionIndices.Count != 0;
                }
            }
            end = rangeInstructions.Length - 1;
            if (rangeLastIsBranchToRet)
            {
                end++;
            }
            var addedIns = new List<Instruction>();
            for (int j = 0; j < end; j++)
            {
                var instruction = rangeInstructions[j];
                addedIns.Add(instruction);
                if (instruction.IsBranchTarget)
                {
                    il.MarkLabel(branchTargetLabels[branchTargetInstructions.BinarySearch(instruction, offsetComparer)]);
                }
                if (instruction.OpCode is (>= ILOpCode.Ldloc_0 and <= ILOpCode.Ldloc_3) or ILOpCode.Ldloc_s or ILOpCode.Ldloca_s or ILOpCode.Ldloc or ILOpCode.Ldloca)
                {
                    var newIndex = locBuilders[EmitUtils.GetLocIndex(instruction)].LocalIndex;
                    if (instruction.OpCode is ILOpCode.Ldloca_s or ILOpCode.Ldloca)
                    {
                        if (newIndex <= byte.MaxValue)
                        {
                            il.Emit(OpCodes.Ldloca_S, (byte)newIndex);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldloca, (ushort)newIndex);
                        }
                    }
                    else
                    {
                        if (newIndex <= 3)
                        {
                            il.Emit(EmitUtils.GetOpCode((ILOpCode)((ushort)ILOpCode.Ldloc_0 + newIndex)));
                        }
                        else if (newIndex <= byte.MaxValue)
                        {
                            il.Emit(OpCodes.Ldloc_S, (byte)newIndex);
                        }
                        else
                        {
                            il.Emit(OpCodes.Ldloc, (ushort)newIndex);
                        }
                    }
                }
                else if (instruction.OpCode is (>= ILOpCode.Stloc_0 and <= ILOpCode.Stloc_3) or ILOpCode.Stloc_s or ILOpCode.Stloc)
                {
                    var newIndex = locBuilders[EmitUtils.GetLocIndex(instruction)].LocalIndex;
                    if (newIndex <= 3)
                    {
                        il.Emit(EmitUtils.GetOpCode((ILOpCode)((ushort)ILOpCode.Stloc_0 + newIndex)));
                    }
                    else if (newIndex <= byte.MaxValue)
                    {
                        il.Emit(OpCodes.Stloc_S, (byte)newIndex);
                    }
                    else
                    {
                        il.Emit(OpCodes.Stloc, (ushort)newIndex);
                    }
                }
                else
                {
                    var opcode = EmitUtils.GetOpCode(instruction.OpCode);
                    switch (opcode.OperandType)
                    {
                        case OperandType.InlineField:
                            il.Emit(opcode, (FieldInfo)instruction.Operand!);
                            break;
                        case OperandType.InlineI:
                            il.Emit(opcode, (int)instruction.Operand!);
                            break;
                        case OperandType.InlineI8:
                            il.Emit(opcode, (long)instruction.Operand!);
                            break;
                        case OperandType.InlineMethod:
                            if (instruction.OpCode == ILOpCode.Newobj)
                            {
                                il.Emit(opcode, (ConstructorInfo)instruction.Operand!);
                            }
                            else
                            {
                                var methodBase = (MethodBase)instruction.Operand!;
                                if (methodBase.IsConstructor)
                                {
                                    il.Emit(opcode, (ConstructorInfo)instruction.Operand!);
                                }
                                else
                                {
                                    il.Emit(opcode, (MethodInfo)instruction.Operand!);
                                }
                            }
                            break;
                        case OperandType.InlineNone:
                            il.Emit(opcode);
                            break;
                        case OperandType.InlineR:
                            il.Emit(opcode, (double)instruction.Operand!);
                            break;
                        case OperandType.InlineSig:
                            throw new NotSupportedException();
                        case OperandType.InlineString:
                            il.Emit(opcode, (string)instruction.Operand!);
                            break;
                        case OperandType.InlineSwitch:
                            il.Emit(opcode, ((Instruction[])instruction.Operand!).Select(x => branchTargetLabels[branchTargetInstructions.BinarySearch(x, offsetComparer)]).ToArray());
                            break;
                        case OperandType.InlineTok:
                            switch (instruction.Operand)
                            {
                                case FieldInfo:
                                    il.Emit(opcode, (FieldInfo)instruction.Operand);
                                    break;
                                case MethodInfo:
                                    il.Emit(opcode, (MethodInfo)instruction.Operand);
                                    break;
                                case Type:
                                    il.Emit(opcode, (Type)instruction.Operand);
                                    break;
                                default:
                                    throw new InvalidDataException(nameof(OperandType.InlineTok));
                            }
                            break;
                        case OperandType.InlineType:
                            il.Emit(opcode, (Type)instruction.Operand!);
                            break;
                        case OperandType.InlineVar:
                            il.Emit(opcode, (ushort)instruction.Operand!);
                            break;
                        case OperandType.InlineBrTarget:
                        case OperandType.ShortInlineBrTarget:
                            if (((Instruction)instruction.Operand!).Offset == instructions[^1].Offset)
                            {
                                if (instruction.OpCode is ILOpCode.Leave or ILOpCode.Leave_s || EmitUtils.GetOpCode(instruction.OpCode).FlowControl == FlowControl.Cond_Branch)
                                {
                                    il.Emit(opcode, branchTargetLabels[branchTargetInstructions.BinarySearch(retInstruction, offsetComparer)]);
                                }
                                else
                                {
                                    il.Emit(OpCodes.Ret);
                                }
                            }
                            else
                            {
                                il.Emit(opcode, branchTargetLabels[branchTargetInstructions.BinarySearch((Instruction)instruction.Operand, offsetComparer)]);
                            }
                            break;
                        case OperandType.ShortInlineR:
                            il.Emit(opcode, (float)instruction.Operand!);
                            break;
                        case OperandType.ShortInlineI:
                        case OperandType.ShortInlineVar:
                            il.Emit(opcode, (sbyte)instruction.Operand!);
                            break;
                    }
                }
            }
            if (retInstruction.IsBranchTarget)
            {
                il.MarkLabel(branchTargetLabels[branchTargetInstructions.BinarySearch(retInstruction, offsetComparer)]);
            }
            il.Emit(EmitUtils.GetOpCode(retInstruction.OpCode));
            addedIns.Add(retInstruction);
            _NPCAIs[aiStyle] = (Action<NPC>)newAiMethod.CreateDelegate(typeof(Action<NPC>));
        }
    }
}
class ExceptionHandlerInfo
{
    public int TryStartIndex;
    public int TryEndIndex;
    public int HandlerStartIndex;
    public int HandlerEndIndex;
    public ExceptionHandlingClauseOptions Flags;
    public Type? CatchType;
}
class TryBlockInfo
{
    public int TryStartIndex;
    public int TryEndIndex;
    public List<(int startIndex, int endIndex, Type catchType)> Catch = new();
    public (int startIndex, int endIndex)? Finally;
    public int EndIndex => Finally?.endIndex ?? Catch[^1].endIndex;
}
class FuncComparer<T> : IComparer<T>
{
    private readonly Func<T?, T?, int> _comparison;
    public FuncComparer(Func<T?, T?, int> comparison)
    {
        ArgumentNullException.ThrowIfNull(comparison);
        _comparison = comparison;
    }
    public int Compare(T? x, T? y) => _comparison(x!, y!);
}
[DebuggerDisplay("OpCode = {OpCode}, Offset = {Offset}")]
public class Instruction
{
    public ILOpCode OpCode;
    public object? Operand;
    public int Offset;
    public bool IsBranchTarget;

    public Instruction(ILOpCode opCode, object? operand = null)
    {
        OpCode = opCode;
        Operand = operand;
    }
    public Instruction(ILOpCode opCode, object? operand, int offset) : this(opCode, operand)
    {
        Offset = offset;
    }
    public int GetSize()
    {
        var opcode = EmitUtils.GetOpCode(OpCode);
        int size = opcode.Size;

        return opcode.OperandType switch
        {
            OperandType.InlineSwitch => size + (1 + ((Array)Operand!).Length) * 4,
            OperandType.InlineI8 or OperandType.InlineR => size + 8,
            OperandType.InlineBrTarget or OperandType.InlineField or OperandType.InlineI or OperandType.InlineMethod or OperandType.InlineString 
            or OperandType.InlineTok or OperandType.InlineType or OperandType.ShortInlineR or OperandType.InlineSig => size + 4,
            OperandType.InlineVar => size + 2,
            OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => size + 1,
            _ => size,
        };
    }
}

static class EmitUtils
{
    private static OpCode?[]? shortOpCodes;
    private static OpCode?[]? largeOpCodes;
    public static OpCode?[] ShortOpCodes
    {
        get
        {
            if (shortOpCodes is null)
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
        Array.Sort(opcodes, static (x, y) => ((ushort)x.Value).CompareTo((ushort)y.Value));
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
            var opcode = opcodeValue == 0xFE ? LargeOpCodes[br.ReadByte()]!.Value : ShortOpCodes[opcodeValue]!.Value;
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
            var opcode = GetOpCode(instruction.OpCode);
            switch (opcode.OperandType)
            {
                case OperandType.InlineBrTarget:
                    {
                        instruction.Operand = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + (int)instruction.Operand!)];
                        ((Instruction)instruction.Operand).IsBranchTarget = true;
                    }
                    break;
                case OperandType.ShortInlineBrTarget:
                    {
                        instruction.Operand = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + (sbyte)instruction.Operand!)];
                        ((Instruction)instruction.Operand).IsBranchTarget = true;
                    }
                    break;
                case OperandType.InlineSwitch:
                    {
                        instruction.Operand = ((int[])instruction.Operand!).Select(offset =>
                        {
                            var targetInstruction = instructions[instructions.BinarySearch(static (x, value) => x.Offset.CompareTo(value), instruction.Offset + instruction.GetSize() + offset)];
                            targetInstruction.IsBranchTarget = true;
                            return targetInstruction;
                        }).ToArray();
                    }
                    break;
                case OperandType.InlineField:
                    instruction.Operand = module.ResolveField((int)instruction.Operand!);
                    break;
                case OperandType.InlineMethod:
                    instruction.Operand = module.ResolveMethod((int)instruction.Operand!);
                    break;
#pragma warning disable CS0618 // 类型或成员已过时
                case OperandType.InlinePhi:
                    throw new NotSupportedException(nameof(OperandType.InlinePhi));
#pragma warning restore CS0618 // 类型或成员已过时
                case OperandType.InlineSig:
                    instruction.Operand = module.ResolveSignature((int)instruction.Operand!);
                    break;
                case OperandType.InlineString:
                    instruction.Operand = module.ResolveString((int)instruction.Operand!);
                    break;
                case OperandType.InlineTok:
                    instruction.Operand = module.ResolveMember((int)instruction.Operand!);
                    break;
                case OperandType.InlineType:
                    instruction.Operand = module.ResolveType((int)instruction.Operand!);
                    break;
            }
        }
    }
    public static OpCode GetOpCode(ILOpCode code) => (ushort)code >= 0xFE00 ? LargeOpCodes[(ushort)code & byte.MaxValue]!.Value : ShortOpCodes[(int)code]!.Value;
    public static int GetLocIndex(Instruction instruction) => instruction.OpCode switch
    {
        ILOpCode.Ldloc_0 or ILOpCode.Ldloc_1 or ILOpCode.Ldloc_2 or ILOpCode.Ldloc_3 => instruction.OpCode - ILOpCode.Ldloc_0,
        ILOpCode.Stloc_0 or ILOpCode.Stloc_1 or ILOpCode.Stloc_2 or ILOpCode.Stloc_3 => instruction.OpCode - ILOpCode.Stloc_0,
        ILOpCode.Ldloc_s or ILOpCode.Ldloca_s or ILOpCode.Stloc_s => (byte)instruction.Operand!,
        ILOpCode.Ldloc or ILOpCode.Ldloca or ILOpCode.Stloc => (ushort)instruction.Operand!,
        _ => throw new NotSupportedException(instruction.OpCode.ToString()),
    };
}
static class IListExtensions
{
    public static int BinarySearch<T, TValue>(this IList<T> values, Func<T, TValue, int> comparison, TValue value)
    {
        int lo = 0;
        int hi = values.Count - 1;
        while (lo <= hi)
        {
            int i = lo + ((hi - lo) >> 1);
            int order = comparison(values[i], value);
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