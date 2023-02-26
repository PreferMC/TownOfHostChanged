using System.Collections.Generic;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Mourner : Role, IListener
{
    private static readonly List<byte> KilledPlayer = new();

    public Mourner() : base(102554, CustomRoles.Mourner)
    {
        Color = "#ff0000";
        Group = TabGroup.ImpostorRoles;
        CanUseVent = true;
        CanKill = true;
        CurrentKillCooldown = 20f;
        HasVision = true;
        DisplayName = "送葬者";
        Info = "尸骨无存！";
        Description = "(内鬼阵营):\n你杀死的玩家他人无法报告尸体。";
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer == null || target == null) return true;

        if (killer.GetCustomRole() == CustomRole && !KilledPlayer.Contains(target.PlayerId)) KilledPlayer.Add(target.PlayerId);
        return true;
    }

    public bool OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (reporter == null || target == null!) return true;

        if (KilledPlayer.Contains(target.PlayerId)) return false;

        return true;
    }

    public void OnGameStarted(AmongUsClient client)
    {
        KilledPlayer.Clear();
    }
}