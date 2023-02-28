using System.Reflection.Emit;

namespace VBY.Basic.Emit;

public class EmitHelper
{
    public static void Ldc_I4(ILGenerator il, int i4)
    {
        switch (i4)
        {
            case 0:
                il.Emit(OpCodes.Ldc_I4_0);
                break;
            case 1:
                il.Emit(OpCodes.Ldc_I4_1);
                break;
            case 2:
                il.Emit(OpCodes.Ldc_I4_2);
                break;
            case 3:
                il.Emit(OpCodes.Ldc_I4_3);
                break;
            case 4:
                il.Emit(OpCodes.Ldc_I4_4);
                break;
            case 5:
                il.Emit(OpCodes.Ldc_I4_5);
                break;
            case 6:
                il.Emit(OpCodes.Ldc_I4_6);
                break;
            case 7:
                il.Emit(OpCodes.Ldc_I4_7);
                break;
            case 8:
                il.Emit(OpCodes.Ldc_I4_8);
                break;
            default:
                if (byte.TryParse(i4.ToString(), out var result))
                    il.Emit(OpCodes.Ldc_I4_S, result);
                else
                    il.Emit(OpCodes.Ldc_I4, i4);
                break;
        }
    }
}
public static class Extensions
{
    public static void EmitNewarr(this ILGenerator il, Type type, int count)
    {
        EmitHelper.Ldc_I4(il, count);
        il.Emit(OpCodes.Newarr, type);
    }
}