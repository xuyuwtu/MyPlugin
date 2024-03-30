using System.Runtime.CompilerServices;

using Terraria.ID;
using Terraria.Localization;

namespace Terraria;

public static class NetMessageSender
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Hello() => NetMessage.SendData(MessageID.Hello);
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Kick(NetworkText? text = null) => NetMessage.SendData(MessageID.Kick, text: text);
}
