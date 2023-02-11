using System.Collections.Generic;
using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

/*
 * 带刀船员职业示例
 */
public class Swordsman : Role, IListener
{
    private static readonly List<byte> KilledOnes = new();
    public Swordsman() : base(114514, CustomRoles.Swordsman)
    {
        Color = "#ffcc00";
        HasTask = false;
        BaseRole = RoleTypes.Impostor;
        CanKill = true;
        CurrentKillCooldown = 1;
        DisplayName = "侠客";
        Info = "行侠仗义！";
        Description = "(船员阵营):\n你可以正义击杀任何人。";
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (target == null || killer == null) return true;

        if (KilledOnes.Contains(killer.PlayerId))
        {
            killer.RpcGuardAndKill(target);
            return false;
        }

        if (killer.GetCustomRole() == CustomRoles.Swordsman && !KilledOnes.Contains(killer.PlayerId)) KilledOnes.Add(killer.PlayerId);
        /* 侠客只能出刀一次 */

        if (target.GetCustomRole() == CustomRoles.SchrodingerCat && target.GetCustomRole() == CustomRoles.Swordsman)
        {
            killer.RpcGuardAndKill(target);
            target.RpcSetCustomRole(CustomRoles.CSchrodingerCat);
            /* 侠客刀薛定谔的猫会变成船员阵营 */
            return false;
        }

        return true;
    }

    public void OnGameStarted(AmongUsClient client)
    {
        KilledOnes.Clear();
    }
}