using System.Reflection.Metadata;

namespace VBY.Reflection.Emit;

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
}
