using System.Collections.Generic;

namespace TownOfHost.Util;

public static class PlayerUtils
{
    public static readonly Dictionary<byte, byte> PlayersWithRpc = new();

    public static bool IsUsingAum(byte callId)
    {
        return callId == 85;
    }
}