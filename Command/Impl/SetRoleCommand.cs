using AmongUs.GameOptions;

namespace TownOfHost.Command.Impl;

public class SetRoleCommand : Command
{
    public SetRoleCommand() : base("setRole", true)
    {
        Canceled = true;
        Aliases = new[] { "sr" };
    }

    public override void OnExecute(PlayerControl player, string[] args)
    {
        if (!DebugModeManager.IsDebugMode)
        {
            HelpCommand.SendMessage("只有调试模式才能够使用这个命令", player);
            return;
        }

        if (!GameStates.InGame)
        {
            HelpCommand.SendMessage("请在游戏开始后使用", player);
            return;
        }

        if (args.Length <= 0)
        {
            HelpCommand.SendMessage("语法：/setRole (职业) [玩家ID]", player);
            return;
        }

        if (args.Length == 1)
        {
            var typeRole = Utils.GetRoleByName(args[0]);
            if (typeRole == null)
            {
                HelpCommand.SendMessage("找不到这个职业", player);
                return;
            }

            CustomRoles role = (CustomRoles)typeRole;

            player.RpcSetRole(role.IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate);
            player.RpcSetCustomRole(role);
            return;
        }

        if (args.Length == 2)
        {
            var typeRole = Utils.GetRoleByName(args[0]);
            if (typeRole == null)
            {
                HelpCommand.SendMessage("找不到这个职业", player);
                return;
            }

            CustomRoles role = (CustomRoles)typeRole;

            if (!int.TryParse(args[1], out int id))
            {
                HelpCommand.SendMessage("ID必须是纯数字", player);
                return;
            }

            var target = Utils.GetPlayerById(id);
            if (target == null)
            {
                HelpCommand.SendMessage("找不到这个玩家", player);
                return;
            }

            target.RpcSetRole(role.IsImpostor() ? RoleTypes.Impostor : RoleTypes.Crewmate);
            target.RpcSetCustomRole(role);
        }
    }
}