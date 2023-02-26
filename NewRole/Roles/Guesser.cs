using TownOfHost.Command;

namespace TownOfHost.NewRole.Roles;

/*
 * 副职业赌怪
 * 开发者开发副职业可以参考这个格式
 */
public class Guesser : Role
{
    public Guesser() : base(4545112, CustomRoles.Guesser)
    {
        SubRole = true;
        Color = "#ff6666";
        DisplayName = "赌怪";
        Info = "生命就是豪赌！";
        Group = TabGroup.Addons;

        new GuessCommand().RegisterCommand();
    }

    private class GuessCommand : Command.Command
    {
        public GuessCommand() : base("guess", false)
        {
            Aliases = new[] { "bt", "gs", "cc" };
            Canceled = true;
        }

        private void SendMessage(string msg, PlayerControl player) => Utils.SendMessage(msg, player.PlayerId);

        public override void OnExecute(PlayerControl player, string[] args)
        {
            if (!GameStates.InGame)
            {
                SendMessage("游戏开始后才能进行赌注", player);
                return;
            }

            if (!player.IsAlive())
            {
                SendMessage("死亡后不能进行赌注", player);
                return;
            }

            if (!player.GetCustomSubRoles().Contains(CustomRoles.Guesser))
            {
                SendMessage("你不是赌怪", player);
                return;
            }

            if (args.Length != 2)
            {
                SendMessage("错误的语法，正确语法：/guess (玩家ID) (猜测职业)" + "\n" + "获取所有玩家ID方法：/id", player);
                return;
            }

            if (!int.TryParse(args[0], out int outNum))
            {
                SendMessage("玩家ID必须是数字！", player);
                return;
            }

            var target = Utils.GetPlayerById(outNum);
            if (target == null)
            {
                SendMessage("找不到这个玩家，请输入/id检查", player);
                return;
            }
            var typeRole = Utils.GetRoleByName(args[1]);
            if (typeRole == null)
            {
                SendMessage("找不到这个职业，请检查拼写。", player);
                return;
            }

            var toKill = target.GetCustomRole() == typeRole ? target : player;

            toKill.Die(DeathReason.Kill, true);
            Utils.SendMessage(toKill.Data.PlayerName + " 在赌局中失利了！");
        }
    }
}