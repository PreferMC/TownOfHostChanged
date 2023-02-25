using System.Collections.Generic;
using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Spy : Role, IListener
{
    private static readonly Dictionary<byte, CustomRoles> KillerRoles = new(); // 第一个是死者，第二个凶手职业。

    public Spy() : base(6, CustomRoles.Spy)
    {
        Color = "#00ffff";
        BaseRole = RoleTypes.Engineer;
        Group = TabGroup.CrewmateRoles;
        CanUseVent = true;
        HasVision = true;
        HasTask = false;
        DisplayName = "侦探";
        Description = "(船员阵营):\n你可以使用通风管道，同时你拥有内鬼视野，你可以通过报告知晓凶手职业。";
        Info = "我总能快你一步！";
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer == null || target == null) return true;

        KillerRoles.Add(target.PlayerId, killer.GetCustomRole());

        return true;
    }

    public void OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (target == null!) return;

        if (reporter.GetCustomRole() != CustomRoles.Spy) return;

        foreach (var keyValuePair in KillerRoles)
            if (keyValuePair.Key == target.PlayerId)
                new LateTask(() =>
                {
                    Utils.SendMessage("凶手的职业是 " + Translator.GetString(keyValuePair.Value.ToString()) , reporter.PlayerId, "★ 侦探信息 ★");
                }, 5.0f, "Spy task");
    }

    public void OnGameStarted(AmongUsClient client)
    {
        KillerRoles.Clear();
    }
}