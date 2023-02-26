using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

/*
 * 中立职业开发示例
 */
public class Amnesiac : Role, IListener
{
    private OptionItem _canReportImpostorBody;

    public Amnesiac() : base(1919810, CustomRoles.Amnesiac)
    {
        Color = "#3399ff";
        HasTask = false;
        Group = TabGroup.NeutralRoles;
        DisplayName = "失忆者";
        Info = "我是谁？";
        Description = "(独立阵营):\n你可以通过报告来偷取被报告死者职业，如果死者已经退出，你会偷取到白板船员。";
    }

    public bool OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (target == null! || reporter == null) return true;
        if (reporter.GetCustomRole() != CustomRoles.Amnesiac) return true;

        new LateTask(() =>
        {
            // var newRole = target.GetCustomRole().GetRoleByCustomRole();
            var typeRole = target.GetCustomRole();
            if (!_canReportImpostorBody.GetBool() && typeRole.CanKill())
            {
                reporter.RpcMurderPlayer(reporter);
                return;
            }

            reporter.RpcSetRole(typeRole.IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate);
            reporter.RpcSetCustomRole(typeRole);
            /*
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
            reporter.RpcSetCustomRole(target.GetCustomRole());*/
        }, 5f, "Amnesiac Task");

        return true;
    }

    public override void SetupOptions()
    {
        base.SetupOptions();
        _canReportImpostorBody = BooleanOptionItem.Create(Id + 2, "CanReportImpostorBody", true, Group, false)
            .SetParent(Options.CustomRoleSpawnChances[CustomRole])
            .SetGameMode(CustomGameMode.Standard);
    }
}