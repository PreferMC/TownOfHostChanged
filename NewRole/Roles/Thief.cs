using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Thief : Role, IListener
{
    public Thief() : base(11112, CustomRoles.Thief)
    {
        Color = "#00ff00";
        HasTask = false;
        Group = TabGroup.NeutralRoles;
        CanKill = true;
        CurrentKillCooldown = 20f;
        DisplayName = "小偷";
        Info = "你的职业不错，我的了！";
        Description = "(独立阵营):\n你可以通过击杀来偷取他人职业。";
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer.GetCustomRole() != CustomRole) return true;

        var typeRole = target.GetCustomRole();
        target.RpcSetRole(RoleTypes.Crewmate);
        target.RpcSetCustomRole(CustomRoles.Crewmate);

        killer.RpcSetCustomRole(typeRole);

        killer.RpcGuardAndKill(target);
        return false;
    }
}