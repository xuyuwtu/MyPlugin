using System.Diagnostics;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace VBY.Reflection.Emit;
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
            OperandType.InlineBrTarget or OperandType.InlineField or OperandType.InlineI or OperandType.InlineMethod or OperandType.InlineString or OperandType.InlineTok or OperandType.InlineType or OperandType.ShortInlineR or OperandType.InlineSig => size + 4,
            OperandType.InlineVar => size + 2,
            OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => size + 1,
            _ => size,
        };
    }
}
public class InstructionOffsetComparer : IComparer<Instruction>
{
    public static InstructionOffsetComparer Instance { get; } = new();
    public int Compare(Instruction? x, Instruction? y) => x!.Offset.CompareTo(y!.Offset);
}
