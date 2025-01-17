using System.Collections.Generic;
using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Augur : Role, IListener
{
    private static readonly Dictionary<PlayerControl, string> ToSendMessage = new();

    private OptionItem _killCooldown;

    public Augur() : base(60606, CustomRoles.Augur)
    {
        Color = "#008080";
        HasTask = false;
        Group = TabGroup.CrewmateRoles;
        BaseRole = RoleTypes.Impostor;
        CanKill = true;
        CurrentKillCooldown = 60f;
        DisplayName = "占卜师";
        Description = "(船员阵营):\n你可以通过出刀来获取被刀者职业。";
        Info = "看看我的水晶球！";
    }

    public override float GetCurrentKillCooldown()
    {
        return _killCooldown.GetFloat();
    }

    public override void SetupOptions()
    {
        base.SetupOptions();
        _killCooldown = FloatOptionItem.Create(Id + 10, "KillCooldown", new(30f, 80f, 2.5f), 60f, Group, false)
            .SetParent(Options.CustomRoleSpawnChances[CustomRole])
            .SetValueFormat(OptionFormat.Seconds);
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer == null || target == null) return true;

        if (killer.GetCustomRole() == CustomRoles.Augur)
        {
            // Utils.SendMessage(target._cachedData.PlayerName + " 的职业是 " + Translator.GetString(target.GetCustomRole().ToString()), killer.PlayerId);
            ToSendMessage.Add(killer, target._cachedData.PlayerName + " 的职业是 " + Translator.GetString(target.GetCustomRole().ToString()));
            killer.RpcGuardAndKill(target);
            return false;
        }

        return true;
    }

    public bool OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        new LateTask(() =>
        {
            foreach (var keyValuePair in ToSendMessage) Utils.SendMessage(keyValuePair.Value, keyValuePair.Key.PlayerId);
            ToSendMessage.Clear();
        }, 5, "Task for augur");

        return true;
    }

    public void OnGameStarted(AmongUsClient client)
    {
        ToSendMessage.Clear();
    }
}