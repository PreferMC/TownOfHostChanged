using System.Collections.Generic;
using AmongUs.GameOptions;
using InnerNet;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Sidekick : Role, IListener
{
    private static readonly List<byte> RecruitedPlayers = new();
    private static readonly Dictionary<byte, byte> PlayerInfo = new(); // 第一个是招募的人，第二个是被招募的

    public void OnGameStarted(AmongUsClient client)
    {
        RecruitedPlayers.Clear();
        PlayerInfo.Clear();
    }

    public Sidekick() : base(88888, CustomRoles.Sidekick)
    {
        Color = "#00b4eb";
        CanUseVent = true;
        HasVision = true;
        Group = TabGroup.NeutralRoles;
        HasTask = false;
        CanKill = true;
        CurrentKillCooldown = 233f;
        GiveRoleOnStart = false; // 开局不会给予这个职业
        DisplayName = "跟班";
        Info = "我是你的狗！";
        Description = "(独立阵营):\n豺狼招募后被招募者会变成这个职业。";
    }

    private static void SetRoleToSidekick(PlayerControl player)
    {
        player.RpcSetRole(RoleTypes.Impostor);
        player.RpcSetCustomRole(CustomRoles.Sidekick);
        player.ResetKillCooldown();
    }

    private static void RiseUpToJackal(PlayerControl player)
    {
        player.RpcSetRole(RoleTypes.Shapeshifter);
        player.RpcSetCustomRole(CustomRoles.Jackal);
        player.ResetKillCooldown();
        player.SetKillCooldown(Jackal.KillCooldown.GetFloat());
        player.RpcResetAbilityCooldown();
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer == null || target == null) return true;

        if (killer.GetCustomRole() == CustomRoles.Sidekick) return false;

        if (target.GetCustomRole() == CustomRoles.Sidekick)
        {
            killer.RpcGuardAndKill(target);
            return false;
        }

        if (target.GetCustomRole() == CustomRoles.Jackal) this.OnPlayerExiled(target.Data);
        return true;
    }

    public void OnPlayerExiled(GameData.PlayerInfo exiled)
    {
        if (exiled.GetCustomRole() == CustomRoles.Jackal)
            foreach (var pair in PlayerInfo)
                if (pair.Key == exiled.PlayerId)
                {
                    var target = Utils.GetPlayerById(pair.Value);
                    if (target != null) RiseUpToJackal(target);
                }
    }

    public void OnPlayerLeft(AmongUsClient client, ClientData data, DisconnectReasons reason)
    {
        if (client.PlayerPrefab.GetCustomRole() == CustomRoles.Jackal)
        {
            this.OnPlayerExiled(client.PlayerPrefab.Data);
        }
    }

    public override string TargetMark(PlayerControl seer, PlayerControl target)
    {
        if (seer.Is(CustomRoles.Sidekick))
            if (target.GetCustomRole() == CustomRoles.Jackal)
                return Utils.ColorString(Utils.GetRoleColor(CustomRoles.Jackal), "❂");

        if (seer.Is(CustomRoles.Jackal))
            if (target.GetCustomRole() == CustomRoles.Sidekick)
                return Utils.ColorString(Utils.GetRoleColor(CustomRoles.Jackal), "❂");

        return "";
    }

    public void OnPlayerShapeShift(PlayerControl shapeShifter, PlayerControl target)
    {
        if (shapeShifter == null || target == null) return;
        if (shapeShifter.GetCustomRole() != CustomRoles.Jackal || shapeShifter.PlayerId == target.PlayerId || !Jackal.CanRecruit.GetBool()) return;

        if (RecruitedPlayers.Contains(shapeShifter.PlayerId)) return;

        SetRoleToSidekick(target);
        Utils.NotifyRoles();
        RecruitedPlayers.Add(shapeShifter.PlayerId);
        PlayerInfo.Add(shapeShifter.PlayerId, target.PlayerId);
    }

    public override void SetupOptions() { } // 不在菜单显示这个职业
}