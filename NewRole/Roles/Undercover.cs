using System;

namespace TownOfHost.NewRole.Roles;

/*
 * 废弃了 有能力的自己做把
 */
[Obsolete]
public class Undercover/* : Role, IListener*/
{
    /*
    public Undercover() : base(151566, CustomRoles.Undercover)
    {
        Color = "#ff8c00";
        Group = TabGroup.CrewmateRoles;
        HasTask = true;
        BaseRole = RoleTypes.Crewmate;
        DisplayName = "卧底";
        Info = "抱歉，我是警察";
        Description = "(船员阵营):\n内鬼看到你是他们中的一员，你最好离他们远点，他会发现的。";
    }*/
/*
    public void OnMeetingHudStart(MeetingHud meetingHud)
    {
        var impostors = new List<PlayerControl>();

        foreach (var player in Main.AllPlayerControls)
            if (player.Is(RoleType.Impostor)) impostors.Add(player);

        foreach (var pva in meetingHud.playerStates)
        {
            PlayerControl target = Utils.GetPlayerById(pva.TargetPlayerId);
            if (target == null) continue;
            // if (target.GetCustomRole().IsImpostor()) pva.NameText.color = Palette.ImpostorRed;
        }
    }
*/
/*
    public void OnGameStarted(AmongUsClient client)
    {
        Main.Logger.LogInfo("Game started & undercover red name displayed");

        var impostors = new List<PlayerControl>();

        foreach (var player in Main.AllPlayerControls)
            if (player.Is(RoleType.Impostor)) impostors.Add(player);

        foreach (var player in Main.AllAlivePlayerControls)
        {
            if (player.GetCustomRole() != CustomRoles.Undercover) continue;

            foreach (var impostor in impostors)
            {
                // player.RpcSetNamePrivate("66666", true, impostor, true);
                player.RpcSetName("666");
            }
        }*/

        /*
        foreach (var player in Main.AllAlivePlayerControls)
        {
            if (player.GetCustomRole() != CustomRoles.Undercover) continue;

            player.RpcSetNamePrivate();

            new LateTask(() =>
            {
                foreach (var impostor in impostors) player.RpcSetNamePrivate($"<color=#ff0000>{player.Data.PlayerName}</color>", false, impostor, true);
            }, 5f, "Undercover task");
        }
    }*/

}