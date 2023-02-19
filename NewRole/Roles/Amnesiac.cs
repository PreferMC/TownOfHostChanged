using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

/*
 * 中立职业开发示例
 */
public class Amnesiac : Role, IListener
{
    public Amnesiac() : base(1919810, CustomRoles.Amnesiac)
    {
        Color = "#3399ff";
        HasTask = false;
        Group = TabGroup.NeutralRoles;
        DisplayName = "失忆者";
        Info = "我是谁？";
        Description = "(独立阵营):\n你可以通过报告来偷取被报告死者职业，如果死者已经退出，你会偷取到白板船员。";
    }

    public void OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (target == null! || reporter == null) return;
        if (reporter.GetCustomRole() != CustomRoles.Amnesiac) return;

        new LateTask(() =>
        {
            var newRole = target.GetCustomRole().GetRoleByCustomRole();
            if (newRole != null)
                if (!newRole.CanUseAbility)
                    reporter.RpcSetRole(newRole.CanKill
                        ? RoleTypes.Impostor : RoleTypes.Crewmate);
                else
                    reporter.RpcSetRole(newRole.CanUseAbility
                        ? RoleTypes.Shapeshifter : newRole.CanKill
                            ? RoleTypes.Impostor : RoleTypes.Crewmate);
            else
                reporter.RpcSetRole(target.GetCustomRole().IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate);
            reporter.RpcSetCustomRole(target.GetCustomRole());
        }, 2, "Amnesiac Task");
    }
}